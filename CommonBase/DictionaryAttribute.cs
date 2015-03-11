using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommonBase
{
    [AttributeUsageAttribute(AttributeTargets.Property, Inherited = true)]
    public class DomainDictionaryAttribute : Attribute
    {
        public Type DomainType { get; private set; }
        public string FriendlyName { get; private set; }
        public DomainDictionaryAttribute(string friendlyName, Type domainType)
        {
            FriendlyName = friendlyName;
            DomainType = domainType;
        }            
    }
}
