using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AForge.Imaging;
using System.Data.Entity;
using AForge.Imaging.Filters;
using Licenta_Project.DAL;
using Licenta_Project.Common;
using Licenta_Project.Common.Extensions;
using Licenta_Project.Utilities;

namespace Licenta_Project.Utility
{
    [LogAspect]
    public class DdsmFileUtility: IDdsmFileUtility
    {
        private readonly string _baseDirectoryPath;
        private readonly IEnumerable<string> _workingDirectoriesPaths;
        

        public IEnumerable<Case> Cases { get; private set; }

        public DdsmFileUtility()
        {
            _baseDirectoryPath = ConfigurationManager.AppSettings["BaseDirectory"];
            _workingDirectoriesPaths = ConfigurationManager.AppSettings["WorkingDirectories"].Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
        }

        public void LoadCasesFromFiles()
        {
            var cases = new List<Case>();
            foreach (var workingDirectoryPath in _workingDirectoriesPaths)
            {
                var ddsmCases = GetFromWorkingDirectory(Path.Combine(_baseDirectoryPath, workingDirectoryPath));
                cases.AddRange(ddsmCases);
            }
            Cases = cases;
        }

        private IEnumerable<Case> GetFromWorkingDirectory(string workingDirectoryPath)
        {
            var cases = new List<Case>();
            var caseDirectories = Directory.GetDirectories(workingDirectoryPath);
            foreach (var caseDirectory in caseDirectories)
            {
                try
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
                catch (Exception ex)
                {
                    Console.WriteLine($"Error {caseDirectory}");
                }
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
