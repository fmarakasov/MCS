using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommonBase
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
