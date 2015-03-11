namespace MCDomain.Model
{
    public class PrepaymentMoneyModel : DefaultMoneyModel
    {
        public PrepaymentMoneyModel(Contractdoc contractdoc):base(contractdoc.Prepaymentndsalgorithm, contractdoc.Nds, contractdoc.Currency, contractdoc.Currencymeasure, contractdoc.Prepaymentsum, contractdoc.Currencyrate)
        {
            PurePrefix = "Аванс без НДС:";
            WithNdsPrefix = "Аванс с НДС:";
        }
    }
}