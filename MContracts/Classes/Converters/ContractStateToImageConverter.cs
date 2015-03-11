using System;
using System.Globalization;
using System.Windows.Data;
using MCDomain.Model;

namespace MContracts.Classes.Converters
{
    internal class ContractStateToImageConverter : IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            WellKnownContractStates state = WellKnownContractStates.Undefined;

            if (value is Contractstate)
            {
                var obj = (Contractstate) value;
                state = obj.State;
            }

            if (value is Contractrepositoryview)
            {
                var obj = (Contractrepositoryview) value;
                state = obj.State;
            }


            switch (state)
            {
                case WellKnownContractStates.Unsigned:
                    return "/MContracts;component/Resources/NotSigned.png";
                case WellKnownContractStates.Signed:
                    return "/MContracts;component/Resources/Signed.png";
                //case ContractStates.ControlRemoved:
                //    return "/MContracts;component/Resources/gnome_mime_application_x_archive.png";
                //case ContractStates.Broken:
                //    return "/MContracts;component/Resources/broken.png";
                default:
                    return "/MContracts;component/Resources/gnome_dialog_question.png";
           }
        }


        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }

        #endregion
    }
}