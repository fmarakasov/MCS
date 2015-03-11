using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommonBase
{
    /// <summary>
    /// Определяет типы с поддержкой базовых свойств персоны
    /// </summary>
    public interface IPerson
    {
        /// <summary>
        /// Получает или устанавливает имя
        /// </summary>
        string Familyname { get; set; }
        /// <summary>
        /// Получает или устанавливает фамилию
        /// </summary>
        string Firstname { get; set; }
        /// <summary>
        /// Получает или устанавливает отчество
        /// </summary>
        string Middlename { get; set; }
        /// <summary>
        /// Получает или устанавливает пол
        /// </summary>
        bool? Sex { get; set; }
    }

  
}
