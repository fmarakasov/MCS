using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MCDomain.Common
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
   
    }
}
