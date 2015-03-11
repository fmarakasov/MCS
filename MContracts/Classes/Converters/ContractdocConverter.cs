using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Data;
using MCDomain.Model;

namespace MContracts.Classes.Converters
{
    public class ContractdocConverter:IValueConverter
    {      
       

        /// <summary>
        /// Получает строку с описанием договора
        /// </summary>
        /// <param name="contractdoc"></param>
        /// <returns></returns>
        public static string GetContractdocName(Contractdoc contractdoc)
        {
            Contract.Requires(contractdoc != null);
            Contract.Ensures(!string.IsNullOrEmpty(Contract.Result<string>()));

            return contractdoc.ToString();

        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Contract.Assert(value is Contractdoc);
            return GetContractdocName(value as Contractdoc);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
