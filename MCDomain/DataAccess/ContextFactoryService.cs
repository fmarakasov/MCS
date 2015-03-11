using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using System.Text;
using Devart.Data.Oracle;
using MCDomain.Common;
using MCDomain.Model;
using CommonBase;

namespace MCDomain.DataAccess
{
    /// <summary>
    /// Определяет фабрику создания экземпляров контекста
    /// </summary>
    public class ContextFactoryService : ILoginProvider, IDataContextProvider
    {
        public TextWriter LogWriter { get; set; }

        /// <summary>
        /// Полчает экземпляр ContextFactoryService по умолчанию
        /// </summary>
        public static readonly ContextFactoryService Instance = new ContextFactoryService();

        /// <summary>
        /// Получает или устанавливает провайдер запроса пользовательских данных  
        /// </summary>
        public IQueryLoginProvider QueryLoginProvider { get; set; }

        /// <summary>
        /// Получает объект подключения к базе данных
        /// </summary>
        public OracleConnection Connection { get; private set; }

        /// <summary>
        /// Создаёт новый контекст используя новое или уже существующее соединение. Если соединение не создано,
        /// то используется указанный провайдер запроса параметров соединения
        /// </summary>
        public McDataContext CreateContext()
        {
            Contract.Assert(Connection!=null);
            return new McDataContext(Connection) {Log = LinqLogWriter.Instance};            
        }
        /// <summary>
        /// Производит попытку подключения к базе данных
        /// </summary>
        public void Connect()
        {
            if (Connection == null)
            {
                if (QueryLoginProvider == null)
                    throw new NoNullAllowedException("QueryLoginProvider не может быть null.");
                Connection = new OracleConnection();
                if (!QueryLoginProvider.QueryCredentails(this))
                {
                    Connection.Dispose();
                    Connection = null;
                }
            }

            
            if (Connection == null) 
                OnCreateFailed(new EventParameterArgs<Exception>(QueryLoginProvider.ConnectionException));
        }

       

        /// <summary>
        /// Событие при ошибке подключения к базе данных
        /// </summary>
        public event EventHandler<EventParameterArgs<Exception>> CreateFailed;

        public void OnCreateFailed(EventParameterArgs<Exception> e)
        {
            EventHandler<EventParameterArgs<Exception>> handler = CreateFailed;
            if (handler != null) handler(this, e);
        }

        /// <summary>
        /// событие при успешном подключении к базе данных
        /// </summary>
        public event EventHandler Connected;

        /// <summary>
        /// Производит подключение к базе данных с использованием переданных пользовательских реквизитов.
        /// </summary>
        /// <param name="userName">Имя пользователя</param>
        /// <param name="password">Пароль</param>
        /// <param name="serverName">Имя сервера</param>
        /// <param name="exception">Исключение при попытке подключения</param>
        /// <returns>Признак успешности подключения</returns>
        public bool Connect(string userName, string password, string serverName, out Exception exception)
        {
            Contract.Assert(Connection != null,"Connection не может быть null.");

            //var builder = new OracleConnectionStringBuilder();
            //builder.ConnectMode = OracleConnectMode.Default;
            //builder.UserId = userName;
            //builder.Password = password;
            //builder.Server = serverName;
            
            Connection.UserId = userName;
            Connection.Password = password;
            Connection.Server = serverName;
            Connection.ClientId = Guid.NewGuid().ToString();            
            

            try
            {
                Connection.Open();
                exception = null;
                if (Connected != null) Connected(this, EventArgs.Empty);
                return true;
            }
            catch (Exception e)
            {
                exception = e;
                return false;
            }
        }
    }
}
