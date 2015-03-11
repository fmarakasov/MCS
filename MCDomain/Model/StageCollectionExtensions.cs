using System;
using System.Collections.Generic;
using System.Linq;

namespace MCDomain.Model
{
    public static class StageCollectionExtensions
    {
        /// <summary>
        /// Получает сумму по этапам, входящим в коллекцию с учётом 
        /// того, что этапы иерархичны и цена этапа верхнего уровня является суммой своих подэтапов 
        /// </summary>
        /// <param name="source">Коллекция этапов</param>
        /// <returns>Сумма по этапам</returns>
        public static decimal GetTotalAmount(this IList<Stage> source)
        {
            if (source == null) throw new ArgumentNullException("source");
            if (!source.Any()) return 0.0M;

            return source.Where(
                stage => !stage.Stages.Any() || !stage.Stages.Any(source.Contains)).Sum(
                    x => x.StageMoneyModel.PriceWithNdsValue);

        }
    }
}
