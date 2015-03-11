using System;
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

        private void WriteLine(string[] array)
        {
            foreach (var s in array)
            {
                Console.WriteLine("{0}, ", s);
                
            }
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
        ///A test for Compare
        ///</summary>
        [TestMethod()]
        public void MatchesEmptyStringTest()
        {
            var obj = new HierarchicalNumberingComparier();

            var actual = obj.Matches(string.Empty);
            WriteLine(actual);
            Assert.AreEqual(0, actual.Length);
        }

        [TestMethod()]
        public void CompareEmptyXYTest()
        {
            var obj = new HierarchicalNumberingComparier();

            var actual = obj.Compare(string.Empty, string.Empty);

            Assert.AreEqual(0, actual);
        }
        [TestMethod()]
        public void CompareEmptyXNonEmptyYTest()
        {
            var obj = new HierarchicalNumberingComparier();

            var actual = obj.Compare(string.Empty, "1");

            Assert.AreEqual(1, actual);
        }
        [TestMethod()]
        public void CompareNonEmptyXEmptyYTest()
        {
            var obj = new HierarchicalNumberingComparier();

            var actual = obj.Compare("1", string.Empty);

            Assert.AreEqual(-1, actual);
        }

        [TestMethod()]
        public void CompareSimpleEqualsTest()
        {
            var obj = new HierarchicalNumberingComparier();

            var actual = obj.Compare("1", "1");

            Assert.AreEqual(0, actual);
        }

        [TestMethod()]
        public void CompareNonSimpleEqualsTest()
        {
            var obj = new HierarchicalNumberingComparier();

            var actual = obj.Compare("1.2.3.4", "1.2.3.4");

            Assert.AreEqual(0, actual);
        }

        [TestMethod()]
        public void CompareNonSimpleNonEquals01Test()
        {
            var obj = new HierarchicalNumberingComparier();

            var actual = obj.Compare("2.2.3.4", "1.2.3.4");

            Assert.AreEqual(-1, actual);
        }

        [TestMethod()]
        public void CompareNonSimpleNonEquals02Test()
        {
            var obj = new HierarchicalNumberingComparier();

            var actual = obj.Compare("1.2.3.4", "1.2.3.5");

            Assert.AreEqual(1, actual);
        }

        [TestMethod()]
        public void CompareNonSimpleNonEquals03Test()
        {
            var obj = new HierarchicalNumberingComparier();

            var actual = obj.Compare("1.2.3.5", "1.2.3.4");

            Assert.AreEqual(-1, actual);
        }

        [TestMethod()]
        public void CompareNonSimpleNonEquals04Test()
        {
            var obj = new HierarchicalNumberingComparier();

            var actual = obj.Compare("1.2.3.4", "2.2.3.4");

            Assert.AreEqual(1, actual);
        }

        [TestMethod()]
        public void CompareNonSimpleNonEquals05Test()
        {
            var obj = new HierarchicalNumberingComparier();

            var actual = obj.Compare("1.9.9.9", "2");

            Assert.AreEqual(1, actual);
        }

        [TestMethod()]
        public void CompareNonSimpleNonEquals06Test()
        {
            var obj = new HierarchicalNumberingComparier();

            var actual = obj.Compare("2", "1.9.9.9.9");

            Assert.AreEqual(-1, actual);
        }
     
        [TestMethod()]
        public void DigitsEmptyStringTest()
        {
            var obj = new HierarchicalNumberingComparier();

            var actual = obj.Digits(obj.Matches(string.Empty));
            
            Assert.AreEqual(0, actual.Length);
        }
        [TestMethod()]
        public void MatchesNonDigitStringTest()
        {
            var obj = new HierarchicalNumberingComparier();

            var actual = obj.Matches("dsfadfasd .sdf .adasdasd /adsfadf / sdfdf\\ sadsdf@#$%");
            WriteLine(actual);
            Assert.AreEqual(0, actual.Length);
        }

        [TestMethod()]
        public void MatchesOneDigitStringTest()
        {
            var obj = new HierarchicalNumberingComparier();
            
            var actual = obj.Matches("1");
            WriteLine(actual);
            Assert.AreEqual(1, actual.Length);
            Assert.AreEqual("1", actual[0]);
        }


        [TestMethod()]
        public void MatchesNumberStringTest()
        {
            var obj = new HierarchicalNumberingComparier();

            var actual = obj.Matches("1234567890");
            WriteLine(actual);
            Assert.AreEqual(1, actual.Length);
            Assert.AreEqual("1234567890", actual[0]);
        }


        [TestMethod()]
        public void MatchesNumberAndSpacesStringTest()
        {
            var obj = new HierarchicalNumberingComparier();

            var actual = obj.Matches("   1234567890   ");
            WriteLine(actual);
            Assert.AreEqual(1, actual.Length);
            Assert.AreEqual("1234567890", actual[0]);
        }


        [TestMethod()]
        public void MatchesHierarchicalNumberStringTest()
        {
            var obj = new HierarchicalNumberingComparier();

            var actual = obj.Matches("9876 57789  1234567890   ");
            WriteLine(actual);
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
            WriteLine(actual);
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
            WriteLine(actual);
            Assert.AreEqual(5, actual.Length);
            Assert.AreEqual("1", actual[0]);
            Assert.AreEqual("2", actual[1]);
            Assert.AreEqual("3", actual[2]);
            Assert.AreEqual("4", actual[3]);
            Assert.AreEqual("5", actual[4]);
        }
    }
}
