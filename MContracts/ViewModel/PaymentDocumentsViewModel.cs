using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Windows.Input;
using MCDomain.DataAccess;
using MCDomain.Model;
using UIShared.Commands;
using UIShared.ViewModel;

namespace MContracts.ViewModel
{
    public class PaymentDocumentsViewModel : ContractdocBaseViewModel
    {
        private ICommand _addPaymentdocCommand;
        private ICommand _deletePaymentdocCommand;
        private IBindingList _paymentdocuments;

        public PaymentDocumentsViewModel(IContractRepository repository, ViewModelBase owner) : base(repository, owner)
        {
        }

        public IEnumerable<Currency> Currencies
        {
            get { return Repository.Currencies; }
        }

        public IEnumerable<Currencymeasure> Currencymeasures
        {
            get { return Repository.Currencymeasures; }
        }

        public IBindingList Paymentdocuments
        {
            get {
                return _paymentdocuments ??
                       (_paymentdocuments = ContractObject.Contractpayments.GetNewBindingList());
            }
        }

        public ICommand AddPaymentdocCommand
        {
            get {
                return _addPaymentdocCommand ??
                       (_addPaymentdocCommand = new RelayCommand(x => AddPaymentDocument(), x => true));
            }
        }

        public Contractpayment SelectedPaymentDocument { get; set; }

        public ICommand DeletePaymentdocCommand
        {
            get
            {
             
                return _deletePaymentdocCommand ??
                       (_deletePaymentdocCommand = new RelayCommand(x => DeletePaymentDocument(),
                                                                         x => CanDeletePaymentdocument()));
            }
        }

        protected override void Save()
        {
            Repository.SubmitChanges();
        }

        protected override bool CanSave()
        {
            return true;
        }

        private void AddPaymentDocument()
        {
            Contract.Requires(ContractObject != null);

            var paymentDcoument = new Paymentdocument
                                      {
                                          Paymentdate = DateTime.Today,
                                          Currencymeasure = Repository.Currencymeasures.FirstOrDefault(x=>x.Factor == 1),
                                          Prepaymentdocumenttype = PrefferedDocumentType,
                                          
                                          Num = "Не задан"
                                      };
            Paymentdocuments.Add(new Contractpayment
                                     {
                                         Paymentdocument = paymentDcoument
                                     });
            }

        private bool CanDeletePaymentdocument()
        {
            return SelectedPaymentDocument != null;
        }

        private void DeletePaymentDocument()
        {
            //Repository.TryGetContext().Contractpayments.DeleteOnSubmit(SelectedPaymentDocument);
            Paymentdocuments.Remove(SelectedPaymentDocument);
        }


        // Платёжные документы всегда в национальной валюте
        public  string SumColumnTitle
        {
            get { return "Сумма, " + Currency.National.CI.NumberFormat.CurrencySymbol; }
        }

        private Prepaymentdocumenttype _prefferedDocumentType;
        public Prepaymentdocumenttype PrefferedDocumentType
        {
            get {
                return _prefferedDocumentType ??
                       (_prefferedDocumentType = Repository.Prepaymentdocumenttypes.FirstOrDefault());
            }
            set
            {
                _prefferedDocumentType = value;
                OnPropertyChanged(()=>PrefferedDocumentType);
            }
        }
    }
}