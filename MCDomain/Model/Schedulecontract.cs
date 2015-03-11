using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MCDomain.Common;
using System.ComponentModel;
using MCDomain.DataAccess;
using CommonBase;

namespace MCDomain.Model
{
    partial class Schedulecontract : IDataErrorInfo, ICloneable, IClonableRecursive
    {
        private readonly DataErrorHandlers<Schedulecontract> _errorHandlers = new DataErrorHandlers<Schedulecontract>
                                                                       {
                                                                           new AppNumErrorHandler()
                                                                       };

        /// <summary>
        /// Выбирает этапы,даты начала и окончания которых удовлетворяет условию 
        /// </summary>
        /// <param name="fromDate">Начальная дата</param>
        /// <param name="toDate">Конечная дата</param>
        /// <param name="selectFunc">Функция выбора</param>
        /// <returns></returns>
        public IEnumerable<Stage> SelectStages(DateTime fromDate, DateTime toDate, Func<DateRangesIntersectionResult, bool> selectFunc)
        {
            return Schedule.SelectStages(fromDate, toDate, selectFunc);
        }

        #region IDataErrorInfo Members

        public string Error
        {
            get { return this.Validate(); }
        }

        public string this[string columnName]
        {
            get { return _errorHandlers.HandleError(this, columnName); }
        }

        #endregion

        #region Nested type: AppNumErrorHandler

        private class AppNumErrorHandler : IDataErrorHandler<Schedulecontract>
        {
            #region IDataErrorHandler<Stage> Members

            public string GetError(Schedulecontract source, string propertyName, ref bool handled)
            {
                if (propertyName == "Appnum")
                {
                    return HandleCurrencyError(source, out handled);
                }

                return string.Empty;
            }

            #endregion

            private static string HandleCurrencyError(Schedulecontract source, out bool handled)
            {
                handled = false;
                if (source.Appnum.HasValue && !source.Appnum.Value.Between(1, 99))
                {
                    handled = true;
                    return "Номер приложения должен быть положительным числом не больше 99!";
                }

                return string.Empty;
            }
        }

        #endregion

        public object Clone()
        {
            var newSchedulecontract = new Schedulecontract
                                                       {
                                                           Appnum = Appnum,
                                                           Schedule = (Schedule) Schedule.Clone()
                                                       };
            
            return newSchedulecontract;
        }


        public object CloneRecursively(object owner, object source)
        {
            var newSchedulecontract = new Schedulecontract()
            {
                Appnum = Appnum,
            };

            if (source == null)
            {
                newSchedulecontract.Schedule = (Schedule)Schedule.CloneRecursively(newSchedulecontract, null);
                newSchedulecontract.Contractdoc = (Contractdoc)owner;
            }
            else
            {
                newSchedulecontract.Schedule = (Schedule)source;
                newSchedulecontract.Contractdoc = (Contractdoc)owner;
            }
                

            //repository.SubmitChanges();
            return newSchedulecontract;
        }


        public override string ToString()
        {
            string s = Appnum.HasValue ? Appnum.ToString() : string.Empty;
            if (Schedule != null&&Schedule.Worktype != null) s = s + "/" + Schedule.Worktype.ToString();
            return s;
        }

        /// <summary>
        /// возвращает наименование какой-либо цены или стоимости, дополненное единицей измерения
        /// и обозначением валюты
        /// </summary>
        /// <param name="mainpart">
        /// часть, которую требуется дополнить (например, передаем "Цена этапа" - на 
        /// выходе имеем "Цена этапа, руб")
        /// </param>
        /// <returns></returns>
        public string GetColumnTitle(string mainpart)
        {
            var s = new StringBuilder();
            s.Append(mainpart);
            s.Append(", ");

            if (Schedule != null && Schedule.Currencymeasure != null)
            {
                s.AppendFormat(Schedule.Currencymeasure.CurrencyMeasureFormat,
                               Contractdoc.CurrencyOrDefault.CI.NumberFormat.CurrencySymbol);
            }
            else
            {

                s.AppendFormat(Currencymeasure.FormValue(1).CurrencyMeasureFormat,
                               Contractdoc.CurrencyOrDefault.CI.NumberFormat.CurrencySymbol);
            }
            return s.ToString();

        }

        public  bool IsTheSameScheduleAsInOriginalContractdoc
        {
            get { return Contractdoc.HasTheSameSchedule(Schedule); }
        }

        public bool IsOtherScheduleAsInOriginalContractdoc
        {
            get { return !IsTheSameScheduleAsInOriginalContractdoc; }
        }


    }
}
