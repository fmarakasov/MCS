using System.Collections;
using System.Linq;
using MCDomain.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace MTests
{
    
    
    /// <summary>
    ///Это класс теста для SearchExtentionsTest, в котором должны
    ///находиться все модульные тесты SearchExtentionsTest
    ///</summary>
    [TestClass()]
    public class SearchExtentionsTest
    {


        private TestContext testContextInstance;

        /// <summary>
        ///Получает или устанавливает контекст теста, в котором предоставляются
        ///сведения о текущем тестовом запуске и обеспечивается его функциональность.
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

        #region Дополнительные атрибуты теста
        // 
        //При написании тестов можно использовать следующие дополнительные атрибуты:
        //
        //ClassInitialize используется для выполнения кода до запуска первого теста в классе
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //ClassCleanup используется для выполнения кода после завершения работы всех тестов в классе
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //TestInitialize используется для выполнения кода перед запуском каждого теста
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //TestCleanup используется для выполнения кода после завершения каждого теста
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion

        /*
         * 
         *  x
         *  |__x1
         *  |__x2
         *      |__x2.1
         *      |__x2.2
         *      |__x2.3
         *          |__x2.3.1
         */
        class TestItem:IEquatable<TestItem>
        {
            public List<TestItem> Childs = new List<TestItem>();
            public string Value { get; set; }
            public TestItem Parent { get; set; }
            public bool Equals(TestItem other)
            {
                return this == other;
            }
            public override string ToString()
            {
                return string.Format("Item: {0}, My Parent: {1}, Child count: {2}", Value, Parent!=null ? Parent.Value : "-", Childs.Count);
            }
        }

        private TestItem x, x1, x2, x21, x22, x23, x231;

        TestItem CreateTestTree()
        {
            x = new TestItem() {Value = "x"};
            x1 = new TestItem() {Value = "x1"};
            x.Childs.Add(x1);
            x1.Parent = x;
            x2 = new TestItem() {Value = "x2"};
            x.Childs.Add(x2);
            x2.Parent = x;
            x21 = new TestItem() {Value = "x2.1"};
            x2.Childs.Add(x21);
            x21.Parent = x2;
            x22 = new TestItem() {Value = "x2.2"};
            x2.Childs.Add(x22);
            x22.Parent = x2;
            x23 = new TestItem() {Value = "x2.3"};
            x2.Childs.Add(x23);
            x23.Parent = x2;
            x231 = new TestItem() {Value = "x2.3.1"};
            x23.Childs.Add(x231);
            x231.Parent = x23;
            return x;


        }


        [TestMethod()]
        public void LeftDepthSearchIncludingItemTest()
        {
            var item = CreateTestTree();
            var actual = item.LeftDepthSearch(p => p.Childs, p=>true);
            Assert.IsTrue(actual.AllUnique());
            Assert.AreEqual(7, actual.Count());

        }


        [TestMethod()]
        public void GetBackwardPath()
        {
            var item = CreateTestTree();
            var actual = x231.GetBackwardPath(p => p.Parent, p => true);
            actual.WriteLine();
            Assert.IsTrue(actual.AllUnique());
            Assert.AreEqual(4, actual.Count());

        }

        [TestMethod()]
        public void RightDepthSearchTest()
        {
          
        }
    }
}
