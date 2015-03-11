using System.Linq;
using MContracts.ViewModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.ComponentModel;
using MCDomain.Model;
using System.Collections.Generic;
using System.Collections;
using MCDomain.DataAccess;
using System.Windows.Input;

namespace MTests
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


       
        public static ActDesignerViewModel CreateActDesignerViewModel()
        {
            return new ActDesignerViewModel(LinqContractRepositoryTest.CreateLinqContractRepository());
        }
       
        /// <summary>
        ///Тест для TransferListAndSetAct
        ///</summary>
        [TestMethod()]
        [DeploymentItem("MContracts.exe")]
        public void TransferListAndSetActTest()
        {

            ActDesignerViewModel_Accessor target =
                new ActDesignerViewModel_Accessor(LinqContractRepositoryTest.CreateLinqContractRepository());
                // TODO: инициализация подходящего значения
           

            var targetList = new List<Stage>();
            var sourceList = CreateTestStages().ToList();
            
            // Коллекция impactList содержит все элементы sourceList + дополнительный элемент.
            // После вызова TransferListAndSetAct коллекция impactedList должна содержать только один элемент
            var impactList = new List<Stage>(sourceList);
            var checkItem = CreateTestStage();
            impactList.Add(checkItem);

            Act act = new Act();
            int oldCount = sourceList.Count;
            target.TransferListAndSetAct(sourceList, targetList, act, impactList);



            Assert.AreEqual(0, sourceList.Count);
            Assert.AreEqual(oldCount, targetList.Count);

            Assert.IsTrue(targetList.All(x=>x.Act == act));
            Assert.AreEqual(1, impactList.Count);
            Assert.AreSame(impactList[0], checkItem);

        }

        private IEnumerable<Stage> CreateTestStages()
        {
            for (int i = 0; i < 10; ++i)
                yield return CreateTestStage();
         
  
        }
        private Stage CreateTestStage()
        {
            var stage = new Stage();
          
            stage.Ndsalgorithmid = 1;
            stage.Ndsid = 1;
            stage.Subject = "Тестовый этап";
            return stage;
        }

        
    }
}
