using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Licenta_Project.Extensions
{
    public static class InputArrayExtensions
    {
        public static double StdDev(this IEnumerable<double> items)
        {
            var mean = items.Average();
            var stdDev = Math.Sqrt(items.Average(p => Math.Pow(p - mean, 2)));
            return stdDev;
        }
    }
}
