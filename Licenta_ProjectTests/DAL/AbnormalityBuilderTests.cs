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
    public class AbnormalityBuilderTests
    {
        [TestMethod()]
        public void BuildMoreInformationTest()
        {
            var abnormalityBuilder = new AbnormalityBuilder();
            abnormalityBuilder.BuildMoreInformation("LESION_TYPE MASS SHAPE IRREGULAR MARGINS SPICULATED");
            var abnormality = abnormalityBuilder.Abnormality;

            Assert.IsTrue(abnormality.MoreInformation.ContainsKey("LESION_TYPE"));
            Assert.IsTrue(abnormality.MoreInformation.ContainsKey("SHAPE"));
            Assert.AreEqual("SPICULATED", abnormality.MoreInformation["MARGINS"]);
        }

        [TestMethod()]
        public void BuildMoreInformationTest1()
        {
            var abnormalityBuilder = new AbnormalityBuilder();
            abnormalityBuilder.BuildMoreInformation("LESION_TYPE CALCIFICATION TYPE PLEOMORPHIC DISTRIBUTION SEGMENTAL");
            var abnormality = abnormalityBuilder.Abnormality;

            Assert.IsTrue(abnormality.MoreInformation.ContainsKey("LESION_TYPE"));
            Assert.IsTrue(abnormality.MoreInformation.ContainsKey("TYPE"));
            Assert.AreEqual("SEGMENTAL", abnormality.MoreInformation["DISTRIBUTION"]);
        }
    }
}