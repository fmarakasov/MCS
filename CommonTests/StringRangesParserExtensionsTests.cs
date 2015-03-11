using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CommonBase;

namespace CommonTests
{
    [TestClass]
    public class StringRangesParserExtensionsTests
    {
        private void CheckRange(IEnumerable<int> actual, params int[] expected)
        {
            var arr = actual.ToArray();
            Assert.IsTrue(expected.SequenceEqual(arr));            

        }
        [TestMethod]
        public void RangesTests()
        {
            var s1 = "5";
            CheckRange(s1.ParseRanges(), 5);
            
            s1 = "1,2";
            CheckRange(s1.ParseRanges(), 1, 2);

            s1 = "1, 2";
            CheckRange(s1.ParseRanges(), 1, 2);

            s1 = "1-3";
            CheckRange(s1.ParseRanges(), 1, 2, 3);

            s1 = "1, 3-5";
            CheckRange(s1.ParseRanges(), 1, 3, 4, 5);

            s1 = "2, 3 - 5";
            CheckRange(s1.ParseRanges(), 2, 3, 4, 5);

            s1 = "2-5, 10 - 12";
            CheckRange(s1.ParseRanges(), 2, 3, 4, 5,10,11,12);

            s1 = " 1, 2 - 4, 10 - 12, 20";
            CheckRange(s1.ParseRanges(), 1, 2, 3, 4, 10, 11, 12, 20);


        }
    }
}
