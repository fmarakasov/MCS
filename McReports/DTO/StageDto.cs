using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using MCDomain.Model;

namespace McReports.DTO
{
    public class StageDto//: EntityDto<Stage>
    {

        #region Properties
        
        /// <summary>
        /// идентификатор этапа в базе данных
        /// </summary>
        public long Id { get; set; }
        
        /// <summary>
        /// номер этапа
        /// </summary>
        public string Num { get; set; }
        /// <summary>
        /// наименование этапа
        /// </summary>
        public string Subject { get; set; }
        /// <summary>
        /// код стройки
        /// </summary>
        public string Code { get; set; }

        private Nds _nds;

        /// <summary>
        /// ставка НДС
        /// </summary>
        public Nds Nds
        {
            get { return _nds; }
            set
            {
                _nds = value;
            }

        }

        private Ndsalgorithm _ndsalgorithm;
        /// <summary>
        /// алгоритм НДС
        /// </summary>
        public Ndsalgorithm Ndsalgorithm
        {
            get { return _ndsalgorithm; }
            set
            { 
                _ndsalgorithm = value; 
            }
        }

        private MoneyModel _stagemoneymodel;
        /// <summary>
        /// модель представления денег для этапа
        /// </summary>
        public MoneyModel StageMoneyModel
        {
            get { return _stagemoneymodel; }
            set
            {
                _stagemoneymodel = value;
            }

        }

        private DateTime? startsat;
        /// <summary>
        /// дата начала
        /// </summary>
        public DateTime? Startsat
        {
            get { return startsat; }
            set
            {
                startsat = value;
                //OnPropertyChanged("Startsat");
            }
        }

        private DateTime? endsat;
        /// <summary>
        /// дата окончания
        /// </summary>
        public DateTime? Endsat 
        { 
            get { return endsat; }
            set 
            { 
                endsat = value;
                //OnPropertyChanged("Endsat");
            }
        }

        private decimal price;
        /// <summary>
        /// стоимость этапа
        /// </summary>
        public decimal Price
        {
            get { return price; }
            set
            {
                price = value;
                //OnPropertyChanged("Price");
            }
        }

        /// <summary>
        /// статус завершения
        /// </summary>
        public StageCondition Condition { get; set; }

        /// <summary>
        /// уровень вложенности
        /// </summary>
        public int Level { get; set; }

        /// <summary>
        /// старший этап
        /// </summary>
        public StageDto Parent { get; set; }

        private ObservableCollection<StageDto> stages = new ObservableCollection<StageDto>();
        /// <summary>
        /// вложенные этапы
        /// </summary>
        public ObservableCollection<StageDto> Stages
        {
            get { return stages; }
        }

        private ScheduleDto _schedule;
        /// <summary>
        /// КП
        /// </summary>
        public ScheduleDto Schedule
        {
            get
            {
                return _schedule;
            }

            set
            {
                _schedule = value;
            }
        }

        private Act _act;
        /// <summary>
        /// акт, приписанный к этапу
        /// </summary>
        public Act Act
        {
            get
            {
                return _act;

            }

            set
            {
                _act = value;
            }

        }

        private ObservableCollection<StageresultDto> stageresults = new ObservableCollection<StageresultDto>();
        public ObservableCollection<StageresultDto> Stageresults
        {
            get { return stageresults; }
            set { stageresults = value; }
        }

        private IBindingList resultsBindingList;
        public IBindingList ResultsBindingList
        {
            get
            {
                if (resultsBindingList == null)
                    resultsBindingList = new BindingList<StageresultDto>();
                return resultsBindingList;
            }
        }

        #endregion

        /*
        #region Methods
        public StageDto()
        {
            Stages.CollectionChanged += new System.Collections.Specialized.NotifyCollectionChangedEventHandler(stages_CollectionChanged);
            this.PropertyChanged += new PropertyChangedEventHandler(StageDto_PropertyChanged);
        }

        void StageDto_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (Parent != null)
            {
                Parent.stages_CollectionChanged(this.Parent.Stages, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
            }
        }

        void stages_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            //при изменении коллекции чайлдов изменяем цену и даты родителя
            var stages = sender as ObservableCollection<StageDto>;
            if (stages.Count > 0)
            {
                this.price = stages.Sum(x => x.Price);
                OnPropertyChanged("Price");
                this.startsat = stages.Min(x => x.Startsat);
                OnPropertyChanged("Startsat");
                this.endsat = stages.Max(x => x.Endsat);
                OnPropertyChanged("Endsat");
            }
        }

  

        public static StageDto AsDto(Stage stage)
        {
            Contract.Requires(stage != null);
            Contract.Ensures(Contract.Result<StageDto>() != null);
            return new StageDto()
            {
                Num = stage.Num,
                Subject = stage.Subject,
                Startsat = stage.Startsat,
                Endsat = stage.Endsat,
                Condition = stage.Stagecondition,
                Price = stage.PriceValue,
                Level = stage.Level,
                Code = stage.Code,
                Id = stage.Id,

                stages = new ObservableCollection<StageDto>(stage.Stages.Select(x => StageDto.AsDto(x)))
            };
        }

        

        public override string ToString()
        {
            return String.Format("{0} - {1}", Num, Subject);
        }

        public void AddStage(StageDto Child)
        {
            if (!Stages.Any(x => x == Child))
            {
                this.Stages.Add(Child);
            }
        }

        public void DeleteStage(StageDto Child)
        {
            if (Stages.Any(x => x == Child))
            {
                this.Stages.Remove(Child);
            }
        }



        
        public void OnResultsChanged()
        {
            OnPropertyChanged("ResultsBindingList");            
        }

        public override void InitializeEntity(Stage entity)
        {
            

            entity.Id = Id;
            entity.Startsat = Startsat;
            entity.Endsat = Endsat;
            entity.Subject = Subject;
            entity.Code = Code;
            entity.PriceValue = Price;
            entity.ParentStage = Parent != null ? Parent.AsEntity : null;
            entity.Nds = Nds;
            entity.Ndsalgorithm = Ndsalgorithm;
            entity.Act = Act;
            
            entity.Num = Num;

            // подэтапы 
            Stage fs;
            foreach (StageDto s in Stages)
            {
                fs = entity.Stages.First(p => p.Id == s.Id);
                if (fs == null)
                {
                    fs = s.AsEntity;
                    fs.Schedule = entity.Schedule;
                    entity.Stages.Add(fs);
                }
                else
                {
                    s.InitializeEntity(fs);
                }
            }

            // результаты
            Stageresult rs;
            foreach (StageresultDto s in Stageresults)
            {
                rs = entity.Stageresults.First(p => p.Id == s.Id);
                if (rs == null)
                {
                    rs = s.AsEntity;
                    rs.Stage = entity;
                    entity.Stageresults.Add(rs);
                }
                else
                {
                    s.InitializeEntity(rs);
                }
            }

        }

        public void SetLevel()
        {

            int level = 0;
            while (Parent != null)
            {
                level++;
                Parent = Parent.Parent;

            }
            Level = level;
        }


        public void OnNdsPriceChanged()
        {
              OnPropertyChanged("StageMoneyModel");
            
        }

        public void OnStageconditionChanged()
        {
                OnPropertyChanged("Stagecondition");
                OnPropertyChanged("IsReadonly");
            
        }



        #endregion
        */
    }
}
