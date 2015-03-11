﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MCDomain.Common
{
    /// <summary>
    /// Определяет типы, имеющие описание цены
    /// </summary>
    public interface IPrice
    {
        /// <summary>
        /// Получает или устанавливает цену
        /// </summary>
        decimal PriceValue { get; set; }
    }

    
}