using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Licenta_Project.Common;

namespace Licenta_Project.FileUtility
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
            var dictionary = new Dictionary<ImageName, CaseImage>()
            {
                { ImageName.LeftCC, new CaseImage{ImagePath = enumerable.FirstOrDefault(n => n.Contains(Constants.LeftCC))}},
                { ImageName.LeftMLO, new CaseImage{ImagePath = enumerable.FirstOrDefault(n => n.Contains(Constants.LeftMLO))}},
                { ImageName.RightCC, new CaseImage{ImagePath = enumerable.FirstOrDefault(n => n.Contains(Constants.RightCC))}},
                { ImageName.RightMLO, new CaseImage{ImagePath = enumerable.FirstOrDefault(n => n.Contains(Constants.RightMLO))}}
            };
            Case.Images = dictionary;
        }

        public void BuildPatientAge(int age)
        {
            Case.PatientAge = age;
        }

        public void BuildDensity(int density)
        {
            Case.Density = density;
        }

        public void BuildOverlies(IEnumerable<string> overlayFiles)
        {
            var enumerable = overlayFiles as string[] ?? overlayFiles.ToArray();
            foreach (var overlayFile in enumerable)
            {
                var overlayBuilder = new OverlayBuilder();

                var totalAbnormalities = GetTotalAbnormalities(overlayFile);
                overlayBuilder.BuildTotalAbnormalities(totalAbnormalities);

                var abnormalities = GetAbnormalities(overlayFile, totalAbnormalities);
                overlayBuilder.BuildAbnormalities(abnormalities);

                var overlay = overlayBuilder.Overlay;
                AssignOverlayToImage(overlayFile, overlay);
            }
        }

        private void AssignOverlayToImage(string overlayFile, Overlay overlay)
        {
            if (overlayFile.Contains(Constants.LeftCC))
            {
                Case.Images[ImageName.LeftCC].Overlay = overlay;
            }
            else if (overlayFile.Contains(Constants.LeftMLO))
            {
                Case.Images[ImageName.LeftMLO].Overlay = overlay;
            }
            else if (overlayFile.Contains(Constants.RightCC))
            {
                Case.Images[ImageName.RightCC].Overlay = overlay;
            }
            else if(overlayFile.Contains(Constants.RightMLO))
            {
                Case.Images[ImageName.RightMLO].Overlay = overlay;
            }
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
