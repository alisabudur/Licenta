using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Licenta_Project.DAL;

namespace Licenta_Project.Services
{
    public class DdsmService
    {
        #region Fields
        private IDdsmFileRepository _ddsmFileRepository;
        private IBaseEntityRepository<Input> _inputRepository;
        private IBaseEntityRepository<Output> _outputRepository;
        #endregion

        #region Constructor
        public DdsmService(
            IDdsmFileRepository ddsmFileRepository,
            IBaseEntityRepository<Input> inputRepository,
            IBaseEntityRepository<Output> outputRepository
            )
        {
            _ddsmFileRepository = ddsmFileRepository;
            _inputRepository = inputRepository;
            _outputRepository = outputRepository;
        }

        #endregion


        #region Methods

        public void AddInputInDb()
        {

        }

        private IEnumerable<Input> GetInputFromFile()
        {
            _ddsmFileRepository.LoadCasesFromFiles();
            var workCases = _ddsmFileRepository.Cases.ToList();
            var inputs = new List<Input>();

            foreach (var caseItem in workCases)
            {
                foreach (var imageKey in caseItem.Images.Keys)
                {
                    using (var image = new Bitmap(caseItem.Images[imageKey].ImagePath))
                    {
                        var imageStatistics = new StatisticsService(image);

                        var input = new Input
                        {
                            PatientAge = caseItem.PatientAge,
                            Density = caseItem.Density,
                            ImageMean = imageStatistics.Mean,
                            ImageMedian = imageStatistics.Median,
                            ImageStdDev = imageStatistics.StdDev,
                            ImageSkew = imageStatistics.Skew,
                            ImageKurt = imageStatistics.Kurt,
                            ImagePath = caseItem.Images[imageKey].ImagePath
                        };
                        inputs.Add(input);
                    }
                }
            }
            return inputs;
        }

        private IEnumerable<Input> NormalizeInput(IEnumerable<Input> inputs)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
