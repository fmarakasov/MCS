using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MContracts.ViewModel
{
    /// <summary>
    /// Определяет типы, которые должны возвращать описание текущего договора
    /// Примечание: для работы с "текущим договором" тип должен реализовывать IContractRefHolder или/или IContractHolder
    /// </summary>
    public interface IContractCaption
    {
        /// <summary>
        /// Получает описание текущего договора
        /// </summary>
        string ContractCaption { get; }
    }
}
