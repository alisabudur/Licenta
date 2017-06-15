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

            //ann.Train(input, output);
            //ann.SaveAnnToFile(@"D:\Facultate\Licenta\Licenta\Licenta_Project\Resources\Network.txt");

            //malign

            var newDbCase = new DbCase
            {
                PatientAge = 55,
                Density = 1,
                ImageMax = 204,
                ImageMin = 10,
                ImageMean = 92.9142886321195,
                ImageStdDev = 19.1312635930521,
                ImageSkew = -0.87473789306472,
                ImageKurt = 5.50005325119627
            };

            //benigns

            var newDbCase2 = new DbCase
            {
                PatientAge = 89,
                Density = 2,
                ImageMax = 179,
                ImageMin = 62,
                ImageMean = 114.439511865602,
                ImageStdDev = 16.5381616685038,
                ImageSkew = -0.36883455246503,
                ImageKurt = 3.10578961414513
            };

            var newDbCase3 = new DbCase
            {
                PatientAge = 52,
                Density = 2,
                ImageMax = 136,
                ImageMin = 64,
                ImageMean = 84.9883434547908,
                ImageStdDev = 9.79461740533323,
                ImageSkew = 0.968386249188609,
                ImageKurt = 3.44823699943195
            };

            //malign

            var newDbCase4 = new DbCase
            {
                PatientAge = 52,
                Density = 2,
                ImageMax = 184,
                ImageMin = 34,
                ImageMean = 106.781674382716,
                ImageStdDev = 33.7836332160514,
                ImageSkew = 0.124457168115598,
                ImageKurt = 1.9943234384615
            };

            var newDbCase5 = new DbCase
            {
                PatientAge = 61,
                Density = 3,
                ImageMax = 198,
                ImageMin = 7,
                ImageMean = 98.4390528100775,
                ImageStdDev = 52.2567642553578,
                ImageSkew = -0.260592040274854,
                ImageKurt = 1.81159052275562
            };

            var newDbCase6 = new DbCase
            {
                PatientAge = 61,
                Density = 3,
                ImageMax = 147,
                ImageMin = 83,
                ImageMean = 119.183555453431,
                ImageStdDev = 11.2851731248036,
                ImageSkew = -0.670204376467035,
                ImageKurt = 3.28797907773086
            };

            ann.LoadAnnFromFile(@"D:\Facultate\Licenta\Licenta\Licenta_Project\Resources\Network.txt");

            var normalizeInput = ddsm.NormalizeInputItem(newDbCase);
            var result = ann.Test(normalizeInput);
            Console.WriteLine($"Result1 is: {result}");

            var normalizeInput2 = ddsm.NormalizeInputItem(newDbCase2);
            var result2 = ann.Test(normalizeInput2);
            Console.WriteLine($"Result2 is: {result2}");

            var normalizeInput3 = ddsm.NormalizeInputItem(newDbCase3);
            var result3 = ann.Test(normalizeInput3);
            Console.WriteLine($"Result3 is: {result3}");

            var normalizeInput4 = ddsm.NormalizeInputItem(newDbCase4);
            var result4 = ann.Test(normalizeInput4);
            Console.WriteLine($"Result4 is: {result4}");

            var normalizeInput5 = ddsm.NormalizeInputItem(newDbCase5);
            var result5 = ann.Test(normalizeInput5);
            Console.WriteLine($"Result5 is: {result5}");

            var normalizeInput6 = ddsm.NormalizeInputItem(newDbCase6);
            var result6 = ann.Test(normalizeInput6);
            Console.WriteLine($"Result5 is: {result6}");
        }
    }
}