using MCDomain.Model;
using System.Collections.Generic;

namespace McReports.Common
{
    /// <summary>
    /// Определяет типы, которые возвращают коллекцию договоров для отчёта 
    /// </summary>
    public interface IReportSourceProvider
    {
        /// <summary>
        /// Коллекция договоров для отчёта
        /// </summary>
        IEnumerable<IContractStateData> Source { get; }
    }
}
