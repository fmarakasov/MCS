using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MContracts.Classes;

namespace MTests
{
   /* [TestClass]
    public class ReflectionExtTest
    {
        [TestMethod]
        public void GetPropertyNameTest()
        {
            var obj = new StubNotEmptyReportViewModel(new StubContractRepositoryUnimplemented());
            obj.IntProperty = 100;
            var actual = obj.GetPropertyValue<int>("IntProperty");
            Assert.AreEqual(100, actual);

        }
        [TestMethod]
        public void SetPropertyNameTest()
        {
            var obj = new StubNotEmptyReportViewModel(new StubContractRepositoryUnimplemented());
            obj.SetPropertyValue("IntProperty", 100);
            var actual = obj.GetPropertyValue<int>("IntProperty");
            Assert.AreEqual(100, actual);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void GetPropertyNameTestShoulThrowException()
        {
            var obj = new StubNotEmptyReportViewModel(new StubContractRepositoryUnimplemented());
            var actual = obj.GetPropertyValue<int>(null);            
        }
    } */
}
