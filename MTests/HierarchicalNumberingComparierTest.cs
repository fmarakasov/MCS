using MCDomain.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace MTests
{
    
    
    /// <summary>
    ///This is a test class for HierarchicalNumberingComparierTest and is intended
    ///to contain all HierarchicalNumberingComparierTest Unit Tests
    ///</summary>
    [TestClass()]
    public class HierarchicalNumberingComparierTest
    {


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



        [TestMethod()]
        public void CompareEmptyStringTest()
        {
            var obj = new HierarchicalNumberingComparier();

            var actual = obj.Compare(string.Empty, string.Empty);

            Assert.AreEqual(0, actual);
        }

        [TestMethod()]
        public void CompareSimpleEqualsStringTest()
        {
            var obj = new HierarchicalNumberingComparier();

            var actual = obj.Compare("1", "1");

            Assert.AreEqual(0, actual);
        }

        [TestMethod()]
        public void CompareSimple2GStringTest()
        {
            var obj = new HierarchicalNumberingComparier();

            var actual = obj.Compare("1", "2");

            Assert.AreEqual(1, actual);
        }

        [TestMethod()]
        public void CompareSimple2LStringTest()
        {
            var obj = new HierarchicalNumberingComparier();

            var actual = obj.Compare("2", "1");

            Assert.AreEqual(-1, actual);
        }


        [TestMethod()]
        public void CompareComplexEqualsTest()
        {
            var obj = new HierarchicalNumberingComparier();

            var actual = obj.Compare("1.2.3.4.5.6.7", "1.2.3.4.5.6.7");

            Assert.AreEqual(0, actual);
        }


        [TestMethod()]
        public void CompareComplex2GEqualsTest01()
        {
            var obj = new HierarchicalNumberingComparier();

            var actual = obj.Compare("1.2.3.4.5.6.7", "2.2.3.4.5.6.7");

            Assert.AreEqual(1, actual);
        }

        [TestMethod()]
        public void CompareComplex2GEqualsTest02()
        {
            var obj = new HierarchicalNumberingComparier();

            var actual = obj.Compare("1.2.3.4.5.6.7", "1.2.3.4.5.6.8");

            Assert.AreEqual(1, actual);
        }

        [TestMethod()]
        public void CompareComplex2LEqualsTest01()
        {
            var obj = new HierarchicalNumberingComparier();

            var actual = obj.Compare("2.2.3.4.5.6.7", "1.2.3.4.5.6.8");

            Assert.AreEqual(-1, actual);
        }

        [TestMethod()]
        public void CompareComplex2LEqualsTest02()
        {
            var obj = new HierarchicalNumberingComparier();

            var actual = obj.Compare("1.2.3.4.5.6.8", "1.2.3.4.5.6.7");

            Assert.AreEqual(-1, actual);
        }


        /// <summary>
        ///A test for Compare
        ///</summary>
        [TestMethod()]
        public void MatchesEmptyStringTest()
        {
            var obj = new HierarchicalNumberingComparier();

            var actual = obj.Matches(string.Empty);

            Assert.AreEqual(0, actual.Length);
        }

        [TestMethod()]
        public void MatchesNonDigitStringTest()
        {
            var obj = new HierarchicalNumberingComparier();

            var actual = obj.Matches("dsfadfasd .sdf .adasdasd /adsfadf / sdfdf\\ sadsdf@#$%");

            Assert.AreEqual(0, actual.Length);
        }

        [TestMethod()]
        public void MatchesOneDigitStringTest()
        {
            var obj = new HierarchicalNumberingComparier();

            var actual = obj.Matches("1");

            Assert.AreEqual(1, actual.Length);
            Assert.AreEqual("1", actual[0]);
        }


        [TestMethod()]
        public void MatchesNumberStringTest()
        {
            var obj = new HierarchicalNumberingComparier();

            var actual = obj.Matches("1234567890");

            Assert.AreEqual(1, actual.Length);
            Assert.AreEqual("1234567890", actual[0]);
        }


        [TestMethod()]
        public void MatchesNumberAndSpacesStringTest()
        {
            var obj = new HierarchicalNumberingComparier();

            var actual = obj.Matches("   1234567890   ");

            Assert.AreEqual(1, actual.Length);
            Assert.AreEqual("1234567890", actual[0]);
        }


        [TestMethod()]
        public void MatchesHierarchicalNumberStringTest()
        {
            var obj = new HierarchicalNumberingComparier();

            var actual = obj.Matches("9876 57789  1234567890   ");

            Assert.AreEqual(3, actual.Length);
            Assert.AreEqual("9876", actual[0]);
            Assert.AreEqual("57789", actual[1]);
            Assert.AreEqual("1234567890", actual[2]);
        }

        [TestMethod()]
        public void MatchesNormalStringTest()
        {
            var obj = new HierarchicalNumberingComparier();

            var actual = obj.Matches("1.2.3.4.5");

            Assert.AreEqual(5, actual.Length);
            Assert.AreEqual("1", actual[0]);
            Assert.AreEqual("2", actual[1]);
            Assert.AreEqual("3", actual[2]);
            Assert.AreEqual("4", actual[3]);
            Assert.AreEqual("5", actual[4]);
        }
        [TestMethod()]
        public void MatchesComplexStringTest()
        {
            var obj = new HierarchicalNumberingComparier();

            var actual = obj.Matches("  1.2/3.4//.5//");

            Assert.AreEqual(5, actual.Length);
            Assert.AreEqual("1", actual[0]);
            Assert.AreEqual("2", actual[1]);
            Assert.AreEqual("3", actual[2]);
            Assert.AreEqual("4", actual[3]);
            Assert.AreEqual("5", actual[4]);
        }
    }
}
