using System.Collections.Generic;
using MCDomain.Model;

namespace MContracts.ViewModel
{
    /// <summary>
    /// Используется для запроса генерального договора для ДС к СД
    /// </summary>
    public class SelectContractdocArgs
    {
        /// <summary>
        /// Получает коллекцию договоров из которых нужно выбрать генеральный
        /// </summary>
        public List<Contractdoc> Contracts { get; private set; } 
        /// <summary>
        /// Получает ДС к СД для которого нужно выбрать генеральный
        /// </summary>
        public Contractdoc NewSubAgreement { get; private set; }
        /// <summary>
        /// Получает или устанавливает генеральный договор
        /// </summary>
        public Contractdoc General { get; set; }
        /// <summary>
        /// Создаёт экземпляр SelectContractdocArgs для указания генерального договора для ДС к СД
        /// </summary>
        /// <param name="newSubAgreement"></param>
        public SelectContractdocArgs(Contractdoc newSubAgreement, List<Contractdoc> contracts)
        {
            NewSubAgreement = newSubAgreement;
            Contracts = contracts;
        }
    }
}