using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using MCDomain.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MTests
{
    [TestClass]
    public class PersonTests
    {
        [TestMethod]
        public void ToShortNameFullNameTest()
        {
            Assert.AreEqual("Н. С. Петров", Person.ToShortName("петров", "николай", "сергеевич"));
        }

        [TestMethod]
        public void CorrectTest()
        {
            Assert.AreEqual("Петров", Person.Correct("петров"));
        }
    }
}
