using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;

namespace McReports.Common
{
    /// <summary>
    /// Интерфейс типов, которые могут конструировать ObjectDataProvider
    /// </summary>
    public interface IObjectDataProvider
    {
        /// <summary>
        /// Получить объект ObjectDataProvider
        /// </summary>
        /// <returns>объект ObjectDataProvider</returns>
        ObjectDataProvider GetDataProvider();
    }
}
