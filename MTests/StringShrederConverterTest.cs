using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using MContracts.Classes.Converters;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MTests
{
    [TestClass]
    public class StringShrederConverterTest
    {
        [TestMethod]
        public void ShederEmptyStringTest()
        {
            Assert.AreEqual(string.Empty, StringShrederConverter.Shreder(string.Empty, 100));
        }
        [TestMethod]
        public void ShederShortStringTest()
        {
            const string sourceString = "ShortString";
            Assert.AreEqual(sourceString, StringShrederConverter.Shreder(sourceString, 100));
        }
        [TestMethod]
        public void ShederExactlyMaxLengthTest()
        {
            const string sourceString = "ShortString";
            Assert.AreEqual(sourceString, StringShrederConverter.Shreder(sourceString, sourceString.Length));
        }

        [TestMethod]
        public void ShederGraterMaxLengthTest()
        {
            const string sourceString = "ShortString";
            const string expected = "ShortStri\u2026";
            Assert.AreEqual(expected, StringShrederConverter.Shreder(sourceString, sourceString.Length-1));
            Console.Write(expected);
        }

        [TestMethod]
        public void ShederFromTheStartTest()
        {
            const string sourceString = "ShortString";
            const string expected = "*hortString";
            Assert.AreEqual(expected, StringShrederConverter.Shreder(sourceString, sourceString.Length - 1, "*", ShrederType.FromTheStart));
            Console.Write(expected);
        }

        [TestMethod]
        public void ShederInTheMiddleTest()
        {
            const string sourceString = "ShortString";
            const string expected = "Shor*tring";
            Assert.AreEqual(expected, StringShrederConverter.Shreder(sourceString, sourceString.Length - 1, "*", ShrederType.InTheMiddle));
            Console.Write(expected);
        }
    }
}
