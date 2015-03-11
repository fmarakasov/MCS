using System.Windows;
using MContracts.ViewModel.Reports;

namespace MContracts.Controls.Dialogs
{


    /// <summary>
    /// Interaction logic for ReportViewerHost.xaml
    /// </summary>
    public partial class ReportViewerHost : Window
    {
        

        public ReportViewerHost()
        {
            InitializeComponent();
            
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var viewModel = this.DataContext as BaseReportViewModel;

            if (viewModel == null) return;

            //Настройки параметров экспорта
            //var crFormatTypeOptions = new ExcelFormatOptions
            //                              {
            //                                  ExcelAreaType = AreaSectionKind.Detail,
            //                                  ExportPageHeadersAndFooters = ExportPageAreaKind.OnEachPage,
            //                                  UExcelAreaType = 150,
            //                                  ExcelTabHasColumnHeadings = true,
            //                                  ShowGridLines = true
            //                              };
            //viewModel.Report.ExportOptions.ExportFormatOptions = crFormatTypeOptions;
         


            //crystalReportsViewer1.ViewerCore.ReportSource = viewModel.Report;
        }
    }
}
