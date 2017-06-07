using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Licenta_Project.Common;
using Licenta_Project.Utilities;

namespace Licenta_Project.Utility
{
    public class AbnormalityBuilder
    {
        public Abnormality Abnormality { get; }

        public AbnormalityBuilder()
        {
            Abnormality = new Abnormality();
        }

        public void BuildLessionType(string line)
        {
            var lession = line
                .Split(new char[] {' '}, StringSplitOptions.RemoveEmptyEntries)
                .ToArray()
                .GetValue(1)
                .ToString();

            Abnormality.LessionType = lession;
        }

        public void BuildShape(string line)
        {
            var shape = line
                .Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)
                .ToArray()
                .GetValue(3)
                .ToString();

            Abnormality.Shape = shape;
        }

        public void BuildMargins(string line)
        {
            var margins = line
                .Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)
                .ToArray()
                .GetValue(5)
                .ToString();

            Abnormality.Margins = margins;
        }

        public void BuildAssesment(int assesment)
        {
            Abnormality.Assesment = assesment;
        }

        public void BuildSubtlety(int subtlety)
        {
            Abnormality.Subtlety = subtlety;
        }

        public void BuildPatology(string patology)
        {
            switch (patology)
            {
                case Constants.BENIGN:
                    Abnormality.Patology = Patology.Benign;
                    break;
                case Constants.MALIGNANT:
                    Abnormality.Patology = Patology.Malignant;
                    break;
                default:
                    Abnormality.Patology = Patology.Undefined;
                    break;
            }
        }

        public void BuildTotalOutlines(int totalOutlines)
        {
            Abnormality.TotalOutlines = totalOutlines;
        }

        public void BuildBoundary(string boundaryLine)
        {
            if(string.IsNullOrEmpty(boundaryLine))
                return;

            var outlineBuilder = new OutlineBuilder();
            outlineBuilder.BuildOutline(boundaryLine);
            var outline = outlineBuilder.Outline;

            Abnormality.Boundary = outline;
        }

        public void BuildCores(IEnumerable<string> coreLines)
        {
            var cores = new List<Outline>();

            foreach (var core in coreLines)
            {
                var outlineBuilder = new OutlineBuilder();
                outlineBuilder.BuildOutline(core);
                var outline = outlineBuilder.Outline;
                cores.Add(outline);
            }

            Abnormality.Cores = cores;
        }
    }
}
