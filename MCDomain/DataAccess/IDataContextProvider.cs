using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CommonBase;

namespace MCDomain.DataAccess
{
    /// <summary>
    /// Обеспечивает получение нового экземпляра контекста данных
    /// </summary>
    public interface IDataContextProvider
    {
        /// <summary>
        /// Создаёт новый экземпляр контекста данных
        /// </summary>
        /// <returns>Контекст данных</returns>
        MCDomain.Model.McDataContext CreateContext();

        /// <summary>
        /// Событие, которое будет вызвано, если подключение закончится с исключением
        /// </summary>
        event EventHandler<EventParameterArgs<Exception>> CreateFailed;
    }
}
