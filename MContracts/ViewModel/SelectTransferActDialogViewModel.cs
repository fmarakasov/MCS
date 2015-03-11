using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using MCDomain.DataAccess;
using MCDomain.Model;
using McUIBase.ViewModel;
using CommonBase;

namespace MContracts.ViewModel
{
    public class SelectTransferActDialogViewModel : RepositoryViewModel
    {
        private Transferacttype _actType;
        private ObservableCollection<Transferact> _acts;

        public Transferacttype ActType
        {
            get { return _actType; }
            set
            {
                if (_actType == value) return;
                
                _actType = value;
                OnPropertyChanged(() => ActType);
                _acts = null;
                OnPropertyChanged(() => Acts);
            }
        }

        public ObservableCollection<Transferact> Acts
        {
            get
            {
                if (_actType == null) return null;
                return _acts ?? (_acts = new ObservableCollection<Transferact>(FetchActs()));
            }
        }
        public override string DisplayName
        {
            get { return ActType.Return(x => string.Format("Выберите {0}", x.ToString().ToLower()), "Выберите акт"); }
            protected set
            {
                base.DisplayName = value;
            }
        }

        private Transferact _selected;

        public Transferact Selected
        {
            get { return _selected; }
            set
            {
                if (_selected == value) return;
                _selected = value;
                OnPropertyChanged(() => Selected);
            }
        }

        private IEnumerable<Transferact> FetchActs()
        {
            return Repository.Transferacts.Where(x => x.Transferacttype == ActType);
        }

        public SelectTransferActDialogViewModel(IContractRepository repository):base(repository)
        {
            
        }
        protected override void Save()
        {
           
        }

        protected override bool CanSave()
        {
            return true;
        }
    }
}
