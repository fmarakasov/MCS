using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MContracts.Controls.Dialogs;
using MContracts.ViewModel;
using MContracts.ViewModel.Reports;
using Microsoft.Office.Interop.Excel;
using Microsoft.Office.Interop.Word;

namespace MContracts.Classes
{
 
    class ExcelReportFactory
    {
        public static void CreateReport(BaseReportViewModel reportViewModel)
        {
            if (!reportViewModel.NeedsInputParameters || (reportViewModel.NeedsInputParameters && QueryInputParameters(reportViewModel) == true))
            {
                reportViewModel.SetReport();
            }
        }

        private static bool QueryInputParameters(BaseReportViewModel reportViewModel)
        {
            bool res = false;
            if (reportViewModel.Parameters != null)
            {
                var reportParamsWindow = new ReportParamsWindow(reportViewModel);
                reportParamsWindow.ShowDialog();
                if (reportParamsWindow.DialogResult == true) res = true;
            }
            else res = true;
            return res;
        }
    }

    public class WordReportFactory
    {
        public static void CreateReport(BaseReportViewModel reportViewModel)
        {
            if (!reportViewModel.NeedsInputParameters || (reportViewModel.NeedsInputParameters && QueryInputParameters(reportViewModel)))
            {
                reportViewModel.SetReport();
            }
        }

        private static bool QueryInputParameters(BaseReportViewModel reportViewModel)
        {
            bool res = false;
            if (reportViewModel.Parameters != null&&reportViewModel.Parameters.Count() > 0)
            {
                var reportParamsWindow = new ReportParamsWindow(reportViewModel);
                reportParamsWindow.ShowDialog();
                if (reportParamsWindow.DialogResult == true) res = true;
            }
            else res = true;
            return res;
        }
    }

    public class ReportFactory
    {
        public static void CreateReport(BaseReportViewModel reportViewModel)
        {
            if (!reportViewModel.NeedsInputParameters||(reportViewModel.NeedsInputParameters&&QueryInputParameters(reportViewModel) == true))
            {
                var dlg = new ReportViewerHost();
                dlg.DataContext = reportViewModel;
                dlg.Show();
            }
        }

        private static bool QueryInputParameters(BaseReportViewModel reportViewModel)
        {
            bool res = false;
            if (reportViewModel.Parameters != null)
            {
                var reportParamsWindow = new ReportParamsWindow(reportViewModel);
                reportParamsWindow.ShowDialog();
                if (reportParamsWindow.DialogResult == true) res = true;
            }
            else res = true;
            return res;
        }
    }
}
