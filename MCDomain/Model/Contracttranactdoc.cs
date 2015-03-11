using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace MCDomain.Model
{
    /// <summary>
    /// Типы документов описи
    /// </summary>
    public enum TrandocType
    {
        /// <summary>
        /// Документ описи для акта
        /// </summary>
        [Description("Aкт")] Act,

        /// <summary>
        /// Документ описи для договора
        /// </summary>
        [Description("Договор")] Contract,

        /// <summary>
        /// Тип документа не задан
        /// </summary>
        [Description("Не задан")] Unknonw
    }

    partial class Contracttranactdoc : IDocumentSetEntry
    {
        public TrandocType DocType
        {
            get
            {
                return (Act != null)
                           ? TrandocType.Act
                           : (Contractdoc != null) ? TrandocType.Contract : TrandocType.Unknonw;
            }
        }
        public override string ToString()
        {
            switch (DocType)
            {
                case TrandocType.Act:
                    return Act.ToString();
                case TrandocType.Contract:
                    return Contractdoc.ToString();
                case TrandocType.Unknonw:
                    return "Тип акта не задан";
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
