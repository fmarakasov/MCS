using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Devart.Data.Linq;
using LCUOF;
using Ninject;
using Ninject.Modules;
using UOW;

namespace MCDomain.IoC
{
    internal class IoCModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IDataContext>().To<LcDataContext>();
            Bind<DataContext>().To<Model.McDataContext>();
            Bind<IRepositoryFactory>().ToConstant(RepositoryFactory.Default);
            Bind<IUnitOfWork>().To<UnitOfWork>();
        }
    }

    internal class IoCContainer
    {
        private IoCContainer()
        {
        
        }
        public static readonly IoCContainer Instance = new IoCContainer();
        
        private readonly IKernel _kernel = new StandardKernel(new IoCModule());

        public IKernel Kernel
        {
            get { return _kernel; }
        }
    }
}
