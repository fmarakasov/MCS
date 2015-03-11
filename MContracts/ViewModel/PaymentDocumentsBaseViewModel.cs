#region

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Windows.Input;
using CommonBase;
using MCDomain.DataAccess;
using MCDomain.Model;
using UIShared.Commands;
using UIShared.ViewModel;

#endregion

namespace MContracts.ViewModel
{
    public class PaymentDocumentsBaseViewModel : ContractdocBaseViewModel
    {
        private ICommand _addPaymentdocCommand;
        private IList<Currency> _currencies;
        private IList<Currencymeasure> _currencymeasures;
        private ICommand _deletePaymentdocCommand;
        private ObservableCollection<Contractpayment> _paymentDocuments;
        private Prepaymentdocumenttype _prefferedDocumentType;
        private IList<Prepaymentdocumenttype> _prepaymentdocumenttypes;


        public PaymentDocumentsBaseViewModel(IContractRepository repository, ViewModelBase owner)
            : base(repository, owner)
        {
        }

        public Contractpayment SelectedPaymentDocument { get; set; }

        [ApplicationCommand("Создать платёжный документ", "/MContracts;component/Resources/act_add.png")]
        public ICommand AddPaymentdocCommand
        {
            get
            {
                return _addPaymentdocCommand ??
                       (_addPaymentdocCommand = new RelayCommand(x => AddPaymentDocument(), x => true));
            }
        }

        [ApplicationCommand("Удалить платёжный документ", "/MContracts;component/Resources/act_delete.png",
            AppCommandType.Confirm, "Выбранный вами платёжный документ будет удалён. Продолжить?")]
        public ICommand DeletePaymentdocCommand
        {
            get
            {
                return _deletePaymentdocCommand ??
                       (_deletePaymentdocCommand =
                        new RelayCommand(x => DeletePaymentDocument(), x => CanDeletePaymentdocument()));
            }
        }


        public IList<Prepaymentdocumenttype> Prepaymentdocumenttypes
        {
            get
            {
                return _prepaymentdocumenttypes ??
                       (_prepaymentdocumenttypes = Repository.Prepaymentdocumenttypes.ToList());
            }
        }

        public IList<Currency> Currencies
        {
            get { return _currencies ?? (_currencies = Repository.Currencies.ToList()); }
        }

        public IList<Currencymeasure> Currencymeasures
        {
            get { return _currencymeasures ?? (_currencymeasures = Repository.Currencymeasures.ToList()); }
        }

        public ObservableCollection<Contractpayment> Paymentdocuments
        {
            get
            {
                if (_paymentDocuments != null) return _paymentDocuments;
                if (ContractObject != null)
                    _paymentDocuments =
                        new ObservableCollection<Contractpayment>(
                            ContractObject.GetBackwardPath(x => x.OriginalContract, x => true).SelectMany(
                                x => x.Contractpayments));
                return _paymentDocuments;
            }
        }

        public Prepaymentdocumenttype PrefferedDocumentType
        {
            get
            {
                return _prefferedDocumentType ??
                       (_prefferedDocumentType = Repository.Prepaymentdocumenttypes.FirstOrDefault());
            }
            set
            {
                if (_prefferedDocumentType == value) return;
                _prefferedDocumentType = value;
                OnPropertyChanged(() => PrefferedDocumentType);
            }
        }

        public string SumColumnTitle
        {
            get { return "Сумма, " + Currency.National.CI.NumberFormat.CurrencySymbol; }
        }

        private void AddPaymentDocument()
        {
            Contract.Requires(ContractObject != null);

            var paymentDcoument = new Paymentdocument
                {
                    Paymentdate = DateTime.Today,
                    Currencymeasure =
                        Repository.Currencymeasures.FirstOrDefault(x => x.Factor == 1),
                    Prepaymentdocumenttype = PrefferedDocumentType,
                    Num = "Не задан"
                };
            var contractPaymnet = new Contractpayment
                {
                    Paymentdocument = paymentDcoument
                };

            Paymentdocuments.Add(contractPaymnet);
            ContractObject.Contractpayments.Add(contractPaymnet);
            var ctx = Repository.TryGetContext();
            ctx.Paymentdocuments.InsertOnSubmit(paymentDcoument);
        }


        protected override void OnWrappedDomainObjectChanged()
        {
            _paymentDocuments = null;
            OnPropertyChanged(() => Paymentdocuments);
        }

        private bool CanDeletePaymentdocument()
        {
            return SelectedPaymentDocument != null;
        }

        private void DeletePaymentDocument()
        {
            Paymentdocuments.Remove(SelectedPaymentDocument);
            ContractObject.Contractpayments.Remove(SelectedPaymentDocument);
        }

        protected override void Save()
        {
            Repository.SubmitChanges();
        }

        protected override bool CanSave()
        {
            return true;
        }
    }
}