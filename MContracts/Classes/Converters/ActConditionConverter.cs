using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using MContracts.ViewModel;

namespace MContracts.Classes.Converters
{
    public class ActConditionConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var obj = value as ActDto;
            return obj == null ? value : obj.IsClosedByThisContract;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}