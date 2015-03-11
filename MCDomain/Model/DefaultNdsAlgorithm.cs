using System;
using System.Diagnostics.Contracts;

namespace MCDomain.Model
{
    /// <summary>
    /// Определяет расчёт при отсутствии НДС
    /// </summary>
    public class DefaultNdsAlgorithm
    {
        /// <summary>
        /// Получает экземпляр класса
        /// </summary>
        public static DefaultNdsAlgorithm CreateInstance(TypeOfNds ndsType)
        {
            switch (ndsType)
            {
                case TypeOfNds.IncludeNds:
                    return new NdsIncludedAlgorithm();
                case TypeOfNds.ExcludeNds:
                    return new NdsExcludedAlgorithm(); 
                default:
                    return new DefaultNdsAlgorithm();                     
            }            
        }


        private static void CheckArguments(decimal price, double fraction)
        {
            /*
            if (price < 0)
                throw new ArgumentOutOfRangeException("price", "Цена не может быть отрицательной.");
            if ((fraction < 0) || (fraction > 1))
                throw new ArgumentOutOfRangeException("fraction", "Доля НДС должена быть от 0 до 1.");
            Contract.EndContractBlock();
             * */

        }
        /// <summary>
        /// Реализация расчёта НДС. Переопределите в производных классах для правильного расчёта НДС
        /// </summary>
        /// <param name="price">Цена</param>
        /// <param name="fraction">Доля НДС</param>
        /// <returns>НДС</returns>
        protected virtual decimal CalcNds(decimal price, double fraction)
        {
            return 0;
        }

        /// <summary>
        /// Расчёт НДС
        /// </summary>
        /// <param name="price">Цена</param>
        /// <param name="fraction">Доля НДС</param>
        /// <returns>НДС</returns>
        public decimal GetNds(decimal price, double fraction)
        {
            CheckArguments(price, fraction);
            return CalcNds(price, fraction);
        }

        /// <summary>
        /// Получает цену договора без НДС
        /// </summary>
        /// <param name="price">Цена договора</param>
        /// <param name="fraction">Доля НДС</param>
        /// <returns>Цена без НДС</returns>
        public virtual decimal GetPure(decimal price, double fraction)
        {
            return price - GetNds(price, fraction);
        }


    }
}