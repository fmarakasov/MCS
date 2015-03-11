using System.Linq;
using McReports.Common;
using McReports.ViewModel;
using System.Diagnostics.Contracts;

namespace McReports
{

    public class ReportFactoryBase
    {
        /// <summary>
        /// Получает или устанавливает провайдер пользовательского интерфейса для ввода параметров запроса
        /// </summary>
        public IUiQueryParametersProvider UiQueryParametersProvider { get; set; }   
        
        public void CreateReport(BaseReportViewModel reportViewModel)
        {
            if (!reportViewModel.NeedsInputParameters || 
                (reportViewModel.NeedsInputParameters &&
                QueryInputParameters(reportViewModel)))
            {
                reportViewModel.SetReport();
            }
        }

        private bool QueryInputParameters(BaseReportViewModel reportViewModel)
        {
            Contract.Requires(reportViewModel != null);
            Contract.Requires(UiQueryParametersProvider != null);
            if (reportViewModel.Parameters == null || !reportViewModel.Parameters.Any()) return true;             
            UiQueryParametersProvider.ViewModel = reportViewModel;
            return UiQueryParametersProvider.GetParameters();                   
        }
    }

   
}
