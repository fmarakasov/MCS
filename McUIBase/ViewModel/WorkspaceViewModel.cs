using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Input;
using MCDomain.DataAccess;
using UIShared.Commands;
using UIShared.Common;

namespace McUIBase.ViewModel
{
    /// <summary>
    ///     This ViewModelBase subclass requests to be removed
    ///     from the UI when its CloseCommand executes.
    ///     This class is abstract.
    /// </summary>
    public abstract class WorkspaceViewModel : RepositoryViewModel, INotifyPropertyChanging
    {
        public readonly Closable Closable;

        #region Fields

        private readonly IDictionary<string, object> _stateRepository = new Dictionary<string, object>();

        private bool _isActive;
        private RelayCommand _showCommand;

        protected WorkspaceViewModel(IContractRepository repository) : base(repository)
        {
            Closable = new Closable(this);
            IsUnchangable = false;
        }

        /// <summary>
        ///     Используется для сохранения пользовательских настроек представления
        /// </summary>
        protected IDictionary<string, object> StateRepository
        {
            get { return _stateRepository; }
        }

        public abstract bool IsClosable { get; }


        /// <summary>
        ///     Получает или устанавливает состояние активности рабочей области
        /// </summary>
        public bool IsActive
        {
            get { return _isActive; }
            set
            {
                if (_isActive == value) return;
                OnPropertyChanging(new PropertyChangingEventArgs("IsActive"));
                _isActive = value;
                OnIsActiveChanged();
            }
        }

        /// <summary>
        ///     Получает имя контекстной группы меню для рабочей области
        /// </summary>
        public string ContextTab { get; protected set; }

        /// <summary>
        ///     Получает команду активации рабочей области
        /// </summary>
        public ICommand ShowCommand
        {
            get { return _showCommand ?? (_showCommand = new RelayCommand(z => IsActive = true)); }
        }

        /// <summary>
        ///     Получает признак того, что модель предметной области не поддерживает операции изменения
        /// </summary>
        public bool IsUnchangable { get; protected set; }

        protected virtual void OnIsActiveChanged()
        {
            OnPropertyChanged(() => IsActive);
        }

        /// <summary>
        ///     Переопределите для сохранения любых настроек при деактивации рабочей области
        /// </summary>
        protected virtual void SaveState()
        {
        }

        /// <summary>
        ///     Переопределите для восстановления любых настроек при активации рабочей области
        /// </summary>
        protected virtual void RestoreState()
        {
        }

        #endregion // Fields

        #region Constructor

        #endregion // Constructor

        public ICommand CloseCommand
        {
            get { return Closable.CloseCommand; }
        }

        public event PropertyChangingEventHandler PropertyChanging;

        protected virtual void OnPropertyChanging(PropertyChangingEventArgs e)
        {
            PropertyChangingEventHandler handler = PropertyChanging;
            if (handler != null) handler(this, e);
        }
    }
}