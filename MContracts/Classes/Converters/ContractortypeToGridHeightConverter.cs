using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Data;
using MCDomain.Model;
using CommonBase;

namespace MContracts.Classes.Converters
{
    public enum DecisionMode
    {
        Equals,
        NotEquals
    }

    public class ContractortypeToGridHeightConverter : IValueConverter
    {
        private static readonly GridLength AutoHeight = new GridLength(1, GridUnitType.Auto);
        private static readonly GridLength CollapsedHeight = new GridLength(0, GridUnitType.Pixel);

        public WellKnownContractorTypes DecisionContractorType { get; set; }
        public DecisionMode Operator { get; set; }

        private bool Decision(Contractortype contractortype)
        {
            if (Operator == DecisionMode.Equals) 
                return DecisionContractorType == contractortype.WellKnownType;
            return DecisionContractorType != contractortype.WellKnownType;
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var contractorType = value as Contractortype;
            return contractorType.Return(x => Decision(x)
                       ? AutoHeight : CollapsedHeight, AutoHeight);

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
