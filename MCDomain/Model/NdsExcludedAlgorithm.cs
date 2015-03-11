namespace MCDomain.Model
{
    /// <summary>
    /// Расчёт НДС при формулировке "Кроме того НДС"
    /// </summary>    
    public class NdsExcludedAlgorithm : DefaultNdsAlgorithm
    {
        protected override decimal CalcNds(decimal price, double fraction)
        {
            return price * (decimal)fraction;
        }

        public override decimal GetPure(decimal price, double fraction)
        {
            return price;
        }
        
    }
}