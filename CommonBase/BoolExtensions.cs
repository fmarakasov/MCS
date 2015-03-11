using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommonBase
{
    /// <summary>
    /// Расширение приведения типовот логическго к числовому и обратно
    /// </summary>
    public static class BoolExtensions
    {        

        /// <summary>
        /// Представляет число с плавающей запятой как логический тип
        /// </summary>
        /// <param name="value">Число</param>
        /// <returns>Логическое</returns>
        public static bool ToBoolean(this double value)
        {
            return value != 0.0;
        }

        public static bool? ToBoolean(this double? value)
        {
            return value.HasValue ? ToBoolean(value.Value) : default(bool?);
        }
        /// <summary>
        /// Представляет целое число как логическое
        /// </summary>
        /// <param name="value">Число</param>
        /// <returns>Логическое</returns>
        public static bool ToBoolean(this int value)
        {
            return value != 0;
        }
        /// <summary>
        /// Представляет целое число как логическое
        /// </summary>
        /// <param name="value">Число</param>
        /// <returns>Логическое</returns>
        public static bool? ToBoolean(this int? value)
        {
            return value.HasValue ? ToBoolean(value.Value) : default(bool?);
        }
        /// <summary>
        /// Представляет целое число как логическое. Если значение value не задано, то возвращает default(bool)
        /// </summary>
        /// <param name="value">Число</param>
        /// <returns>Логическое</returns>
        public static bool ToBooleanOrDefault(this int? value)
        {
            return value.HasValue ? ToBoolean(value.Value) : default(bool);
        }
        /// <summary>
        /// Представляет логическое как число с вещественное 
        /// </summary>
        /// <param name="value">Логическое</param>
        /// <returns>Число</returns>
        public static double ToDouble(this bool value)
        {
            return (value) ? 1.0 : 0.0;
        }
         
        /// <summary>
        /// Представляет логическое как целое
        /// </summary>
        /// <param name="value">Логическое</param>
        /// <returns>Целое</returns>
        public static int ToInt(this bool value)
        {
            return (value) ? 1 : 0;
        }

        public static int? ToInt(this bool? value)
        {
            return (value.HasValue) ? ToInt(value.Value) : default(int?);
        }

        public static double? ToDouble(this bool? value)
        {
            return (value.HasValue) ? ToDouble(value.Value) : default(double?);
        }
    }
   
}
