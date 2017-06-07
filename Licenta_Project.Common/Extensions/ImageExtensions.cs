using AForge.Imaging.Filters;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AForge.Imaging;

namespace Licenta_Project.Common.Extensions
{
    public static class ImageExtensions
    {
        public static double GetBlobMaxArea(this Bitmap image)
        {
            var medianFilter = new Median();
            medianFilter.ApplyInPlace(image);

            var conservativeSmoothingFilter = new ConservativeSmoothing();
            conservativeSmoothingFilter.ApplyInPlace(image);

            var grayFilter = Grayscale.CommonAlgorithms.BT709;
            image = grayFilter.Apply(image);

            var contrastCorrectionFilter = new ContrastCorrection(70);
            contrastCorrectionFilter.ApplyInPlace(image);

            var thresholdFilter = new Threshold(185);
            thresholdFilter.ApplyInPlace(image);

            var conectedComponentsFilter = new ConnectedComponentsLabeling();
            image = conectedComponentsFilter.Apply(image);

            var bc = new BlobCounter
            {
                BlobsFilter = new BlobFilter(),
                FilterBlobs = true
            };
            bc.ProcessImage(image);
            var blobs = bc.GetObjectsInformation();

            if (blobs.Length < 1)
                return 0;

            var maxArea = blobs.Max(r => r.Area);
            return maxArea;
        }
    }
}
