using System;
using System.Collections.Generic;
using System.Linq;

namespace CommonBase
{
    /// <summary>
    /// Расширение работы с чивловыми типами
    /// </summary>
    public static class NumericExtensions
    {
        public static bool ToBoolean(this long source)
        {
            return (source != 0);
        }

        /// <summary>
        /// Преобразует строку в коллекцию целых. Если для элемента не удаётся выполнить 
        /// преобразование, то в коллекцию помещается значение, заданное в параметре defValue
        /// </summary>
        /// <param name="value"></param>
        /// <param name="defValue"></param>
        /// <param name="separator"></param>
        /// <returns></returns>
        public  static IEnumerable<int> ParseOrDefault(this string value, int defValue, params char[] separator)
        {
            var arr = value.Split(separator);

            return arr.Select(x =>
                                  {
                                      int result;
                                      if (int.TryParse(value, out result)) return result;
                                      return defValue;
                                  });
        }




    }
}

