using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MContracts.Classes.Converters;

namespace MTests
{
    /// <summary>
    /// Summary description for BoolToVisiblityConverterTest
    /// </summary>
    [TestClass]
    public class BoolToVisiblityConverterTest
    {
        public BoolToVisiblityConverterTest()
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
        public void ToBooleanShouldReturnTrueTest()
        {
            const Visibility visibility = Visibility.Visible;
            Assert.IsTrue(visibility.ToBoolean());
        }

        [TestMethod]
        public void ToBooleanShouldReturnFalseTest()
        {
            const Visibility visibility = Visibility.Hidden;
            Assert.IsFalse(visibility.ToBoolean());
        }

        [TestMethod]
        public void ToBooleanShouldReturnFalseOnCollapsedTest()
        {
            const Visibility visibility = Visibility.Collapsed;
            Assert.IsFalse(visibility.ToBoolean());
        }

        [TestMethod]
        public void OrTest()
        {
            const Visibility visibility1 = Visibility.Collapsed;
            const Visibility visibility2 = Visibility.Visible;
            
            Visibility actual = visibility2.Or(visibility1);
            Assert.AreEqual(Visibility.Visible, actual);
        }

        public void AndTest()
        {
            const Visibility visibility1 = Visibility.Collapsed;
            const Visibility visibility2 = Visibility.Visible;

            Visibility actual = visibility2.Or(visibility1);
            Assert.AreEqual(Visibility.Collapsed, actual);
        }
        [TestMethod]
        public void ToVisibilityShouldRetVisible()
        {
            bool target = true;
            Assert.AreEqual(Visibility.Visible, target.ToVisibility());
        }

        [TestMethod]
        public void ToVisibilityShouldRetCollapsed()
        {
            bool target = false;
            Assert.AreEqual(Visibility.Collapsed, target.ToVisibility());
        }

    }
}
