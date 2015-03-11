using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MCDomain.Model
{
    [AttributeUsage(AttributeTargets.Property)]
    public class IgnoreSequence : Attribute
    {
    }
}
