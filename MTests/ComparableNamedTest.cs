using MCDomain.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MTests
{
    internal class NamedStub : INamed
    {
        #region Implementation of INamed

        public double Id { get; set; }

        public string Name { get; set; }

        #endregion
    }

    /// <summary>
    ///This is a test class for ComparableNamedTest and is intended
    ///to contain all ComparableNamedTest Unit Tests
    ///</summary>
    [TestClass]
    public class ComparableNamedTest
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
        ///A test for Compare
        ///</summary>
        public void CompareTestHelper<T>()
            where T : INamed, new()
        {
            var obj = new T();
            var other = new T();

            obj.Name = "A";
            other.Name = "B";

            int expected = -1;
            int actual;
            actual = obj.CompareNames(other);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CompareTest()
        {
            CompareTestHelper<NamedStub>();
        }
    }
}