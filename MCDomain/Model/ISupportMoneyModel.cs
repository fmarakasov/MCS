using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MCDomain.Model
{
    /// <summary>
    /// Определяет типы, поддерживающие MoneyModel
    /// </summary>
    public interface ISupportMoneyModel
    {
        /// <summary>
        /// Получает данные о деньгах по заданному свойству объекта
        /// </summary>
        /// <param name="moneySubject">Название свайства</param>
        /// <returns>Объект MoneyModel</returns>
        MoneyModel this[string moneySubject] { get; }
    }
    
}
