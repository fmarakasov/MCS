using CommonBase;
using MCDomain.DataAccess;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace McReports.ViewModel
{
    public abstract class DateRangeReportViewModel : BaseReportViewModel
    {

        private DateRange _period;

        public DateRangeReportViewModel(IContractRepository repository) : base(repository) { }
        /// <summary>
        /// Получает или устанавливает диапазон дат для которых строится отчёт
        /// </summary>
        public DateRange Period
        {
            get { return _period; }
            set
            {
                if (_period.Equals(value)) return;
                _period = value;
                OnPeriodChanged();                
            }
        }

        /// <summary>
        /// Переопределите в производном классе, если требуется обработать изменения значения свойства Period
        /// </summary>
        protected virtual void OnPeriodChanged()
        {
            OnPropertyChanged(() => Period);
        }

        /// <summary>
        /// Задаёт диапазон дат отчёта через объект IDateRange
        /// </summary>
        /// <param name="range">Тип, для которгого определён диапазон дат</param>
        public void SetDateRange(IDateRange range)
        {
            Contract.Requires(range != null);
            Period = range.Range;
        }


    }
}
