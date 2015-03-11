using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using MCDomain.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MTests
{
    /// <summary>
    /// Summary description for EditableObjectTest
    /// </summary>
    [TestClass]
    public class EditableObjectTest
    {
        public EditableObjectTest()
        {

        }

        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
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

        [TestMethod]
        public void TestEditableObjectEdit()
        {
            var wrapped = new StubEditableObject();
            EditableObject<StubEditableObject> obj = new EditableObject<StubEditableObject>(wrapped);
            
            object obj1 = new object();
            wrapped.Property1 = 100;
            wrapped.Property2 = "Hello";
            wrapped.Property3 = 10;
            wrapped.Property4 = obj1;
            wrapped.Property5 = new List<int>();

            
            object obj2 = new object();
            List<int> prop5 = new List<int>();

            obj.BeginEdit();
            wrapped.Property1 = 200;
            wrapped.Property2 = "lo";
            wrapped.Property3 = 10;
            wrapped.Property4 = obj2;
            wrapped.Property5 = prop5;
            obj.EndEdit();
            
            Assert.AreEqual(200, wrapped.Property1);
            Assert.AreEqual("lo", wrapped.Property2);
            Assert.AreEqual(10, wrapped.Property3);
            Assert.AreSame(wrapped.Property4, obj2);
            Assert.AreSame(wrapped.Property5, prop5);

            obj.BeginEdit();
            wrapped.Property1 = 300;
            wrapped.Property2 = "Helo";
            wrapped.Property3 = 1000;
            wrapped.Property4 = obj1;
            wrapped.Property5 = new List<int>();
            obj.CancelEdit();
            
            Assert.AreEqual(200, wrapped.Property1);
            Assert.AreEqual("lo", wrapped.Property2);
            Assert.AreEqual(10, wrapped.Property3);
            Assert.AreSame(wrapped.Property4, obj2);
            Assert.AreNotSame(wrapped.Property5, prop5);
            
        }
    }

    class StubEditableObject : ICloneable
    {
        
        public int Property1 { get; set; }
        public string Property2 { get; set; }
        public int? Property3 { get; set; }
        public object Property4 { get; set; }
        public List<int> Property5 { get; set; }

        public object Clone()
        {
            return new StubEditableObject()
                       {
                           Property1 = this.Property1,
                           Property2 = this.Property2,
                           Property3 = Property3,
                           Property4 = Property4,
                           Property5 = Property5
                       };
        }
    }
}
