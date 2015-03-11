using MCDomain.Model;
using MContracts.Classes.Converters;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Windows.Media;

namespace MCUITests
{
    /// <summary>
    ///Это класс теста для ApprovalstateTest, в котором должны
    ///находиться все модульные тесты ApprovalstateTest
    ///</summary>
    [TestClass()]
    public class ApprovalstateTest
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


        /// <summary>
        ///Тест для StateColor
        ///</summary>
        //[TestMethod()]
        public void StateColorTest()
        {
            var colorConverter = new IntToBrushConverter();
            var brush = new SolidColorBrush(Colors.BlueViolet);
            var actualInt = colorConverter.ConvertBack(brush, null, null, null);
            var actualColor = colorConverter.Convert(actualInt, null, null, null);
            Assert.AreEqual(brush, actualColor);
        }
    }
}
