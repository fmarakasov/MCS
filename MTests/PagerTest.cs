using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using MContracts.Classes;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MTests
{
    [TestClass]
    public class PagerTest
    {
        [TestMethod]
        public void TestOnStartIndex()
        {
            Pager pager = new Pager(10, 10);
            Assert.AreEqual(0, pager.Index);
        }

        [TestMethod]
        public void TestMoveNextIndex()
        {
            Pager pager = new Pager(10, 100);
            pager.Next();
            Assert.AreEqual(10, pager.Index);
        }
        [TestMethod]
        public void TestMoveLastIndex()
        {
            Pager pager = new Pager(10, 100);
            pager.Last();
            Assert.AreEqual(90, pager.Index);
        }
    }
}
