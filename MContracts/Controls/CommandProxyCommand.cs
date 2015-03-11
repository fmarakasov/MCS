using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace MContracts.Controls
{
    /// <summary>
    /// Обеспечивает доступ к команде в заданном объекте по имени свойства в объекте с типом ICommand. 
    /// Реализует интерфейс ICommand для прозрачного использования вместо свойства.  
    /// </summary>
    public class CommandProxyCommand : ICommand
    {
        /// <summary>
        /// Получает имя свойства в объекте-источнике с типом ICommand
        /// </summary>
        public string CommandName { get; private set; }
        /// <summary>
        /// Получает объект-источник
        /// </summary>
        public object CommandSource { get; private set; }
        /// <summary>
        /// Создаёт экземпляр команды-заместителя
        /// </summary>
        /// <param name="commandName">Имя свойства в объекте с типом ICommand</param>
        /// <param name="commandSource">Объект-источник</param>
        public CommandProxyCommand(string commandName, object commandSource)
        {
            Contract.Requires(!string.IsNullOrWhiteSpace(commandName));
            Contract.Requires(commandSource != null);
            CommandName = commandName;
            CommandSource = commandSource;
        }
        
        object InvokeCommandMemeber(string name, object parameter)
        {
            var cmd = CommandSource.GetType().GetProperty(CommandName).GetValue(CommandSource, null);
            return cmd.GetType().InvokeMember(name, BindingFlags.InvokeMethod, null, cmd, new[] {parameter}); 
        }
        /// <summary>
        /// Вызывает команду
        /// </summary>
        /// <param name="parameter"></param>
        public void Execute(object parameter)
        {
            InvokeCommandMemeber("Execute", parameter);            
        }
        /// <summary>
        /// Пороверяет возможность вызова команды
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public bool CanExecute(object parameter)
        {
            return (bool) InvokeCommandMemeber("CanExecute", parameter);
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
    }

    /// <summary>
    /// Прокси комманда для команд, требующих подтверждения выполнения (вызова метода Execute)
    /// </summary>
    class ConfirmableCommandProxyCommand : CommandProxyCommand, ICommand
    {
        private readonly string _confirmMessage;

        public ConfirmableCommandProxyCommand(string commandName, object commandSource, string confirmMessage) : base(commandName, commandSource)
        {
            _confirmMessage = confirmMessage;
        }



        new public void Execute(object sender)
        {
            if (System.Windows.MessageBox.Show(_confirmMessage, "Выполнить команду", 
                MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                base.Execute(sender);
        }
    }
}
