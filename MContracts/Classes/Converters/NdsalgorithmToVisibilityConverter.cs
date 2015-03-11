using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Data;
using MCDomain.Model;

namespace MContracts.Classes.Converters
{
    class NdsalgorithmToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var ndsalg = value as Ndsalgorithm;
            if (ndsalg == null) return value;
            switch (ndsalg.NdsType)
            {
                case TypeOfNds.Undefinded:
                    return Visibility.Collapsed;
                case TypeOfNds.IncludeNds:
                    return Visibility.Visible;
                case TypeOfNds.ExcludeNds:
                    return Visibility.Visible;
                case TypeOfNds.NoNds:
                    return Visibility.Collapsed;
                default:
                    return Visibility.Collapsed;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
