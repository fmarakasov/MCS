using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Data;
using CommonBase;

namespace MContracts.Classes.Converters
{
    /// <summary>
    /// Конвертер значений перечислимого типа в строку и обратно на основе атрибута Description
    /// </summary>
    public class EnumDescriptionConverter : IValueConverter
    { 
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is Enum)) throw new ArgumentException("Value is not an Enum");
            return (value as Enum).Description();
        }
      
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is string)) return value;            

            foreach (var item in Enum.GetValues(targetType))
            {

                var asString = (item as Enum).Description();
                if (asString == (string)value)
                {
                    return item;
                }
            }
            throw new ArgumentException("Unable to match string to Enum description");
        }
    }
}
