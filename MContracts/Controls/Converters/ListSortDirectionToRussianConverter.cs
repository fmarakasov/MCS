using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.ComponentModel;

namespace MContracts.Controls.Converters
{
    class ListSortDirectionToRussianConverter: IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            ListSortDirection LSD = (ListSortDirection)value;
            if (LSD == ListSortDirection.Ascending)
            {
                return "По возрастанию";
            }
            if (LSD == ListSortDirection.Descending)
            {
                return "По убыванию";
            }

            return String.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
