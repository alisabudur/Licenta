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
            var cases = ddsm.GetCases();
            var ddsmCase = cases.FirstOrDefault(c => c.RightCCPath.Contains("C_0001_1"));
            if (ddsmCase == null)
            {
                Assert.Fail();
                return;
            }
            Assert.AreEqual(@"G:\DDSM\cases\cancers\cancer_01\case0001\PNGFiles\C_0001_1.LEFT_CC.png", ddsmCase.LeftCCPath);
            Assert.AreEqual(@"G:\DDSM\cases\cancers\cancer_01\case0001\PNGFiles\C_0001_1.RIGHT_CC.png", ddsmCase.RightCCPath);
            Assert.AreEqual(@"G:\DDSM\cases\cancers\cancer_01\case0001\PNGFiles\C_0001_1.LEFT_MLO.png", ddsmCase.LeftMLOPath);
            Assert.AreEqual(@"G:\DDSM\cases\cancers\cancer_01\case0001\PNGFiles\C_0001_1.RIGHT_MLO.png", ddsmCase.RightMLOPath);

            Assert.AreEqual(65, ddsmCase.PatientAge);
        }
    }
}