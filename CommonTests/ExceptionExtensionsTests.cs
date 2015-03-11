using System;
using CommonBase.Exceptions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CommonTests
{
    [TestClass]
    public class ExceptionExtensionsTests
    {
        [TestMethod]
        public void TestAggregateException()
        {
            var ex = new Exception("Top");
            Assert.AreEqual("Top\r\n",ex.AggregateMessages());

            ex = new Exception("Top", new Exception("Inner"));
            Assert.AreEqual("Top\r\nInner\r\n", ex.AggregateMessages());
            
        }
    }
}
