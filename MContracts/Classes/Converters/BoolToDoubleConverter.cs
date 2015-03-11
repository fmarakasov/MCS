using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Data;
using CommonBase;

namespace MContracts.Classes.Converters
{
    public class BoolToDoubleConverter:IValueConverter
    {
        private double _falseValue;
        private double _trueValue;
        private double _defaultValue;
        
        public double FalseValue
        {
            get { return _falseValue; }
            set
            {
                Contract.Assert(value.Between(0, 1));
                _falseValue = value;
            }
        }
        public double TrueValue
        {
            get { return _trueValue; }
            set
            {
                Contract.Assert(value.Between(0, 1));
                _trueValue = value;
            }
        }

        public double DefaultValue
        {
            get { return _falseValue; }
            set
            {
                Contract.Assert(value.Between(0, 1));
                _defaultValue = value;
            }
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Contract.Assert(value is bool?);            

            var val = (bool?) value;
            return val.Value ? TrueValue : FalseValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
