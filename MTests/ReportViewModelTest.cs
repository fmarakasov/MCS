using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MCDomain.DataAccess;
using MContracts.ViewModel.Reports;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MTests
{
    /*
    class StubEmptyReportViewModel : ExcelReportViewModel
    {
        public StubEmptyReportViewModel(IContractRepository repository) : base(repository)
        {
        }

        public override string ReportFileName
        {
            get { throw new NotImplementedException(); }
        }

 
    }

    class StubNotEmptyReportViewModel : ExcelReportViewModel
    {
        [InputParameter(typeof(int), "Int Property")]
        public int IntProperty { get; set; }
        [InputParameter(typeof(DateTime), "DateTime Property")]
        public DateTime DateTimeProperty { get; set; }
        [InputParameter(typeof(string), "String Property")]
        public string StringProperty { get; set; }

        public StubNotEmptyReportViewModel(IContractRepository repository)
            : base(repository)
        {
        }

        public override string ReportFileName
        {
            get { throw new NotImplementedException(); }
        }

   
    }


    [TestClass]
    public class ReportViewModelTest
    {

        [TestMethod]
        public void ReportViewModelEmptyPropertiesTest()
        {
            var vm = new StubEmptyReportViewModel(new StubContractRepository());
            var res = vm.Parameters;
            Assert.IsNotNull(res);
            Assert.AreEqual(0, res.Count());
        }

        [TestMethod]
        public void ReportViewModelPropertiesTest()
        {
            var vm = new StubNotEmptyReportViewModel(new StubContractRepository());
            var res = vm.Parameters;
            Assert.IsNotNull(res);
            Assert.AreEqual(3, res.Count());
            Assert.IsTrue(res.Any(x=>x.PropertyName == "IntProperty"));
            Assert.IsTrue(res.Any(x => x.PropertyName == "DateTimeProperty"));
            Assert.IsTrue(res.Any(x => x.PropertyName == "StringProperty"));
        }
    }
     */
}
