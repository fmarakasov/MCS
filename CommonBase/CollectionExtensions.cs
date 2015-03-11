using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Threading.Tasks;

namespace CommonBase
{
    public static class CollectionExtensions
    {
        /// <summary>
        /// Добавление коллекции к списку
        /// </summary>
        /// <typeparam name="T">Тип элемента коллекции</typeparam>
        /// <param name="source">Список</param>
        /// <param name="objsrc">Коллекция</param>
        public static void AddRange<T>(this IList<T> source, IEnumerable<T> objsrc)
        {
            foreach (var obj in objsrc)
            {
                source.Add(obj);
            }
        }

        /// <summary>
        /// Проверяет, содержит ли коллекция только уникальные элементы
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static bool AllUnique<T>(this IEnumerable<T> source) where T : IEquatable<T>
        {
            if (source == null) throw new ArgumentNullException("source");
            var enumerable = source as T[] ?? source.ToArray();
            return enumerable.Select(item => enumerable.Count(item.Equals)).All(matches => matches <= 1);
        }

       /// <summary>
       /// Сравнивает коллекцию с элементами различных типов с использованием заданного выражения сравнения
       /// </summary>
       /// <typeparam name="T">Тип элементов коллекции</typeparam>
       /// <typeparam name="TU">Тип элементов другой коллекции</typeparam>
       /// <param name="source">Коллекция</param>
       /// <param name="other">Коллекция с которой проводится сравнение</param>
       /// <param name="compareExpression">Выражение для сравнения элементов двух коллекций</param>
       /// <returns>Признак равенства коллекций</returns>
        public static bool IsEquals<T, TU>(this IEnumerable<T> source, IEnumerable<TU> other,
                                          Func<T, TU, bool> compareExpression)
        {
            if (source == null) throw new ArgumentNullException("source");
            if (other == null) throw new ArgumentNullException("other");
            if (compareExpression == null) throw new ArgumentNullException("compareExpression");
            var sourceArray = source.ToArray();
            var otherArray = other.ToArray();
            return sourceArray.Count() == otherArray.Count() && 
                sourceArray.Select(item => otherArray.Any(otherItem => 
                    compareExpression(item, otherItem))).All(contains => contains);
        }

        /// <summary>
        /// Выводит коллекцию на консоль
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        public static void WriteLine<T>(this IEnumerable<T> source)
        {
            if (source == null) throw new ArgumentNullException("source");
            foreach (var item in source)
            {
                Console.WriteLine(item);
            }
        }
        /// <summary>
        /// Применяет заданную операцию к каждому элементу коллекции параллельно
        /// </summary>
        /// <typeparam name="T">Тип элемента коллекции</typeparam>
        /// <param name="source">Коллекция</param>
        /// <param name="action">Действие над элементом</param>
        public static void ParallelApply<T>(this IEnumerable<T> source, Action<T> action)
        {
            Contract.Requires(source != null);
            Contract.Requires(action != null);
            Parallel.ForEach(source, action);
        }

        /// <summary>
        /// Применяет заданную операцию к каждому элементу коллекции
        /// </summary>
        /// <typeparam name="T">Тип элемента коллекции</typeparam>
        /// <param name="source">Коллекция</param>
        /// <param name="action">Действие над элементом</param>
        public static void Apply<T>(this IEnumerable<T> source, Action<T> action)
        {
            foreach (var item in source)
            {
                action(item);
            }
        }
    }
}
