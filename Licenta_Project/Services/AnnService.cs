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
using Licenta_Project.Extensions;

namespace Licenta_Project.Services
{
    [LogAspect]
    public class AnnService
    {
        private readonly List<Case> _cases;
        private readonly DdsmService _ddsm;

        public AnnService()
        {
            var context = new DdsmContext();
            _ddsm = new DdsmService
                (
                    new DdsmFileRepository(), 
                    new BaseEntityRepository<Input>(context),
                    new BaseEntityRepository<Output>(context)
                );
        }

        public void Ann()
        {
            var network = new ActivationNetwork(new SigmoidFunction(), 7, 10, 10, 4);
            var learning = new BackPropagationLearning(network)
            {
                LearningRate = 1
            };
            var needToStop = false;

            var input = _ddsm.GetInputNormalized();
            var output = _ddsm.GetOutputNormalized();

            var iterations = 0;
            double error = 0;
            while (!needToStop && iterations <= 10000)
            {
                error = learning.RunEpoch(input, output) / 1668;
                iterations++;
            }

            var imageStatistics = new ImageStatistics(new Bitmap(@"G:\DDSM-images\cases\cancers\cancer_02\case0018\PNGFiles\C_0018_1.LEFT_CC.png"));
            var histogram = imageStatistics.Red;
            var inputItem = new double[7];

            inputItem[0] = 52;
            inputItem[1] = 2;
            inputItem[2] = histogram.Mean;
            inputItem[3] = histogram.Median;
            inputItem[4] = histogram.StdDev;
            inputItem[5] = histogram.Skew();
            inputItem[6] = histogram.Kurt();

            var outputNetwork = network.Compute(inputItem);
        }
    }
}
