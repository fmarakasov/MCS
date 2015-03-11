using System;
using CommonBase;
using MCDomain.DataAccess;
using MContracts;
using MContracts.Classes;
using MContracts.ViewModel;

namespace MCUITests
{
    
    class RepositoryHelper<TFactory> where TFactory : IRepositoryFactory
    {
        internal static IContractRepository CreateRepository()
        {
            return Activator.CreateInstance<TFactory>().CreateRepository();
        }

        internal static ActDesignerViewModel CreateActDesignerViewMode()
        {
            return new ActDesignerViewModel(CreateRepository());
        }

        internal static ActsViewModel CreateActsViewModel()
        {
            return new ActsViewModel(CreateRepository());
        }

        internal static CatalogViewModel CreateCatalogViewModel(CatalogType type)
        {
            var vm = new CatalogViewModel(CreateRepository());
            vm.CatalogType = type;
            return vm;

        }

        internal static DisposalContentViewModel CreateDisposalContentViewModel()
        {
            return  new DisposalContentViewModel(CreateRepository());
        }

        internal static ScheduleViewModel CreateScheduleViewModel()
        {
            return new ScheduleViewModel(CreateRepository());
        }

        internal static ApprovalStateEditorViewModel CreateApprovalstateEditorViewModel()
        {
           return new ApprovalStateEditorViewModel(CreateRepository());
        }

        internal static ActRepositoryViewBasedViewModel CreateActRepositoryViewBasedViewModel()
        {
            return new ActRepositoryViewBasedViewModel(CreateRepository());
        }
    }
}
