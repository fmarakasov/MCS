using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MCDomain.Common
{
    interface IGenericQueue<T>
    {
        void Push(T item);
        T Pop();
        int Count { get; }

    }
    class StackQueue<T>:IGenericQueue<T>
    {
        readonly Stack<T> _stack = new Stack<T>(); 
        public void Push(T item)
        {
           _stack.Push(item);
        }

        public T Pop()
        {
            return _stack.Pop();
        }

        public int Count
        {
            get { return _stack.Count; }
        }
    }
    class QueueQueue<T>:IGenericQueue<T>
    {
        readonly Queue<T> _queue = new Queue<T>(); 
        public void Push(T item)
        {
           _queue.Enqueue(item);
        }

        public T Pop()
        {
            return _queue.Dequeue();
        }

        public int Count
        {
            get { return _queue.Count; }
        }
    }

    /// <summary>
    /// Определяет методы для осуществления поиска в пространстве состояний
    /// </summary>
    public static class SearchExtentions
    {

        private static IEnumerable<T> CommonDepthSearch<T, TCollection>(T source, Func<T, IEnumerable<T>> childSelector, 
            Func<T, bool> predicate, Func<T, bool> stopCondition = null) where TCollection : IGenericQueue<T>, new()
        {
            if (childSelector == null) throw new ArgumentNullException("childSelector");
            var stack = new TCollection();
            
            stack.Push(source);
        
       
            while (stack.Count != 0)
            {
                var item = stack.Pop();
                
                if (predicate(item))
                    yield return item;

                if (stopCondition != null)
                    if (stopCondition(item)) yield break;

                foreach (var child in childSelector(item))
                {
                    stack.Push(child);
                }

            }
        }


        /// <summary>
        /// Возвращает все элементы, достижимые из заданного. Используется правосторонний обход в глубину
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="childSelector"></param>

        /// <param name="predicate"> </param>
        /// <param name="stopCondition"> </param>
        /// <returns></returns>
        public static IEnumerable<T> RightDepthSearch<T>(this T source, Func<T, IEnumerable<T>> childSelector, Func<T, bool> predicate, Func<T, bool> stopCondition = null)
        {

            return CommonDepthSearch<T, StackQueue<T>>(source, childSelector, predicate, stopCondition);
        }

        public static IEnumerable<T> LeftDepthSearch<T>(this T source, Func<T, IEnumerable<T>> childSelector,  Func<T, bool> predicate, Func<T, bool> stopCondition = null)
        {

            return CommonDepthSearch<T, QueueQueue<T>>(source, childSelector, predicate, stopCondition);
        }

        /// <summary>
        /// Возвращает коллекцию элементов достижимых обратным проходом по дереву
        /// </summary>
        /// <typeparam name="T">Тип элемента коллекции</typeparam>
        /// <param name="source">Объект</param>
        /// <param name="parentSelector">Определяет свойство, возвращающее родительский элемент</param>

        /// <param name="predicate">Условие включения элемента в список</param>
        /// <returns>Коллекция достижимых объектов</returns>
        public static IEnumerable<T> GetBackwardPath<T>(this T source, Func<T, T> parentSelector, Func<T, bool> predicate)
        {
            if (parentSelector == null) throw new ArgumentNullException("parentSelector");
            if (predicate == null) throw new ArgumentNullException("predicate");

            T current = source;
            while (!Equals(current, default(T)))
            {
                if (predicate(current)) 
                    yield return current;
                current = parentSelector(current);
            }
        }
    }
}
