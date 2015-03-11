using MCDomain.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MTests
{
    /// <summary>
    ///This is a test class for BoolExtensionsTest and is intended
    ///to contain all BoolExtensionsTest Unit Tests
    ///</summary>
    [TestClass]
    public class BoolExtensionsTest
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
        ///A test for ToBoolean
        ///</summary>
        [TestMethod]
        public void ToBooleanTest()
        {
            double? value = null;
            Assert.IsNull(value.ToBoolean());
            value = 10;
            Assert.IsTrue(value.Value.ToBoolean());
            value = 0;
            Assert.IsFalse(value.Value.ToBoolean());
        }
    }
}