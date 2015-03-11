using System;
using MCDomain.AOP;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace MTests
{
    class StubLogger : ICoreLogger
    {
        public void Write(string logMessage)
        {
            Console.WriteLine(logMessage);
        }
    }

    [Log(typeof(StubLogger))]
    class ClassForLog
    {
        internal void CallMeFirst()
        {

        }
    }

    [TestClass]
    public class AopLogAttributeTests
    {
        [TestMethod]
        public void TestClassLogMessages()
        {
            var obj = new ClassForLog();
            obj.CallMeFirst();
        }
    }
}
