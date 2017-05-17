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
    public class BaseEntityRepositoryTests
    {
        private IBaseEntityRepository<Input> _inputRepository;

        public BaseEntityRepositoryTests()
        {
            var context = new DdsmContext();
            _inputRepository = new BaseEntityRepository<Input>(context);
        }

        [TestMethod()]
        public void GetAllTest()
        {
            var inputs = _inputRepository.GetAll().ToList();
            Assert.IsNotNull(inputs);
            Assert.IsTrue(inputs.Any());
        }
    }
}