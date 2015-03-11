using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Data;
using MCDomain.Model;

namespace MContracts.Classes.Converters
{
    class AgreementToImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Contractrepositoryview)
            {
                var doc = value as Contractrepositoryview;
                return doc.IsAgreement ? "/MContracts;component/Resources/stargreen.ico" :
                    "/MContracts;component/Resources/star.icl.ico";
            }
            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
