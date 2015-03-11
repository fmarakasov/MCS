using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MCDomain.DataAccess;

namespace MContracts.ViewModel.Helpers
{
    /// <summary>
    /// Создаёт экземпляры репозитария 
    /// </summary>
    public static class RepositoryFactory
    {
        /// <summary>
        /// Создаёт экземпляр репозитария
        /// </summary>
        /// <returns>Новый экземпляр репозитария</returns>
        public static IContractRepository CreateContractRepository()
        {
            //return new StubContractRepository();
            System.Diagnostics.Debug.WriteLine("RepositoryFactory.CreateContractRepository(): Новый экземпляр создан.");
            return new LinqContractRepository(ContextFactoryService.Instance);
        }
    }
}
