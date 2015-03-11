using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Windows;

namespace MContracts.Classes.Converters
{
    public class BoolToVisibilityConverter: IValueConverter
    {
        /// <summary>
        /// Преобразует тип bool в тип Visibility
        /// </summary>
        /// <param name="value">Значение булевского типа</param>
        /// <param name="trueVisibility">Результат, если значение True</param>
        /// <param name="falseVisibility">Результат, если значение False</param>
        /// <returns>Тип Visibility</returns>
        public static Visibility BoolToVisibility(bool value, Visibility trueVisibility, Visibility falseVisibility)
        {
            return value ? trueVisibility : falseVisibility;
        }
        /// <summary>
        /// Преобразует тип Visibility в тип bool
        /// </summary>
        /// <param name="visibility"></param>
        /// <param name="trueVisibility"></param>
        /// <returns></returns>
        public static bool VisibilityToBoolean(Visibility visibility, Visibility trueVisibility)
        {
            return visibility == trueVisibility;
        }

        #region IValueConverter Members

        public Visibility TrueVisibility { get; set; }

        public Visibility FalseVisibility { get; set; }
        
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return BoolToVisibility((bool)value, TrueVisibility, FalseVisibility);
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
