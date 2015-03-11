using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using MCDomain.DataAccess;
using MCDomain.Model;
using MContracts.ViewModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MTests
{
    [TestClass]
    public class ScheduleViewModelTest
    {
        public static void InsertScheduleWrapper(Action<ScheduleViewModel, LinqContractRepository> assertFunc)
        {
            ContractdocViewModelTest.InsertNewContractWrapper((vm, rep) =>
                                                                  {
                                                                      var schedulecontract = new Schedulecontract()
                                                                                   {
                                                                                       Schedule =
                                                                                           new Schedule()
                                                                                               {
                                                                                                   Currencymeasure =
                                                                                                       rep.Context.
                                                                                                       Currencymeasures.
                                                                                                       First(),
                                                                                                   Worktype =
                                                                                                       rep.Context.Worktypes
                                                                                                       .First()
                                                                                               },
                                                                                       Appnum = 3,
                                                                                   };
                                                                      vm.Contractdoc.Schedulecontracts.Add(schedulecontract);
                                                                      if (assertFunc != null)
                                                                          assertFunc(vm.ScheduleViewModel, rep);
                                                                  });
        }


        [TestMethod]
        public void InsertScheduleToNewContract()
        {
            InsertScheduleWrapper(null);
        }

        [TestMethod]
        public void ImportScheduleToNewContractFromExcel()
        {
            InsertScheduleWrapper((vm, rep)=> { } );
        }

    }
}
