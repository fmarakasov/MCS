using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Data;
using MCDomain.Model;

namespace MContracts.Classes.Converters
{
    public enum AggregateProperty
    {
        Price,
        FundsDisbursed,
        FundsLeft
    }

    class AggregateCollectionConverter:IValueConverter
    {
        public AggregateProperty Property { get; set; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is IEnumerable)) return value;
            var obj = (value as IEnumerable).Cast<Contractrepositoryview>();
            var sum = 0.0M;
            var goodStatus = obj.Where(x => x.ValidPrice);
            
            if (Property == AggregateProperty.Price)
                sum = goodStatus.Sum(x => x.PriceMoneyModel.National.Factor.PriceWithNdsValue);
            else if (Property == AggregateProperty.FundsDisbursed)
                sum = goodStatus.Sum(x => x.FundsDisbursed.Price);
            else
                sum = goodStatus.Sum(x => x.FundsLeft.Price);
            
            return Currency.National.FormatMoney(sum);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
