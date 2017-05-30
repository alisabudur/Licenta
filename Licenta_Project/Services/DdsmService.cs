using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AForge.Imaging;
using Licenta_Project.DAL;
using Licenta_Project.Extensions;

namespace Licenta_Project.Services
{
    public class DdsmService
    {
        #region Fields
        private IDdsmFileRepository _ddsmFileRepository;
        private IBaseEntityRepository<DbCase> _dbCaseRepository;
        #endregion

        #region Constructor
        public DdsmService(
            IDdsmFileRepository ddsmFileRepository,
            IBaseEntityRepository<DbCase> caseRepository
            )
        {
            _ddsmFileRepository = ddsmFileRepository;
            _dbCaseRepository = caseRepository;

            //_ddsmFileRepository.LoadCasesFromFiles();
        }

        #endregion


        #region Methods

        public AnnTrainingData GetTrainingData()
        {
            var cases = GetShuffledCases();
            var input = NormalizeInput(cases);
            var output = NormalizeOutput(cases);

            return new AnnTrainingData
            {
                Input = input,
                Output = output
            };
        }

        public double[] NormalizeInputItem(DbCase dbCase)
        {
            //TODO add dbCase in cases in the future!!!

            var cases = _dbCaseRepository.GetAll().ToList();
            #region Initialize Mean & StdDev

            var meanPacientAge = cases.Average(p => p.PatientAge);
            var stdDevPatientApe = cases.Select(p => p.PatientAge).StdDev();

            var meanDensity = cases.Average(p => p.Density);
            var stdDevDensity = cases.Select(p => p.Density).StdDev();

            var meanImageMean = cases.Average(p => p.ImageMean);
            var stdDevImageMean = cases.Select(p => p.ImageMean).StdDev();

            var meanImageMedian = cases.Average(p => p.ImageMedian);
            var stdDevImageMedian = cases.Select(p => p.ImageMedian).StdDev();

            var meanImageStdDev = cases.Average(p => p.ImageStdDev);
            var stdDevImageStdDev = cases.Select(p => p.ImageStdDev).StdDev();

            var meanImageSkew = cases.Average(p => p.ImageSkew);
            var stdDevImageSkew = cases.Select(p => p.ImageSkew).StdDev();

            var meanImageKurt = cases.Average(p => p.ImageKurt);
            var stdDevImageKurt = cases.Select(p => p.ImageKurt).StdDev();

            #endregion

            return new double[]
            {

                (dbCase.PatientAge - meanPacientAge) / stdDevPatientApe,
                (dbCase.Density - meanDensity) / stdDevDensity,
                (dbCase.ImageMean - meanImageMean) / stdDevImageMean,
                (dbCase.ImageMedian - meanImageMedian) / stdDevImageMedian,
                (dbCase.ImageStdDev - meanImageStdDev) / stdDevImageStdDev,
                (dbCase.ImageSkew - meanImageSkew) / stdDevImageSkew,
                (dbCase.ImageKurt - meanImageKurt) / stdDevImageKurt

            };
        }

        private IEnumerable<DbCase> GetShuffledCases()
        {
            var benings = _dbCaseRepository.FindBy(d => d.Patology == (double)Patology.Benign).ToArray();
            var maligns = _dbCaseRepository.FindBy(d => d.Patology == (double)Patology.Malignant).ToArray();
            //var normals = _dbCaseRepository.FindBy(d => d.Patology == (double)Patology.Normal).ToArray();

            var count = new int[] { benings.Length, maligns.Length/*, normals.Length*/ }.Min();
            var cases = new List<DbCase>();

            for (var i = 0; i < count; i++)
            {
                cases.Add(benings[i]);
                cases.Add(maligns[i]);
                //cases.Add(normals[i]);
            }
            return cases;
        }

        private IEnumerable<DbCase> GetCasesFromFile()
        {
            var workCases = _ddsmFileRepository.Cases.ToList();
            var inputs = new List<DbCase>();

            foreach (var caseItem in workCases)
            {
                foreach (var imageKey in caseItem.Images.Keys)
                {
                    using (var image = new Bitmap(caseItem.Images[imageKey].ImagePath))
                    {
                        var imageStatistics = new ImageStatistics(image);
                        var histogram = imageStatistics.Red;

                        var input = new DbCase
                        {
                            PatientAge = caseItem.PatientAge,
                            Density = caseItem.Density,
                            ImageMean = histogram.Mean,
                            ImageMedian = histogram.Median,
                            ImageStdDev = histogram.StdDev,
                            ImageSkew = histogram.Skew(),
                            ImageKurt = histogram.Kurt(),
                            ImagePath = caseItem.Images[imageKey].ImagePath,
                            Patology = (double)caseItem.Images[imageKey].Overlay.Abnormalities
                            .ToList()
                            .First()
                            .Patology
                        };
                        inputs.Add(input);
                    }
                }
            }
            return inputs;
        }

        private double[][] NormalizeInput(IEnumerable<DbCase> inputs)
        {
            #region Initialize Mean & StdDev

            var meanPacientAge = inputs.Average(p => p.PatientAge);
            var stdDevPatientApe = inputs.Select(p => p.PatientAge).StdDev();

            var meanDensity = inputs.Average(p => p.Density);
            var stdDevDensity = inputs.Select(p => p.Density).StdDev();

            var meanImageMean = inputs.Average(p => p.ImageMean);
            var stdDevImageMean = inputs.Select(p => p.ImageMean).StdDev();

            var meanImageMedian = inputs.Average(p => p.ImageMedian);
            var stdDevImageMedian = inputs.Select(p => p.ImageMedian).StdDev();

            var meanImageStdDev = inputs.Average(p => p.ImageStdDev);
            var stdDevImageStdDev = inputs.Select(p => p.ImageStdDev).StdDev();

            var meanImageSkew = inputs.Average(p => p.ImageSkew);
            var stdDevImageSkew = inputs.Select(p => p.ImageSkew).StdDev();

            var meanImageKurt = inputs.Average(p => p.ImageKurt);
            var stdDevImageKurt = inputs.Select(p => p.ImageKurt).StdDev();

            #endregion

            var normalizeInputs = inputs
                .Select(p => new double[] {

                    (p.PatientAge - meanPacientAge) / stdDevPatientApe,
                    (p.Density - meanDensity) / stdDevDensity,
                    (p.ImageMean - meanImageMean) / stdDevImageMean,
                    (p.ImageMedian - meanImageMedian) / stdDevImageMedian,
                    (p.ImageStdDev - meanImageStdDev) / stdDevImageStdDev,
                    (p.ImageSkew - meanImageSkew) / stdDevImageSkew,
                    (p.ImageKurt - meanImageKurt) / stdDevImageKurt

                })
                .ToArray();

            return normalizeInputs;
        }



        private double[][] NormalizeOutput(IEnumerable<DbCase> output)
        {
            var values = output.Select(p => GetPatology(p.Patology)).ToArray();
            return values;
        }

        private double[] GetPatology(double patology)
        {
            var value = (Patology)patology;

            if (value == Patology.Benign)
                return new double[] { 1, -1};
            //if (value == Patology.Malignant)
                return new double[] { -1, 1};
            //return new double[] { 0, 0, 1 };
        }
        #endregion
    }
}
