using System.Collections.Generic;

namespace MCDomain.Common
{
    /// <summary>
    /// Интерфейс для типов, реализующих структуру типа дерево
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface ITree<T>
    {
        /// <summary>
        /// Получает или устанавливает значение элемента дерева
        /// </summary>
        T Item { get; set; }
        /// <summary>
        /// Получает коллекцию дочерних узлов дерева
        /// </summary>
        IEnumerable<ITree<T>> Childs { get; }
    }
}