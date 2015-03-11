using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace MContracts.ViewModel.Reports
{
    [AttributeUsage(AttributeTargets.Property)]
    public class InputParameterAttribute: Attribute
    {
        /// <summary>
        /// Получает предпочитаемый тип свойства
        /// </summary>
        public Type DesiredType { get; private set; }
        /// <summary>
        /// Получает human имя параметра
        /// </summary>
        public string DisplayName { get; private set; } 
      
        public InputParameterAttribute(Type desiredType, string displayName)
        {
            Contract.Requires(desiredType != null);
            Contract.Requires(displayName != null);
            DesiredType = desiredType;
            DisplayName = displayName;
        }
    }
}
