using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using MCDomain.Model;

namespace MContracts.Classes.Converters
{
    class ContractToButtonImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            try
            {
                Contractdoc doc = (Contractdoc)value;
                if (doc == null)
                {
                    return "/MContracts;component/Resources/window-close.png";
                }
                else
                {
                    if (doc.IsDeleted)
                        return "/MContracts;component/Resources/cancel_delete.png";
                    else
                        return "/MContracts;component/Resources/window-close.png";
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }
    }
}
