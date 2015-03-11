using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Text;

namespace CommonBase
{    
    /// <summary>
    /// Определяет типы, которые могут возвращать статус ошибки для заданного типа
    /// </summary>
    /// <typeparam name="T">Тип</typeparam>        
    public interface IDataErrorHandler<in T>
    {
        string GetError(T source, string propertyName, ref bool handled);
    }

    /// <summary>
    /// Коллекция обработчиков статуса ошибки
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class DataErrorHandlers<T> : List<IDataErrorHandler<T>>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public string HandleError(T sender, string propertyName)
        {
            string result;
            bool handled = false;
            foreach (var item in this)
            {
                result = item.GetError(sender, propertyName, ref handled);
                if (handled)
                    return result;
            }
            return string.Empty;
        }
    }
}
