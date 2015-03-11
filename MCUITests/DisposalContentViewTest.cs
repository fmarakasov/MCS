using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using MCDomain.Common;
using MCDomain.DataAccess;
using MCDomain.Model;
using MContracts;
using MContracts.Controls.Dialogs;
using MContracts.View;
using MContracts.ViewModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MCUITests
{
    [TestClass]
    public class DisposalContentViewTest
    {

        [TestMethod]
        public void ShowDisposalContentViewTest()
        {
            DialogShell host = new DialogShell();
            var vm = RepositoryHelper<DebugLinqReposotoryFactory>.CreateDisposalContentViewModel();
            //Contractdoc cc = vm.Repository.TryGetContext().Contractdocs.FirstOrDefault(c => c.Id == 10040);
            Disposal dd = vm.Repository.TryGetContext().Disposals.FirstOrDefault(d => d.Id == 10040);

 
            if (dd!=null)
            {
                var disposalView = new DisposalContentView() {DataContext = vm};
                disposalView.Resources.MergedDictionaries.Add(new ResourceDictionary());
                
                vm.Disposal = dd;
                host.ViewModel = vm;
                
                host.Content = disposalView;
                
                host.ShowDialog();
            }
        }
    }
}