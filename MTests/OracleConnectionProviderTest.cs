using System;
using System.Collections.Generic;
using System.Linq;
using MCDomain.DataAccess;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MTests
{
    /// <summary>
    ///This is a test class for OracleConnectionProviderTest and is intended
    ///to contain all OracleConnectionProviderTest Unit Tests
    ///</summary>
    [TestClass]
    public class OracleConnectionProviderTest
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
        ///A test for OracleDataSources
        ///</summary>
        [TestMethod]
        public void OracleDataSourcesTest()
        {
            var target = new OracleConnectionProvider(); 
            IEnumerable<OracleDataSource> actual;
            actual = target.OracleDataSources;
            Assert.IsTrue(actual != null);
            Assert.IsTrue(actual.Count() > 0);
            OracleDataSource ds = actual.First();
            Console.WriteLine(ds.InstanceName);
            Console.WriteLine(ds.ServerName);
            Console.WriteLine(ds.ServiceName);
            Console.WriteLine(ds.Protocol);
            Console.WriteLine(ds.Port);
        }
    }
}