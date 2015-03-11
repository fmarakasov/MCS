namespace MCDomain.Model
{
    public class  DefaultMoneyModel: MoneyModel
    {
        public DefaultMoneyModel(Ndsalgorithm algorithm, Nds nds, Currency currency, Currencymeasure measure, decimal? price, decimal? rate)
            : base(algorithm, nds, currency, measure, price, rate)
        {
            NdsPrefix = "НДС: ";
            PurePrefix = "Цена без НДС: ";
            WithNdsPrefix = "Цена с НДС: ";
            NoInfoMessage = "-";
        }
    }
}