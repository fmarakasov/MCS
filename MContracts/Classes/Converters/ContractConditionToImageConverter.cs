using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using MCDomain.Model;

namespace MContracts.Classes.Converters
{
    class ContractConditionToImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            try
            {
                ContractCondition state = (ContractCondition)value;

                switch (state)
                {
                    case ContractCondition.NormalActive:
                        return "/MContracts;component/Resources/Equipment.png";
                    //case ContractCondition.TransparentActive:
                    //return "/MContracts;component/Resources/Redo.png";
                    //case ContractCondition.TroubledActive:
                    //     return "/MContracts;component/Resources/Warning.png";
                    case ContractCondition.Closed:
                        return "/MContracts;component/Resources/Apply.png";
                    //case ContractCondition.AgreementInitiated:
                    //    return "/MContracts;component/Resources/Boss.png";
                    //case ContractCondition.Unconditioned:
                    //    return "/MContracts;component/Resources/Flag_red.png";
                    //case ContractCondition.Undefined:
                    //    return "/MContracts;component/Resources/Flag_red.png";
                    //case ContractCondition.Deleted:
                    //    return "/MContracts;component/Resources/deletedcontract.png";
                }
                return null;
            }
            catch (Exception)
            {
                return value;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }
    }
}
