using System;
using System.Linq;
using System.Diagnostics.Contracts;

namespace UIShared.Commands
{
    public enum AppCommandType
    {
        /// <summary>
        /// Команда должна быть выполнена без подтверждения
        /// </summary>
        Silent,
        /// <summary>
        /// Команда перед выполнением требует подтверждения от пользователя
        /// </summary>
        Confirm
        
    }

    /// <summary>
    /// Положение сепаратора, связанного с кнопкой команды 
    /// </summary>
    public enum SeparatorType
    {
        /// <summary>
        /// Нет сепаратора
        /// </summary>
        None,
        /// <summary>
        /// Перед кнопкой команды
        /// </summary>
        Before,
        /// <summary>
        /// После кнопки команды
        /// </summary>
        After
    }
    /// <summary>
    /// Используется для пометки свойств типа ICommand, 
    /// которые должны быть доступны в интерфейсе пользователя через панель команд
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class ApplicationCommandAttribute:Attribute
    {
        /// <summary>
        /// Получает тип, ответственный за получение строковых данных для комманды
        /// </summary>
        public Type StringProviderType { get; private set; }
        /// <summary>
        /// Получает сообщение для пользователя при выполнении подтверждения команды
        /// </summary>
        public string ConfirmMessage { get; private set; }
        /// <summary>
        /// Получает тип команды
        /// </summary>
        public AppCommandType CmdType { get; private set; }
        /// <summary>
        /// Получает описание команды
        /// </summary>
        public string ToolTip { get; private set; }
        /// <summary>
        /// Получает строку с названием ресурса, представляющим картинку команды
        /// </summary>
        public string ImageResource { get; private set; }
        /// <summary>
        /// Получает или устанавливает положение сепаратора, связанного с командой
        /// </summary>
        public SeparatorType Separator { get; private set; }

        /// <summary>
        /// Создаёт экземпляр атрибута
        /// </summary>
        /// <param name="toolTip">Описание команды</param>
        /// <param name="imageResource">Имя ресурса с картинкой для команды</param>
        /// <param name="cmdType">Тип команды </param>
        /// <param name="confirmMessage">Сообщения для пользователя для подтверждения выполнения команды </param>
        /// <param name="separator">Тип сепаратора</param>
        public ApplicationCommandAttribute(string toolTip, string imageResource, AppCommandType cmdType = AppCommandType.Silent, 
            string confirmMessage = "Подтвердите выполлнение команды", SeparatorType separator = SeparatorType.None)
        {
            ToolTip = toolTip;
            ImageResource = imageResource;
            CmdType = cmdType;
            ConfirmMessage = confirmMessage;
            Separator = separator;
        }

        public ApplicationCommandAttribute(Type stringProviderType, AppCommandType cmdType = AppCommandType.Silent,
                                           SeparatorType separator = SeparatorType.None)
        {
            Contract.Requires(stringProviderType.GetInterfaces().Any(x=>x == typeof(IApplicationStringProvider)));
            StringProviderType = stringProviderType;
            CmdType = cmdType;
            Separator = separator;
        }
    }
}
