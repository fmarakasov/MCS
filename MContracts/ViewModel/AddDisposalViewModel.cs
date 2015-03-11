using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using CommonBase;
using MCDomain.DataAccess;
using MCDomain.Model;
using UIShared.Commands;
using MContracts.Controls.Dialogs;
using MContracts.Classes;
using McUIBase.ViewModel;

namespace MContracts.ViewModel
{

    public class AddDisposalViewModel : WorkspaceViewModel
    {

        public RelayCommand OkPressedAction
        {
            get { return _okpressedaction ?? (_okpressedaction = new RelayCommand(OkPressed, CanPressOk)); }
        }

        private bool CanPressOk(object obj)
        {
            return (Disposal.Validate() == string.Empty);
        }

        private void OkPressed(object obj)
        {
            
        }

        //public event EventHandler<RoutedEventArgs> DoAfterOkPressed;

        public void SendPropertyChanged(string propertyName)
        {
        }


        public AddDisposalViewModel(IContractRepository repository): base(repository)
        {

        }

        public override bool IsClosable
        {
            get { return true; }
        }

        protected override void Save()
        {
            return;
        }

        protected override bool CanSave()
        {
            return true;
        }

        private Disposal _disposal;
        public Disposal Disposal
        {
            get { return _disposal; }
            set
            {
                if (_disposal == value) return;
                _disposal = value;
            }
        }

        private Contractdoc _selectedcontract;
        public Contractdoc SelectedContract
        {
            get
            {
                return _selectedcontract;
            }

            set
            {
                if (_selectedcontract == value) return;
                _selectedcontract = value;
            }
        }


        private RelayCommand _okpressedaction;


    }

}
