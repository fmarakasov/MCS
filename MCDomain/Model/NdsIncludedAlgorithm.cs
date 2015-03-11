namespace MCDomain.Model
{
    /// <summary>
    /// Расчёт НДС при формулировке "Включая НДС"
    /// </summary>
    public class NdsIncludedAlgorithm : DefaultNdsAlgorithm
    {        
        protected override decimal CalcNds(decimal price, double fraction)
        {

            return (decimal)fraction * price / (1 + (decimal)fraction);  
        }

    }
}