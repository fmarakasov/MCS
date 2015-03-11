using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using MCDomain.Model;

namespace MContracts.ViewModel.Helpers
{
    /// <summary>
    /// Тип создаваемого договора
    /// </summary>
    enum NewContractdocType
    {
        /// <summary>
        /// Новый генеральный договор
        /// </summary>
        NewGeneral,
        /// <summary>
        /// Новое соглашение к существующему договору
        /// </summary>
        NewAgreement,
        /// <summary>
        /// Новый субподрядный договор
        /// </summary>
        NewSubgeneral,
        /// <summary>
        /// Новое соглашение к субподрядному договору
        /// </summary>
        NewAgreementToSubgeneral
    }
    /// <summary>
    /// Параметры создания нового договора
    /// </summary>
    class NewContractdocParams
    {
        /// <summary>
        /// Получает данные о генеральном договоре с которым будет производиться связка этапов.
        /// Используется только совместно с параметром NewAgreementToSubgeneral
        /// </summary>
        public Contractdoc General { get; private set; }
        /// <summary>
        /// Получает данные об оригинальном договоре.
        /// </summary>
        public Contractdoc Contractdoc { get; private set; }
        /// <summary>
        /// Получает тип создаваемого договора
        /// </summary>
        public NewContractdocType ContractdocType { get; private set; }

        /// <summary>
        /// Создаёт экземпляр класса параметров нового договора.
        /// </summary>
        /// <param name="contractdoc">Оригинальный договор на основе которого создаётся ДС или СД. Для таких договоров этот параметр не может быть null</param>
        /// <param name="contractdocType">Тип создаваемого договора</param>
        /// <param name="general">Генеральный договор или его версия с которым будет связано новое ДС к СД</param>
        public NewContractdocParams(Contractdoc contractdoc, NewContractdocType contractdocType, Contractdoc general = null)
        {
            Contractdoc = contractdoc;
            ContractdocType = contractdocType;
            General = general;
            if (contractdocType != NewContractdocType.NewGeneral) Contract.Assert(Contractdoc != null);
            //if (contractdocType == NewContractdocType.NewAgreementToSubgeneral) Contract.Assert(General != null);
        }
    }
}
