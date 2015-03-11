using System;
using System.Collections.Generic;
using CommonBase;
using MCDomain.Model;
using System.Linq;

namespace MCDomain.Common
{
    public static class StagesExtensions
    {
        /// <summary>
        /// Позволяет получить коллекцию этапов в соответствие с заданной строкой запроса по номерам
        /// </summary>
        /// <param name="source">Коллекция этапов</param>
        /// <param name="strRange">Строка запроса этапов</param>
        /// <returns>Коллекция этапов, удовлетворяющих запросу</returns>
        public static IEnumerable<Stage> QueryStringRange(this IEnumerable<Stage> source, string strRange)
        {
            if (source == null) throw new ArgumentNullException("source");
            if (string.IsNullOrWhiteSpace(strRange)) throw new ArgumentNullException("strRange");
            var stages = source.ToArray();
            var ranges = strRange.Split(',');
            return ranges.Select(x => x.Split('-'))
                .SelectMany(contRng => contRng.Length == 1 ? 
                    ProceedUnaryQuery(contRng, stages) : ProceedRangeQuery(contRng, stages));
        }

        private static IEnumerable<Stage> ProceedRangeQuery(IList<string> contRng,
            IEnumerable<Stage> stages)
        {
            var low = contRng[0].Trim();
            var high = contRng[contRng.Count - 1].Trim();
            return stages.Where(
                x =>
                (HierarchicalNumberingComparier.Instance.Compare(x.Num, low) >= 0) &&
                (HierarchicalNumberingComparier.Instance.Compare(x.Num, high) <= 0));

     
        }

        private static IEnumerable<Stage> ProceedUnaryQuery(IList<string> contRng, 
            IEnumerable<Stage> stages)
        {
            if (contRng[0].IsWildcardPattern())
            {
                var wc = contRng[0].Trim();
                var expr = new Wildcard(wc);
                var wcStages = stages.Where(x => expr.IsMatch(x.Num));
                foreach (var stage in wcStages)
                    yield return stage;
            }
            else
            {
                var stage = SelectStage(stages, contRng[0].Trim());
                if (stage != null) yield return stage;
            }
        }

        private static Stage SelectStage(IEnumerable<Stage> source, string p)
        {
            return source.SingleOrDefault(x => x.Num == p);
        }
    }
}
