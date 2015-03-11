using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Windows.Data;
using MCDomain.Model;

namespace MContracts.Classes.Converters
{
    class StageConditionToImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            Contract.Assert(value is StageCondition);
            //Contract.Ensures(!string.IsNullOrEmpty(Contract.Result<object>().ToString()));

            var state = (StageCondition)value;

            switch (state)
            {
                case StageCondition.Active:
                    return "/MContracts;component/Resources/Equipment.png";
                case StageCondition.Closed:
                    return "/MContracts;component/Resources/Apply.png";
                case StageCondition.Overdue:
                    return "/MContracts;component/Resources/Warning.png";
                case StageCondition.Pending:
                    return "/MContracts;component/Resources/Hourglass.png";
                
                default:
                    return String.Empty;
            }   
                        
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }
    }
}
