using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MCDomain.Model
{
    public interface IAct : IPriceRefInfo
    {
        decimal? Totalsum { get; }
        decimal? Sumfortransfer { get; }       
    }
}
