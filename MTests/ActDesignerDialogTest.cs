using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using MCDomain.DataAccess;
using MCDomain.Model;
using MContracts.Controls.Dialogs;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MTests
{
    [TestClass]
    public class ActDesignerDialogTest
    {
        [TestMethod]
        public void ShowActDesignerDialogTest()
        {
            ActDesignerDialog dlg= new ActDesignerDialog();
            var vm = ActDesignerViewModelTest.CreateActDesignerViewModel();
            vm.CurrentAct = new Act();
            vm.CurrentSchedule = vm.Repository.TryGetContext().Schedules.First(x => x.Stages.Count > 1);

            dlg.DataContext = vm;
            dlg.ShowDialog();
        }
    }
}
