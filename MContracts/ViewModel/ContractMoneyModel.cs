using MCDomain.Model;

namespace MContracts.ViewModel
{
    class ContractMoneyModel : DefaultMoneyViewModel
    {
        public ContractMoneyModel(Contractdoc contractdoc)
            : base(contractdoc.Ndsalgorithm, contractdoc.Nds, contractdoc.Currency, contractdoc.Currencymeasure, contractdoc.Price)
        {            
        }
    }
    
}