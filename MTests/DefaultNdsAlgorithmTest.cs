using System;
using MCDomain.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MTests
{
    /// <summary>
    ///This is a test class for DefaultNdsAlgorithmTest and is intended
    ///to contain all DefaultNdsAlgorithmTest Unit Tests
    ///</summary>
    [TestClass]
    public class DefaultNdsAlgorithmTest
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

        private static void TestNdsAlg<T>(decimal price, double fraction, decimal expectedNds, decimal expectedPure)
            where T : DefaultNdsAlgorithm
        {
            var alg = Activator.CreateInstance<T>();
            decimal actualNds = alg.GetNds(price, fraction);
            decimal actualPure = alg.GetPure(price, fraction);
            Assert.AreEqual(expectedNds, actualNds, "Ожидалось другое значение НДС");
            Assert.AreEqual(expectedPure, actualPure, "Ожидалось другое значение суммы без НДС");
        }

        /// <summary>
        ///A test for GetNds
        ///</summary>
        [TestMethod]
        public void GetNoNdsTest()
        {
            TestNdsAlg<DefaultNdsAlgorithm>(100, 0.2, 0, 100);
        }
        
        [TestMethod]
        public void GetExcludeNdsTest()
        {
            TestNdsAlg<NdsExcludedAlgorithm>(100, 0.2, 20, 100);
        }

        [WorkItem(431), TestMethod]
        public void GetIncludeNdsTest()
        {
            const double nds = 0.18F;
            TestNdsAlg<NdsIncludedAlgorithm>(60000M, nds, (decimal)(nds * 60000 / (1 + nds)), (decimal)(60000 / (1 + nds)));
        }
    }
}