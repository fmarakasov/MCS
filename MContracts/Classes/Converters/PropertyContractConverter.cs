using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using MCDomain.Model;

namespace MContracts.Classes.Converters
{
    class PropertyContractConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is ContractCondition)
            {
                ContractCondition state = (ContractCondition)value;

                switch (state)
                {
                    case ContractCondition.NormalActive:
                        return "Действующий";
                    //case ContractCondition.TransparentActive:
                    //    return "Переходящие этапы";
                    //case ContractCondition.TroubledActive:
                    //    return "Просроченные этапы";
                    case ContractCondition.Closed:
                        return "Завершён";
                    //case ContractCondition.AgreementInitiated:
                    //    return "Согласование доп. соглашения";
                }
                return null;
            }
            if (value is bool) //основной субподрядный
            {
                return (bool)value == true ? "Основной договор" : "Соисполнители";
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }
    }
}
