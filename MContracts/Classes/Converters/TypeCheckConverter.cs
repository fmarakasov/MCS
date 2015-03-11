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
    public class TypeCheckConverter : IValueConverter
    {
        /// <summary>
        /// Получает и устанавливает тип, который должен быть у значения value
        /// </summary>
        public Type DesiredType { get; set; }

        /// <summary>
        /// Требуется ли строгая проверка типов
        /// </summary>
        public bool IsStrict { get; set; }

        public TypeCheckConverter()
        {
            DesiredType = typeof (object);
            IsStrict = false;
        }
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Contract.Assert(DesiredType != null);
            var valType = value.Return(x => x.GetType(), null);
            var eqTest = valType == DesiredType;
            if (!DesiredType.IsInterface)
                return IsStrict ? eqTest : valType.Return(x => x.IsSubclassOf(DesiredType) || eqTest, false);
            return valType.With(x => x.GetInterfaces()).Return(c => c.Any(x => x == DesiredType), false);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
