using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AForge.Imaging;
using Licenta_Project.Extensions;
using System.Data.Entity;
using Licenta_Project.Services;
using Licenta_Project.Aspects;

namespace Licenta_Project.DAL
{
    [LogAspect]
    public class DDSM
    {
        private readonly string _baseDirectoryPath;
        private readonly IEnumerable<string> _workingDirectoriesPaths;
        private readonly DDSMEntities _dbContext;

        public IEnumerable<Case> Cases { get; private set; }

        public DDSM()
        {
            _baseDirectoryPath = ConfigurationManager.AppSettings["BaseDirectory"];
            _workingDirectoriesPaths = ConfigurationManager.AppSettings["WorkingDirectories"].Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
            _dbContext = new DDSMEntities();
        }

        public void GetCases()
        {
            var cases = new List<Case>();
            foreach (var workingDirectoryPath in _workingDirectoriesPaths)
            {
                var ddsmCases = GetFromWorkingDirectory(Path.Combine(_baseDirectoryPath, workingDirectoryPath));
                cases.AddRange(ddsmCases);
            }
            Cases = cases;
        }

        public void PutInputInDb()
        {
            var workCases = Cases.ToList();

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
                        _dbContext.Inputs.Add(input);
                        _dbContext.SaveChanges();
                    }
                }
            }
            
        }

        public void PutOutputInDb()
        {
            var workCases = Cases.ToList();

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
                    _dbContext.Outputs.Add(output);
                    _dbContext.SaveChanges();
                }
            }
        }

        public double[][] GetInputFromDb()
        {
            var index = 0;
            var result = new double[_dbContext.Inputs.Count()][];

            foreach (var input in _dbContext.Inputs)
            {
                var resultItem = new double[6];
                resultItem[0] = input.PatientAge;
                resultItem[1] = input.Density;
                resultItem[2] = input.ImageMean;
                resultItem[3] = input.ImageStdDev;
                resultItem[4] = input.ImageSkew;
                resultItem[5] = input.ImageKurt;
                result[index] = resultItem;
                index++;
            }
            return result;
        }

        public double[][] GetOutputFromDb()
        {
            var index = 0;
            var result = new double[_dbContext.Inputs.Count()][];

            foreach (var output in _dbContext.Outputs)
            {
                var resultItem = new double[1];
                resultItem[0] = output.Patology;
                result[index] = resultItem;
                index++;
            }
            return result;
        }

        private IEnumerable<Case> GetFromWorkingDirectory(string workingDirectoryPath)
        {
            var cases = new List<Case>();
            var caseDirectories = Directory.GetDirectories(workingDirectoryPath);
            foreach (var caseDirectory in caseDirectories)
            {
                var caseBuilder = new CaseBuilder();
                var caseDirectoryPath = Path.Combine(_baseDirectoryPath, caseDirectory);

                var pngFiles = GetPNGFiles(caseDirectoryPath);
                caseBuilder.BuildImages(pngFiles);

                var age = GetPatientAge(caseDirectoryPath);
                caseBuilder.BuildPatientAge(age);

                var density = GetDensity(caseDirectoryPath);
                caseBuilder.BuildDensity(density);

                var overlayFiles = GetOverlayFiles(caseDirectoryPath);
                caseBuilder.BuildOverlies(overlayFiles);

                cases.Add(caseBuilder.Case);
            }
            return cases;
        }

        private IEnumerable<string> GetPNGFiles(string caseDirectory)
        {
            var pngFiles = Directory.GetFiles($"{caseDirectory}\\PNGFiles");
            return pngFiles;
        }

        private int GetPatientAge(string caseDirectory)
        {
            var icsFile = Directory.GetFiles(caseDirectory)
                .FirstOrDefault(f => f.Contains(".ics"));

            var age = File.ReadLines(icsFile)
                .Where(line => line.Contains(Constants.PATIENT_AGE))
                .ToList()
                .First()
                .Split(' ')
                .GetValue(1)
                .ToString()
                .ToInt();
            return age;
        }

        private int GetDensity(string caseDirectory)
        {
            var icsFile = Directory.GetFiles(caseDirectory)
                .FirstOrDefault(f => f.Contains(".ics"));

            var age = File.ReadLines(icsFile)
                .Where(line => line.Contains(Constants.DENSITY))
                .ToList()
                .First()
                .Split(' ')
                .GetValue(1)
                .ToString()
                .ToInt();
            return age;
        }

        private IEnumerable<string> GetOverlayFiles(string caseDirectory)
        {
            var overlayFiles = Directory.GetFiles(caseDirectory)
                .Where(f => f.Contains(".OVERLAY"))
                .ToList();
            return overlayFiles;
        }
    }
}
