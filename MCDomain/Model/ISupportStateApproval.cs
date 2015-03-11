using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MCDomain.Model
{
    /// <summary>
    /// Определяет типы, для которых может быть задано свойство состояния согласования
    /// </summary>
    public interface ISupportStateApproval
    {
        /// <summary>
        /// Получает или устанавливает состояние согласования 
        /// </summary>
        Approvalstate Approvalstate { get; set; }

        /// <summary>
        /// Получает битовую карту типа, для фильтрации подходящих типов согласования
        /// </summary>
        byte TypeMask { get; }

        /// <summary>
        /// Получает или устанавливает описание состояние процесса согласования
        /// </summary>
        string Statedescription { get; set; }
        
        /// <summary>
        /// Получает или устанавливает дату состояния
        /// </summary>
        DateTime? Statedate { get; set; }
    }


}
