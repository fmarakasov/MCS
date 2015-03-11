using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Windows.Media;
using MCDomain.Model;

namespace MContracts.Classes.Converters
{
    class ContractToBrushConverter : IValueConverter
    {
        private SolidColorBrush AgreementBrush = new SolidColorBrush(Color.FromRgb(204, 192, 217));
        private SolidColorBrush TroubledActiveBrush = new SolidColorBrush(Color.FromRgb(217, 149, 148));
        private SolidColorBrush EndedBrush = new SolidColorBrush(Color.FromRgb(194, 214, 155));
        private SolidColorBrush DefaultBrush = new SolidColorBrush(Color.FromRgb(255, 255, 255));
        
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            try
            {
                Contractdoc contract = (Contractdoc)value;

                if (contract.IsAgreement)
                    return AgreementBrush;

                //switch (contract.Condition)
                //{
                //    case ContractCondition.NormalActive:
                //        return TroubledActiveBrush;
                //    case ContractCondition.Closed:
                //        return EndedBrush;
                //}
                return DefaultBrush;
            }
            catch (Exception)
            {
                return DefaultBrush;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }
    }
}
