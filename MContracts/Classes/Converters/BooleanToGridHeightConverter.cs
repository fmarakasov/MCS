using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace MContracts.Classes.Converters
{

    public class BooleanToGridHeightConverter : IValueConverter
    {
        private static readonly GridLength AutoHeight = new GridLength(1, GridUnitType.Auto);
        private static readonly GridLength CollapsedHeight = new GridLength(0, GridUnitType.Pixel);

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var bb = value is bool && (bool) value;
            return bb ? AutoHeight : CollapsedHeight;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
