using MCDomain.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MTests
{
    /// <summary>
    ///This is a test class for NdsTest and is intended
    ///to contain all NdsTest Unit Tests
    ///</summary>
    [TestClass]
    public class NdsTest
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

        /// <summary>
        ///A test for Fraction
        ///</summary>
        [TestMethod]
        public void FractionTest()
        {
            var nds = new Nds();
            nds.Percents = 50;
            Assert.AreEqual(0.5, nds.Fraction);
            nds.Fraction = 0.2;
            Assert.AreEqual(20, nds.Percents);
        }

        private int _hits = 0;

        [TestMethod]
        public void FractionChangedTest()
        {
            var nds = new Nds();
            nds.PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler(nds_PropertyChanged);
            nds.Percents = 10;
            Assert.AreEqual(1, _hits);
        }

        void nds_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Fraction")
                _hits++;
        }
    }
}