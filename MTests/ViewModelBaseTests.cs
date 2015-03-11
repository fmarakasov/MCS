using System;
using System.ComponentModel;
using MContracts.ViewModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MTests
{
    [TestClass]
    public class ViewModelBaseTests
    {
        #region Boilerplate Code

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext { get; set; }

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

        #endregion // Boilerplate Code

        [TestMethod]
        public void TestPropertyChangedIsRaisedCorrectly()
        {
            var target = new TestViewModel();
            bool eventWasRaised = false;
            target.PropertyChanged +=
                (sender, e) =>
                eventWasRaised = (e.PropertyName == "GoodProperty");

            target.GoodProperty = "Some new value...";
            Assert.IsTrue(eventWasRaised, "PropertyChanged event was not raised correctly.");
        }
        [TestMethod]
        public void TestPropertyChangedExpressionIsRaisedCorrectly()
        {
            var target = new TestViewModel();
            bool eventWasRaised = false;
            target.PropertyChanged += (sender, e) => eventWasRaised = e.PropertyName == "GoodProperty" || e.PropertyName == "ExpressionGoodProperty";
            target.ExpressionGoodProperty = "Some new value...";
            Assert.IsTrue(eventWasRaised, "PropertyChanged event was not raised correctly.");
        }

        [TestMethod]
        public void TestExceptionIsThrownOnInvalidPropertyName()
        {
            var target = new TestViewModel();
            try
            {
                target.BadProperty = "Some new value...";
#if DEBUG
                Assert.Fail("Exception was not thrown when invalid property name was used in DEBUG build.");
#endif
            }
            catch (Exception)
            {
#if !DEBUG
                Assert.Fail("Exception was thrown when invalid property name was used in RELEASE build.");
#endif
            }
        }

        [TestMethod]
        public void TestDataErrorProxy()
        {
            var vm = new TestViewModel();
            Assert.IsNotNull(vm["TestString"]);
            Assert.IsNotNull(vm.Error);
        }

        [TestMethod]
        public void TestDlrProxy()
        {
            dynamic vm = new TestViewModel();
            vm.TestString = "Hello";
            Assert.AreEqual("Hello", vm.TestString);
        }

        #region Nested type: TestEntity

        private class TestEntity : IDataErrorInfo
        {
            public string TestString { get; set; }

            #region IDataErrorInfo Members

            public string this[string columnName]
            {
                get
                {
                    if (columnName == "TestString")
                        if (string.IsNullOrWhiteSpace(TestString)) return "Column Error";
                    return null;
                }
            }

            public string Error
            {
                get { return "Test Error"; }
            }

            #endregion
        }

        #endregion

        #region Nested type: TestViewModel

        private class TestViewModel : ViewModelBase
        {
            public TestViewModel()
            {
                WrappedDomainObject = new TestEntity();
            }

            protected override bool ThrowOnInvalidPropertyName
            {
                get { return true; }
            }

            public string GoodProperty
            {
                get { return null; }
                set { base.OnPropertyChanged("GoodProperty"); }
            }

            public string ExpressionGoodProperty
            {
                get { return null; }
                set { base.OnPropertyChanged(()=>ExpressionGoodProperty); }
            }

            public string BadProperty
            {
                get { return null; }
                set { base.OnPropertyChanged("ThisIsAnInvalidPropertyName!"); }
            }
        }

        #endregion
    }
}