using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AForge.Imaging;
using AForge.Neuro;
using AForge.Neuro.Learning;
using Licenta_Project.DAL;

namespace Licenta_Project.Services
{
    public class AnnService
    {
        private readonly List<Case> _cases;
        private readonly DDSM _ddsm;

        public AnnService()
        {
            _ddsm = new DDSM();
            _ddsm.GetCases();
            _cases = _ddsm.Cases.ToList();
        }

        public double[][] GetInput()
        {
            var index = 0;
            var input = new double[20][];
            var workCases = _cases.Take(5).ToList();

            foreach (var caseItem in workCases)
            {
                foreach (var imageKey in caseItem.Images.Keys)
                {
                    using (var image = new Bitmap(caseItem.Images[imageKey].ImagePath))
                    {
                        var imageStatistics = new ImageStatistics(image);
                        var histogram = imageStatistics.Red;

                        var inputItem = new double[4];

                        inputItem[0] = caseItem.PatientAge;
                        inputItem[1] = histogram.Mean;
                        inputItem[2] = histogram.Median;
                        inputItem[3] = histogram.StdDev;

                        input[index] = inputItem;
                        index++;
                    }
                }
            }
            return input;
        }

        public double[][] GetOutput()
        {
            var index = 0;
            var output = new double[20][];
            var workCases = _cases.Take(5).ToList();

            foreach (var caseItem in workCases)
            {
                foreach (var imageKey in caseItem.Images.Keys)
                {
                    var outputItem = new double[2];

                    if (caseItem.Images[imageKey].Overlay != null)
                    {
                        outputItem[0] = (double)caseItem.Images[imageKey].Overlay.Abnormalities
                            .ToList()
                            .First()
                            .LessionType;

                        outputItem[1] = (double)caseItem.Images[imageKey].Overlay.Abnormalities
                            .ToList()
                            .First()
                            .Patology;
                    }
                    else
                    {
                        outputItem[0] = (double)LessionType.Undefined;
                        outputItem[1] = (double)Patology.Normal;
                    }
                    output[index] = outputItem;
                    index++;
                }
            }
            return output;
        }

        public void Ann()
        {
            var network = new ActivationNetwork(new SigmoidFunction(2), 4, 1);
            var learning = new DeltaRuleLearning(network)
            {
                LearningRate = 1
            };
            var needToStop = false;

            var input = _ddsm.GetInputFromDb();
            var output = _ddsm.GetOutputFromDb();

            var iterations = 0;
            double error = 0;
            while (!needToStop && iterations <= 10000)
            {
                error = learning.RunEpoch(input, output) / 1668;
                if (error < 3)
                {
                    Console.WriteLine(error);
                    var outputNetwork1 = network.Compute(input[1]);
                    Console.WriteLine(outputNetwork1[0] + " - " + output[1][0]);
                }
                iterations++;
            }

            var imageStatistics = new ImageStatistics(new Bitmap(@"G:\DDSM-images\cases\cancers\cancer_03\case1024\PNGFiles\A_1024_1.RIGHT_MLO.png"));
            var histogram = imageStatistics.Red;

            var inputItem = new double[4];

            inputItem[0] = 52;
            inputItem[1] = 2;
            inputItem[2] = histogram.Mean;
            inputItem[3] = histogram.StdDev;

            var outputNetwork = network.Compute(inputItem);

        }
    }
}
