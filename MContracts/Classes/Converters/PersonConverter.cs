using System;
using System.Globalization;
using System.Windows.Data;
using CommonBase;

namespace MContracts.Classes.Converters
{
    [Obsolete("Используйте прямую привязку к типу Person что-бы воспользоваться реализацией Person.ToString()")]
    class PersonConverter : IValueConverter
    {
        #region Implementation of IValueConverter

        /// <summary>
        /// Converts a value. 
        /// </summary>
        /// <returns>
        /// A converted value. If the method returns null, the valid null value is used.
        /// </returns>
        /// <param name="value">The value produced by the binding source.</param><param name="targetType">The type of the binding target property.</param><param name="parameter">The converter parameter to use.</param><param name="culture">The culture to use in the converter.</param>        
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            MCDomain.Model.Person person = (MCDomain.Model.Person)value;
            if (person == null) return null;
            string result = person.GetShortFullName();
            if (person.HasDegree) result += ", " + person.Degree.Name;
            result += ", " + person.Contractorposition.Contractor;
            return result;
        }

        /// <summary>
        /// Converts a value. 
        /// </summary>
        /// <returns>
        /// A converted value. If the method returns null, the valid null value is used.
        /// </returns>
        /// <param name="value">The value that is produced by the binding target.</param><param name="targetType">The type to convert to.</param><param name="parameter">The converter parameter to use.</param><param name="culture">The culture to use in the converter.</param>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }

        #endregion
    }
}