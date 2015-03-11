using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Data;

namespace MContracts.Classes.Converters
{
    public class RemoveExtraSpaceConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return TrimSpaces(value);
        }

        private static object TrimSpaces(object value)
        {
            if (value is string)
            {
                var strVal = value as string;
                return strVal.Trim();
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return TrimSpaces(value);
        }
    }
}
