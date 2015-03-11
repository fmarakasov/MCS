using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using MCDomain.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MTests
{
    [TestClass]
    public class StageComparierByStageNumTest
    {
        [TestMethod]
        public void EmptyStageNumsTest()
        {
            VerifyComparierResult(string.Empty, string.Empty, 0);
        }
        [TestMethod]
        public void SimpleStageNumsEqualsTest()
        {
            VerifyComparierResult("1", "1", 0);
        }
        [TestMethod]
        public void SimpleStageNumsFirstGraterTest()
        {
            VerifyComparierResult("2", "1", 1);
        }

        [TestMethod]
        public void SimpleStageNumsSecondGraterTest()
        {
            VerifyComparierResult("1", "2", -1);
        }

        [TestMethod]
        public void DotedStageNumsSecondGraterTest()
        {
            VerifyComparierResult("1.1", "1.2", -1);
        }

        [TestMethod]
        public void DotedStageNumsFirstGraterTest()
        {
            VerifyComparierResult("1.2", "1.1", 1);
        }

        [TestMethod]
        public void DotedStageNumsFirstGraterComplexTest()
        {
            VerifyComparierResult("1.2", "1", 1);
        }

        [TestMethod]
        public void DotedStageNumsSecondGraterComplexTest()
        {
            VerifyComparierResult("1", "1.2", -1);
        }

        [TestMethod]
        public void DotedStageNumsEqualsTest()
        {
            VerifyComparierResult("1.2", "1.2", 0);
        }

        
        private static void VerifyComparierResult(string num1, string num2, int expectedResult)
        {
            var comparier = new HierarchicalNumberingComparier();
            Assert.AreEqual(expectedResult, comparier.Compare(num1, num2));
        }
    }
}
