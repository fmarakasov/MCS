using System;
using System.Diagnostics.Contracts;

namespace McReports.Common
{
    [AttributeUsage(AttributeTargets.Property)]
    public class InputParameterAttribute: Attribute
    {
        /// <summary>
        /// Получает тип, ответственный за предоставление коллекции элементов
        /// </summary>
        public Type LookUpType { get; private set; }

        /// <summary>
        /// Получает предпочитаемый тип свойства
        /// </summary>
        public Type DesiredType { get; private set; }
        /// <summary>
        /// Получает human имя параметра
        /// </summary>
        public string DisplayName { get; private set; } 
      
        public InputParameterAttribute(Type desiredType, string displayName, Type lookUpType)
        {
            Contract.Requires(desiredType != null);
            Contract.Requires(displayName != null);
            DesiredType = desiredType;
            DisplayName = displayName;
            LookUpType = lookUpType;
        }

        public InputParameterAttribute(Type desiredType, string displayName)
        {
            Contract.Requires(desiredType != null);
            Contract.Requires(displayName != null);
            DesiredType = desiredType;
            DisplayName = displayName;
            LookUpType = null;
        }
    }
}
