using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;

namespace CommonBase
{
    /// <summary>
    /// Расширение класса строк за счёт интерпретации строк как диапазона значений
    /// </summary>
    public static class StringRangesParserExtensions
    {

        /// <summary>
        /// Позволяет получить из строки заданные диапазоны значений в виде коллекции, входящих в этот диапазон значений.
        /// Формат задания диапазонов соответствует заданию строк печати в Microsoft Office
        /// </summary>
        /// <param name="source">Строка</param>
        /// <returns>Коллекция значений диапазона</returns>
        public static IEnumerable<int> ParseRanges(this string source)
        {
            Contract.Requires(!string.IsNullOrWhiteSpace(source));
            var ranges = source.Split(',');
            foreach (var contRange in ranges.Select(range => range.Split('-')))
            {
                if (contRange.Length == 1) yield return Convert.ToInt32(contRange[0].Trim());
                else
                {
                    var low = Convert.ToInt32(contRange[0].Trim());
                    var high = Convert.ToInt32(contRange[contRange.Length - 1].Trim());
                    for (var i = low; i <= high; ++i)
                    {
                        yield return i;
                    }
                }
            }
        }

        /// <summary>
        /// Проверят содежит ли строка специальные символы для задания шаблона
        /// </summary>
        /// <param name="source">Строка</param>
        /// <returns>Признак наличия специальных символов в строке</returns>
        public static bool IsWildcardPattern(this string source)
        {
            return source.Contains("*") || source.Contains("?");
        }

        /// <summary>
        /// Проверят строку на соответствие шаблону
        /// </summary>P:\Projects\TAFT PROJECTS\MC\CommonBase\ReflectionExtensions.cs
        /// <param name="input">Строка</param>
        /// <param name="pattern">Шаблон строки</param>
        /// <returns>Признак соответствия строки шаблону</returns>
        public static bool IsMatchWildcard(this string input, string pattern)
        {
            var exp = new Wildcard(pattern);
            return exp.IsMatch(input);

        }

    }
}

