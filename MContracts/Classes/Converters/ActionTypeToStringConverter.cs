using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Data;

namespace MContracts.Classes.Converters
{
    /// <summary>
    /// Класс для преобразования логического значения в строку
    /// </summary>
    public class ActionTypeToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            ActionType state = (ActionType)value;

            switch (state)
            {
                case ActionType.Add:
                    return "Добавить элемент";
                case ActionType.Edit:
                    return "Редактировать элемент";
            }

            return String.Empty;

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
