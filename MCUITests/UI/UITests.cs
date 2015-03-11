using System;
using System.Linq;
using System.Windows;
using MCDomain.DataAccess;
using MContracts.Classes;
using MContracts.Controls.Dialogs;
using MContracts.View;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MCUITests
{
    [TestClass]
    public class UITests
    {
        [TestMethod]
        [DeploymentItem("Templates\\act.dot", "Templates")]
        public void ShowActsView()
        {
            var host = new HostWindow();
            var vm = RepositoryHelper<DebugLinqReposotoryFactory>.CreateActsViewModel();
            vm.ContractObject = vm.Repository.GetContractdoc(11600);
                  
            var actsView = new ActsView() {DataContext = vm};
            actsView.Resources.MergedDictionaries.Add(new ResourceDictionary());
            host.Content = actsView;
            host.ShowDialog();
        }
        [TestMethod]
        public void ShowActDesignerDialogTest()
        {
            var dlg = new ActDesignerDialog();
            var vm = RepositoryHelper<DebugLinqReposotoryFactory>.CreateActDesignerViewMode();

            vm.CurrentContract = vm.Repository.GetContractdoc(11600);
            vm.CurrentSchedule = vm.CurrentContract.Schedulecontracts.First().Schedule;
            vm.CurrentAct = vm.Repository.NewAct(vm.CurrentContract);
            Console.WriteLine("{0} : {1}", vm.CurrentContract.Id, vm.CurrentContract);

            dlg.DataContext = vm;
            dlg.ShowDialog();
        }


        [TestMethod]
        public void ShowActDesignerDialogForEditTest()
        {
            var vm = RepositoryHelper<DebugLinqReposotoryFactory>.CreateActDesignerViewMode();
            var dlg = new ActDesignerDialog();
            vm.CurrentContract = vm.Repository.GetContractdoc(11600);
            vm.CurrentSchedule = vm.CurrentContract.Schedulecontracts.First().Schedule;
            vm.CurrentAct = vm.Repository.TryGetContext().Acts.Single(x => x.Num == "1");
            Console.WriteLine("{0} : {1}", vm.CurrentContract.Id, vm.CurrentContract);

            dlg.DataContext = vm;
            dlg.ShowDialog();



        }

        [TestMethod]
        public void ShowRegionEditor()
        {
            var dlg = new CatalogEditorDialog();

            var vm = RepositoryHelper<DebugLinqReposotoryFactory>.CreateCatalogViewModel(CatalogType.Region);

            dlg.DataContext = vm;

            dlg.ShowDialog();

        }
        [TestMethod]
        public void ShowEnterpriseEditor()
        {
            var dlg = new CatalogEditorDialog();

            var vm = RepositoryHelper<DebugLinqReposotoryFactory>.CreateCatalogViewModel(CatalogType.Enterpriseauthority);

            dlg.DataContext = vm;

            dlg.ShowDialog();

        }

        [TestMethod]
        public void ShowScheduleView()
        {


            HostWindow host = new HostWindow();
            var vm = RepositoryHelper<DebugLinqReposotoryFactory>.CreateScheduleViewModel();
            vm.ContractObject = vm.Repository.GetContractdoc(14470);
           
            var schedule = new ScheduleView() {DataContext = vm};
            host.Content = schedule;
            host.ShowDialog();
        }

        [TestMethod]
        public void ShowStateApprovalEditor()
        {

            var vm = RepositoryHelper<DebugLinqReposotoryFactory>.CreateApprovalstateEditorViewModel();
            vm.Approval = vm.Repository.TryGetContext().Stages.First();
            var dlg = new ApprovalStateEditor {DataContext = vm};
            dlg.ShowDialog();
        }

        [TestMethod]
        public void ShowActRepositoryViewBasedView()
        {

            var host = new HostWindow();
            var vm = RepositoryHelper<DebugLinqReposotoryFactory>.CreateActRepositoryViewBasedViewModel();
            var view = new ActRepositoryViewBasedView() {DataContext = vm};
            host.Content = view;
            host.ShowDialog();
        }


    }
}
