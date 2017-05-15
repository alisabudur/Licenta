using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AForge.Imaging;
using AForge.Math;

namespace Licenta_Project.Services
{
    public class StatisticsService
    {
        private readonly Histogram _histogram;

        public StatisticsService(Bitmap image)
        {
            var imageStatistics = new ImageStatistics(image);
            _histogram = imageStatistics.Red;
        }

        public double Mean => _histogram.Mean;

        public double Median => _histogram.Median;

        public double StdDev => _histogram.StdDev;

        public double Skew => ComputeSkew();

        public double Kurt => ComputeKurt();

        #region Private Methods

        private double ComputeSkew()
        {
            double skew = 0;

            for (var color = 0; color < 256; color++)
            {
                var value = Math.Pow(((color - _histogram.Mean) / _histogram.StdDev), 3);
                var probability = (double) _histogram.Values[color] / _histogram.TotalCount;
                skew += value * probability;
            }

            return skew;
        }

        private double ComputeKurt()
        {
            double kurt = 0;

            for (var color = 0; color < 256; color++)
            {
                var value = Math.Pow(((color - _histogram.Mean) / _histogram.StdDev), 4);
                var probability = (double)_histogram.Values[color] / _histogram.TotalCount;
                kurt += value * probability;
            }

            return kurt;
        }

        #endregion
    }
}
