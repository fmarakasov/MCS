using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Windows.Input;
using MCDomain.DataAccess;
using MCDomain.Model;
using UIShared.Commands;
using MContracts.Controls.Dialogs;
using McReports.ViewModel;
using MContracts.Classes;
using McUIBase.ViewModel;
using McUIBased.Commands;

namespace MContracts.ViewModel
{

    public class ActRepositoryEntity
    {
        public Act Act
        {
            get;
            set;
        }

        /// <summary>
        /// Получает или устанавливает договор
        /// </summary>
        public Contractdoc Contract { get; set; }
        /// <summary>
        /// Получает или устанавливает номер акта
        /// </summary>
        public string ActNum { get; set; }
        /// <summary>
        /// Получает или устанавливает номер договора
        /// </summary>
        public string ContractNum { get; set; }
        /// <summary>
        /// Получает или устанавливает дату подписания акта
        /// </summary>
        public DateTime SignDate { get; set; }
        /// <summary>
        /// Получает или устанавливает номера этапов, которые закрыты актом
        /// </summary>
        public string StageNums { get; set; }
        /// <summary>
        /// Получает или устанавливет стоимость без НДС
        /// </summary>
        public string PurePrice { get; set; }
        /// <summary>
        /// Получает или устанавливает НДС
        /// </summary>
        public string NdsPrice { get; set; }
        /// <summary>
        /// Получает или устанавливает стоимость с НДС
        /// </summary>
        public string WithNdsPrice { get; set; }
        /// <summary>
        /// Получает или устанавливает сумму зачтённую авансом
        /// </summary>
        public string Credited { get; set; }
        /// <summary>
        /// Получает или устанавливает сумму к перечислению
        /// </summary>
        public string TransferSum { get; set; }
        /// <summary>
        /// Получает или устанавливает дату подписания договора (или доп. соглашения)
        /// </summary>
        public DateTime ContractSignDate { get; set; }
        /// <summary>
        /// Получает или устанавливает данные о контрагенте (исполнителе) 
        /// </summary>
        public string Contractor { get; set; }

        //цифровые значения свойств
        public decimal WithNdsPriceValue { get; set; }
        public decimal TransferSumValue { get; set; }
        public decimal CreditedValue { get; set; }
        public decimal PureValue { get; set; }
        public decimal NdsValue { get; set; }

    }
    
    class ActRepositoryViewModel: WorkspaceViewModel
    {
        
        /// <summary>
        /// Получает коллекцию актов
        /// </summary>
        private IEnumerable<ActRepositoryEntity> Entities
        {
            get 
            { 
                var ctx = Repository.TryGetContext();
                if (ctx != null)
                {
                    if (SelectedContracts.Count == 0)
                    {
                        return ctx.Acts.Where(x => x.Signdate >= BeginDate && x.Signdate <= EndDate).Select(x => CreateEntity(x));
                    }
                    else
                    {
                        List<ActRepositoryEntity> list = new List<ActRepositoryEntity>();
                        foreach (Act act in ctx.Acts)
                        {
                            if ((act.Signdate >= BeginDate && act.Signdate <= EndDate) &&
                                (act.ContractObject != null) &&
                                (selectedContracts.Select(x=>x.Id).Contains(act.ContractObject.Id)))
                            {
                                list.Add(CreateEntity(act));
                            }
                        }
                        //var q = ctx.Acts.Where(x => x.Signdate >= BeginDate && x.Signdate <= EndDate).Where(x => SelectedContracts.Contains(x.ContractObject)).Select(x => CreateEntity(x));
                        return list;
                    }
                }
                return new List<ActRepositoryEntity>().AsReadOnly(); 
            }
        }

        public ListCollectionView EntitiesView
        {
            get
            {
                ListCollectionView listCollectionView = new ListCollectionView(Entities.ToList());
                listCollectionView.GroupDescriptions.Add(new PropertyGroupDescription("Contract"));
                return listCollectionView;
            }
        }

        public DateTime BeginDate
        {
            get; set;
        }

        public DateTime EndDate
        {
            get; set;
        }

        private ActRepositoryEntity CreateEntity(Act x)
        {
            ActRepositoryEntity result = new ActRepositoryEntity();

            result.Act = x;
            
            result.ActNum = x.Num;
            if (x.ContractObject != null)
            {
                result.ContractSignDate = x.ContractObject.Approvedat.GetValueOrDefault();
                result.ContractNum = x.ContractObject.Internalnum;
                //Исправить!!!
                if (x.ContractObject.Contractor != null)
                  result.Contractor = x.ContractObject.Contractor.Name == "ОАО Газпром" ? x.ContractObject.Contractor.Name : x.ContractObject.Contractor.Name;
                result.Contract = x.ContractObject;
            }
            
            result.StageNums = x.StagesNumbers;
            if (x.Signdate.HasValue)
                result.SignDate = x.Signdate.Value;
            
            MoneyModel moneyModel = x.ActMoney.Factor.National;
            result.NdsPrice = moneyModel.PriceNds;
            result.PurePrice = moneyModel.PricePure;
            result.WithNdsPrice = moneyModel.PriceWithNds;
            result.WithNdsPriceValue = moneyModel.WithNdsValue;
            result.PureValue = moneyModel.PureValue;
            result.NdsValue = moneyModel.NdsValue;

            MoneyModel transferMoneyModel = x.TransferSumMoney.National.Factor;
            result.TransferSum = transferMoneyModel.PriceWithNds;
            result.TransferSumValue = transferMoneyModel.WithNdsValue;
            
            MoneyModel creditedMoneyModel = x.CreditedMoneyModel.National.Factor;
            result.Credited = creditedMoneyModel.PriceWithNds;
            result.CreditedValue = creditedMoneyModel.WithNdsValue;
            
            return result;
        }

        public ActsViewModel actsViewModel;
        
        public ActRepositoryViewModel(IContractRepository repository) : base(repository)
        {
            actsViewModel = new ActsViewModel(Repository);
        }

        public override string DisplayName
        {
            get { return "Реестр актов"; }
        }

        public override bool IsClosable
        {
            get { return true; }
        }

        protected override void Save()
        {
            
        }

        protected override bool CanSave()
        {
            return false;
        }

        public string ActsCount
        {
            get { return Entities.Count().ToString(); }
        }

        public string ActsPriceSum
        {
            get { return Math.Round(Entities.Sum(x=>x.WithNdsPriceValue),2) + " р."; }
        }

        public string TransferPriceSum
        {
            get { return Entities.Sum(x=>x.TransferSumValue) + " р."; }
        }

        public string CreditedSum
        {
            get { return Entities.Sum(x => x.CreditedValue) + " р."; }
        }

        public String SumPure
        {
            get
            {
                return Math.Round(Entities.Sum(x => x.PureValue),2).ToString() + " р.";
            }
        }

        public String SumNDS
        {
            get
            {
                return Math.Round(Entities.Sum(x => x.NdsValue),2).ToString() + " р.";
            }
        }

        private List<Contractdoc> selectedContracts = new List<Contractdoc>();
        public List<Contractdoc> SelectedContracts
        {
            get { return selectedContracts; }
            set { selectedContracts = value; }
        }

        public void FilteringActsByDates(DateTime begin, DateTime end)
        {
            BeginDate = begin;
            EndDate = end;
            OnPropertyChanged("EntitiesView");
            OnPropertyChanged("ActsCount");
            OnPropertyChanged("ActsPriceSum");
            OnPropertyChanged("TransferPriceSum");
            OnPropertyChanged("CreditedSum");
            OnPropertyChanged("SumNDS");
            OnPropertyChanged("SumPure");
        }

        public void FilteringActsByContracts(List<Contractdoc> contracts)
        {
            SelectedContracts = contracts;
            OnPropertyChanged("EntitiesView");
            OnPropertyChanged("ActsCount");
            OnPropertyChanged("ActsPriceSum");
            OnPropertyChanged("TransferPriceSum");
            OnPropertyChanged("CreditedSum");
            OnPropertyChanged("SumNDS");
            OnPropertyChanged("SumPure");
        }

        private ActRepositoryEntity selectedAct;
        public ActRepositoryEntity SelectedAct
        {
            get { return selectedAct; }
            set 
            { 
                selectedAct = value;
                actsViewModel.SelectedAct = value.Act;
            }
        }

        public ICommand CreateActCommand
        {
            get
            {
                return new AutoSubmitCommand(actsViewModel.CreateAct, x => CanCreateAct, Repository);
            }
        }

        public void CreateActReport(object o)
        {
            throw new NotImplementedException();
            //var viewModel = new ActReportViewModel(Repository);
            //viewModel.CurrentAct = SelectedAct.Act;

            //WordReportFactory.CreateReport(viewModel);
        }

        public bool CanCreateActReport
        {
            get { return (SelectedAct != null); }
        }
        
        private ICommand _createactreportcommand;
        public ICommand CreateActReportCommand
        {
            get { return _createactreportcommand ?? (_createactreportcommand = new RelayCommand(CreateActReport, x => CanCreateActReport)); }
        }

        private IList<ActRepositoryEntity> selectedActs = new List<ActRepositoryEntity>();
        public IList<ActRepositoryEntity> SelectedActs
        {
            get { return selectedActs; }
        }

        public Contractdoc SelectedContract { get; set; }

        public bool CanCreateAct
        {
            get { return true; }
        }

        public ICommand DeleteActCommand
        {
            get
            {
                return new AutoSubmitCommand(actsViewModel.DeleteAct(), x => CanDeleteAct, Repository);
            }
        }

        public bool CanDeleteAct
        {
            get { return SelectedAct != null && SelectedAct.Act.Stages.Count == 0 && SelectedAct.Act.Actpaymentdocuments.Count == 0; }
        }

        public ICommand EditActCommand
        {
            get
            {
                return new AutoSubmitCommand(actsViewModel.EditAct, x => CanEditAct, Repository);
            }
        }

        public bool CanEditAct
        {
            get { return SelectedAct != null && !(SelectedContract is NullContractdoc) && SelectedAct.Act.ContractObject == SelectedContract; }
        }

        public void MultiEdit()
        {
            MultiEditActWindow dlg = new MultiEditActWindow(Repository);
            dlg.viewModel.Acts = this.SelectedActs;

            dlg.viewModel.SelectedRegion = this.SelectedActs.Select(x=>x.Act).FirstOrDefault().Region;
            dlg.viewModel.SelectedEnterpriseauthority = this.SelectedActs.Select(x => x.Act).FirstOrDefault().Enterpriseauthority; 
            dlg.viewModel.SelectedCurrency = this.SelectedActs.Select(x => x.Act).FirstOrDefault().Currency; 
            dlg.viewModel.SelectedCurrencymeasure = this.SelectedActs.Select(x => x.Act).FirstOrDefault().Currencymeasure;
            dlg.viewModel.SelectedNds = this.SelectedActs.Select(x => x.Act).FirstOrDefault().Nds;
            dlg.viewModel.SelectedNdsalgorithm = this.SelectedActs.Select(x => x.Act).FirstOrDefault().Ndsalgorithm; 
            dlg.viewModel.SelectedActtype = this.SelectedActs.Select(x => x.Act).FirstOrDefault().Acttype; 

            if (dlg.ShowDialog() == true)
            {
                OnPropertyChanged("EntitiesView");
            }
        }
        public override bool IsModified
        {
            get { return false; }
        }
    }
}
