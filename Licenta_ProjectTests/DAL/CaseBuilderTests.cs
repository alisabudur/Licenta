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
    public class CaseBuilderTests
    {
        [TestMethod()]
        public void BuildImagesTest()
        {
            var caseBuilder = new CaseBuilder();
            var list = new List<string>()
            {
                @"G:\DDSM\cases\cancers\cancer_01\case0001\PNGFiles\C_0001_1.LEFT_CC.png",
                @"G:\DDSM\cases\cancers\cancer_01\case0001\PNGFiles\C_0001_1.RIGHT_CC.png",
                @"G:\DDSM\cases\cancers\cancer_01\case0001\PNGFiles\C_0001_1.LEFT_MLO.png",
                @"G:\DDSM\cases\cancers\cancer_01\case0001\PNGFiles\C_0001_1.RIGHT_MLO.png"
            };
            caseBuilder.BuildImages(list);
            var ddsmCase = caseBuilder.Case;
            Assert.AreEqual(@"G:\DDSM\cases\cancers\cancer_01\case0001\PNGFiles\C_0001_1.LEFT_CC.png", ddsmCase.LeftCCPath);
            Assert.AreEqual(@"G:\DDSM\cases\cancers\cancer_01\case0001\PNGFiles\C_0001_1.RIGHT_CC.png", ddsmCase.RightCCPath);
            Assert.AreEqual(@"G:\DDSM\cases\cancers\cancer_01\case0001\PNGFiles\C_0001_1.LEFT_MLO.png", ddsmCase.LeftMLOPath);
            Assert.AreEqual(@"G:\DDSM\cases\cancers\cancer_01\case0001\PNGFiles\C_0001_1.RIGHT_MLO.png", ddsmCase.RightMLOPath);
        }

        [TestMethod()]
        public void BuildPatientAgeTest()
        {
            var caseBuilder = new CaseBuilder();
            caseBuilder.BuildPatientAge(43);
            var ddssmCase = caseBuilder.Case;
            Assert.AreEqual(43, ddssmCase.PatientAge);
        }
    }
}