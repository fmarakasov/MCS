using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MCDomain.Model
{
    public interface IPriceRefInfo
    {
        long Currencymeasureid { get; }
        long Currencyid { get; }
        long Ndsid { get; }
        long Ndsalgorithmid { get; }
    }
}
