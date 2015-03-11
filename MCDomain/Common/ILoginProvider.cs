using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MCDomain.Common
{
    /// <summary>
    /// Обеспечивает подключение к источнику данных
    /// </summary>
    public interface ILoginProvider
    {
        /// <summary>
        /// Производит подключение к источнику данных с использованных переданных параметров подключения
        /// </summary>
        /// <param name="userName">Имя пользователя</param>
        /// <param name="password">Пароль</param>
        /// <param name="serverName">Имя сервера</param>
        /// <param name="exception">Объект исключение</param>
        /// <returns>Истина, если подключение было успешным</returns>
        bool Connect(string userName, string password, string serverName, out Exception exception);
    }
}
