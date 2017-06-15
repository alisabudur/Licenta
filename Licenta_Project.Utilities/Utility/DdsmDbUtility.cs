using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AForge.Imaging;
using AForge.Imaging.Filters;
using Licenta_Project.Common;
using Licenta_Project.DAL;
using Licenta_Project.Utilities;

namespace Licenta_Project.Utility
{
    public class DdsmDbUtility
    {
        private DdsmFileUtility _fileUtility;
        private readonly IBaseEntityRepository<DbCase> _dbCaseRepository;

        public DdsmDbUtility()
        {
            _fileUtility = new DdsmFileUtility();
            _fileUtility.LoadCasesFromFiles();

            var dbContext = new DdsmContext();
            _dbCaseRepository = new BaseEntityRepository<DbCase>(dbContext);
        }

        public void PutCasesInDb()
        {
            var workCases = _fileUtility.Cases.ToList();

            foreach (var caseItem in workCases)
            {
                foreach (var imageKey in caseItem.Images.Keys)
                {
                    using (var image = new Bitmap(caseItem.Images[imageKey].ImagePath))
                    {
                        if (caseItem.Images[imageKey].Overlay != null)
                            foreach (var abnormality in caseItem.Images[imageKey].Overlay.Abnormalities)
                            {
                                AddDataInDb(caseItem, imageKey, image, abnormality.Patology, abnormality.Boundary);

                                if(abnormality.Cores != null)
                                    foreach (var core in abnormality.Cores)
                                    {
                                        AddDataInDb(caseItem, imageKey, image, abnormality.Patology, core);
                                    }
                            }
                    }
                }
            }
        }

        private Bitmap GetAbnormalityCrop(Bitmap image, Outline outline)
        {
            var x = outline.StartX;
            var y = outline.StartY;

            var minX = int.MaxValue;
            var minY = int.MaxValue;
            var maxX = int.MinValue;
            var maxY = int.MinValue;

            var xCoordinate = new int[] { 0, 1, 1, 1, 0, -1, -1, -1 };
            var yCoordinate = new int[] { -1, -1, 0, 1, 1, 1, 0, -1 };

            foreach (var point in outline.Chain)
            {
                x = x + xCoordinate[point];
                y = y + yCoordinate[point];

                if (x < minX) minX = x;
                if (y < minY) minY = y;
                if (x > maxX) maxX = x;
                if (y > maxY) maxY = y;
            }

            var cropPointX = minX;
            var cropPointY = minY;
            var width = Math.Abs(maxX - minX);
            var height = Math.Abs(maxY - minY);
            
            var filter = new Crop(new Rectangle(cropPointX, cropPointY, width, height));
            var cropImage = filter.Apply(image);

            return cropImage;
        }

        private void AddDataInDb(Case caseItem, ImageName imageKey, Bitmap image, Patology patology, Outline outline)
        {
            if(outline == null)
                return;

            using (var cropImage = GetAbnormalityCrop(image, outline))
            {
                var imageStatistics = new ImageStatistics(cropImage);
                var histogram = imageStatistics.Red;

                var input = new DbCase
                {
                    PatientAge = caseItem.PatientAge,
                    Density = caseItem.Density,
                    ImageMax = histogram.Max,
                    ImageMin =  histogram.Min,
                    ImageMean = histogram.Mean,
                    ImageStdDev = histogram.StdDev,
                    ImageSkew = histogram.Skew(),
                    ImageKurt = histogram.Kurt(),
                    ImagePath = caseItem.Images[imageKey].ImagePath,
                    Patology = (double)patology
                };
                _dbCaseRepository.Add(input);
            }
        }

    }
}
