using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MCDomain.Common
{
    /// <summary>
    /// Класс, расширяющий возможности IComparable
    /// </summary>
    public static class ComparableExtensions
    {
        /// <summary>
        /// Проверяет находится ли элемент в заданном диапазоне, используя нестрогое сравнение
        /// </summary>
        /// <param name="source">элемент</param>
        /// <param name="min">нижняя граница диапазона</param>
        /// <param name="max">верхняя граница диапазона</param>
        /// <returns>Истина, если элемент находится в заданном диапазоне</returns>
        public static bool Between<T>(this T source, T min, T max) where T:IComparable
        {
            return (source.CompareTo(min) >= 0) && (source.CompareTo(max) <= 0);
        }

        /// <summary>
        /// Проверяет находится ли элемент в заданном диапазоне, используя строгое сравнение
        /// </summary>
        /// <param name="source">элемент</param>
        /// <param name="min">нижняя граница диапазона</param>
        /// <param name="max">верхняя граница диапазона</param>
        /// <returns>Истина, если элемент находится в заданном диапазоне</returns>
        public static bool StrictBetween<T>(this T source, T min, T max) where T : IComparable
        {
            return (source.CompareTo(min) > 0) && (source.CompareTo(max) < 0);
        }

        public static bool In<T>(this T value, params T[] checkSet) where T: IComparable
        {
            return checkSet.Any(x =>x.CompareTo(value) == 0);
        }
    }
}
