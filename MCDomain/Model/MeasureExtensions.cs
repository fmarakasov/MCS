using System.Diagnostics.Contracts;
using CommonBase;

namespace MCDomain.Model
{
    public static class MeasureExtensions
    {
        /// <summary>
        /// Возвращает стоимость в заданных единицах измерения
        /// </summary>
        /// <param name="source">Объект стоимости</param>
        /// <param name="toFactor">Новая единица измерения</param>
        /// <returns>Стоимость в новой единице измерения</returns>
        public static decimal? ToFactor(this IMeasureSupport source, int toFactor)
        {
            Contract.Requires(source != null);
            return source.PriceValue*source.Measure.Return(x => x.Factor, default(decimal?)) / toFactor;
        }
    }
}