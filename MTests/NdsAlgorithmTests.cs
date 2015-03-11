using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using MCDomain.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MTests
{
    [TestClass]
    public class NdsAlgorithmTests
    {
        private static void TestAlgorithm(double Id, Ndsalgorithm obj)
        {
            Assert.IsNotNull(obj);
            Assert.AreEqual(Id, obj.Id);
            Assert.AreEqual(Ndsalgorithm.TypeIdToObject(Id).NdsType, obj.NdsType);
            
 
        }
        [TestMethod]
        public void StaticNdsAlgorithmsTest()
        {
            TestAlgorithm(1, Ndsalgorithm.IncludeNdsAlgorithm);
            TestAlgorithm(2, Ndsalgorithm.ExcludeNdsAlgorithm);
            TestAlgorithm(3, Ndsalgorithm.NoNdsAlgorithm);
        }
    }
}
