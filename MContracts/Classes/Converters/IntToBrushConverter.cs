using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace MContracts.Classes.Converters
{
    /// <summary>
    /// Преобразует int в System.Windows.Media.SolidColorBrush и обратно
    /// </summary>
    public class IntToBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (targetType != typeof(Brush) || value == null) return value;

            int nval; 
            if (!int.TryParse(value.ToString(), out nval)) return value;
            
            var bytes = BitConverter.GetBytes(nval);
            var color = Color.FromRgb(bytes[2], bytes[1], bytes[0]);
            return new SolidColorBrush(color);

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return null;
            var brush = (SolidColorBrush) value;
            var color = brush.Color;
            return BitConverter.ToInt32(new[] {color.B, color.G, color.R, color.A}, 0);
        }
    }
}
