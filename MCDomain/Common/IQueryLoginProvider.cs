using System;
using System.Diagnostics.Contracts;

namespace MCDomain.Common
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
    }


    [ContractClassFor(typeof(IQueryLoginProvider))]
    abstract public class QueryLoginProviderContract:IQueryLoginProvider
    {        
        public bool QueryCredentails(ILoginProvider loginProvider)
        {
            Contract.Requires(loginProvider!=null);
            return default(bool);
        }
    }
}