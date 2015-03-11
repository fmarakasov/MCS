using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.ComponentModel;

namespace MContracts.Controls.Converters
{
    class PropertyToRussianConverter : IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            //переделать через рефлекшн!!!!!!!!!!!!!!!!!!
            if (value == null) return String.Empty;
            String val = (String)value;
            if (val.Contains("Contract.Contracttype"))
            {
                return "Вид договора";
            }
            if (val.Contains("Contract.Price"))
            {
                return "Сумма договора";
            }
            if (val.Contains("Contract.Appliedat"))
            {
                return "Дата принятия";
            }
            if (val.Contains("Contract.Approvedat"))
            {
                return "Дата утверждения";
            }
            if (val.Contains("Contract.IsGeneral"))
            {
                return "Основной / соисполнители";
            }
            if (val.Contains("Subject"))
            {
                return "Тема договора";
            }
            if (val.Contains("Contract.Condition"))
            {
                return "Состояние работ";
            }
            if (val.Contains("Contract.IsActive"))
            {
                return "Активные/все";
            }
            if (val.Contains("Contract.ContractMoney.Factor.National.WithNdsValue"))
            {
                return "Цена с НДС";
            }
            if (val.Contains("Num"))
            {
                return "Номер договора";
            }
            if (val.Contains("Contract.ContractorTypeName"))
            {
                return "Тип заказчика";
            }

            return String.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }

        #endregion
    }
}
