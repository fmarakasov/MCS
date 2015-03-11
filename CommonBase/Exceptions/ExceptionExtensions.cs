using System;
using System.Linq;
using System.Text;

namespace CommonBase.Exceptions
{
    /// <summary>
    /// Методы расширений для обработки исключений
    /// </summary>
    public static class ExceptionExtensions
    {
        /// <summary>
        /// Получает агрегированную строку сообщений по всей иерарархии исключений
        /// </summary>
        /// <param name="source">Исключение</param>
        /// <returns>Составное сообщение об исключении</returns>
        public static string AggregateMessages(this Exception source)
        {
            return
                source.With(
                    s =>
                    s.LeftDepthSearch(x => x.InnerException.AsSingleElementCollection(), x => x != null, x=>x == null)
                     .Aggregate(new StringBuilder(), (x, y) => x.AppendLine(y.Message))
                     .ToString());
        }
    }
}