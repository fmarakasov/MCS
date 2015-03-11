using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Windows.Media;

namespace MContracts.Classes.Converters
{
    /// <summary>
    /// Преобразует логическое значение в цветовое представление
    /// </summary>
   public class ValidToColorConverter : IValueConverter
    {
        /// <summary>
        /// Получает или устанавливает кисть для дейсвующего документа
        /// </summary>
        public Brush ValidBrush { get; set; }

        /// <summary>
        /// Получает или устанавливает кисть для недействующего документа
        /// </summary>        
        public Brush NotValidBrush { get; set; }

        /// <summary>
        /// Получает или устанавливает кисть для случаев, если значение не задано документа
        /// </summary>        
        public Brush DefaultBrush { get; set; }


        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool? val = (bool?) value;
            if (val.HasValue)
            {
                return val.Value ? ValidBrush : NotValidBrush;
            }
            return DefaultBrush;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
