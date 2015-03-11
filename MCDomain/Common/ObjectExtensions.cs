using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Collections.ObjectModel;

namespace MCDomain.Common
{
    public static class ObjectExtensions
    {

        public static void AddRange<T>(this IList<T> source, IEnumerable<T> objsrc)
        {
            foreach(T obj in objsrc)
            {
                source.Add(obj);
            }
        }

        public static T CastTo<T>(this object source) where T : class
        {
            Contract.Requires(source != null);
            if (source is T) return source as T;
            throw new InvalidOperationException(string.Format("Невозможно привести тип {0} к {1}", source.GetType(),
                                                              typeof (T)));
        }

        /// <summary>
        /// Безопасное получение значения свойства объекта
        /// </summary>
        /// <typeparam name="T">Тип объекта</typeparam>
        /// <typeparam name="TU">Тип свойства</typeparam>
        /// <param name="source">Объект</param>
        /// <param name="selector">Селектор свойства</param>
        /// <returns>Значение свойства</returns>
        public static TU With<T, TU>(this T source, Func<T, TU> selector)
            where T : class
            where TU : class
        {
            if (null == source) return null;
            return selector(source);
        }

        /// <summary>
        /// Безопасное получение значения свойства объекта. Если объект null, то выбрасывается исключение ArgumentNullException
        /// </summary>
        /// <typeparam name="T">Тип объекта</typeparam>
        /// <typeparam name="TU">Тип свойства</typeparam>
        /// <param name="source">Объект</param>
        /// <param name="selector">Селектор свойства</param>
        /// <returns>Значение свойства</returns>
        /// <exception>ArgumentNullException</exception>
        public static TU WithAssert<T, TU>(this T source, Func<T, TU> selector)
            where T : class
            where TU : class
        {
            if (null == source) throw new ArgumentNullException("source", "Аргумент не может быть null!");
            return selector(source);
        }

        public static TResult Return<TInput, TResult>(this TInput o, Func<TInput, TResult> evaluator,
                                                      TResult failureValue) where TInput : class
        {

            if (o == null) return failureValue;

            return evaluator(o);
        }

        public static TInput If<TInput>(this TInput o, Func<TInput, bool> evaluator) where TInput : class
        {
            if (o == null) return null;
            return evaluator(o) ? o : null;
        }

        public static TInput Unless<TInput>(this TInput o, Func<TInput, bool> evaluator) where TInput : class
        {
            if (o == null) return null;
            return evaluator(o) ? null : o;
        }

        public static TInput Do<TInput>(this TInput o, Action<TInput> action) where TInput : class
        {
            if (o == null) return null;
            action(o);
            return o;
        }


    }
}
