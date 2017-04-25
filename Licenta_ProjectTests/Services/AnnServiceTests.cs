using Microsoft.VisualStudio.TestTools.UnitTesting;
using Licenta_Project.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Licenta_Project.Services.Tests
{
    [TestClass()]
    public class AnnServiceTests
    {
        [TestMethod()]
        public void GetInputTest()
        {
            var annService = new AnnService();
            var input = annService.GetInput();

            Assert.IsNotNull(input);
            Assert.AreEqual(input[0][0], 65);
        }
    }
}