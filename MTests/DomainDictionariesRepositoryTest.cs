using System;
using System.Collections.Generic;
using Devart.Data.Linq;
using MCDomain.Common;
using MCDomain.DataAccess;
using MCDomain.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MTests
{
    /// <summary>
    ///This is a test class for DomainDictionariesRepositoryTest and is intended
    ///to contain all DomainDictionariesRepositoryTest Unit Tests
    ///</summary>
    [TestClass]
    public class DomainDictionariesRepositoryTest
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
        ///A test for DomainDictionaries
        ///</summary>
        [TestMethod]
        public void DomainDictionariesTest()
        {
            var target = new DomainDictionariesRepository<McContextTest>();
            IDictionary<string, DomainDictionaryAttribute> actual;
            actual = target.DomainDictionaries;
            Assert.IsTrue(actual.ContainsKey("TestNds"), "Не найдено тестовое свойство TestNds");
            Type dt = actual["TestNds"].DomainType;
            Assert.IsTrue(typeof (Nds) == dt);
            ITable res = target.DataContext.GetTable(dt);
            Assert.IsNotNull(res);
        }

        #region Nested type: McContextTest

        private class McContextTest : McDataContext
        {
            [DomainDictionary("Тестовый словарь", typeof (Nds))]
            public Table<Nds> TestNds
            {
                get { return Nds; }
            }
        }

        #endregion
    }
}