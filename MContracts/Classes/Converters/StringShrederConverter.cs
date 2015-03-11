using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Data;

namespace MContracts.Classes.Converters
{
    public enum ShrederType
    {
        /// <summary>
        /// Замена строки по умолчанию
        /// </summary>
        Default,
        /// <summary>
        /// Производить замену вначале строки
        /// </summary>
        FromTheStart,
        /// <summary>
        /// Производить замену в середине строки
        /// </summary>
        InTheMiddle,
        /// <summary>
        /// Производить замену в конце строки
        /// </summary>
        AtTheEnd
    }
    public class StringShrederConverter:IValueConverter
    {
        public int MaxLength { get; set; }
        public ShrederType Shreding { get; set; }
        public string Substitute { get; set; }

        public StringShrederConverter()
        {
            Shreding = ShrederType.Default;
            MaxLength = 150;
            Substitute = "\u2026";
        }

        public static string Shreder(string value, int maxLength, string substitue = "\u2026" , ShrederType shreding = ShrederType.Default)
        {
            if (string.IsNullOrEmpty(value)) return value;

            if (value.Length <= maxLength || value.Length < substitue.Length+1) return value;

            StringBuilder sb;
            switch (shreding)
            {
                case ShrederType.FromTheStart:
                    sb = new StringBuilder(value.Substring(value.Length - maxLength - 1 + substitue.Length, maxLength));
                    sb.Insert(0, substitue);
                    break;
                case ShrederType.InTheMiddle:
                    var half = (maxLength/2) - substitue.Length;
                    var start = value.Substring(0, half);
                    var end = value.Substring(value.Length - half, half);
                    sb = new StringBuilder(maxLength);
                    sb.Append(start);
                    sb.Append(substitue);
                    sb.Append(end);
                    break;
                default:
                    sb = new StringBuilder(value.Substring(0, maxLength - substitue.Length));
                    sb.Append(substitue);
                    break;
            }
            
            return sb.ToString();

        }
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Shreder((string)value, MaxLength, Substitute, Shreding);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
