using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;


namespace CommonBase
{

    public enum Months
    {
        [Description("Январь")]
        January = 1,
        [Description("Февраль")]
        Febuary = 2,
        [Description("Март")]
        March = 3,
        [Description("Апрель")]
        April = 4,
        [Description("Май")]
        May = 5,
        [Description("Июнь")]
        June = 6,
        [Description("Июль")]
        July = 7,
        [Description("Август")]
        August = 8,
        [Description("Сентябрь")]
        September = 9,
        [Description("Октябрь")]
        October = 10,
        [Description("Ноябрь")]
        November = 11,
        [Description("Декабрь")]
        December = 12
    }

    public enum Quarters
    {
        [Description("I")]
        Первый = 1,
        [Description("II")]
        Второй = 2,
        [Description("III")]
        Третий = 3,
        [Description("IV")]
        Четвертый = 4
    }

    /// <summary>
    /// Класс, расширяющий возможности DateTime
    /// </summary>
    public static class DateTimeExtensions
    {
        ///// <summary>
        ///// Проверяет находится ли дата в заданном диапазоне, используя нестрогое сравнение
        ///// </summary>
        ///// <param name="source">Дата</param>
        ///// <param name="firstDate">Начальная дата</param>
        ///// <param name="endDate">Конечная дата</param>
        ///// <returns>Истина, если дата находится в заданном диапазоне</returns>
        //public static bool Between(this DateTime source, DateTime firstDate, DateTime endDate)
        //{
        //    return (source >= firstDate) && (source <= endDate);
        //}
        ///// <summary>
        ///// Проверяет находится ли дата в заданном диапазоне, используя строгое сравнение
        ///// </summary>
        ///// <param name="source">Дата</param>
        ///// <param name="firstDate">Начальная дата</param>
        ///// <param name="endDate">Конечная дата</param>
        ///// <returns>Истина, если дата находится в заданном диапазоне</returns>
        //public static bool StrictBetween(this DateTime source, DateTime firstDate, DateTime endDate)
        //{
        //    return (source > firstDate) && (source < endDate);
        //}
        /// <summary>
        /// Получает дату, соответствующую началу заданного месяца в году
        /// </summary>        
        /// <param name="month">Месяц</param>
        /// <param name="year">Год</param>
        /// <returns>Дата, соответствующая началу месяца</returns>
        public static DateTime StartOfTheMonth(int month, int year)
        {
            return DateTime.Parse(string.Format("01.{0}.{1}", month, year));
        }

        /// <summary>
        /// Получает дату, соответствующую окончанию заданного месяца в году
        /// </summary>        
        /// <param name="month">Месяц</param>
        /// <param name="year">Год</param>
        /// <returns>Дата, соответствующая концу месяца</returns>
        public static DateTime EndOfTheMonth(int month, int year)
        {
            return StartOfTheMonth(month, year).AddMonths(1);
        }

        /// <summary>
        /// Получает DateTime, соответствующий началу заданного года (01.01.XXXX)
        /// </summary>
        /// <param name="year">Год</param>
        /// <returns>Объект DateTime</returns>
        public static DateTime StartOfTheYear(int year)
        {
            return DateTime.Parse("01.01." + year.ToString());
        }

        /// <summary>
        /// Получает DateTime, соответствующий концу заданного года (01.01.XXXX+1)
        /// </summary>
        /// <param name="year">Год</param>
        /// <returns>Объект DateTime</returns>
        public static DateTime EndOfTheYear(int year)
        {
            return DateTime.Parse("31.12." + year.ToString() + " 23:59:59");
        }

        public static IEnumerable<T> SelectRange<T>(this IEnumerable<T> source, DateTime fromDate, DateTime toDate, 
            Func<DateRangesIntersectionResult, bool> selectFunc) where T:IDateRange
        {            
            DateRange range = new DateRange() { Start = fromDate, End = toDate };
            return SelectRange(source, range, selectFunc);
        }

        public static IEnumerable<T> SelectRange<T>(this IEnumerable<T> source, DateRange range, 
            Func<DateRangesIntersectionResult, bool> selectFunc) where T:IDateRange
        {
            if (selectFunc == null)
                throw new ArgumentNullException("selectFunc", "Параметр selectFunc не может быть null.");
            if (source == null)
                throw new ArgumentNullException("source", "Параметр source не может быть null.");
            
            //return source.AsEnumerable().Where(x => selectFunc(x.Range.CheckIntersect(range)));
            return source.AsEnumerable().Where(x => selectFunc(range.CheckIntersect(x.Range)));
        }


        /// <summary>
        /// Возвращает первый месяц, т.е. 1
        /// </summary>
        public static int FirstMonth
        {
            get { return 1; }
        }

        public static string QuarterToRoman(Quarters quarter)
        {
            return quarter.Description();
        }


        public static Quarters GetPreviousQuarters(Quarters quarter)
        {
            if (quarter == Quarters.Первый) return Quarters.Четвертый;
            else return ((Quarters)((int)(quarter - 1)));
            //if (quarter == Quarters.Второй) return ((Quarters)((int)(quarter - 1))).Description();
            //return ((Quarters)(1)).Description() + " - " + ((Quarters)((int)(quarter - 1))).Description();
        }

        /// <summary>
        /// Возвращаяет последний месяц года, т.е. 12
        /// </summary>
        public static int LastMonth
        {
            get { return 12; }
        }

        /// <summary>
        /// Возвращает первый день месяца, т.е. 1
        /// </summary>
        public static int FirstMonthDay
        {
            get { return 1; }
        }

        public static DateTime GetFirstMonthDay(int year, int month)
        {
            return new DateTime(year, month, FirstMonthDay);
        }

        public static DateTime GetFirstQuarterDay(int year, int quarter)
        {
            //первый месяц квартала
            int firstQuarterMonth = quarter * 3 - 2;

            return new DateTime(year, firstQuarterMonth, FirstMonthDay);

        }

        /// <summary>
        /// Получает первый день года
        /// </summary>
        /// <param name="year">Год</param>
        /// <returns>Первый день года</returns>
        public static DateTime GetFirstYearDay(int year)
        {
            return new DateTime(year, FirstMonth, FirstMonthDay);
        }


        public static void GetMonthByQuarter(Quarters quarter, out string Month1, out string Month2, out string Month3)
        {
            int intQuarter = (int)quarter;
            Month1 = ((Months)(intQuarter * 3 - 2)).Description();
            Month2 = ((Months)(intQuarter * 3 - 1)).Description();
            Month3 = ((Months)(intQuarter * 3)).Description();
        }

        /// <summary>
        /// Получает дату последнего дня года
        /// </summary>
        /// <param name="year">Год</param>
        /// <returns>Дата последнего дня года</returns>
        public static DateTime GetLastYearDay(int year)
        {
            return new DateTime(year, LastMonth, DateTime.DaysInMonth(year, LastMonth));
        }

        /// <summary>
        /// Получает дату последнего дня месяца года
        /// </summary>
        /// <param name="year">Год</param>
        /// <param name="month">Месяц</param>
        /// <returns>Дата последнего дня месяца года</returns>
        public static DateTime GetLastMonthDay(int year, int month)
        {
            int lastDay = DateTime.DaysInMonth(year, month);
            return new DateTime(year, month, lastDay, 23, 59, 59);
        }

        /// <summary>
        /// Получает дату последнего дня квартала
        /// </summary>
        /// <param name="year">Год</param>
        /// <param name="quarter">Квартал</param>
        /// <returns>Дата последнего дня квартала</returns>
        public static DateTime GetLastQuarterDay(int year, int quarter)
        {
            int lastMonth = quarter * 3;
            int lastDay = DateTime.DaysInMonth(year, lastMonth);
            return new DateTime(year, lastMonth, lastDay);
        }

        /// <summary>
        /// Определяет есть ли предыдущие кварталы
        /// </summary>
        /// <param name="quarter">квартал</param>
        /// <returns>True если квартал не первый</returns>
        public static bool HasPereviousQuarters(int quarter)
        {
            return quarter > 1;
        }


        public static Quarters GetQuarterByDate(this DateTime dt)
        {

            if (dt.Month >= 1 && dt.Month <= 3) return Quarters.Первый;
            else if (dt.Month >= 4 && dt.Month <= 6) return Quarters.Второй;
            else if (dt.Month >= 7 && dt.Month <= 9) return Quarters.Третий;
            else if (dt.Month >= 10 && dt.Month <= 12) return Quarters.Четвертый;
            else return Quarters.Первый;
        }
    }
}
