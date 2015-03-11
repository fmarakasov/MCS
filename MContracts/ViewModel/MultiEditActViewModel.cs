using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Windows.Input;
using MCDomain.DataAccess;
using MCDomain.Model;
using UIShared.Commands;
using MContracts.ViewModel.Helpers;
using McUIBase.ViewModel;

namespace MContracts.ViewModel
{
    public class MultiEditActViewModel: WorkspaceViewModel
    {
        public override bool IsClosable
        {
            get { return true; }
        }

        protected override void Save()
        {
            
        }

        protected override bool CanSave()
        {
            return true;
        }

        public MultiEditActViewModel(IContractRepository repository)
            : base(repository)
        {
            
        }

        public IList<ActRepositoryEntity> Acts { get; set; }

        public Nds SelectedNds { get; set; }
        public IEnumerable<Nds> Ndses
        {
            get
            {
                return Repository.Nds;
            }
        }

        public Enterpriseauthority SelectedEnterpriseauthority { get; set; }
        public IEnumerable<Enterpriseauthority> Enterpriseauthorities
        {
            get
            {
                return Repository.Enterpriseauthorities;
            }
        }

        public Ndsalgorithm SelectedNdsalgorithm { get; set; }
        public IEnumerable<Ndsalgorithm> Ndsalgorithms
        {
            get
            {
                return Repository.Ndsalgorithms;
            }
        }

        public Region SelectedRegion { get; set; }
        public IEnumerable<Region> Regions
        {
            get
            {
                return Repository.Regions;
            }
        }

        public Currency SelectedCurrency { get; set; }
        public IEnumerable<Currency> Currencies
        {
            get
            {
                return Repository.Currencies;
            }
        }

        public Acttype SelectedActtype { get; set; }
        public IEnumerable<Acttype> Acttypes
        {
            get
            {
                return Repository.Acttypes;
            }
        }

        public Currencymeasure SelectedCurrencymeasure { get; set; }
        public IEnumerable<Currencymeasure> Currencymeasures
        {
            get
            {
                return Repository.Currencymeasures;
            }
        }

        public void ActtypesChanged()
        {
            OnPropertyChanged("Acttypes");
        }

        public void EditActs()
        {
            foreach (Act act in Acts.Select(x=>x.Act))
            {
                act.Region = SelectedRegion;
                act.Enterpriseauthority = SelectedEnterpriseauthority;
                act.Currency = SelectedCurrency;
                act.Currencymeasure = SelectedCurrencymeasure;
                act.Nds = SelectedNds;
                act.Ndsalgorithm = SelectedNdsalgorithm;
                act.Acttype = SelectedActtype;
            }
        }
    }
}
