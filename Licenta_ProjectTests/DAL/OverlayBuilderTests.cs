using Microsoft.VisualStudio.TestTools.UnitTesting;
using Licenta_Project.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Licenta_Project.DAL.Tests
{
    [TestClass()]
    public class OverlayBuilderTests
    {
        [TestMethod()]
        public void BuildTotalAbnormalitiesTest()
        {
            var overlayBuilder = new OverlayBuilder();
            overlayBuilder.BuildTotalAbnormalities(3);
            var overlay = overlayBuilder.Overlay;
            Assert.AreEqual(3, overlay.TotalAbnormalities);
        }

        [TestMethod()]
        public void BuildAbnormalitiesTest()
        {
            var overlayBuilder = new OverlayBuilder();

            var abnorm = new Dictionary<string, IEnumerable<string>>()
            {
                {"ABNORMALITY 1", new List<string>()
                {
                    "LESION_TYPE MASS SHAPE OVAL MARGINS MICROLOBULATED",
                    "ASSESSMENT 4",
                    "SUBTLETY 4",
                    "PATHOLOGY BENIGN"
                } },
                {"ABNORMALITY 2", new List<string>()
                {
                    "LESION_TYPE MASS SHAPE OVAL MARGINS MICROLOBULATED",
                    "ASSESSMENT 4",
                    "SUBTLETY 4",
                    "PATHOLOGY BENIGN"
                } }
            };
            overlayBuilder.BuildAbnormalities(abnorm);

            var overlay = overlayBuilder.Overlay;
            Assert.IsTrue(overlay.Abnormalities.Count() == 2);

            var abnormality = overlay.Abnormalities.First();
            Assert.AreEqual(abnormality.Assesment, 4);
            Assert.AreEqual(abnormality.Subtlety, 4);
            Assert.AreEqual(abnormality.Patology, Patology.Benign);
        }
    }
}