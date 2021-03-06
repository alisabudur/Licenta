﻿using System;
using System.Collections.Generic;
using System.Drawing.Design;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Licenta_Project.Common;

namespace Licenta_Project.Utility
{
    public class OverlayBuilder
    {
        public Overlay Overlay { get; }
        public OverlayBuilder()
        {
            Overlay = new Overlay();
        }

        public void BuildTotalAbnormalities(int totalAbnormalities)
        {
            Overlay.TotalAbnormalities = totalAbnormalities;
        }

        public void BuildAbnormalities(IDictionary<string, IEnumerable<string>> abnormalities)
        {
            var overlayAbnormalities = new List<Abnormality>();

            foreach (var abnormality in abnormalities.Keys)
            {
                var abnormalityBuilder = new AbnormalityBuilder();

                var abnormalityInformation = abnormalities[abnormality];

                //TODO moreInformation null - why?
                var information = abnormalityInformation as string[] ?? abnormalityInformation.ToArray();

                var moreInformation = information.FirstOrDefault(i => i.Contains("LESION_TYPE"));
                if (moreInformation != null)
                {
                    abnormalityBuilder.BuildLessionType(moreInformation);
                    abnormalityBuilder.BuildShape(moreInformation);
                    abnormalityBuilder.BuildMargins(moreInformation);
                }

                var assesment = GetAssesment(information);
                abnormalityBuilder.BuildAssesment(assesment);

                var subtlety = GetSubtlety(information);
                abnormalityBuilder.BuildSubtlety(subtlety);

                var patology = GetPatology(information);
                abnormalityBuilder.BuildPatology(patology);

                var totalOutlines = GetTotalOutlines(information);
                abnormalityBuilder.BuildTotalOutlines(totalOutlines);

                var boundary = GetBoundary(information);
                abnormalityBuilder.BuildBoundary(boundary);

                var cores = GetCores(information, totalOutlines);
                abnormalityBuilder.BuildCores(cores);

                var overlayAbnormality = abnormalityBuilder.Abnormality;
                overlayAbnormalities.Add(overlayAbnormality);
            }

            Overlay.Abnormalities = overlayAbnormalities;
        }

        private int GetAssesment(IEnumerable<string> abnormalityInformation)
        {
            var firstOrDefault = abnormalityInformation.FirstOrDefault(i => i.Contains("ASSESSMENT"));
            if (firstOrDefault != null)
            {
                var assesment = firstOrDefault
                    .Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)
                    .GetValue(1)
                    .ToString()
                    .ToInt();
                return assesment;
            }
            return 0;
        }

        private int GetSubtlety(IEnumerable<string> abnormalityInformation)
        {
            var firstOrDefault = abnormalityInformation.FirstOrDefault(i => i.Contains("SUBTLETY"));
            if (firstOrDefault != null)
            {
                var subtlety = firstOrDefault
                    .Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)
                    .GetValue(1)
                    .ToString()
                    .ToInt();
                return subtlety;
            }
            return 0;
        }

        private string GetPatology(IEnumerable<string> abnormalityInformation)
        {
            var firstOrDefault = abnormalityInformation.FirstOrDefault(i => i.Contains("PATHOLOGY"));
            if (firstOrDefault != null)
            {
                var patology = firstOrDefault
                    .Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)
                    .GetValue(1)
                    .ToString();
                return patology;
            }
            return string.Empty;
        }

        private int GetTotalOutlines(IEnumerable<string> abnormalityInformation)
        {
            var firstOrDefault = abnormalityInformation.FirstOrDefault(i => i.Contains("TOTAL_OUTLINES"));
            if (firstOrDefault != null)
            {
                var patology = firstOrDefault
                    .Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)
                    .GetValue(1)
                    .ToString()
                    .ToInt();
                return patology;
            }
            return 0;
        }

        private string GetBoundary(IEnumerable<string> abnormalityInformation)
        {
            var firstOrDefault = abnormalityInformation.SkipWhile(i => i != "BOUNDARY")
                .Skip(1)
                .Take(1)
                .FirstOrDefault();

            return firstOrDefault;
        }

        private IEnumerable<string> GetCores(IEnumerable<string> abnormalityInformation, int totalOutlines)
        {
            if (totalOutlines <= 1)
                return new List<string>();

            var cores = new List<string>();

            for (var i = 0; i < totalOutlines; i++)
            {
                var core = abnormalityInformation.SkipWhile(x => x != "BOUNDARY")
                .Skip(1 + 2 * i)
                .Take(1)
                .FirstOrDefault();
                cores.Add(core);
            }

            return cores;
        }
    }
}
