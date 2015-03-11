#region

using System;
using System.Diagnostics.Contracts;
using System.Windows.Input;
using MCDomain.DataAccess;
using MContracts.Commands;

#endregion

namespace MContracts.ViewModel
{
    /// <summary>
    ///   Модель представления с поддержкой доступа к репозиторию
    /// </summary>
    public abstract class RepositoryViewModel : ViewModelBase
    {
        private IContractRepository _repository;
        private RelayCommand _saveCommand;

        private RepositoryViewModel()
        {
        }

        private MainWindowViewModel _mainViewModel;
        public MainWindowViewModel MainViewModel
        {
            get { return _mainViewModel; }
            set
            {
                if (value == _mainViewModel) return;
                _mainViewModel = value;
                OnPropertyChanged("MainViewModel");
            }
        }
        public override void CollectDiagnosticsData(Exception exception)
        {
            base.CollectDiagnosticsData(exception);
            exception.Data.Add("ViewModel.Repository.", Repository.TryGetContext().ToString());
        }

        protected RepositoryViewModel(IContractRepository repository, ViewModelBase owner = null)
            : base(owner)
        {
            Contract.Requires(repository != null);
            Contract.Ensures(_repository == repository);
            Repository = repository;
        }

        /// <summary>
        ///   Получает репозиторий модели представления
        /// </summary>
        public IContractRepository Repository
        {
            get { return _repository; }
            set
            {
                Contract.Requires(value != null);
                Contract.Ensures(_repository == value);
                if (_repository == value) return;
                _repository = value;
                OnPropertyChanged("Repository");
            }
        }

        /// <summary>
        ///   Получает команду сохранения изменений в рабочей области
        /// </summary>
        public ICommand SaveCommand
        {
            get
            {
                if (_saveCommand == null)
                    _saveCommand = new RelayCommand(p => Save(), x => CanSave());
                return _saveCommand;
            }
        }

        /// <summary>
        ///   Возвращает признак того, что данные модели были модифицированы
        /// </summary>
        public virtual bool IsModified
        {
            get { return Repository.IsModified; }
        }

        /// <summary>
        /// Выполняет делегат, вызывая функцию фиксации результатов в репозитории
        /// </summary>
        /// <param name = "action">Делегат</param>
        /// <exception cref="SubmitChangesFailed"></exception>
        protected void AutoSubmit(Action action)
        {
            action();
            Submit();
        }

        private void Submit()
        {
            try
            {
                Repository.SubmitChanges();
            }
            catch (Exception exception)
            {
                throw new SubmitChangesFailed("Не удалось сохранить результаты.", exception);
            }
        }

        /// <summary>
        /// Выполняет делегат, вызывая функцию фиксации результатов в репозитории
        /// </summary>
        /// <param name = "action">Делегат</param>
        /// <param name="obj">Параметр метода</param>
        /// <exception cref="SubmitChangesFailed"></exception>
        protected void AutoSubmit<T>(Action<T> action, T obj)
        {
            action(obj);
            Submit();
        }
        /// <summary>
        /// Выполняет делегат, вызывая функцию фиксации результатов в репозитории
        /// </summary>
        /// <param name = "action">Делегат</param>
        /// <param name="obj1">Первый параметр метода</param>
        /// <param name="obj2">Второй параметр метода</param>
        /// <exception cref="SubmitChangesFailed"></exception>
        protected void AutoSubmit<T1, T2>(Action<T1, T2> action, T1 obj1, T2 obj2)
        {
            action(obj1, obj2);
            Submit();
        }

        /// <summary>
        /// Выполняет делегат, вызывая функцию фиксации результатов в репозитории
        /// </summary>
        /// <param name = "action">Делегат</param>
        /// <param name="obj1">Первый параметр метода</param>
        /// <param name="obj2">Второй параметр метода</param>
        /// <param name="obj3">Третий параметр метода</param>
        /// <exception cref="SubmitChangesFailed"></exception>
        protected void AutoSubmit<T1, T2, T3>(Action<T1, T2, T3> action, T1 obj1, T2 obj2, T3 obj3)
        {
            action(obj1, obj2, obj3);
            Submit();
        }

        /// <summary>
        /// Выполняет делегат, вызывая функцию фиксации результатов в репозитории
        /// </summary>
        /// <param name = "action">Делегат</param>
        /// <param name="obj1">Первый параметр метода</param>
        /// <param name="obj2">Второй параметр метода</param>
        /// <param name="obj3">Третий параметр метода</param>
        /// <param name="obj4">Четвёртый параметр метода</param>
        /// <exception cref="SubmitChangesFailed"></exception>
        protected void AutoSubmit<T1, T2, T3, T4>(Action<T1, T2, T3, T4> action, T1 obj1, T2 obj2, T3 obj3, T4 obj4)
        {
            action(obj1, obj2, obj3, obj4);
            Submit();
        }


        /// <summary>
        ///   Переопределите для задания логики сохранения изменений в модели
        /// </summary>
        protected abstract void Save();

        /// <summary>
        ///   Переопределите для проверки возможности сохранения модели
        /// </summary>
        /// <returns></returns>
        protected abstract bool CanSave();

        protected override void OnDispose()
        {
            if (Repository != null) Repository.Dispose();
            base.OnDispose();
        }
    }
}
