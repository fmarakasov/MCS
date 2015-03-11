using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Collections.ObjectModel;

namespace CommonBase
{
    /// <summary>
    /// Класс расширений, применимый для базового типа object
    /// </summary>
    public static class ObjectExtensions
    {
        /// <summary>
        /// Кэш объектов одиночек
        /// </summary>
        private static readonly IDictionary<Type, object> Cache = new Dictionary<Type, object>();

        /// <summary>
        /// Проверка объекта на null
        /// </summary>
        /// <typeparam name="T">Тип объекта</typeparam>
        /// <param name="source">Объект</param>
        /// <returns>Истина, если объект null</returns>
        public static bool IsNull<T>(this T source) where T : class
        {
            return source == null;
        }

        /// <summary>
        /// Позволяет вернуть объект как коллекцию одного элемента
        /// </summary>
        /// <typeparam name="T">Тип объекта</typeparam>
        /// <param name="item">Объект</param>
        /// <returns>Коллекция из элемента item</returns>
        public static IEnumerable<T> AsSingleElementCollection<T>(this T item)
        {
            yield return item;
        }

        /// <summary>
        /// Управляет созданием экземпляров заданного типа, позволяя иметь не более одного экземпляра 
        /// </summary>
        /// <typeparam name="T">Запрашиваемый тип экземпляра</typeparam>
        /// <param name="source"></param>
        /// <returns>Объект  заданного типа</returns>
        public static T Singleton<T>(this Type source) where T : new()
        {
            object result;
            if (!Cache.TryGetValue(typeof(T), out result))
            {
                result = new T();
                Cache.Add(typeof(T), result);
            }
            return (T)result;
        }
        /// <summary>
        /// Преобразует тип объекта к заданному. Если преобразование невозможно, то выбрасывается исключение InvalidOperationException
        /// </summary>
        /// <typeparam name="T">Новый тип объекта</typeparam>
        /// <param name="source">Объект</param>
        /// <returns>Объект, преобразованный к новому типу</returns>
        /// <exception cref="InvalidOperationException">Если преобразование невозможно</exception>
        public static T CastTo<T>(this object source) where T : class
        {
            Contract.Requires(source != null);
            if (source is T) return source as T;
            throw new InvalidOperationException(String.Format("Невозможно привести тип {0} к {1}", source.GetType(),
                                                              typeof (T)));
        }

        /// <summary>
        /// Преобразует тип к заданному. 
        /// Если параметр равен null, то метод возвращает null, 
        /// если параметр отличен от null, но не является заданным типом, то выбрасывается исключение
        /// InvalidOperationException
        /// </summary>
        /// <typeparam name="T">Тип</typeparam>
        /// <param name="source">Объект</param>
        /// <returns>Объект заданного типа, либо null</returns>
        public static T NullCastTo<T>(this object source) where T : class
        {
            if (source == null) return null;
            if (source is T) return source as T;
            throw new InvalidOperationException(String.Format("Невозможно привести тип {0} к {1}", source.GetType(),
                                                              typeof(T)));
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
            return null == source ? null : selector(source);
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

        /// <summary>
        /// Безопасный возврат результата: метод возвращает значение, заданное аргументом failureValue, 
        /// если аргумент null, в противном случае возвращается значение вычисления выражения evaluator
        /// </summary>
        /// <typeparam name="TInput">Тип объекта</typeparam>
        /// <typeparam name="TResult">Тип выходного значения</typeparam>
        /// <param name="o">Объект</param>
        /// <param name="evaluator">Выражение, вычисляющее результат от входного аргумента</param>
        /// <param name="failureValue">Результат, в случае равенства null аргумента</param>
        /// <returns>Результат evaluator либо failureValue</returns>
        public static TResult Return<TInput, TResult>(this TInput o, Func<TInput, TResult> evaluator,
                                                      TResult failureValue) where TInput : class
        {
            return o == null ? failureValue : evaluator(o);
        }

        public static TResult IfReturn<TInput, TResult>(this TInput o, Func<TInput, TResult> evaluator,
                                                      TResult failureValue) where TInput : class
        {
            return o == null ? failureValue :  evaluator(o);
        }

        /// <summary>
        /// Безопасное условие: возвращается объект, если он не равен null и выражение evaluator вернуло true, в противном случае возвращается null
        /// </summary>
        /// <typeparam name="TInput">Тип объекта</typeparam>
        /// <param name="o">Объект</param>
        /// <param name="evaluator">Выражение, возвращающее  истинность условия для объекта </param>
        /// <returns>Объект либо null</returns>
        public static TInput If<TInput>(this TInput o, Func<TInput, bool> evaluator) where TInput : class
        {
            return o == null ? null : (evaluator(o) ? o : null);
        }

        /// <summary>
        /// Безопасное условие: возвращается объект, если он не равен null и выражение evaluator вернуло false, в противном случае возвращается null
        /// </summary>
        /// <typeparam name="TInput">Тип объекта</typeparam>
        /// <param name="o">Объект</param>
        /// <param name="evaluator">Выражение, возвращающее  истинность условия для объекта </param>
        /// <returns>Объект либо null</returns>
        public static TInput Unless<TInput>(this TInput o, Func<TInput, bool> evaluator) where TInput : class
        {
            return o == null ? null : (evaluator(o) ? null : o);
        }

        /// <summary>
        /// Безопасное действие над объектом: если объект не равен null, то выполняется действие, заданное выражением action
        /// </summary>
        /// <typeparam name="TInput">Тип объекта</typeparam>
        /// <param name="o">Объект</param>
        /// <param name="action">Выражение действия над объектом</param>
        /// <returns>Объект</returns>
        public static TInput Do<TInput>(this TInput o, Action<TInput> action) where TInput : class
        {
            if (o == null) return null;
            action(o);
            return o;
        }
    }
}
