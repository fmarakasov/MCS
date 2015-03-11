using MContracts.ViewModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using MCDomain.DataAccess;
using MContracts.ViewModel.Helpers;
using MCDomain.Model;

namespace MCUITests
{
    
    
    /// <summary>
    ///Это класс теста для MainWindowViewModelTest, в котором должны
    ///находиться все модульные тесты MainWindowViewModelTest
    ///</summary>
    [TestClass()]
    public class MainWindowViewModelTest
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
        ///Тест для CreateNewSubgeneralContractdoc
        ///</summary>
        [TestMethod()]
        [DeploymentItem("MContracts.exe")]
        public void CreateNewSubgeneralContractdocTest()
        {
           
        }
    }
}
