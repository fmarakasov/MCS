using System.Text;
using System.Windows.Data;
using System.Linq;

namespace MContracts.Classes.Converters
{
    public class StageMarginConverter : IValueConverter
    {
        #region „лены IValueConverter

        public object Convert(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            const string space = "  ";
            if (value == null) return null;
            var val = value.ToString();
            var dots = val.Count(x => x == '.');
            var sb = new StringBuilder();
            for (var i = 0; i < dots; ++i)
                sb.Append(space);
            sb.Append(val);
            return sb.ToString();
        }

        public object ConvertBack(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value;
        }

        #endregion
    }
}