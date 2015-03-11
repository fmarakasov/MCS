using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Data;

namespace MContracts.Classes.Converters
{
    /// <summary>
    /// Класс для преобразования логического значения в строку
    /// </summary>
    public class BooleanToStringConverter : IValueConverter
    {
        /// <summary>
        /// Получает или устанавливает строку, соответствующую истине
        /// </summary>
        /// 
        public string TrueMessage { get; set; }
        /// <summary>
        /// Получает или устанавливает строку, соответствующую лжи
        /// </summary>        
        public string FalseMessage { get; set; }
        /// <summary>
        /// Получает или устанавливает строку, соответствующую не заданному значению
        /// </summary>        
        public string DefaultMessage { get; set; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Contract.Ensures(Contract.Result<object>() is string);

            if (value == null) return DefaultMessage;
            var val = (bool) value;
            return val ? TrueMessage : FalseMessage;
            
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Contract.Assert(value is string || value == null);

            if (value == null) return null;

            var val = (string) value;

            if (val == TrueMessage) return true;
            if (val == FalseMessage) return false;
            return null;
        }
    }
}
