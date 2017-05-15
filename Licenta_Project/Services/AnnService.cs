using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AForge.Imaging;
using AForge.Neuro;
using AForge.Neuro.Learning;
using Licenta_Project.Aspects;
using Licenta_Project.DAL;

namespace Licenta_Project.Services
{
    [LogAspect]
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

        public void Ann()
        {
            var network = new ActivationNetwork(new SigmoidFunction(2), 6, 10, 10, 1);
            var learning = new BackPropagationLearning(network)
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

            var imageStatistics = new StatisticsService(new Bitmap(@"G:\DDSM-images\cases\cancers\cancer_03\case1024\PNGFiles\A_1024_1.RIGHT_MLO.png"));
            var inputItem = new double[6];

            inputItem[0] = 52;
            inputItem[1] = 2;
            inputItem[2] = imageStatistics.Mean;
            inputItem[3] = imageStatistics.StdDev;
            inputItem[4] = imageStatistics.Skew;
            inputItem[5] = imageStatistics.Kurt;

            var outputNetwork = network.Compute(inputItem);
            throw new Exception("Test exception");
        }
    }
}
