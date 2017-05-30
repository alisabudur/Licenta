using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
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
        private Network _network;

        public AnnService()
        {
            _network = new ActivationNetwork(new SigmoidFunction(), 7, 10, 10, 2);
        }

        public void Train(double[][] input, double[][] output)
        {
            var learning = new BackPropagationLearning((ActivationNetwork)_network)
            {
                LearningRate = 1
            };
            var needToStop = false;
            double error = 0;
            var iterations = 0;
            while (!needToStop && iterations <= 10000)
            {
                error = learning.RunEpoch(input, output) / 996;
                if (error < 0.1)
                    needToStop = true;
                iterations++;
            }
            Console.WriteLine($"Error: {error}, iterations: {iterations}");
        }

        public Patology Test(double[] input)
        {
            var outputNetwork = _network.Compute(input);
            var patology = GetPatology(outputNetwork);
            return patology;
        }

        public void SaveAnnToFile(string fileName)
        {
            _network.Save(fileName);
        }

        public void LoadAnnFromFile(string fileName)
        {
            _network = Network.Load(fileName);
        }

        private Patology GetPatology(double[] output)
        {
            var max = output.Max();
            var positionOfMax = output.ToList().IndexOf(max);

            if(positionOfMax == 0)
                return Patology.Benign;
            //if (positionOfMax == 1)
                return Patology.Malignant;
            //return Patology.Normal;
        }
    }
}
