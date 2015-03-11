using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MCDomain.Model
{
    /// <summary>
    /// Определяет типы, имеющие данные о порядке отображения в отчёте
    /// </summary>
    interface IReportOrderProvider
    {
        /// <summary>
        /// Получает или устанавливает номер в отчёте
        /// </summary>
        int? Reportorder { get; set; }
    }
}
