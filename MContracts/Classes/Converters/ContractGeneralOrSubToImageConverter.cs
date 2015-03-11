using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using MCDomain.Model;

namespace MContracts.Classes.Converters
{
    class ContractGeneralOrSubToImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is Contractdoc)
            {
                var doc = value as Contractdoc;
                if (doc.IsAgreement) return "/MContracts;component/Resources/stargreen.ico";
                if (doc.IsGeneral) return "/MContracts;component/Resources/General.png";
                if (doc.IsSubContract) return "/MContracts;component/Resources/subcontract.ico";

            }
            if (value is Contractrepositoryview)
            {
                    var doc = value as Contractrepositoryview;
                    return doc.IsGeneral ? "/MContracts;component/Resources/General.png" : 
                        "/MContracts;component/Resources/subcontract.ico";
            }
            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }
    }
}
