using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Data;

namespace MContracts.Classes.Converters
{
    /// <summary>
    /// Класс для преобразования логического значения в ComboBoxItem
    /// </summary>
    public class ComboBoxItemToBoolConverter : IValueConverter
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
            IEnumerable<ComboBoxItem> items = (IEnumerable<ComboBoxItem>) parameter;
            
            if (value == null) return null;

            bool val = (bool)value;

            if (val) return items.Where(x => x.Content.ToString() == TrueMessage).FirstOrDefault();

            if (!val) return items.Where(x => x.Content.ToString() == FalseMessage).FirstOrDefault();

            return items.Where(x => x.Content.ToString() == DefaultMessage).FirstOrDefault();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return null;

            ComboBoxItem val = (ComboBoxItem)value;
            if (val.Content.ToString() == TrueMessage) return true;
            if (val.Content.ToString() == FalseMessage) return false;

            return null;
        }
    }
}
