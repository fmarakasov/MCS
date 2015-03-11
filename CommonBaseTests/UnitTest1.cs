using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CommonBaseTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            IEnumerable<char> str = new[] {'a', 'b', 'c'};
            var res = str.Aggregate(new StringBuilder(), (x, y) => {x.Append(y); return x;} , (x) => x.ToString());
            Console.WriteLine(res);
            
        }
    }
}
