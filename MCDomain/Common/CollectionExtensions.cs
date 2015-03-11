using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MCDomain.Common
{
    public static class CollectionExtensions
    {
        private static readonly IDictionary<Type, object> Cache = new Dictionary<Type, object>();

        /// <summary>
        /// Управляет созданием экземпляров заданного типа, позволяя иметь не более одного экземпляра 
        /// </summary>
        /// <typeparam name="T">Запрашиваемый тип экземпляра</typeparam>
        /// <param name="source"></param>
        /// <returns>Объект  заданного типа</returns>
        public static T Singleton<T>(this Type source) where T : new()
        {
            object result;
            if (!Cache.TryGetValue(typeof (T), out result))
            {
                result = new T();
                Cache.Add(typeof (T), result);
            }
            return (T) result;
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
            return source.Select(item => source.Count(item.Equals)).All(matches => matches <= 1);
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
    }
}
