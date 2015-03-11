using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Windows;

namespace MContracts.Classes.Converters
{
    class NullToVisibilityConverter : IValueConverter
    {
        public static Visibility BoolToVisibility(object value, Visibility nullVisibility, Visibility notnullVisibility)
        {
            return value == null ? nullVisibility : notnullVisibility;
        }
        
        #region IValueConverter Members

        public Visibility NullVisibility { get; set; }

        public Visibility NotNullVisibility { get; set; }
        
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return BoolToVisibility(value, NullVisibility, NotNullVisibility);
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }

}
