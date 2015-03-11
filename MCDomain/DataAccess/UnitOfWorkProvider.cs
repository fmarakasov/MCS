using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UOW;

namespace MCDomain.DataAccess
{
  

    public interface IUnitOfWorkProvider
    {
        IUnitOfWork GetUnitOfWork(IContractRepository repository);
    }
}
