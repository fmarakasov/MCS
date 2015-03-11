using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MCDomain.Model
{
    /// <summary>
    /// Определяет типы, поддерживающие открытые даты окончания работ
    /// </summary>
    public interface ISupportDeltaEndDate
    {
        /// <summary>
        /// Получает или устанавливает количество дней от события для вычисления конечной даты
        /// </summary>
        int? Delta { get; set; }
        /// <summary>
        /// Получает или устанавливает комментарий к вычислению Delta
        /// </summary>
        string Deltacomment { get; set; }

        /// <summary>
        /// Получает или устанавливает начало 
        /// </summary>
        DateTime? Startat { get; set; }

        /// <summary>
        /// Получает или устанавливает окончательную дату
        /// </summary>
        DateTime? Endsat { get; set; }


   
    }

    public static class DeltaEndDateExtensions
    {
        /// <summary>
        /// Получает расчётную дату окончания.
        /// Если задана дата окончания, то возвращается она, если задана Delta, то возвразается Startat + Delta
        /// Если Startat не задан, но задана Delta, то возвращается default(DateTime?)
        /// Если не задана Delta, то возвращается Endsat
        /// </summary>
        /// <param name="source"></param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <returns></returns>
        public static DateTime? GetCalculatedEndsat(this ISupportDeltaEndDate source)
        {
            if (source == null) throw new ArgumentNullException("source");

            if (source.Delta.HasValue)
                return source.Startat.HasValue ? source.Startat.Value.AddDays(source.Delta.Value) : default(DateTime?);         
            return source.Endsat;

        }
    }
}
