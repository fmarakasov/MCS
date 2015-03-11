using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace CommonBase
{
    /// <summary>
    /// Определяет класс процентов
    /// </summary>
    public static class Percent
    {
        /// <summary>
        /// Минимальный процент
        /// </summary>
        public const int Max = 100;

        /// <summary>
        /// Максимальный процент
        /// </summary>
        public const int Min = 0;

        /// <summary>
        /// Получает fraction от total в процентах
        /// </summary>
        /// <param name="fraction">Доля</param>
        /// <param name="total">Всего</param>
        /// <returns>Доля в процентах</returns>
        public static float GetPercent(decimal fraction, decimal total)
        {
            Contract.Requires(fraction >= 0);
            Contract.Requires(total > 0);
            
            return (float)(100*(fraction/total));
        }

        /// <summary>
        /// Получает долю от процента числа
        /// </summary>
        /// <param name="percent">Процент</param>
        /// <param name="total">Всего</param>
        /// <returns>Доля</returns>
        public static decimal Inverse(float percent, decimal total)
        {
            Contract.Requires(percent >= Min && percent<=Max);
            Contract.Requires(total >= 0);
            Contract.Ensures(Contract.Result<decimal>()>=0);
            return (decimal)(total * ((decimal)percent / 100));
        }
    }
}


