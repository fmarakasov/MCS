using System;
using System.ComponentModel;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using MCDomain.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MDomain.Common;
namespace MTests
{
    class StubClass:IDataErrorInfo
    {
        public int ReadonlyProperty { get { return 100; } }
        public string ReadWriteProperty { get; set; }
        public string WriteOnlyProperty { set; private get; }
        protected int ProtectedProperty { get; set; }

        public string this[string columnName]
        {
            get
            {
                if (columnName == "ReadonlyProperty") return "Error in ReadonlyProperty";
                if (columnName == "ReadWriteProperty") return "Error in ReadWriteProperty";
                if (columnName == "WriteOnlyProperty") return "Error in WriteOnlyProperty";
                if (columnName == "ProtectedProperty") return "Error in ProtectedProperty";
                return string.Empty;
            }
        }

        public string Error
        {
            get { return string.Empty; }
        }
    }

    /// <summary>
    /// Summary description for DataErrorInfoValidatorTest
    /// </summary>
    [TestClass]
    public class DataErrorInfoValidatorTest
    {
        public DataErrorInfoValidatorTest()
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
        public void ValidateTest()
        {
            var obj = new StubClass();
            string result = obj.Validate();
            var strings = result.Split('\n');
            Assert.IsTrue(strings.Contains("Error in ReadonlyProperty"));
            Assert.IsTrue(strings.Contains("Error in ReadWriteProperty"));
            Assert.IsTrue(strings.Contains("Error in WriteOnlyProperty"));
            Assert.IsFalse(strings.Contains("Error in ProtectedProperty"));

        }
    }
}
