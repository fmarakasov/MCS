namespace MCDomain.Model
{
    class ContractMoneyModel : DefaultMoneyModel
    {
        public ContractMoneyModel(Contractdoc contractdoc)
            : base(contractdoc.Ndsalgorithm, contractdoc.Nds, contractdoc.Currency, contractdoc.Currencymeasure, contractdoc.Price, contractdoc.Currencyrate)
        {            
        }
    }
    
}