using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Windows.Media;
using Brush = System.Drawing.Brush;

namespace MContracts.Classes.Converters
{
    class BoolToBrushConverter: IValueConverter
    {
        public SolidColorBrush TrueBrush { get; set; }
        public SolidColorBrush FalseBrush { get; set; }

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null) return TrueBrush;
            if (value.ToString() == "\n") return TrueBrush;
            
            bool val = String.IsNullOrEmpty(value.ToString());
            return val ? TrueBrush : FalseBrush;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
