using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using MCDomain.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MTests
{
    /// <summary>
    /// Summary description for PercentTests
    /// </summary>
    [TestClass]
    public class PercentTests
    {
        public PercentTests()
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
        public void TestGetPercent1of100()
        {
            Assert.AreEqual(1, Percent.GetPercent(1, 100));
        }

        [TestMethod]
        public void TestGetPercent0of100()
        {
            Assert.AreEqual(0, Percent.GetPercent(0, 100));
        }

        [TestMethod]
        public void TestGetPercent100of100()
        {
            Assert.AreEqual(100, Percent.GetPercent(100, 100));
        }

        [TestMethod]
        public void TestInverse100of1000()
        {
            Assert.AreEqual(1000, Percent.Inverse(100, 1000));
        }

        [TestMethod]
        public void TestInverse0of1000()
        {
            Assert.AreEqual(0, Percent.Inverse(0, 1000));
        }

        [TestMethod]
        public void TestInverse10of1000()
        {
            Assert.AreEqual(100, Percent.Inverse(10, 1000));
        }

        

        
    }
}

