using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace CommonBase
{
    /// <summary>
    /// Результат проверки диапазона дат на принадлежность другому диапазону дат
    /// </summary>
    public enum DateRangesIntersectionResult
    {
        /// <summary>
        /// Диапазоны не пересекаются
        /// </summary>
        NotInRange,
        /// <summary>
        /// Дата начала первого диапазона входит в другой диапазон
        /// </summary>
        StartsInRane,
        /// <summary>
        /// Диапазон дат входит в другой диапазон
        /// </summary>
        InRange,
        /// <summary>
        /// Дата окончания перовго диапазона входит в другой диапазон
        /// </summary>
        EndsInRange
    }
    /// <summary>
    /// Определяет типы, которые поддерживают диапазоны дат
    /// </summary>
    public struct DateRange
    {
        /// <summary>
        /// Получает или устанавливает начало диапазона
        /// </summary>
        public DateTime Start { get; set;}
        /// <summary>
        /// Получает или устанавливает конец диапазона
        /// </summary>
        public DateTime End { get; set; }        
        /// <summary>
        /// Получает результат теста пересечения двух диапазонов дат
        /// </summary>
        /// <param name="other">Диапазон дат с которым проверяется пересечение</param>
        /// <returns>Результат проверки попадания данного диапазона в переданный диапазон дат</returns>
        public DateRangesIntersectionResult CheckIntersect(DateRange other)
        {            
            DateRangesIntersectionResult result = DateRangesIntersectionResult.NotInRange;

            if (other.Start.Between(Start, End))
                if (other.End.Between(Start, End))
                    result = DateRangesIntersectionResult.InRange;
                else
                    result = DateRangesIntersectionResult.StartsInRane;
            else if (other.End.Between(Start, End))
                result = DateRangesIntersectionResult.EndsInRange;

            return result;
        }

        public override bool Equals(object other)
        {
            return other is DateRange && (Start == ((DateRange)other).Start) && (End == ((DateRange)other).End);
        }

        public override int GetHashCode()
        {
            return 0;
        }

       
    }
    
}
