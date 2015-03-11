using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using MContracts.Commands;

namespace MContracts.ViewModel.Helpers
{
    /// <summary>
    /// Вспомогательный класс для реализации команды закрытия
    /// </summary>
    public class Closable
    {
        public object Client { get; private set; }

        public Closable(object client)
        {
            Client = client;
        }

        private ICommand _closeCommand;
        /// <summary>
        /// Returns the command that, when invoked, attempts
        /// to remove this workspace from the user interface.
        /// </summary>
        public ICommand CloseCommand
        {
            get
            {
                if (_closeCommand == null)
                    _closeCommand = new RelayCommand(param => OnRequestClose());

                return _closeCommand;
            }
        }

        #region RequestClose [event]

        /// <summary>
        /// Raised when this workspace should be removed from the UI.
        /// </summary>
        public event EventHandler RequestClose;

        private void OnRequestClose()
        {
            EventHandler handler = RequestClose;
            if (handler != null)
                handler(Client, EventArgs.Empty);
        }

        #endregion // RequestClose [event]
        
    }
}
