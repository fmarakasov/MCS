using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace MContracts.Classes
{
    public static class ViewModelExtensions
    {
        /// <summary>
        /// Применяет заданную операцию к изменившимся значениям свойства зависимостей заданного типа
        /// </summary>
        /// <typeparam name="T">Тип свойства зависимостей</typeparam>
        /// <param name="e">Аргументы</param>
        /// <param name="action">Действие</param>
        public static void DependencyPropertyAction<T>(this DependencyPropertyChangedEventArgs e,  Action<T, T> action ) where T : class
        {
            var oldVal = e.OldValue as T;
            var newVal = e.NewValue as T;
            action(oldVal, newVal);

        }
    }
}
