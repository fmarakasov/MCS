using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace MCDomain.Common
{
    /// <summary>
    /// Определяет типы у которых есть атрибут имени
    /// </summary>
    public interface INamed
    {
        /// <summary>
        /// Получает или устанавливает имя объекта
        /// </summary>
        string Name { get; set; }
    }

    /// <summary>
    /// Возвращает результат сравнения двух типов поддерживающих интерфейс INamed
    /// </summary>
    public static class ComparableNamed
    {
        /// <summary>
        /// Сравнивает два объекта типа INamed по именам
        /// </summary>
        /// <typeparam name="T">Тип INamed</typeparam>
        /// <param name="obj">Первый объект</param>
        /// <param name="other">Второй объект</param>
        /// <returns>Реузультат сравнения имён</returns>
        public static int CompareNames<T>(this T obj, T other) where T : INamed
        {
            if (obj == null)
                throw new ArgumentNullException("obj", "Параметр other должен быть задан.");

            if (other == null)
                throw new ArgumentNullException("other", "Параметр other должен быть задан.");

            if (string.IsNullOrEmpty(obj.Name) || (string.IsNullOrEmpty(other.Name)))
                throw new NoNullAllowedException("Свойство Name должно быть задано.");

            return obj.Name.CompareTo(other.Name);
        }        
    }
}
