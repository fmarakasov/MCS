using System;

namespace MCDomain.Model
{
    /// <summary>
    /// Типы, представляющие текущее время
    /// </summary>
    public interface ITimeResolver
    {
        /// <summary>
        /// Получает текущее время
        /// </summary>
        DateTime Now { get; }
    }
}