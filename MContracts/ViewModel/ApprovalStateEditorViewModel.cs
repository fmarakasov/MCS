using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MCDomain.DataAccess;
using MCDomain.Model;
using MContracts.ViewModel.Helpers;
using McUIBase.ViewModel;

namespace MContracts.ViewModel
{
    public class ApprovalStateEditorViewModel : RepositoryViewModel
    {
        public ApprovalStateEditorViewModel(IContractRepository repository):base(repository)
        {
            
        }

        public override string DisplayName
        {
            get
            {
                if (Approval != null)
                    return Approval.ToString();
                return base.DisplayName;
            }

        }

        /// <summary>
        /// Получаетт или устанавливает объект с поддержкой задания его состояния согласования
        /// </summary>
        public ISupportStateApproval Approval
        {
            get { return WrappedDomainObject as ISupportStateApproval; }
            set
            {
                WrappedDomainObject = value;
                OnPropertyChanged(() => Approval);
            }
        }

        private IList<Approvalstate> _approvalstates;
        public IList<Approvalstate> Approvalstates
        {
            get
            {
                return _approvalstates ?? (_approvalstates = Repository.TryGetContext().Approvalstates.Where(
                    x => !x.Statedomain.HasValue || x.Statedomain.GetValueOrDefault() == Approval.TypeMask).ToList());
            }
        }

        protected override void Save()
        {
           Repository.SubmitChanges();
           ViewMediator.NotifyColleagues(RequestRepository.REFRESH_REFRESH_APPROVAL, Approval);
        }

        protected override bool CanSave()
        {
            return true;
        }
    }
}
