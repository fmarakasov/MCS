using McReports.ViewModel;

namespace McReports.Common
{
    /// <summary>
    ///Используется для описания классов для запроса параметров отчёта от пользователя
    /// </summary>
    public interface  IUiQueryParametersProvider
    {
        /// <summary>
        /// Получает или устанавливает модель представления для отчёта
        /// </summary>
        BaseReportViewModel ViewModel { get; set; }

        /// <summary>
        /// Вызывает пользовательский элемент для запроса параметров отчёта
        /// </summary>
        /// <returns>Результат вызова диалога</returns>
        bool GetParameters();
    }
    
}
