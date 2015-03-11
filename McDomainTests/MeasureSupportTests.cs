using System;
using MCDomain.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace McDomainTests
{
    [TestClass]
    public class MeasureSupportTests
    {
        private static Mock<IMeasureSupport> CreateMock(int factor, decimal price)
        {
            var stub = new Mock<IMeasureSupport>();
            stub.Setup(x => x.Measure).Returns(new Currencymeasure() {Factor = factor});
            stub.Setup(x => x.PriceValue).Returns(price);
            return stub;
        }
        [TestMethod]
        public void ToFactorTest()
        {
            Assert.AreEqual(1, CreateMock(1, 1).Object.ToFactor(1));
            Assert.AreEqual(10, CreateMock(1, 10).Object.ToFactor(1));
            Assert.AreEqual(1000, CreateMock(1000, 1).Object.ToFactor(1));
            Assert.AreEqual(1000, CreateMock(1000000, 1).Object.ToFactor(1000));
            Assert.AreEqual(10000, CreateMock(1000000, 10).Object.ToFactor(1000));
        }
    }
}
