using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using MCDomain.Model;

namespace MContracts.Classes.Converters
{
    class ContractGeneralOrSubToTextConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is Contractdoc)
            {
                Contractdoc doc = value as Contractdoc;
                if (doc.IsAgreement) return "Дополнительное соглашение";
                if (doc.IsGeneral) return "Генеральный договор";
                if (doc.IsSubContract) return "Договор с соисполнителями";

                return "";
            }
            
            if (value is Contractrepositoryview)
            {
                Contractrepositoryview obj = value as Contractrepositoryview;               
                if (obj.IsGeneral) return "Генеральный договор";
                return "Договор с соисполнитялями";

            }
            
            return "";
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }
    }
}
