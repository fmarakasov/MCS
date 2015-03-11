using McReports.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace McReports
{
    /// <summary>
    ///Используется для описания классов для запроса параметров отчёта от пользователя
    /// </summary>
    public interface  IUIQueryParametersProvider
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
