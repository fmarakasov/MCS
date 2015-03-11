using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.ComponentModel;
using CommonBase;
using MCDomain.DataAccess;
using MContracts.Classes.Filtering;
using UIShared.Commands;
using System.Windows.Input;
using System.Windows.Controls;
using MCDomain.Model;
using MContracts.ViewModel;
using McUIBase.ViewModel;

namespace MContracts.Controls
{

    public class FilterEventArgs: EventArgs
    {
        public Filterstate Filterstate { get; set; }
    }

    public class FilterContractsViewModel: MainViewModelViewModelBase
    {

        public FilterContractsViewModel(IContractRepository repository)
            : base(repository)
        {
        
        }

        #region Overrides of RepositoryViewModel

        /// <summary>
        ///   Переопределите для задания логики сохранения изменений в модели
        /// </summary>
        protected override void Save() {}

        /// <summary>
        ///   Переопределите для проверки возможности сохранения модели
        /// </summary>
        /// <returns></returns>
        protected override bool CanSave()
        {
            return true;
        }

        #endregion
        
        public  IEnumerable<Filterstate> Filterstates
        {
            get
            {
                return Repository.Filterstates.OrderByDescending(x => x.Id);
            }
        }


        public event EventHandler FilterChanged;

        private Filterstate _selectedfilterstate;
        public Filterstate SelectedFilterstate
        {
            get
            {
                return _selectedfilterstate ?? Repository.Filterstates.FirstOrDefault(f => f.Id == (long)FilterstateType.CurrentYear); ;
            }
            set 
            { 
                _selectedfilterstate = value;
                OnPropertyChanged(()=>SelectedFilterstate);
                ApplySelectedFilter();
                if (FilterChanged != null)
                {
                    var e = new FilterEventArgs();
                    e.Filterstate = _selectedfilterstate;
                    FilterChanged(this, e);
                }
            }
        }

        public void ApplySelectedFilter()
        {
            if (MainViewModel != null) MainViewModel.SelectedFilterstate = SelectedFilterstate;
        }

        public void SendPropertyChanged(string  propertyname)
        {
            OnPropertyChanged(propertyname);
        }

        public int _year = 0;
        public int Year
        {
            get
            {
                if (_year == 0) Year = DateTime.Today.Year;
                return _year;
            }

            set
            {
                if (_year == value) return;
                _year = value;
                foreach (Filterstate f in Filterstates) { f.ResetYear(_year); }
                SendPropertyChanged("Year");
                SendPropertyChanged("SelectedFilterstate");
                ApplySelectedFilter();
            }
        }
    }
}

