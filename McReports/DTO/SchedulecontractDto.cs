using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MCDomain.Model;

namespace McReports.DTO
{
    /// <summary>
    /// DTO связи КП и договора
    /// </summary>
    public class SchedulecontractDto : EntityDto<Schedulecontract>
    {

        #region properties


        private ScheduleDto _schedule;
        /// <summary>
        /// реальный КП
        /// </summary>
        public ScheduleDto Schedule
        {
            get { return _schedule; }
            set
            {
                _schedule = value;
            }
        }


        
        private long _Id;
        /// <summary>
        /// идентификатор связи
        /// </summary>
        public long Id
        {
            get
            {
                return _Id;
            }

            set
            {
                _Id = value;
            }
        }

        private Contractdoc _contractdoc;
        /// <summary>
        /// связь с договором
        /// </summary>
        public Contractdoc Contractdoc
        {
            get
            {
                return _contractdoc;
            }

            set
            {
                _contractdoc = value;
            }
        }

        private int? _Appnum;
        /// <summary>
        /// номер приложения
        /// </summary>
        public int? Appnum
        {
           get 
           {
               return _Appnum;
           }

           set 
           {
              _Appnum = value;
              OnPropertyChanged("Appnum");
           }
        }

        #endregion

        #region methods

        /// <summary>
        /// преобразуем без КП, он будет прицеплен позже
        /// </summary>
        /// <param name="entity">объект предметной области</param>
        public override void InitializeEntity(Schedulecontract entity)
        {
            entity.Id = Id;
            entity.Contractdoc = Contractdoc;
            entity.Appnum = Appnum;
        }

        #endregion
    }
}
