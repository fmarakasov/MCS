using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Windows.Data;
using MCDomain.Model;

namespace MContracts.Classes.Converters
{
    class ContractConditionConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var contractState = value as Contractrepositoryview;
            if (contractState != null)
            {
                return (contractState.Condition & ContractCondition.Actual) == ContractCondition.Actual;
            }
            return true;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }
    }
}
