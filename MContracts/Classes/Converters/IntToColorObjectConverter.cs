using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Windows.Media;
using System.Drawing;
using MediaColor = System.Windows.Media.Color;
using DrawingColor = System.Drawing.Color;


namespace MContracts.Classes.Converters
{
    public class IntToColorObjectConverter: IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (targetType != typeof(MediaColor) || value == null) return value;

            int nval; 
            if (!int.TryParse(value.ToString(), out nval)) return value;

            var bytes = BitConverter.GetBytes(nval);
            var color = MediaColor.FromRgb(bytes[2], bytes[1], bytes[0]);
            return color;

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return null;
            var color = (MediaColor)value;
            return BitConverter.ToInt32(new[] {color.B, color.G, color.R, color.A}, 0);
        }
    }



}
