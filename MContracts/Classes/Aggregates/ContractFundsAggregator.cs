using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MCDomain.Model;

namespace MContracts.Classes.Aggregates
{
    /// <summary>
    /// Класс агрегации средств по группе договоров.
    /// Алгоритм: выбрать актуальные договора по группе и проссумирувать по заданному полю
    /// </summary>
    public static class ContractFundsAggregator
    {
        public static decimal AggregateFunds<TSource>(IEnumerable<TSource> source, Func<TSource, decimal> selector)
        {
            if (source == null) throw new ArgumentNullException("source");
            if (selector == null) throw new ArgumentNullException("selector");

            // Производит выборку актуальных генеральных договоров последних версий и рассчитывает сумму по заданному свойству
            return (from item in source let contract = item as Contractrepositoryview 
                    where contract != null where contract.IsLastVersion && contract.IsGeneral select selector(item)).Sum();
        }
    }
}
