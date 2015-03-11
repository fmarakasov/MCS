using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Windows.Data;
using MCDomain.Model;

namespace MContracts.Classes.Converters
{
    public class ActSignedToImageConverter: IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var val = (bool?) value;
            if (val.GetValueOrDefault())
                return "/MContracts;component/Resources/Signed.png";
            return "/MContracts;component/Resources/NotSigned.png";
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }
    }
}
