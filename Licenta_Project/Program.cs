using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AForge.Imaging;
using Licenta_Project.Common;
using Licenta_Project.Common.Extensions;
using Licenta_Project.DAL;
using Licenta_Project.Services;

namespace Licenta_Project
{
    [LogAspect]
    class Program
    {
        static void Main(string[] args)
        {
            var context = new DdsmContext();
            var ddsm = new DdsmService
            (
                new BaseEntityRepository<DbCase>(context)
            );

            var ann = new AnnService();

            var annTrainingData = ddsm.GetTrainingData();
            var input = annTrainingData.Input;
            var output = annTrainingData.Output;

            ann.Train(input, output);
            ann.SaveAnnToFile(@"D:\Facultate\Licenta\Licenta\Licenta_Project\Resources\Network.txt");

            var image = new Bitmap(@"G:\DDSM-images\cases\benigns\benign_01\case0029\PNGFiles\C_0029_1.LEFT_CC.png");
            var imageStatistics = new ImageStatistics(image);

            var histogram = imageStatistics.Red;
            var newDbCase = new DbCase
            {
                PatientAge = 66,
                Density = 3,
                MaxBlobArea = histogram.Max,
                ImageMean = histogram.Mean,
                ImageStdDev = histogram.StdDev,
                ImageSkew = histogram.Skew(),
                ImageKurt = histogram.Kurt()
            };

            //ann.LoadAnnFromFile(@"D:\Facultate\Licenta\Licenta\Licenta_Project\Resources\Network.txt");

            var normalizeInput = ddsm.NormalizeInputItem(newDbCase);
            var result = ann.Test(normalizeInput);
            Console.WriteLine($"Result is: {result}");
        }
    }
}