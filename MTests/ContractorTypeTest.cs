using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using MCDomain.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MTests
{
    /// <summary>
    /// Summary description for ContractorTypeTest
    /// </summary>
    [TestClass]
    public class ContractorTypeTest
    {
        public ContractorTypeTest()
        {

        }

        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        [TestMethod]
        public void WellKnownContractorTypeTest()
        {
            Contractortype target0 = new Contractortype(){Id=0};
            Contractortype target1 = new Contractortype() { Id = 1 };
            Contractortype target2 = new Contractortype() { Id = 2 };
            Contractortype target3 = new Contractortype() { Id = 3 };
            Contractortype target1000 = new Contractortype() { Id = 1000 };

            Assert.AreEqual(WellKnownContractorTypes.Undefined, target0.WellKnownType);
            Assert.AreEqual(WellKnownContractorTypes.Gazprom, target1.WellKnownType);
            Assert.AreEqual(WellKnownContractorTypes.Subsidiary, target2.WellKnownType);
            Assert.AreEqual(WellKnownContractorTypes.Other, target3.WellKnownType);
            Assert.AreEqual(WellKnownContractorTypes.Other, target1000.WellKnownType);
        }
    }
}
