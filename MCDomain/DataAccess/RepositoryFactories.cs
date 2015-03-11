using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CommonBase;

namespace MCDomain.DataAccess
{
    /// <summary>
    /// Отладочная фабрика репозиториев.
    /// Использует предопределённый провайдер подключения с известными именами/паролями
    /// </summary>
    public class DebugLinqReposotoryFactory : IRepositoryFactory
    {
        public IContractRepository CreateRepository()
        {
            IQueryLoginProvider provider = new StubQueryLoginProvider("UD", "sys", "XE");
            ContextFactoryService.Instance.QueryLoginProvider = provider;
            return new LinqContractRepository(ContextFactoryService.Instance);
        }
    }

    /// <summary>
    /// Фабрика репозитариев-заглушек
    /// </summary>
    public class StubRepositoryFactory : IRepositoryFactory
    {
        public IContractRepository CreateRepository()
        {
            return new StubContractRepository(null);
        }
    }
}
