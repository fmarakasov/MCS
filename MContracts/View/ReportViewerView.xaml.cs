using System.Windows;
using System.Windows.Controls;
using MContracts.ViewModel.Reports;

namespace MContracts.View
{
    /// <summary>
    /// Interaction logic for ReportViewerView.xaml
    /// </summary>
    public partial class ReportViewerView : UserControl
    {
        private BaseReportViewModel Viewmodel;
        
        public ReportViewerView()
        {
            InitializeComponent();
            Loaded += new RoutedEventHandler(ReportViewerView_Loaded);
        }

        void ReportViewerView_Loaded(object sender, RoutedEventArgs e)
        {
            Viewmodel = this.DataContext as BaseReportViewModel;
           // LoadReport();
        }

        private void LoadReport()
        {
            //crystalReportsViewer1.ViewerCore.ReportSource = Viewmodel.Report;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            LoadReport();
        }

    }
}
