using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Reflection;

namespace CommonBase
{
    /// <summary>
    /// Возвращает заданное сообщение об ошибке для свойств, помеченных как NOT NULL
    /// </summary>
    /// <typeparam name="T">Тип</typeparam>
    public class DataErrorHandlerForNonNullable<T> : IDataErrorHandler<T>
    {
        public string ErrorMessage { get; set; }

        public string GetError(T source, string propertyName, ref bool handled)
        {
            PropertyInfo pi = source.GetType().GetProperty(propertyName);
            
            if (pi == null) return null;
            
            var columnAttributes = pi.GetCustomAttributes(false).OfType<ColumnAttribute>();
            
            if (columnAttributes.Count() > 0)
            {
                return HandleNonNullableError(source, pi, columnAttributes, out handled);
            }
            return string.Empty;
        }

        private string HandleNonNullableError(T source, PropertyInfo pi, IEnumerable<ColumnAttribute> columnAttributes, out bool handled)
        {
            Contract.Requires(pi != null);
            Contract.Requires(columnAttributes!=null);

            var ca = columnAttributes.First();
            if (!ca.CanBeNull)
            {
                object value = pi.GetValue(source, null);
                if (value == null)
                {
                    handled = true;
                    return ErrorMessage;
                }
                if (pi.PropertyType == typeof(string))
                {
                    if (String.IsNullOrWhiteSpace(value.ToString()))
                    {
                        handled = true;
                        return ErrorMessage;
                    }

                }
            }
            handled = false;
            return string.Empty;
        }
    }
}