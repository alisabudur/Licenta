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
    public class DDSMTests
    {
        [TestMethod()]
        public void GetCasesTest()
        {
            var ddsm = new DDSM();
            ddsm.GetCases();
            var cases = ddsm.Cases;
            var ddsmCase = cases.FirstOrDefault(c => c.Images[ImageName.LeftCC].ImagePath.Contains("C_0001_1"));
            if (ddsmCase == null)
            {
                Assert.Fail();
                return;
            }
            Assert.AreEqual(@"G:\DDSM-images\cases\cancers\cancer_01\case0001\PNGFiles\C_0001_1.LEFT_CC.png", ddsmCase.Images[ImageName.LeftCC].ImagePath);
            Assert.AreEqual(@"G:\DDSM-images\cases\cancers\cancer_01\case0001\PNGFiles\C_0001_1.RIGHT_CC.png", ddsmCase.Images[ImageName.RightCC].ImagePath);
            Assert.AreEqual(@"G:\DDSM-images\cases\cancers\cancer_01\case0001\PNGFiles\C_0001_1.LEFT_MLO.png", ddsmCase.Images[ImageName.LeftMLO].ImagePath);
            Assert.AreEqual(@"G:\DDSM-images\cases\cancers\cancer_01\case0001\PNGFiles\C_0001_1.RIGHT_MLO.png", ddsmCase.Images[ImageName.RightMLO].ImagePath);

            Assert.AreEqual(65, ddsmCase.PatientAge);
            Assert.AreEqual(2, ddsmCase.Density);
        }
    }
}