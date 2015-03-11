using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Data;
using MCDomain.Model;

namespace MContracts.Classes.Converters
{
    public class SeverityToImageSourceConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Contract.Assert(value.GetType() == typeof(ErrorSeverity));
            var severity = (ErrorSeverity) value;
            switch (severity)
            {
                case ErrorSeverity.Critical:
                    return "/MContracts;component/Resources/bulb_red.png";
                case ErrorSeverity.Warning:
                    return "/MContracts;component/Resources/bulb_yellow.png";
                case ErrorSeverity.Hint:
                    return "/MContracts;component/Resources/bulb_blue.png";
                case ErrorSeverity.None:
                    return "/MContracts;component/Resources/bulb_green.png";
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
