using System;
using System.Linq;
using MCDomain.Model;
using MContracts.ViewModel;
using MContracts.ViewModel.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MTests
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
            MainWindowViewModel_Accessor target = new MainWindowViewModel_Accessor();
            var repository = LinqContractRepositoryTest.CreateLinqContractRepository();
            var param = new NewContractdocParams_Accessor(repository.GetContractdoc(100000),
                                                          NewContractdocType_Accessor.NewSubgeneral);
            var actual = target.CreateNewSubgeneralContractdoc(repository, param);
            repository.SubmitChanges();
            
            Contracthierarchy hierarchi = null;
            try
            {
                Assert.IsTrue(actual.Id > 100000);
                var newId = actual.Id;
                hierarchi = repository.Context.Contracthierarchies.Single(x => x.Subcontractdocid == newId);
                Assert.AreEqual(param.Contractdoc.Id, hierarchi.Generalcontractdocid);
            }
            finally
            {
                //repository.Context.Contracthierarchies.DeleteOnSubmit(hierarchi);
                repository.Context.Contractdocs.DeleteOnSubmit(actual);
                repository.SubmitChanges();
            }
        }
    }
}
