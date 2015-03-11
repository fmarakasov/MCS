using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CommonBase;

namespace MCDomain.Model
{
    /// <summary>
    /// Определяет типы с поддержкой единицы измерения стоимости
    /// </summary>
    public interface IMeasureSupport : IPrice 
    {
        /// <summary>
        /// Получает единицу измерения стоимости
        /// </summary>
        Currencymeasure Measure { get; } 
    }
}
