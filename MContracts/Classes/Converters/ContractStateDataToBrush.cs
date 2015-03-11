using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Windows.Media;
using MCDomain.Model;
using CommonBase;

namespace MContracts.Classes.Converters
{
    public class ContractStateDataToBrush : IValueConverter
    {
        public Brush HealthyBrush { get; set; }
        public Brush OverdueBrush { get; set; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is IContractStateData)) return value;
            var obj = value.CastTo<IContractStateData>();
            return Contractrepositoryview.SeemsToBeOverdue(obj, DateTime.Today) ? OverdueBrush : HealthyBrush;

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
