using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Windows;

namespace MContracts.Classes.Converters
{
    /// <summary>
    /// конвертер отсекает время от типа DateTime и приводит дату к русскому формату
    /// </summary>
    public class DateTimeToDateConverter: IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            //string formatterString = parameter.ToString();
            string formatterString = "0:dd MMMM yyyy г.";
            if (!string.IsNullOrEmpty(formatterString))
            {
                //System.Globalization.CultureInfo c = new System.Globalization.CultureInfo("ru-RU");
                System.Globalization.CultureInfo c = System.Globalization.CultureInfo.CurrentUICulture;
                return string.Format(c, "{" + formatterString + "}", value);
            }
            return value.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string strValue = value.ToString();
            DateTime resultDateTime;
            if (DateTime.TryParse(strValue, out resultDateTime))
            {
                return resultDateTime;
            }
            return value;
        }

        #endregion
    }
}
