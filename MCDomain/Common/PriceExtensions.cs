using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MCDomain.Common
{
    public static class PriceExtensions
    {
        /// <summary>
        /// Получает сумму по коллекции типов IPrice
        /// </summary>
        /// <typeparam name="T">Тип элемента коллекции</typeparam>
        /// <param name="source">Коллекция элементов IPrice</param>
        /// <returns>Сумма</returns>
        public static decimal Total<T>(this IEnumerable<T> source) where T : IPrice
        {
            if (source == null)
                throw new ArgumentNullException("source", "Параметр source не может быть null.");
            return source.Sum(x => x.PriceValue);
        }
    }
}
