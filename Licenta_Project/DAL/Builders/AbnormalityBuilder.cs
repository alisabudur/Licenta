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

        public void BuildLessionType(string lessionType)
        {
            var lession = lessionType
                .Split(new char[] {' '}, StringSplitOptions.RemoveEmptyEntries)
                .ToArray()
                .GetValue(1)
                .ToString();

            switch (lession)
            {
                case Constants.MASS:
                    Abnormality.LessionType = LessionType.Mass;
                    break;
                case Constants.CALCIFICATION:
                    Abnormality.LessionType = LessionType.Calcification;
                    break;
                default:
                    Abnormality.LessionType = LessionType.Undefined;
                    break;
            }
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
