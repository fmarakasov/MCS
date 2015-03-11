using MContracts.ViewModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace MCUITests
{


    /// <summary>
    ///Это класс теста для ActDesignerViewModelTest, в котором должны
    ///находиться все модульные тесты ActDesignerViewModelTest
    ///</summary>
    [TestClass()]
    public class ActDesignerViewModelTest
    {


        private TestContext testContextInstance;

        /// <summary>
        ///Получает или устанавливает контекст теста, в котором предоставляются
        ///сведения о текущем тестовом запуске и обеспечивается его функциональность.
        ///</summary>
        public TestContext TestContext
        {
            get { return testContextInstance; }
            set { testContextInstance = value; }
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
        ///Тест для GetRecomendedPrepayment
        ///</summary>
        //[TestMethod()]
        [DeploymentItem("MContracts.exe")]
        public void GetRecomendedPrepaymentTest()
        {
            CheckRecomendedPrepayment(10, 10, null, 10);
            CheckRecomendedPrepayment(10, 100, 20, 10);
            CheckRecomendedPrepayment(100, 10, 20, 2);
        }

        private static void CheckRecomendedPrepayment(decimal rest, decimal total, double? percent, decimal expected)
        {
            var actual = ActDesignerViewModel_Accessor.GetRecomendedPrepayment(rest, total, percent);
            Assert.AreEqual(expected, actual);
        }
    }
}
