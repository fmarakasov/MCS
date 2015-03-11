using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Data;
using MCDomain.Model;

namespace MContracts.Classes.Converters
{
    public class ContractToVisibilityConverter : IValueConverter
    {      
       
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Contract.Assert(value is Contractdoc);
            return value is NullContractdoc ? Visibility.Collapsed : Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
