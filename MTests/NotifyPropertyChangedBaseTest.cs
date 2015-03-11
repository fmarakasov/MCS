using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using MCDomain.Common;
using MContracts.Classes;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MTests
{
    class StubNotifyPropertyChanged :  NotifyPropertyChangedBase
    {
        private int _some;
        private int _bad;

        public int SomeProperty
        {
            get { return _some; }
            set 
            {
                _some = value;
                OnPropertyChanged("SomeProperty");
            }
        }

      
        public int SomeBadProperty
        {
            get
            {
                return _bad;
            }
            set
            {
                OnPropertyChanged("NoSuchProperty");
            }
        }
    }

    /// <summary>
    /// Summary description for NotifyPropertyChangedBaseTest
    /// </summary>
    [TestClass]
    public class NotifyPropertyChangedBaseTest
    {
        public NotifyPropertyChangedBaseTest()
        {

        }

        private TestContext testContextInstance;
        private int _hits;

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
        public void TestPropertyChanged()
        {
            StubNotifyPropertyChanged obj = new StubNotifyPropertyChanged();
            obj.PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler(obj_PropertyChanged);
            Assert.AreEqual(0, _hits);
            obj.SomeProperty = 10;
            Assert.AreEqual(1, _hits);
        }
        

        void obj_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            ++_hits;
        }
    }
}
