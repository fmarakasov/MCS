using System;
using System.Linq;
using System.Text;
using CommonBase;

namespace MContracts
{
    public static class ExceptionExtensions
    {
        public static string AggregateMessages(this Exception source)
        {
            return
                source.With(
                    s =>
                    s.LeftDepthSearch(x => x.InnerException.AsSingleElementCollection(), x => x.InnerException == null)
                     .Aggregate(new StringBuilder(), (x, y) => x.AppendLine(y.Message))
                     .ToString());
        }
    }
}