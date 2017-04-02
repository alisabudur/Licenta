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

        public void BuildMoreInformation(string moreInformation)
        {
            var result = new Dictionary<string, string>();
            var informationItem = moreInformation
                .Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)
                .ToArray();

            for (var i = 0; i < informationItem.Length; i += 2)
            {
                result.Add(informationItem[i], informationItem[i + 1]);
            }
            
            Abnormality.MoreInformation = result;
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
            Abnormality.Patology = patology;
        }
    }
}
