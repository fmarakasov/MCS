using System;
using CommonBase;

namespace MCDomain.Model
{
    /// <summary>
    /// Проверят диапазоны значений некоторых атрибутов
    /// </summary>
    internal static class ValueRangeChecker
    {
        /// <summary>
        /// Проверят значение атрибута ReportOrder
        /// </summary>
        /// <param name="value">Значение</param>
        internal static void CheckReportOrderValue(int? value)
        {
            if (value.HasValue)
            {
                if (!value.Value.Between(0, 999)) throw new ArgumentOutOfRangeException("Значение параметра должно быть между 0 и 999.");
            }
        }        
    }
    
}