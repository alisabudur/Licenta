using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Licenta_Project.Extensions;

namespace Licenta_Project.DAL
{
    public class CaseBuilder
    {
        public Case Case { get; }

        public CaseBuilder()
        {
            Case = new Case();
        }

        public void BuildImages(IEnumerable<string> imagesPaths)
        {
            var enumerable = imagesPaths as string[] ?? imagesPaths.ToArray();

            Case.LeftCCPath = enumerable.FirstOrDefault(n => n.Contains(Constants.LeftCC));
            Case.LeftMLOPath = enumerable.FirstOrDefault(n => n.Contains(Constants.LeftMLO));
            Case.RightCCPath = enumerable.FirstOrDefault(n => n.Contains(Constants.RightCC));
            Case.RightMLOPath = enumerable.FirstOrDefault(n => n.Contains(Constants.RightMLO));
        }

        public void BuildPatientAge(int age)
        {
            Case.PatientAge = age;
        }

        public void BuildOverlies(IEnumerable<string> overlayFiles)
        {
            var enumerable = overlayFiles as string[] ?? overlayFiles.ToArray();
            var overlies = new List<Overlay>();
            foreach (var overlayFile in enumerable)
            {
                var overlayBuilder = new OverlayBuilder();
                overlayBuilder.BuildImageName(overlayFile);

                var totalAbnormalities = GetTotalAbnormalities(overlayFile);
                overlayBuilder.BuildTotalAbnormalities(totalAbnormalities);

                var abnormalities = GetAbnormalities(overlayFile, totalAbnormalities);
                overlayBuilder.BuildAbnormalities(abnormalities);

                var overlay = overlayBuilder.Overlay;
                overlies.Add(overlay);
            }
            Case.Overlays = overlies;
        }

        private int GetTotalAbnormalities(string overlayFileName)
        {
            var totalAbnormalities = File.ReadLines(overlayFileName)
                .Where(line => line.Contains(Constants.TOTAL_ABNORMALITIES))
                .ToList()
                .First()
                .Split(' ')
                .GetValue(1)
                .ToString()
                .ToInt();
            return totalAbnormalities;
        }

        private IDictionary<string, IEnumerable<string>> GetAbnormalities(string overlayFileName, int totalAbnormalities)
        {
            var result = new Dictionary<string, IEnumerable<string>>();
            for (var i = 1; i <= totalAbnormalities; i++)
            {
                var abnormality = $"{Constants.ABNORMALITY} {i}";
                var abnormalityInformation = File.ReadLines(overlayFileName)
                    .SkipWhile(line => line != abnormality)
                    .Skip(1)
                    .Take(4)
                    .ToArray();
                result.Add(abnormality, abnormalityInformation);
            }
            return result;
        }
    }
}
