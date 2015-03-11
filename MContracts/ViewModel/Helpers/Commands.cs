using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using MCDomain.Model;
using UIShared.Commands;
using UIShared.ViewModel;

namespace MContracts.ViewModel.Helpers
{
    /// <summary>
    /// Повторно используемые команды 
    /// </summary>
    public class Commands : ViewModelBase
    {
        private static readonly RelayCommand<Contractdoc> _openContractCommand;

        static Commands()
        {
            _openContractCommand = new RelayCommand<Contractdoc>(OpenContract);
        }

        /// <summary>
        /// Команда открытия договора
        /// </summary>
        public static RelayCommand<Contractdoc> OpenContractCommand
        {
            get { return _openContractCommand; }
        }

        private static void OpenContract(Contractdoc contractdoc)
        {
            Contract.Requires(contractdoc != null);
            ViewMediator.NotifyColleagues(RequestRepository.REQUEST_OPEN_CONTRACTDOC, contractdoc);
        }

    }


}
