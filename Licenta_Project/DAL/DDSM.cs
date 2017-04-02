using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Licenta_Project.Extensions;

namespace Licenta_Project.DAL
{
    public class DDSM
    {
        private readonly string _baseDirectoryPath;
        private readonly IEnumerable<string> _workingDirectoriesPaths;

        public DDSM()
        {
            _baseDirectoryPath = ConfigurationManager.AppSettings["BaseDirectory"];
            _workingDirectoriesPaths = ConfigurationManager.AppSettings["WorkingDirectories"].Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
        }

        public IEnumerable<Case> GetCases()
        {
            var cases = new List<Case>();
            foreach (var workingDirectoryPath in _workingDirectoriesPaths)
            {
                var ddsmCases = GetFromWorkingDirectory(Path.Combine(_baseDirectoryPath, workingDirectoryPath));
                cases.AddRange(ddsmCases);
            }
            return cases;
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

        private IEnumerable<string> GetOverlayFiles(string caseDirectory)
        {
            var overlayFiles = Directory.GetFiles(caseDirectory)
                .Where(f => f.Contains(".OVERLAY"))
                .ToList();
            return overlayFiles;
        }
    }
}
