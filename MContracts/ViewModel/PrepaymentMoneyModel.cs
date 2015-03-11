using MCDomain.Model;

namespace MContracts.ViewModel
{
    class PrepaymentMoneyModel : DefaultMoneyViewModel
    {
        public PrepaymentMoneyModel(Contractdoc contractdoc):base(contractdoc.Ndsalgorithm1, contractdoc.Nds, contractdoc.Currency, contractdoc.Currencymeasure, contractdoc.Prepaymentsum)
        {            
        }
    }
}