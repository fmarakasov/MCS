using System;

namespace MContracts.ViewModel
{
    /// <summary>
    /// Строка таблицы реестра с данными об акте
    /// </summary>
    public class ActDto
    {
        public long Id;
        public string Num { get; set; }
        public DateTime? Signdate { get; set; }
        public bool? Issigned { get; set; }
        public string Stages { get; set; }
        public decimal TotalSumWithNds { get; set; }
        public decimal TotalSumWithNoNds { get; set; }
        public decimal TotalSumNds { get; set; }
        public decimal TransferSum { get; set; }
        public decimal PrepaymentWithNds { get; set; }
        public decimal PrepaymentWithNoNds { get; set; }
        /// <summary>
        /// Получает или устанавливает признак, что акт был создан для этапа в этой версии договора
        /// </summary>
        public bool IsClosedByThisContract { get; set; }
        /// <summary>
        /// Получает или устанавливает название типа акта подписания
        /// </summary>
        public string Acttype { get; set; }

        /// <summary>
        /// Получает или устанваливает признак возможности редактирования акта.
        /// Акт может бвть отредактирован, если он был создан для этапа КП этого договора (IsClosedByThisContract), либо 
        /// договор является ДС с неоткреплённым календарным планом.
        /// </summary>
        public bool CanEdit { get; set; }
    }
}