using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Windows.Input;
using MCDomain.DataAccess;
using MCDomain.Model;
using McUIBase.Factories;
using UIShared.Commands;
using MContracts.Controls.Dialogs;
using System.ComponentModel;
using MContracts.View;
using MediatorLib;
using MContracts.Classes;
using MContracts.ViewModel.Helpers;
using McUIBase.ViewModel;


namespace MContracts.ViewModel
{
    public class DisposalsRepositoryViewModel: WorkspaceViewModel
    {

        protected override void OnPropertyChanged(string propertyName)
        {
            base.OnPropertyChanged(propertyName);
        }

        public void SendPropertyChanged(string propertyName)
        {
            OnPropertyChanged(propertyName);
        }

        [MediatorMessageSink(RequestRepository.CATALOG_CHANGED, ParameterType = typeof(CatalogType))]
        public void CatalogChanged(CatalogType c)
        {
            if (c == CatalogType.Employee)
            {
                Repository.Refresh(System.Data.Linq.RefreshMode.OverwriteCurrentValues, Repository.Employees);
            }
            else if (c == CatalogType.Role)
            {
                Repository.Refresh(System.Data.Linq.RefreshMode.OverwriteCurrentValues, Repository.Roles);
            }
        }

        [MediatorMessageSink(RequestRepository.DISPOSALS_CHANGED, ParameterType = typeof(object))]
        public void ReloadDisposals(object o)
        {
            _disposals = null;
            SendPropertyChanged("Disposals");
            SelectedDisposal = Repository.Disposals.FirstOrDefault(d => d.Id == SelectedDisposalId);
            SendPropertyChanged("SelectedDisposal");
        }

        private IBindingList _disposals;
        public IBindingList Disposals
        {
            get
            {
                if (_disposals == null)
                {
                    _disposals = new BindingList<Disposal>();
                    Repository.TryGetContext().Refresh(RefreshMode.OverwriteCurrentValues, Repository.Disposals);
                    foreach (Disposal d in Repository.Disposals)
                    {
                        _disposals.Add(d);
                    }
                }
                return _disposals;
            }
        }

        private Disposal _selecteddisposal;
        public Disposal SelectedDisposal
        {
            get
            {
                return _selecteddisposal;
            }
            set
            {
                _selecteddisposal = value;

            }

        }

        public long SelectedDisposalId { get; set; }

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

        public DisposalsRepositoryViewModel(IContractRepository repository): base(repository)
        {
            ViewMediator.Register(this);
        }

        public override string DisplayName
        {
            get { return "Распоряжения"; }
        }



        public void CreateDisposal()
        {


            var vm = new DisposalContentViewModel(RepositoryFactory.CreateContractRepository());

            var dlg = new DialogShell {Content = new DisposalContentView() {DataContext = vm}};
            dlg.ViewModel = vm;

            dlg.ShowDialog();
            if (vm.Disposal != null)
                SelectedDisposalId = vm.Disposal.Id;
            SendPropertyChanged("Disposals");
            ViewMediator.NotifyColleagues(RequestRepository.DISPOSALS_CHANGED, (Disposal) null);
        }

        public bool CanCreateDisposal
        {
            get
            {
                return true;
            }
        }

        private RelayCommand _newDisposalCommand;
        public RelayCommand NewDisposalCommand
        {
            get
            {
                if (_newDisposalCommand == null)
                    _newDisposalCommand = new RelayCommand(x => CreateDisposal(), x => CanCreateDisposal);
                return _newDisposalCommand;

            }
        }

        public void EditDisposal()
        {

            //AddActWindow dlg = new AddActWindow(RepositoryFactory.CreateContractRepository())
            var r = RepositoryFactory.CreateContractRepository();
            
            var vm = new DisposalContentViewModel(r)
                         {Disposal = r.Disposals.FirstOrDefault(x => x.Id == SelectedDisposal.Id)};

            var dlg = new DialogShell {ViewModel = vm, Content = new DisposalContentView() {DataContext = vm}};
            


            dlg.ShowDialog();
            if (vm.Disposal != null)
                SelectedDisposalId = vm.Disposal.Id;
            SendPropertyChanged("Disposals");
            ViewMediator.NotifyColleagues(RequestRepository.DISPOSALS_CHANGED, (Disposal)null);
            
        }

        public bool CanEditDisposal
        {
            get
            {
                return (SelectedDisposal != null);
            }
        }

        private RelayCommand _editDisposalCommand;
        public RelayCommand EditDisposalCommand
        {
            get
            {
                if (_editDisposalCommand == null)
                    _editDisposalCommand = new RelayCommand(x => EditDisposal(), x => CanEditDisposal);
                return _editDisposalCommand;

            }
        }

        public void DeleteDisposal()
        {

            Repository.DeleteDisposal(SelectedDisposal);
            Disposals.Remove(SelectedDisposal);
            Repository.SubmitChanges();
            SendPropertyChanged("Disposals");

        }

        public bool CanDeleteDisposal
        {
            get
            {
                return (SelectedDisposal != null);
            }
        }

        private RelayCommand _deleteDisposalCommand;
        public RelayCommand DeleteDisposalCommand
        {
            get
            {
                if (_deleteDisposalCommand == null)
                    _deleteDisposalCommand = new RelayCommand(x => DeleteDisposal(), x => CanDeleteDisposal);
                return _deleteDisposalCommand;

            }
        }

    }
}
