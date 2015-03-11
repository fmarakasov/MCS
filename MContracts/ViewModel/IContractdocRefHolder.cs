using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MContracts.ViewModel
{
    /// <summary>
    /// Определяет типы, ссылающиеся на договор через его иденитфикатор
    /// </summary>
    public interface IContractdocRefHolder
    {
        /// <summary>
        /// Получает идентификатор договора. Если договор не задан, то возвращается default(long?)
        /// </summary>
        long? ContractdocId { get; }

        /// <summary>
        /// Получает идентификатор группы договоров, т.е. идентификатор генерального договора оригинальной версии
        /// </summary>
        long? Maincontractid { get; }
    }

}
