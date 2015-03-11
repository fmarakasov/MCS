using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;

namespace MContracts.Classes.Converters
{
    public class NullDateConverter: IValueConverter
    {
        
            #region IValueConverter Members

            public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
            {
                if (((DateTime)value) == new DateTime(1, 1, 1))
                    return String.Empty;
                else
                    return String.Format("0:dd MM yyyy", value);//You may apply your format string here
            }

            public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
            {
                return value;
            }

            #endregion
        

    }
}
