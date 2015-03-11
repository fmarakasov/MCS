using MCDomain.Model;

namespace MContracts.ViewModel
{
    public class  DefaultMoneyViewModel: MoneyViewModel
    {
        public DefaultMoneyViewModel(Ndsalgorithm algorithm, Nds nds, Currency currency, Currencymeasure measure, double? price)
            : base(algorithm, nds, currency, measure, price)
        {
            NdsPrefix = "НДС: ";
            PurePrefix = "Цена без НДС: ";
            WithNdsPrefix = "Цена с НДС: ";
            NoInfoMessage = "-";
        }
    }
}