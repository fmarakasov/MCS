using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MCDomain.Model;
using System.ComponentModel;
using System.Collections.ObjectModel;


namespace MContracts.DTO
{
    public class StageresultDto : EntityDto<Stageresult>, INotifyPropertyChanged
    {

        #region Properties
        public long Id
        {
            get;
            set;
        }


        public string Name { get; set; }

        public StageDto Stage { get; set; }

        public Ntpsubview Ntpsubview { get; set; }

        public Economefficiencytype Economefficiencytype { get; set; }

        private ObservableCollection<EfparameterstageresultDto> efparameterstageresults = new ObservableCollection<EfparameterstageresultDto>();
        public ObservableCollection<EfparameterstageresultDto> Efparameterstageresults
        {
            get { return efparameterstageresults; }
            set { efparameterstageresults = value; }
        }


        private IBindingList parametersBindingList;
        public IBindingList ParametersBindingList
        {
            get
            {
                if (parametersBindingList == null)
                {
                    parametersBindingList = new BindingList<EfparameterstageresultDto>();

                    foreach (EfparameterstageresultDto p in Efparameterstageresults)
                    {
                        parametersBindingList.Add(p);
                    }
                }
                return parametersBindingList;
            }
        }

        #endregion

        #region Methods

        public override void InitializeEntity(Stageresult entity)
        {
            entity.Id = Id;
            entity.Name = Name;
            entity.Ntpsubview = Ntpsubview;
            entity.Economefficiencytype = Economefficiencytype;
        }


        private Economefficiencytype backuptype;
        private bool _inTx = false;

        public void BeginEdit()
        {
            if (!_inTx)
            {
                backuptype = this.Economefficiencytype;
                _inTx = true;
            }
        }

        public void CancelEdit()
        {
            if (_inTx)
            {
                this.Economefficiencytype = backuptype;
                _inTx = false;
            }
        }

        public void EndEdit()
        {
            if (_inTx)
            {
                backuptype = null;
                _inTx = false;
            }
        }

        private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);

        public event PropertyChangingEventHandler PropertyChanging;


        protected virtual void SendPropertyChanging()
        {
            if (this.PropertyChanging != null)
                this.PropertyChanging(this, emptyChangingEventArgs);
        }

        protected virtual void SendPropertyChanging(String propertyName)
        {
            if (this.PropertyChanging != null)
                this.PropertyChanging(this, new PropertyChangingEventArgs(propertyName));
        }

        protected virtual void SendPropertyChanged(String propertyName)
        {
            
                OnPropertyChanged(propertyName);
        }

        #endregion
    }
}
