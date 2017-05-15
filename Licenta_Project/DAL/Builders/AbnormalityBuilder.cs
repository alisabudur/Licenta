using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Licenta_Project.DAL
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
                .GetValue(3)
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
    }
}
