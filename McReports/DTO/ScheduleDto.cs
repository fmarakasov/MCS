using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using MCDomain.Model;

namespace McReports.DTO
{
    public class ScheduleDto//: EntityDto<Schedule>
    {

        #region Properties

        private long _Id;
        /// <summary>
        /// идентификатор КП
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

        private SchedulecontractDto _schedulecontract;
        /// <summary>
        /// родительский ДТО связи с договором
        /// </summary>
        public SchedulecontractDto Schedulecontract
        {
            get
            {
                return _schedulecontract;
            }
            set
            {
                _schedulecontract = value;
            }
        }

        public Currencymeasure Currencymeasure { get; set; }
        public Worktype Worktype { get; set; }

        private ObservableCollection<StageDto> stages = new ObservableCollection<StageDto>();
        public ObservableCollection<StageDto> Stages
        {
            get { return stages; }
            set { stages = value; }
        }


        #endregion

        /*
        #region Methods
        public ScheduleDto AsDto(Schedulecontract schedulecontract)
        {
            Contract.Requires(schedulecontract != null);
            Contract.Ensures(Contract.Result<ScheduleDto>() != null);
            
            return new ScheduleDto() 
            {
                Schedulecontract = this.Schedulecontract,
                Currencymeasure = schedulecontract.Schedule.Currencymeasure,
                Worktype = schedulecontract.Schedule.Worktype,
                Stages = new ObservableCollection<StageDto>(schedulecontract.Schedule.Stages.Select(x => StageDto.AsDto(x)))
            };
        }



        public override string ToString()
        {
            return Schedulecontract.Appnum.ToString() + " " + Worktype;
        }


        public override void InitializeEntity(Schedule entity)
        {
            entity.Id = Id;
            entity.Worktype = Worktype;
            entity.Currencymeasure = Currencymeasure;

            Stage fs;
            foreach (StageDto s in Stages) 
            {
                fs = entity.Stages.First(p => p.Id == s.Id);
                if (fs == null)
                {
                    fs = s.AsEntity;
                    fs.Schedule = entity;
                    entity.Stages.Add(fs);
                }
                else
                {
                    s.InitializeEntity(fs);
                }
            }
        }

        #endregion
         */
    }
}
