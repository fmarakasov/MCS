using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MCDomain.Common
{
    /// <summary>
    /// Определяет типы, поддерживающие кэширование вычисленного состояния
    /// </summary>
    public interface ICacheble
    {
        /// <summary>
        /// Объявляет кэшированные данные объекта недействительными
        /// </summary>
        void Invalidate();
    }
}
