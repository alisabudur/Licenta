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
        public void BuildImageNameTest()
        {
            var overlayBuilder = new OverlayBuilder();
            overlayBuilder.BuildImageName(@"G:\DDSM\cases\cancers\cancer_01\case0001\C_0001_1.RIGHT_CC.OVERLAY");
            var overlay = overlayBuilder.Overlay;
            Assert.AreEqual("RIGHT_CC", overlay.ImageName);
        }

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

            Assert.IsTrue(abnormality.MoreInformation.ContainsKey("LESION_TYPE"));
            Assert.IsTrue(abnormality.MoreInformation.ContainsKey("SHAPE"));
            Assert.AreEqual("MICROLOBULATED", abnormality.MoreInformation["MARGINS"]);
            Assert.AreEqual(abnormality.Assesment, 4);
            Assert.AreEqual(abnormality.Subtlety, 4);
            Assert.AreEqual(abnormality.Patology, "BENIGN");
        }
    }
}