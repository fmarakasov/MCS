using MCDomain.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MTests
{
    /// <summary>
    ///This is a test class for ContracttypeTest and is intended
    ///to contain all ContracttypeTest Unit Tests
    ///</summary>
    [TestClass]
    public class ContracttypeTest
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
        ///A test for CompareTo
        ///</summary>
        [TestMethod]
        public void ContracttypeCompareToTest()
        {
            var target = new Contracttype {Name = "A"}; 
            var other = new Contracttype {Name = "B"};
            int expected = -1; 
            int actual;
            actual = target.CompareTo(other);
            Assert.AreEqual(expected, actual);
        }
        [TestMethod]
        public void TestWellKnownTypes()
        {
            var target = new Contracttype();
            Assert.AreEqual(WellKnownContractTypes.Undefined, target.WellKnownType);
            target.Id = 1;
            Assert.AreEqual(WellKnownContractTypes.Niokr, target.WellKnownType);
            target.Id = 10;
            Assert.AreEqual(WellKnownContractTypes.Other, target.WellKnownType);
            target.Id = -1;
            Assert.AreEqual(WellKnownContractTypes.Other, target.WellKnownType);
        }
    }
}