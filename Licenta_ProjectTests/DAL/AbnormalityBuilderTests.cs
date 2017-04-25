﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
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
        public void BuildLessionTypeTest()
        {
            var abnormalityBuilder = new AbnormalityBuilder();
            abnormalityBuilder.BuildLessionType("LESION_TYPE MASS SHAPE IRREGULAR MARGINS SPICULATED");
            var abnormality = abnormalityBuilder.Abnormality;

            Assert.AreEqual(abnormality.LessionType, LessionType.Mass);
        }

        [TestMethod()]
        public void BuildMoreInformationTest1()
        {
            var abnormalityBuilder = new AbnormalityBuilder();
            abnormalityBuilder.BuildLessionType("LESION_TYPE CALCIFICATION TYPE PLEOMORPHIC DISTRIBUTION SEGMENTAL");
            var abnormality = abnormalityBuilder.Abnormality;

            Assert.AreEqual(abnormality.LessionType, LessionType.Calcification);
        }
    }
}