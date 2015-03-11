using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MCDomain.Model;
using MCDomain.Common;
using CommonBase;
using System.Linq;

namespace McDomainTests
{
    [TestClass]
    public class StagesExtensionsTests
    {
        [TestMethod]
        public void TestQueryStringRange()
        {
            var list = new List<Stage>
                {
                    new Stage() {Num = "1"},
                    new Stage() {Num = "1.1"},
                    new Stage() {Num = "1.1.1"},
                    new Stage() {Num = "1.1.2"},
                    new Stage() {Num = "1.1.3"},
                    new Stage() {Num = "1.1.4"},
                    new Stage() {Num = "1.2"},
                    new Stage() {Num = "1.2.1"},
                    new Stage() {Num = "1.2.2"},
                    new Stage() {Num = "1.2.3"},
                    new Stage() {Num = "1.2.4"},
                    new Stage() {Num = "2"},
                    new Stage() {Num = "3"},
                    new Stage() {Num = "4"},
                    new Stage() {Num = "4.1"},
                    new Stage() {Num = "4.2"},
                    new Stage() {Num = "4.3"},
                    new Stage() {Num = "4.3.1"},
                    new Stage() {Num = "4.3.1.1"},
                    new Stage() {Num = "4.3.1.2"}
                };

            CheckActual(list.QueryStringRange("0"), new string[]{});
            CheckActual(list.QueryStringRange("1"), "1");
            CheckActual(list.QueryStringRange("1,2"), "1", "2");
            CheckActual(list.QueryStringRange("1.1"), "1.1");
           
            // Range tests
            CheckActual(list.QueryStringRange("1-2"), "1", "1.1", "1.1.1", "1.1.2", "1.1.3", "1.1.4", "1.2", "1.2.1", "1.2.2", "1.2.3", "1.2.4", "2");
            CheckActual(list.QueryStringRange("1, 2-4"), "1", "2", "3", "4");
            
            // Wildcard tests
            CheckActual(list.QueryStringRange("*"), list.Select(x=>x.Num).ToArray());
            CheckActual(list.QueryStringRange("4*1"), "4.1", "4.3.1", "4.3.1.1");
            CheckActual(list.QueryStringRange("1*"), "1", "1.1", "1.1.1", "1.1.2", "1.1.3", "1.1.4", "1.2", "1.2.1", "1.2.2", "1.2.3", "1.2.4");
            CheckActual(list.QueryStringRange("1, 2, 4.3-4.4"), "1", "2", "4.3", "4.3.1", "4.3.1.1", "4.3.1.2");
           
            //Complex test
            CheckActual(list.QueryStringRange("1, 4*1, 2-4"), "1", "4.1", "4.3.1", "4.3.1.1", "2", "3", "4");

            // Wrong string tests
            CheckActual(list.QueryStringRange("-"), new string[] { });
            CheckActual(list.QueryStringRange("asdfklasdf asdfklsdf sd sdfl, asdsdf - sdsdf sdsdf 0sdf8732489 2-1-"), new string[] { });
            
        
        }

        private static void CheckActual(IEnumerable<Stage> actual, params string[] expected)
        {
            Assert.IsTrue(expected.IsEquals(actual, (num, stage) => stage.Num == num));
        }
    }
}
