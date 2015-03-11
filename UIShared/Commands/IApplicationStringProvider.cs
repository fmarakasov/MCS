using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace UIShared.Commands
{
    public interface IApplicationStringProvider
    {
        string GetToolTip(object obj, PropertyInfo propertyInfo);
        string GetImageResource(object obj, PropertyInfo propertyInfo);
        string GetConfirmMessage(object obj, PropertyInfo propertyInfo);
    }
}
