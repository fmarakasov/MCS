using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Data;
using MCDomain.Model;

namespace MContracts.Classes.Converters
{
    public class ContractToSubContractConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var subgeneral = value as Contractdoc;
            if (subgeneral == null) return value;
            var general =subgeneral.ContextContract;
            if (general == null)  return value;
            
            return general.Contracthierarchies.Select(x=>x.SubContractdoc).Contains(subgeneral);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
