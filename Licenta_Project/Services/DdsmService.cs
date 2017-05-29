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

            //_ddsmFileRepository.LoadCasesFromFiles();
        }

        #endregion


        #region Methods

        public void AddInputInDb()
        {

        }

        public double[][] GetInputNormalized()
        {
            var inputs = _inputRepository.GetAll().ToList();
            var result = NormalizeInput(inputs);
            return result;
        }

        public double[][] GetOutputNormalized()
        {
            var outputs = _outputRepository.GetAll().ToList();
            var result = NormalizeOutput(outputs);
            return result;
        }

        private IEnumerable<Input> GetInputFromFile()
        {
            var workCases = _ddsmFileRepository.Cases.ToList();
            var inputs = new List<Input>();

            foreach (var caseItem in workCases)
            {
                foreach (var imageKey in caseItem.Images.Keys)
                {
                    using (var image = new Bitmap(caseItem.Images[imageKey].ImagePath))
                    {
                        var imageStatistics = new ImageStatistics(image);
                        var histogram = imageStatistics.Red;

                        var input = new Input
                        {
                            PatientAge = caseItem.PatientAge,
                            Density = caseItem.Density,
                            ImageMean = histogram.Mean,
                            ImageMedian = histogram.Median,
                            ImageStdDev = histogram.StdDev,
                            ImageSkew = histogram.Skew(),
                            ImageKurt = histogram.Kurt(),
                            ImagePath = caseItem.Images[imageKey].ImagePath
                        };
                        inputs.Add(input);
                    }
                }
            }
            return inputs;
        }

        private IEnumerable<Output> GetOutputFromFile()
        {
            var workCases = _ddsmFileRepository.Cases.ToList();
            var outputs = new List<Output>();

            foreach (var caseItem in workCases)
            {
                foreach (var imageKey in caseItem.Images.Keys)
                {
                    var output = new Output();

                    if (caseItem.Images[imageKey].Overlay != null)
                    {
                        output.Patology = (double)caseItem.Images[imageKey].Overlay.Abnormalities
                            .ToList()
                            .First()
                            .Patology;
                        output.ImagePath = caseItem.Images[imageKey].ImagePath;
                    }
                    else
                    {
                        output.Patology = (double)Patology.Normal;
                        output.ImagePath = caseItem.Images[imageKey].ImagePath;
                    };
                    outputs.Add(output);
                }
            }
            return outputs;
        }

        private double[][] NormalizeInput(IEnumerable<Input> inputs)
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

        private double[][] NormalizeOutput(IEnumerable<Output> output)
        {
            var values = output.Select(p => GetPatology(p.Patology)).ToArray();
            return values;
        }

        private double[] GetPatology(double patology)
        {
            var value = (Patology)patology;
            switch (value)
            {
                case Patology.Benign:
                    return new double[] { 1, 0, 0, 0 };
                case Patology.Malignant:
                    return new double[] { 0, 0, 1, 0 };
                case Patology.Normal:
                    return new double[] { 0, 1, 0, 0 };
                default:
                    return new double[] { 0, 0, 0, 1 };
            }
        }
        #endregion
    }
}
