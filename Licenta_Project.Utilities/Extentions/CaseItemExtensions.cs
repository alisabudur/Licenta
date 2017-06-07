using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Licenta_Project.Utility;

namespace Licenta_Project.Utilities
{
    public static class CaseItemExtensions
    {
        public static double GetPatology(this Case caseItem, ImageName imageKey)
        {
            var abnormalities = caseItem.Images[imageKey].Overlay?.Abnormalities?.ToArray();

            if (abnormalities == null)
                return (double)Patology.Normal;

            var malignAbnormailies = abnormalities.Where(a => a.Patology == Patology.Malignant)
                .ToArray()
                .Length;

            if (malignAbnormailies > 0)
                return (double)Patology.Malignant;

            var benignAbnormailies = abnormalities.Where(a => a.Patology == Patology.Benign)
                .ToArray()
                .Length;

            if (benignAbnormailies > 0)
                return (double)Patology.Benign;

            return (double)Patology.Normal;
        }
    }
}
