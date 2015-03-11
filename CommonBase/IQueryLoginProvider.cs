using System;
using System.Diagnostics.Contracts;

namespace CommonBase
{
    /// <summary>
    /// Определяет типы, беспечивающие запрос параметров подключения
    /// </summary>
    [ContractClass(typeof(QueryLoginProviderContract))]    
    public interface IQueryLoginProvider
    {
        /// <summary>
        /// Запрашивает параметры подключения и передаёт их провайдеру подключения
        /// </summary>
        /// <param name="loginProvider">Провайдер подключения</param>
        /// <returns></returns>
        bool QueryCredentails(ILoginProvider loginProvider);

        /// <summary>
        /// Получает экземпляр исключения, который вернул провайдер 
        /// </summary>
        Exception ConnectionException { get; }
    }


    [ContractClassFor(typeof(IQueryLoginProvider))]
    abstract public class QueryLoginProviderContract:IQueryLoginProvider
    {        
        public bool QueryCredentails(ILoginProvider loginProvider)
        {
            Contract.Requires(loginProvider!=null);
            return default(bool);
        }

        public Exception ConnectionException { get; private set; }
    }
}