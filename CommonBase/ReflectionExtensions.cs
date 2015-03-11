using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace CommonBase
{
    /// <summary>
    /// Класс расширений Reflection
    /// </summary>
    public static class ReflectionExtensions
    {
        /// <summary>
        /// Получает значение неиндексируемого свойства по имени
        /// </summary>
        /// <typeparam name="T">Тип свойства</typeparam>
        /// <param name="obj">Объект</param>
        /// <param name="propertyName">Имя свойства</param>
        /// <returns>Значение свойства</returns>
        public static T GetPropertyValue<T>(this object obj, string propertyName)
        {
            if (obj == null) throw new ArgumentNullException("obj");
            if (string.IsNullOrWhiteSpace(propertyName)) throw new ArgumentException("Имя свойства должно быть задано", "propertyName");
            var propertyInfo = obj.GetType().GetProperty(propertyName);
            Contract.Assert(propertyInfo != null);
            return (T)propertyInfo.GetValue(obj, null);
        }
        /// <summary>
        /// Устанавливает значение неиндексируемого свойства по имени
        /// </summary>
        /// <typeparam name="T">Тип свойства</typeparam>
        /// <param name="obj">Объект</param>
        /// <param name="propertyName">Имя свойства</param>
        /// <param name="value">Значение свойства</param>
        public static void SetPropertyValue<T>(this object obj, string propertyName, T value)
        {
            if (obj == null) throw new ArgumentNullException("obj");
            if (string.IsNullOrWhiteSpace(propertyName)) throw new ArgumentException("Имя свойства должно быть задано", "propertyName");
            

            var propertyInfo = obj.GetType().GetProperty(propertyName);
            Contract.Assert(propertyInfo != null);
            propertyInfo.SetValue(obj, value, null);
        }
    }
}
