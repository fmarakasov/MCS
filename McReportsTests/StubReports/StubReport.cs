using MCDomain.DataAccess;
using McReports.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MCDomain.Model;

namespace McReportsTests.StubReports
{
    /// <summary>
    /// Проверка работоспособности системы отчётов
    /// </summary>
    //class StubReport : ExcelReportViewModel
    //{
    //    public StubReport(IContractRepository repository)
    //        : base(repository)
    //    {
    //    }

    //    public override string ReportFileName
    //    {
    //        get { return string.Empty; }
    //    }

    //    public override string MainWorkSheetName
    //    {
    //        get { return ActiveWorksheet.Name; }
    //    }

    //    protected override void BuildReport()
    //    {
    //        int i = 1;
    //        foreach (var item in Repository.TryGetContext().Contractdocs.ToList())
    //        {
    //            MainWorkSheet.Cells[i, 1] = item.ToString();
    //            ++i;
    //        }
    //    }
    //}

    class StubReport : WordReportViewModel
    {
        public StubReport(IContractRepository repository)
            : base(repository)
        {
        }

        protected override string ReportFileName
        {
            get { return string.Empty; }
        }



        protected override void BuildReport()
        {

            int i = 1;

            foreach (var item in Repository.TryGetContext().Contractdocs.ToList())
            {
                Word.ActiveDocument.Range(null, null).Text = item.ToString();
                ++i;
            }
        }
    }

}
