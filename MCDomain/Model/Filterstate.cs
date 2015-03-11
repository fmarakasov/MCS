using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using MCDomain.Common;
using CommonBase;

namespace MCDomain.Model
{

    public enum FilterstateType
    {
        /// <summary>
        /// Текущий год
        /// </summary>
        [Description("Текущий год")]
        CurrentYear = -5,

        /// <summary>
        /// Первый квартал текущего года
        /// </summary>
        [Description("Первый квартал текущего года")]
        FirstQuarter = -1,

        /// <summary>
        /// Второй квартал текущего года
        /// </summary>
        [Description("Второй квартал текущего года")]
        SecondQuarter = -2,

        /// <summary>
        /// Третий квартал текущего года
        /// </summary>
        [Description("Третий квартал текущего года")]
        ThirdQuarter = -3,

        /// <summary>
        /// Четвертый квартал текущего года
        /// </summary>
        [Description("Четвертый квартал текущего года")]
        FourthQuarter = -4,

        [Description("Первое полугодие")]
        FirstHalfYear = -6,

        [Description("Второе полугодие")]
        SecondHalfYear = -7,

        [Description("Все")]
        All = -8


    }


    public partial class Filterstate: IObjectId
    {
        public DateTime Startdate { get; set; }
        public DateTime Finishdate { get; set; }



        public bool IsWellKnownId()
        {
            var en = Enum.GetValues(typeof(FilterstateType));
            foreach (var ch in en)
            {
                if ((FilterstateType)Id == (FilterstateType)ch) return true;
            }
            return false;
        }

        /// <summary>
        /// Получает известный тип экономической эффективности
        /// </summary>
        public FilterstateType WellKnownType
        {
            get
            {
                if (IsWellKnownId())
                    return (FilterstateType)Id;
                return FilterstateType.CurrentYear;
            }
        }


        partial void OnCreated()
        {
            
        }

        public bool IsSystem
        {
            get { return (Owner == 0); }
        }

        public bool IsQuarter

        {
            get
            {
                return (Id == (long)FilterstateType.FirstQuarter || Id == (long)FilterstateType.SecondQuarter ||
                        Id == (long)FilterstateType.ThirdQuarter || Id == (long)FilterstateType.FourthQuarter);
            }
        }

        public bool IsYear
        {
            get { return Id == (long)FilterstateType.CurrentYear; }
        }


        public void ResetYear(int Year)
        {
            switch (Id)
            {
                case (long) FilterstateType.CurrentYear:
                    Startdate = DateTimeExtensions.StartOfTheYear(Year);
                    Finishdate = DateTimeExtensions.EndOfTheYear(Year);
                    break;
                case (long) FilterstateType.FirstQuarter:
                    Startdate = DateTimeExtensions.GetFirstQuarterDay(Year, 1);
                    Finishdate = DateTimeExtensions.GetLastQuarterDay(Year, 1);
                    break;
                case (long) FilterstateType.SecondQuarter:
                    Startdate = DateTimeExtensions.GetFirstQuarterDay(Year, 2);
                    Finishdate = DateTimeExtensions.GetLastQuarterDay(Year, 2);
                    break;
                case (long) FilterstateType.ThirdQuarter:
                    Startdate = DateTimeExtensions.GetFirstQuarterDay(Year, 3);
                    Finishdate = DateTimeExtensions.GetLastQuarterDay(Year, 3);
                    break;
                case (long) FilterstateType.FourthQuarter:
                    Startdate = DateTimeExtensions.GetFirstQuarterDay(Year, 4);
                    Finishdate = DateTimeExtensions.GetLastQuarterDay(Year, 4);
                    break;
                case (long)FilterstateType.FirstHalfYear:
                    Startdate = DateTimeExtensions.GetFirstQuarterDay(Year, 1);
                    Finishdate = DateTimeExtensions.GetLastQuarterDay(Year, 2);
                    break;
                case (long)FilterstateType.SecondHalfYear:
                    Startdate = DateTimeExtensions.GetFirstQuarterDay(Year, 3);
                    Finishdate = DateTimeExtensions.GetLastQuarterDay(Year, 4);
                    break;
                case (long) FilterstateType.All:
                    DateTime dt;
                    DateTime.TryParse("01.01.1970", out dt);
                    Startdate = dt;
                    DateTime.TryParse("01.01.2070", out dt);
                    Finishdate = dt;
                    break;
            }
        }

        partial void OnLoaded()
        {
            ResetYear(DateTime.Today.Year);
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
