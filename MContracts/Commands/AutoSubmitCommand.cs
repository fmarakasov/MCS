using System;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using MCDomain.DataAccess;
using System.Windows.Input;

namespace MContracts.Commands
{
    /// <summary>
    /// Реализует RelayCommand с функцией автоматического вызова SubmitChanges из связанного с командой объекта IContractRepository/>
    /// </summary>
    public class AutoSubmitCommand : RelayCommand, ICommand, IDisposable
    {
        /// <summary>
        /// Получает объект IContractRepository в котором вызывается SubmitChanges
        /// </summary>
        public IContractRepository Repository { get; private set; } 
        public AutoSubmitCommand(Action<object> execute, IContractRepository repository) : base(execute)
        {
            Contract.Requires(repository != null);
            Repository = repository;
        }

        public AutoSubmitCommand(Action<object> execute, Predicate<object> canExecute, IContractRepository repository) : base(execute, canExecute)
        {
            Contract.Requires(repository != null);
            Repository = repository;
        }

        private Action _update;
        public AutoSubmitCommand(Action<object> execute, Predicate<object> canExecute, Action update, IContractRepository repository)
            : base(execute, canExecute)
        {
            Contract.Requires(repository != null);
            Repository = repository;
            _update = update;
        }


        public new void Execute(object parameter)
        {
            SubmitImpl(parameter);
        }

        void ICommand.Execute(object parameter)
        {
            SubmitImpl(parameter);
        }

        private void SubmitImpl(object parameter)
        {
            base.Execute(parameter);
            Repository.SubmitChanges();
            if (_update != null) _update();
        }

        public void Dispose()
        {
            Repository.Dispose();
        }
    }
    /// <summary>
    /// Реализует RelayCommand с функцией автоматического вызова SubmitChanges из связанного с командой объекта IContractRepository/>
    /// </summary>
    /// <typeparam name="T">Тип параметра команды</typeparam>
    public class AutoSubmitCommand<T> : RelayCommand<T>, ICommand
    {
        /// <summary>
        /// Получает объект IContractRepository в котором вызывается SubmitChanges
        /// </summary>
        public IContractRepository Repository { get; private set; }
        public AutoSubmitCommand(Action<T> execute, IContractRepository repository)
            : base(execute)
        {
            Contract.Requires(repository != null);
            Repository = repository;
        }

        public AutoSubmitCommand(Action<T> execute, Predicate<T> canExecute, IContractRepository repository)
            : base(execute, canExecute)
        {
            Contract.Requires(repository != null);
            Repository = repository;
        }


        private Action _update;
        public AutoSubmitCommand(Action<T> execute, Predicate<T> canExecute, Action update, IContractRepository repository)
            : base(execute, canExecute)
        {
            Contract.Requires(repository != null);
            Repository = repository;
            _update = update;
        }

        public new void Execute(object parameter)
        {
            SubmitImpl(parameter);
        }

        void ICommand.Execute(object parameter)
        {
            SubmitImpl(parameter);
        }

        private void SubmitImpl(object parameter)
        {
            base.Execute(parameter);
            Repository.SubmitChanges();
            if (_update != null) _update();
        }
       
    }

}