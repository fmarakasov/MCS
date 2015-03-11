using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MCDomain.Model
{
    public class ActMoneyModel : DefaultMoneyModel
    {
        public ActMoneyModel(Contractdoc contractdoc, Act act, Stage stage)
            : base(stage.Ndsalgorithm, act.Nds, contractdoc.Currency, contractdoc.Currencymeasure, act.Totalsum, contractdoc.Currencyrate)
        {            

        }
    }
}
