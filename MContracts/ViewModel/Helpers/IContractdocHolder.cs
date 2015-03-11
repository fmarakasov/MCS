using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MCDomain.Model;

namespace MContracts.ViewModel.Helpers
{
    /// <summary>
    /// Определяет классы, поддерживающие ссылку на объект договора
    /// </summary>
    public interface IContractdocHolder
    {
        /// <summary>
        /// Получает текущий договор
        /// </summary>
        Contractdoc SelectedContract { get; }
    }
}
