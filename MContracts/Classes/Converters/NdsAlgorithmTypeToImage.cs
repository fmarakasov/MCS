using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Data;
using MCDomain.Model;

namespace MContracts.Classes.Converters
{
    class NdsAlgorithmTypeToImage : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is Ndsalgorithm)) return value;
            var obj = value as Ndsalgorithm;
            switch (obj.NdsType)
            {
                case TypeOfNds.Undefinded:
                    return "/MContracts;component/Resources/alert.png";
                case TypeOfNds.IncludeNds:
                    return "/MContracts;component/Resources/symbol_remove.png";
                case TypeOfNds.ExcludeNds:
                    return "/MContracts;component/Resources/symbol_add.png";
                case TypeOfNds.NoNds:
                    return "/MContracts;component/Resources/symbol_delete.png";
                default:
                    return value;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}
