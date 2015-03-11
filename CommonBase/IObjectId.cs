using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommonBase
{
    /// <summary>
    /// Определяет типы, которые имеют идентификатор
    /// </summary>
    public interface IObjectId
    {
        /// <summary>
        /// Получает или устанавливает идентификатор объекта
        /// </summary>
        long Id { get; set; }
        /// <summary>
        /// Возвращает сведения о том, является ли указанный Id предопределенным
        /// </summary>
        /// <returns></returns>
        bool IsWellKnownId();

    }
}
