using System;
using System.Linq;
using CommonBase;
using MCDomain.DataAccess;
using MCDomain.Model;
using McReports.Common;
using McReports.ViewModel;
using McUIBase.Factories;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace McReportsTests
{
    class TestTemplateProvider:ITemplateProvider
    {
        public TestTemplateProvider(string templatePath)
        {
            TemplatePath = templatePath;
        }

        public string GetTemplate(string propertyName)
        {
            return TemplatePath;
        }

        public string TemplatePath { get; private set; }
    }

    [TestClass]
    public class McReportTests
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


        private void CreateReport(BaseReportViewModel reportViewModel, string fileName)
        {
            var reporter = new ConsoleProgressReporter();
            reportViewModel.TemplateProvider = new TestTemplateProvider(fileName);
            reportViewModel.ProgressReporter = reporter;
            reportViewModel.SetReport();        
            reportViewModel.ShowReport();
            Assert.AreEqual(100, reporter.Percents);
 
        }

        [DeploymentItem(@"MContracts\Templates\9_Act.dot")]
        [TestMethod]
        public void TransferAct9Test()
        {
            ContextFactoryService.Instance.QueryLoginProvider = new StubQueryLoginProvider();
            var rpt = RepositoryFactory.CreateEmptyRepository();
            var report = new ActTransferReportViewModel(rpt);
            var ctx = rpt.TryGetContext();
            var act = new Transferact() {Num = 10000, Id = 9999, Signdate = DateTime.Today, };
            
            rpt.Transferacts.Add(act);

            var contract1 = new Contractdoc() { Internalnum = "200-09-1", Approvedat = DateTime.Today };
            // поля Internalnum и Approvedat не заполнены
            var contract2 = new Contractdoc() { Price = 10 }; 
            var contractor = new Contractor() {Name = "ООО \"Газпромдобыча Ямбург\""};
            // не задано значение поля Name
            var contractor2 = new Contractor(); 

            var doc1 = new Document() { Name = "Договор" };
            var doc2 = new Document() { Name = "Задание" };
            var doc3 = new Document() { Name = "Календарный план" };
            var doc4 = new Document() { Name = "Протокол" };
            var doc5 = new Document() { Name = "Письма" };
            var doc6 = new Document() { Name = "Протокол1" };

            contract1.Contractorcontractdocs.Add(new Contractorcontractdoc(){Contractor = contractor});
            act.Contracttranactdocs.Add(new Contracttranactdoc(){Contractdoc = contract1, Document = doc1, Pagescount = 5});
            act.Contracttranactdocs.Add(new Contracttranactdoc() { Contractdoc = contract1, Document = doc1, Pagescount = 5 });
            act.Contracttranactdocs.Add(new Contracttranactdoc() { Contractdoc = contract1, Document = doc2, Pagescount = 1 });
            act.Contracttranactdocs.Add(new Contracttranactdoc() { Contractdoc = contract1, Document = doc3, Pagescount = 2 });
            act.Contracttranactdocs.Add(new Contracttranactdoc() { Contractdoc = contract1, Document = doc4, Pagescount = 3 });
            act.Contracttranactdocs.Add(new Contracttranactdoc() { Contractdoc = contract1, Document = doc5}); 
            act.Contracttranactdocs.Add(new Contracttranactdoc() { Contractdoc = contract1, Document = doc6});

            contract2.Contractorcontractdocs.Add(new Contractorcontractdoc(){Contractor = contractor2});
            act.Contracttranactdocs.Add(new Contracttranactdoc() { Contractdoc = contract2, Document = doc1, Pagescount = 5 });
            act.Contracttranactdocs.Add(new Contracttranactdoc() { Contractdoc = contract2, Document = doc1, Pagescount = 2 });
            act.Contracttranactdocs.Add(new Contracttranactdoc() { Contractdoc = contract2, Document = doc2 });
            act.Contracttranactdocs.Add(new Contracttranactdoc() { Contractdoc = contract2, Document = doc3, Pagescount = 1 });
            act.Contracttranactdocs.Add(new Contracttranactdoc() { Contractdoc = contract2, Document = doc4, Pagescount = 1 });
            act.Contracttranactdocs.Add(new Contracttranactdoc() { Contractdoc = contract2, Document = doc5, Pagescount = 4 });
            act.Contracttranactdocs.Add(new Contracttranactdoc() { Contractdoc = contract2, Document = doc6, Pagescount = 8 });

            report.TransferActId = 9999;
            CreateReport(report, TestContext.DeploymentDirectory + @"\9_act.dot");         
        }

        [TestMethod]
        [DeploymentItem(@"MContracts\Templates\InformationConcludedContracts.xlt")]
        public void InformConcludedContract()
        {
            
            ContextFactoryService.Instance.QueryLoginProvider = new StubQueryLoginProvider();
            ContextFactoryService.Instance.Connect();
            var rpt = RepositoryFactory.CreateContractRepository();
            //var rpt = RepositoryFactory.CreateEmptyRepository();
            var report = new InformationConcludedContracts_ViewModel(rpt);
            try
            {


                var rspMock = new Mock<IReportSourceProvider>();

                rspMock.Setup(x => x.Source).Returns(() => rpt.Contracts.Take(20));
                var date1 = new DateTime(2011, 10, 1, 0, 0, 0); //год месяц день 
                var date2 = new DateTime(2011, 12, 30, 0, 0, 0);
                report.Period = new DateRange() {Start = date1, End = date2};
                report.ReportSource = rspMock.Object;

                CreateReport(report, TestContext.DeploymentDirectory + @"\InformationConcludedContracts.xlt");
            }
            finally
            {
                report.Dispose();
                rpt.Dispose();
            }

        }

        [TestMethod]
        [DeploymentItem(@"MContracts\Templates\Act2.dot")]
        public void Act2Test()
        {

            ContextFactoryService.Instance.QueryLoginProvider = new StubQueryLoginProvider();
            ContextFactoryService.Instance.Connect();
            var rpt = RepositoryFactory.CreateEmptyRepository();
            var report = new Act2ReportViewModel(rpt);
            try
            {
                var scheduld = new Schedule()
                    {
                        Currencymeasure = rpt.Currencymeasures[0],
                        Worktype = new Worktype() {Id = 1, Name = "Какая-то"}
                    };

                var contract = rpt.Contracts.First(x => x.Acttype.WellKnownType == WellKnownActtypes.RegionGasHolding);

                contract.Schedulecontracts.Add(new Schedulecontract() {Schedule = scheduld});

                var stage = new Stage()
                    {
                        Nds = rpt.Nds[0],
                        Ndsalgorithm = rpt.Ndsalgorithms[0],
                        Startat = DateTime.Today,
                        Endsat = DateTime.Today.AddDays(270),
                        Price = 100000,
                        Num = "1",
                        Code = "21",
                        Id = 1
                    };

                scheduld.Stages.Add(stage);
                var act = new Act()
                    {
                        Nds = stage.Nds,
                        Ndsalgorithm = stage.Ndsalgorithm,
                        Currency = contract.Currency,
                        Currencymeasure = scheduld.Currencymeasure,
                        Totalsum = stage.StageMoneyModel.Factor.National.WithNdsValue,
                        Sumfortransfer = stage.StageMoneyModel.Factor.National.WithNdsValue,
                        Num = "1",
                        Issigned = true,
                        Signdate = DateTime.Today.AddDays(255)
                    };
                stage.Act = act;
                act.ContractObject = contract;
                report.CurrentAct = act;
                CreateReport(report, TestContext.DeploymentDirectory + @"\Act2.dot");                

            }
            finally
            {
                rpt.Dispose();
                report.Dispose();
            }
        }
    }
}
