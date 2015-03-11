using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using MCDomain.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MTests
{
    [TestClass]
    public class ModelExtensionsTest
    {
        [TestMethod]
        public void GetReservedUndefinedTestOnTable()
        {
            using (var ctx = LinqContractRepositoryTest.CreateLinqContractRepository().Context)
            {
                var actual = ctx.Degrees.GetReservedUndefined();
                Assert.IsNotNull(actual);
                Assert.AreEqual(EntityBase.ReservedUndifinedOid, actual.Id);
            }
        }
        

        
    }
}
