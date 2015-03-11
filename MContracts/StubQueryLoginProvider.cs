using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CommonBase;
using CommonBase;

namespace MContracts
{
    /// <summary>
    /// Заглушка, обеспечивающая передачу провайдеру подключения предопределённые идентификаторы пользователя и сервера БД
    /// </summary>
    public class StubQueryLoginProvider : IQueryLoginProvider
    {
        /// <summary>
        /// Получает имя пользователя
        /// </summary>
        public string UserId { get; private set; }
        /// <summary>
        /// Получает пароль
        /// </summary>
        public string Password { get; private set; }
        /// <summary>
        /// Получает название базы данных
        /// </summary>
        public string Database { get; private set; }

        /// <summary>
        /// Создаёт экземпляр заглушки
        /// </summary>
        /// <param name="userId">Имя пользователя</param>
        /// <param name="password">Пароль</param>
        /// <param name="database">Название базы данных</param>
        public StubQueryLoginProvider(string userId, string password, string database)
        {
            UserId = userId;
            Password = password;
            Database = database;
        }
        Exception _lastException;
        public bool QueryCredentails(ILoginProvider loginProvider)
        {

            return loginProvider.Connect(UserId, Password, Database, out _lastException);
        }

        public Exception ConnectionException { get { return _lastException; } }
    }
}
