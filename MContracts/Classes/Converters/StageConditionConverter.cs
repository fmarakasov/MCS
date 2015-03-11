using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Windows.Data;
using MCDomain.Model;
using CommonBase;

namespace MContracts.Classes.Converters
{
    class StageConditionConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            
            Contract.Ensures(!string.IsNullOrEmpty(Contract.Result<object>().ToString()));
            Contract.Assert(value is StageCondition);

            var state = (StageCondition)value;

            return state.Description();


            //switch (state)
            //{
            //    case StageCondition.Active:
            //        return "Выполняется";
            //    case StageCondition.Closed:
            //        return "Завершен";
            //    case StageCondition.Overdue:
            //        return "Просрочен";
            //    case StageCondition.Pending:
            //        return "Ожидает выполнения";

            //    default:
            //        return "Не определено";
            //}   

        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }
    }
}
