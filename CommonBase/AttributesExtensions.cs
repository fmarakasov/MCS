using System;
using System.Collections.Generic;
using System.Linq;

namespace CommonBase
{
    /// <summary>
    /// Расширяет тип object для поддержки работы с атрибутами
    /// </summary>
    public static class AttributesExtensions
    {
        /// <summary>
        /// Получает атрибуты класса заданного типа для объекта
        /// </summary>
        /// <typeparam name="TAttrib">Тип атрибута</typeparam>
        /// <param name="obj">Объект</param>
        /// <returns>Коллекция атрибутов заданного типа</returns>
        public static IEnumerable<TAttrib> GetAttributes<TAttrib>(this object obj)
        {
            if (obj == null) throw new ArgumentNullException("obj");
            return GetAttributes<TAttrib>(obj.GetType());
        }
        /// <summary>
        /// Получает атрибуты класса заданного типа для типа
        /// </summary>
        /// <typeparam name="TAttrib">Тип атрибута</typeparam>
        /// <param name="t">Тип</param>
        /// <returns>Коллекция атрибутов заданного типа</returns>
        public static IEnumerable<TAttrib> GetAttributes<TAttrib>(this Type t)
        {
            if (t == null) throw new ArgumentNullException("t");
            return t.GetCustomAttributes(true).OfType<TAttrib>();
        }
    }
}
