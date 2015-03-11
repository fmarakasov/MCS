using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MContracts.Classes.Filtering
{
    public enum ConditionTypes
    {
        Equal = 0,
        GreaterThen = 1,
        LessThen = 2,
        GreaterOrEqualThen = 3,
        LessOrEqualThen = 4,
        NotEqual = 5,
        Containing = 6, 
        NotContaining = 7
    }
}
