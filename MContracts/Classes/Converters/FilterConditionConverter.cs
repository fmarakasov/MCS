using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.ComponentModel;
using MContracts.Classes.Filtering;

namespace MContracts.Controls.Converters
{
    class FilterConditionConverter : IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            ConditionTypes cond = (ConditionTypes)value;
            switch (cond)
            {
                case ConditionTypes.Equal: return "=";
                case ConditionTypes.GreaterOrEqualThen: return ">=";
                case ConditionTypes.GreaterThen: return ">";
                case ConditionTypes.LessOrEqualThen: return "<=";
                case ConditionTypes.LessThen: return "<";
                case ConditionTypes.NotEqual: return "!=";
                case ConditionTypes.NotContaining: return "не содержит";
                case ConditionTypes.Containing: return "содержит";
            }

            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }

        #endregion
    }
}

