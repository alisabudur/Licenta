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
using Licenta_Project.Common;
using Licenta_Project.DAL;

namespace Licenta_Project.WPF.Services
{
    [LogAspect]
    public class AnnService
    {
        private Network _network;

        public AnnService()
        {
            _network = new ActivationNetwork(new SigmoidFunction(), 8, 10, 10, 10, 1);
        }

        public void Train(double[][] input, double[][] output)
        {
            var learning = new BackPropagationLearning((ActivationNetwork)_network)
            {
                LearningRate = 0.01
            };
            var needToStop = false;
            double error = 0;
            var iterations = 0;
            while (!needToStop && iterations <= 10000)
            {
                error = learning.RunEpoch(input, output) / input.Length;
                if (error < 0.065)
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

        //private Patology GetPatology(double[] output)
        //{
        //    var max = output.Max();
        //    var positionOfMax = output.ToList().IndexOf(max);

        //    //if (positionOfMax == 0)
        //    //    return Patology.Normal;
        //    //if (positionOfMax == 1)
        //    //    return Patology.Malignant;
        //    //return Patology.Benign;

        //    if (positionOfMax == 0)
        //        return Patology.Malignant;
        //    return Patology.Benign;
        //}

        public Patology GetPatology(double[] output)
        {
            if (output[0] >= 0.5)
                return Patology.Malignant;
            return Patology.Benign;
            
        }
    }
}
