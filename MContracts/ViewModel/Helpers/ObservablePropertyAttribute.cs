using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MContracts.ViewModel.Helpers
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public class ObservablePropertyAttribute:Attribute
    {
    }        
}
