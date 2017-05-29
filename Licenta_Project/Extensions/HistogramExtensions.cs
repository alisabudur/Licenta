using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AForge.Math;

namespace Licenta_Project.Extensions
{
    public static class HistogramExtensions
    {
        public static double Skew(this Histogram histogram)
        {
            double skew = 0;

            for (var color = 0; color < 256; color++)
            {
                var value = Math.Pow(((color - histogram.Mean) / histogram.StdDev), 3);
                var probability = (double)histogram.Values[color] / histogram.TotalCount;
                skew += value * probability;
            }

            return skew;
        }

        public static double Kurt(this Histogram histogram)
        {
            double kurt = 0;

            for (var color = 0; color < 256; color++)
            {
                var value = Math.Pow(((color - histogram.Mean) / histogram.StdDev), 4);
                var probability = (double)histogram.Values[color] / histogram.TotalCount;
                kurt += value * probability;
            }

            return kurt;
        }
    }
}
