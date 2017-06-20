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
        public Network Network { get; set; }

        public AnnService()
        {
        }

        public double GetEpochAccuracy(double[][] input, double[][] output)
        {
            var goodNetOutputCounts = 0;
            for (var i = 0; i < input.Length; i++)
            {
                var netOutput = Network.Compute(input[i]);
                var netPathology = GetPatology(netOutput);
                var actualPathology = GetPatology(output[i]);
                if (netPathology == actualPathology)
                    goodNetOutputCounts++;
            }
            var result = goodNetOutputCounts * 100 / input.Length;
            return result;
        }

        public double GetEpochPrecision(double[][] input, double[][] output)
        {
            var trueBenigns = 0;
            var falseBenigns = 0;
            for (var i = 0; i < input.Length; i++)
            {
                var netOutput = Network.Compute(input[i]);
                var netPathology = GetPatology(netOutput);
                var actualPathology = GetPatology(output[i]);

                if (netPathology == Patology.Benign && actualPathology == Patology.Benign)
                    trueBenigns++;

                if (netPathology == Patology.Benign && actualPathology != Patology.Benign)
                    falseBenigns++;
            }
            if (trueBenigns + falseBenigns == 0)
                return 0;

            var result = (double)trueBenigns / (double)(trueBenigns + (double)falseBenigns);
            return result;
        }

        public double GetEpochRecall(double[][] input, double[][] output)
        {
            var trueBenigns = 0;
            var falseMaligns = 0;

            for (var i = 0; i < input.Length; i++)
            {
                var netOutput = Network.Compute(input[i]);
                var netPathology = GetPatology(netOutput);
                var actualPathology = GetPatology(output[i]);

                if (netPathology == Patology.Benign && actualPathology == Patology.Benign)
                    trueBenigns++;

                if (netPathology == Patology.Malignant && actualPathology != Patology.Malignant)
                    falseMaligns++;
            }

            if (trueBenigns + falseMaligns == 0)
                return 0;

            var result = (double)trueBenigns / (double)(trueBenigns + (double)falseMaligns);
            return result;
        }

        public Patology Test(double[] input)
        {
            var outputNetwork = Network.Compute(input);
            var patology = GetPatology(outputNetwork);
            return patology;
        }

        public void SaveAnnToFile(string fileName)
        {
            Network.Save(fileName);
        }

        public void LoadAnnFromFile(string fileName)
        {
            Network = Network.Load(fileName);
        }

        public Patology GetPatology(double[] output)
        {
            if (output[0] >= 0.5)
                return Patology.Malignant;
            return Patology.Benign;
            
        }
    }
}
