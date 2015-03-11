using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using MCDomain.DataAccess;
using MCDomain.Model;
using UIShared.ViewModel;
using CommonBase;

namespace MContracts.ViewModel
{
    public class SelectContractDialogViewModel : ViewModelBase
    {
    
        private Contractdoc _general;
        private Contractdoc _newContractdoc;
       
        public string Title
        {
            get
            {
                return NewContract.Return(x => string.Format("Выберите генеральный договор для СД. №{0}", x.Internalnum),
                                          "Выбор генерального договора");
            }
        }

        public Contractdoc NewContract
        {
            get { return _newContractdoc; }
            set
            {
                if (_newContractdoc == value) return;
                _newContractdoc = value;
                OnPropertyChanged(() => NewContract);
            }
        }

        private List<Contractdoc> _contracts;
        public List<Contractdoc> Contracts
        {
            get { return _contracts; }
            set
            {
                if (_contracts == value) return;
                _contracts = value;
                OnPropertyChanged(() => Contracts);
            }
        }

        public Contractdoc General
        {
            get { return _general; }
            set
            {
                if (_general == value) return;
                _general = value;
                OnPropertyChanged(() => General);
            }
        }
    }
}
