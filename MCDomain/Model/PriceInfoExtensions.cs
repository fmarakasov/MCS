using System;
using System.Collections.Generic;
using System.Linq;
using MCDomain.DataAccess;

namespace MCDomain.Model
{
    struct MoneyCache
    {
        public IList<Currency> Currency { get; set; }
        public IList<Currencymeasure> Measure { get; set; }
        public IList<Nds> Nds { get; set; }
        public IList<Ndsalgorithm> Ndsalgorithms { get; set; } 
    }
    public static class PriceInfoExtensions
    {
        private static IDictionary<IContractRepository, MoneyCache> _caches =
            new Dictionary<IContractRepository, MoneyCache>(); 
        
        public static MoneyModel GetPriceInfoMoneyModel(this IPriceRefInfo source, decimal? price,
                                                        IContractRepository repository, decimal? rate=default(decimal?))
        {
            if (source == null) throw new ArgumentNullException("source");
            if (repository == null) throw new ArgumentNullException("repository");

            MoneyCache cache;
            if (!_caches.TryGetValue(repository, out cache))
            {
                cache = new MoneyCache()
                    {
                        Currency = repository.Currencies.ToList(),
                        Measure = repository.Currencymeasures.ToList(),
                        Nds = repository.Nds.ToList(),
                        Ndsalgorithms = repository.Ndsalgorithms.ToList()
                    };
                _caches.Add(repository,cache);
               

            }
            var currency = cache.Currency.Single(x => x.Id == source.Currencyid);
            var nds = cache.Nds.Single(x => x.Id == source.Ndsid);
            var ndsalgorithm = cache.Ndsalgorithms.Single(x => x.Id == source.Ndsalgorithmid);
            var measure = cache.Measure.Single(x => x.Id == source.Currencymeasureid);
            return new MoneyModel(ndsalgorithm, nds, currency, measure, price, rate);
        }
    }
}