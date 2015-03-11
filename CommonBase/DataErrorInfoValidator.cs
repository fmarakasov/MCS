using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Reflection;
using System.Text;

namespace CommonBase
{
    public static class DataErrorInfoValidator
    {
        /// <summary>
        /// Производит обход всех публичных свойств, доступных для чтения и добпяет в результирующую строку сообщения об ошибках 
        /// </summary>
        /// <typeparam name="T">Тип объекта</typeparam>
        /// <param name="item">Объект для которого проверяются значения свойств</param>
        /// <returns>Строка с сообщениями об ошибках</returns>
        public static string Validate<T>(this T item) where T : class, IDataErrorInfo
        {
            Contract.Requires(item != null);
            var pi = item.GetType().GetProperties();            
            return pi.Select(propertyInfo => item[propertyInfo.Name]).Where(er => er != string.Empty).Aggregate(string.Empty, (current, er) => current + (er + "\n"));
        }
    }
}
