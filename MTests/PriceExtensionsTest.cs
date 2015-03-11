using System.Collections.Generic;
using MCDomain.Common;
using MCDomain.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MTests
{
    /// <summary>
    ///This is a test class for PriceExtensionsTest and is intended
    ///to contain all PriceExtensionsTest Unit Tests
    ///</summary>
    [TestClass]
    public class PriceExtensionsTest
    {
        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext { get; set; }

        #region Additional test attributes

        // 
        //You can use the following additional attributes as you write your tests:
        //
        //Use ClassInitialize to run code before running the first test in the class
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //Use ClassCleanup to run code after all tests in a class have run
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //Use TestInitialize to run code before running each test
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //

        #endregion

        [TestMethod]
        public void TotalTest()
        {
            IList<Stage> stages = new List<Stage>();
            stages.Add(new Stage {PriceValue = 10});
            stages.Add(new Stage {PriceValue = 10});
            stages.Add(new Stage {PriceValue = 10});
            Assert.AreEqual(30, stages.Total());
        }
    }
}