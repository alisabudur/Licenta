using Microsoft.VisualStudio.TestTools.UnitTesting;
using Licenta_Project.Services;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Licenta_Project.Services.Tests
{
    [TestClass()]
    public class StatisticsServiceTests
    {
        [TestMethod()]
        public void StatisticsServiceTest()
        {
            var stat = new StatisticsService(new Bitmap(@"G:\DDSM-images\cases\cancers\cancer_03\case1024\PNGFiles\A_1024_1.RIGHT_MLO.png"));
            Assert.IsNotNull(stat.Skew);
            Assert.IsNotNull(stat.Kurt);
        }
    }
}