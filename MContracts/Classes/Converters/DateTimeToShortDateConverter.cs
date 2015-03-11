using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Windows;

namespace MContracts.Classes.Converters
{
    /// <summary>
    /// конвертер отсекает время от типа DateTime и приводит дату к русскому формату
    /// </summary>
    public class DateTimeToShortDateConverter : IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var dt = (DateTime?)value;

            if (dt.HasValue)
            {
                if (dt.Value.Year != 1)
                    return dt.Value.ToShortDateString();
            }

            return "-";
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value;
        }

        #endregion
    }
}
