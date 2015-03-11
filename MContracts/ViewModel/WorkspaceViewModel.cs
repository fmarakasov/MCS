using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using MCDomain.DataAccess;
using MContracts.Commands;
using MContracts.ViewModel.Helpers;

namespace MContracts.ViewModel
{
    /// <summary>
    /// This ViewModelBase subclass requests to be removed 
    /// from the UI when its CloseCommand executes.
    /// This class is abstract.
    /// </summary>
    public abstract class WorkspaceViewModel : RepositoryViewModel
    {
      

        public ICommand CloseCommand { get { return Closable.CloseCommand; } }
        public readonly Closable Closable;

        #region Fields

        private readonly IDictionary<string, object> _stateRepository = new Dictionary<string, object>();

        private bool _isActive;
        private RelayCommand _showCommand;

        protected WorkspaceViewModel(IContractRepository repository) : base(repository)
        {
            Closable = new Closable(this);
        }

        /// <summary>
        /// Используется для сохранения пользовательских настроек представления
        /// </summary>
        protected IDictionary<string, object> StateRepository
        {
            get { return _stateRepository; }
        }

        public abstract bool IsClosable { get; }


        /// <summary>
        /// Получает или устанавливает состояние активности рабочей области
        /// </summary>
        public bool IsActive
        {
            get { return _isActive; }
            set
            {
                if (_isActive == value) return;

                _isActive = value;

                // Если область активируется, то послать команду на восстановление состояние для View, в противном случае - сохранить состояние View
                if (_isActive)
                {
                    foreach (var ws in MainViewModel.Workspaces.Where(ws => ws != this))
                    {
                        ws.IsActive = false;
                    }

                    System.Diagnostics.Debug.WriteLine("Модель представления InstanceId={0} активирована. this.ToString():{1}", InstanceId, ToString());
                    RestoreState();
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("Модель представления InstanceId={0} деактивирована. this.ToString():{1}", InstanceId, ToString());
                    SaveState();
                }

                OnPropertyChanged(()=>IsActive);
            }
        }

        /// <summary>
        /// Получает команду активации рабочей области
        /// </summary>
        public ICommand ShowCommand
        {
            get
            {
                if (_showCommand == null)
                    _showCommand = new RelayCommand(z => Show());
                return _showCommand;
            }
        }

        /// <summary>
        /// Переопределите для сохранения любых настроек при деактивации рабочей области
        /// </summary>
        protected virtual void SaveState()
        {
        }

        /// <summary>
        /// Переопределите для восстановления любых настроек при активации рабочей области
        /// </summary>
        protected virtual void RestoreState()
        {
        }


        private void Show()
        {
            IsActive = true;
        }

        #endregion // Fields

        #region Constructor

        #endregion // Constructor
    }
}