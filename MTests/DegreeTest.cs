using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using MCDomain.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MTests
{
    /// <summary>
    /// Summary description for DegreeTest
    /// </summary>
    [TestClass]
    public class DegreeTest
    {
        public DegreeTest()
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
        public void ConvertToShortTest()
        {
            Assert.AreEqual("Д.т.н.",Degree.ConvertToShort("Доктор технических наук"));
            Assert.AreEqual(string.Empty, Degree.ConvertToShort(""));
        }
        [TestMethod]
        public void DegreeAsEditableObjectTest()
        {
            var wrapped = new Degree();

            wrapped.BeginEdit();
            wrapped.Name = "No degree";
            wrapped.EndEdit();
            Assert.AreEqual(wrapped.Name, "No degree");
            
            wrapped.BeginEdit();
            wrapped.Name = "Yes";
            wrapped.CancelEdit();
            Assert.AreEqual(wrapped.Name, "No degree");
            
            
            
        }
    }
}
