using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using MCDomain.DataAccess;
using MCDomain.Model;
using MContracts.Classes;
using UIShared.Commands;
using MContracts.ViewModel.Helpers;
using CommonBase;
using McUIBase.ViewModel;
using McUIBased.Commands;

namespace MContracts.ViewModel
{
    public class CatalogViewModel : WorkspaceViewModel
    {

        public event CatalogTypeEventHandler CatalogTypeChanged;
        public delegate void CatalogTypeEventHandler(object sender, CatalogTypeEventsArgs e);

        public delegate void EditWindowShowedEventHandler(object sender, EditWindowShowedEventsArgs e);
        public event EditWindowShowedEventHandler EditWindowShowed;

        public CatalogViewModel(IContractRepository repository)
            : base(repository)
        {
            ViewMediator.Register(this);
        }



        protected override void Save()
        {
            Repository.SubmitChanges();
            OnPropertyChanged("IsModified");
        }

        protected override bool CanSave()
        {
            return true;
        }

        public override string DisplayName
        {
            get
            {
                switch (this.CatalogType)
                {
                    case CatalogType.ContractType:
                        return "Типы договоров";
                    case CatalogType.ContractState:
                        return "Состояния договоров";
                    case CatalogType.Nds:
                        return "Ставки НДС";
                    case CatalogType.Currency:
                        return "Валюты";
                    case CatalogType.Degree:
                        return "Ученые степени";
                    case CatalogType.Worktype:
                        return "Виды работ";
                    case CatalogType.Economefficiencytype:
                        return "Типы экономической эффективности";
                    case CatalogType.Economefficiencyparameter:
                        return "Параметры экономической эффективности";
                    case CatalogType.Efficienceparametertype:
                        return "Параметры типов экономической эффективности";
                    case CatalogType.Ntpview:
                        return "Виды НТП";
                    case CatalogType.Ntpsubview:
                        return "Подвиды НТП";
                    case CatalogType.Contractortype:
                        return "Типы контрагентов";
                    case CatalogType.Position:
                        return "Должности";
                    case CatalogType.Property:
                        return "Свойства";
                    case CatalogType.Ndsalgorithm:
                        return "Алгоритмы вычисления НДС";
                    case CatalogType.Troublesregistry:
                        return "Перечни проблем";
                    case CatalogType.Currencymeasure:
                        return "Единицы измерения";
                    case CatalogType.Contractor:
                        return "Контрагенты";
                    case CatalogType.Region:
                        return "Регионы";
                    case CatalogType.Prepaymentdocumenttype:
                        return "Типы платежных документов";
                    case CatalogType.Role:
                        return "Роли";
                    case CatalogType.Authority:
                        return "Основания";
                    case CatalogType.Acttype:
                        return "Типы актов сдачи-приемки";
                    case CatalogType.Person:
                        return "Персоны";
                    case CatalogType.Contractorposition:
                        return "Должности контрагентов";
                    case CatalogType.Employee:
                        return "Сотрудники Промгаза";
                    case CatalogType.Trouble:
                        return "Проблемы";
                    case CatalogType.Functionalcustomer:
                        return "Функциональные заказчики";
                    case CatalogType.Functionalcustomertype:
                        return "Типы функциональных заказчиков";
                    case CatalogType.Document:
                        return "Документы";
                    case CatalogType.Transferacttype:
                        return "Типы актов приемки-передачи";
                    case CatalogType.Transferacttypedocument:
                        return "Типы документов";
                    case CatalogType.Funccustomerperson:
                        return "Представители функциональных заказчиков";
                    case CatalogType.Contractorpropertiy:
                        return "Свойства контрагентов";
                    case CatalogType.Sightfuncpersonscheme:
                        return "Схемы визирования функционального заказчика";
                    case CatalogType.Location:
                        return "Места нахождения";
                    case CatalogType.MissiveType:
                        return "Типы писем";
                    case CatalogType.ApprovalGoal:
                        return "Цели направления на согласование";
                    case CatalogType.ApprovalState:
                        return "Состояние согласования";
                    case CatalogType.Department:
                        return "Подразделения Газпром промгаз";
                    case CatalogType.Enterpriseauthority:
                        return "Подразделения Газпром промгаз";
                    case CatalogType.Yearreportcolor:
                        return "Цвета тематического плана на год";
                    default:
                        return "Какой-то справочник";
                }
            }
            protected set
            {
                base.DisplayName = value;
            }
        }

        private CatalogType catalogType;
        public CatalogType CatalogType
        {
            get
            {
                return catalogType;
            }
            set
            {
                catalogType = value;
                if (CatalogTypeChanged != null)
                {
                    CatalogTypeChanged(this, new CatalogTypeEventsArgs(CatalogType));
                }
            }
        }

        private object selectedItem;
        public object SelectedItem
        {
            get { return selectedItem; }
            set
            {
                selectedItem = value;
                OnPropertyChanged("SelectedItem");

                //if (SelectedIndex != -1 && Objects != null && selectedItem != null && Objects.GetType().GetGenericArguments()[0] == selectedItem.GetType())
                //{
                //    Objects[SelectedIndex] = selectedItem;
                //}

            }
        }

        private int selectedIndex;
        public int SelectedIndex
        {
            get { return selectedIndex; }
            set { selectedIndex = value; }
        }

        private IBindingList objects;
        public IBindingList Objects
        {
            get
            {
                return this.objects;
            }
            set
            {
                this.objects = value;
            }
        }

        public ICollectionView CollectionView
        {
            get
            {
                ObservableCollection<Object> temp = new ObservableCollection<object>();

                foreach (var o in objects)
                {
                    temp.Add(o);
                }

                ICollectionView collectionView = CollectionViewSource.GetDefaultView(temp);
                return collectionView;
            }
        }

        #region Commands

        private RelayCommand deleteCommand;
        public ICommand DeleteCommand
        {
            get
            {
                return deleteCommand ??
                       (deleteCommand = new RelayCommand(Delete, x => CanDelete));
            }
        }

        private void Delete(object o)
        {
            switch (CatalogType)
            {
                case CatalogType.ContractType:
                    Repository.DeleteContractType(selectedItem as Contracttype);
                    break;
                case CatalogType.ContractState:
                    Repository.DeleteContractState(selectedItem as Contractstate);
                    break;
                case CatalogType.Nds:
                    Repository.DeleteNds(selectedItem as Nds);
                    break;
                case CatalogType.Currency:
                    Repository.DeleteCurrency(selectedItem as Currency);
                    break;
                case CatalogType.Degree:
                    Repository.DeleteDegree(selectedItem as Degree);
                    break;
                case CatalogType.Worktype:
                    Repository.DeleteWorktype(selectedItem as Worktype);
                    break;
                case CatalogType.Economefficiencytype:
                    Repository.DeleteEconomefficiencytype(selectedItem as Economefficiencytype);
                    break;
                case CatalogType.Efficienceparametertype:
                    Repository.DeleteEfficienceparametertype(selectedItem as Efficienceparametertype);
                    break;
                case CatalogType.Ntpview:
                    Repository.DeleteNtpview(selectedItem as Ntpview);
                    break;
                case CatalogType.Ntpsubview:
                    Repository.DeleteNtpsubview(selectedItem as Ntpsubview);
                    break;
                case CatalogType.Contractortype:
                    Repository.DeleteContractortype(selectedItem as Contractortype);
                    break;
                case CatalogType.Position:
                    Repository.DeletePosition(selectedItem as Position);
                    break;
                case CatalogType.Property:
                    Repository.DeleteProperty(selectedItem as Property);
                    break;
                case CatalogType.Ndsalgorithm:
                    Repository.DeleteNdsalgorithm(selectedItem as Ndsalgorithm);
                    break;
                case CatalogType.Troublesregistry:
                    Repository.DeleteTroublesregistry(selectedItem as Troublesregistry);
                    break;
                case CatalogType.Currencymeasure:
                    Repository.DeleteCurrencymeasure(selectedItem as Currencymeasure);
                    break;
                case CatalogType.Contractor:
                    Repository.DeleteContractor(selectedItem as Contractor);
                    break;
                case CatalogType.Region:
                    Repository.DeleteRegion(selectedItem as Region);
                    break;
                case CatalogType.Role:
                    Repository.DeleteRole(selectedItem as Role);
                    break;
                case CatalogType.Prepaymentdocumenttype:
                    Repository.DeletePrepaymentdocumenttype(selectedItem as Prepaymentdocumenttype);
                    break;
                case CatalogType.Authority:
                    Repository.DeleteAuthority(selectedItem as Authority);
                    break;
                case CatalogType.Acttype:
                    Repository.DeleteActtype(selectedItem as Acttype);
                    break;
                case CatalogType.Contractorposition:
                    Repository.DeleteContractorposition(selectedItem as Contractorposition);
                    break;
                case CatalogType.Person:
                    Repository.DeletePerson(selectedItem as Person);
                    break;
                case CatalogType.Enterpriseauthority:
                    Repository.TryGetContext().Enterpriseauthorities.DeleteOnSubmit(selectedItem as Enterpriseauthority);
                    break;
                case CatalogType.Employee:
                    Repository.DeleteEmployee(selectedItem as Employee);
                    break;
                case CatalogType.Trouble:
                    Repository.DeleteTrouble(selectedItem as Trouble);
                    break;
                case CatalogType.Functionalcustomer:
                    Repository.DeleteFunctionalcustomer(selectedItem as Functionalcustomer);
                    break;
                case CatalogType.Funccustomerperson:
                    Repository.DeleteFunccustomerperson(selectedItem as Funccustomerperson);
                    break;
                case CatalogType.Functionalcustomertype:
                    Repository.DeleteFunctionalcustomertype(selectedItem as Functionalcustomertype);
                    break;
                case CatalogType.Document:
                    Repository.DeleteDocument(selectedItem as Document);
                    break;
                case CatalogType.Transferacttype:
                    Repository.DeleteTransferacttype(selectedItem as Transferacttype);
                    break;
                case CatalogType.Transferacttypedocument:
                    Repository.DeleteTransferacttypedocument(selectedItem as Transferacttypedocument);
                    break;
                case CatalogType.Contractorpropertiy:
                    Repository.DeleteContractorpropertiy(selectedItem as Contractorpropertiy);
                    break;
                case CatalogType.Sightfuncpersonscheme:
                    Repository.DeleteSightfuncpersonscheme(selectedItem as Sightfuncpersonscheme);
                    break;
                case CatalogType.Economefficiencyparameter:
                    Repository.DeleteEconomefficiencyparameter(selectedItem as Economefficiencyparameter);
                    break;
                case CatalogType.Location:
                    Repository.DeleteLocation(selectedItem as Location);
                    break;
                case CatalogType.MissiveType:
                    Repository.DeleteMissiveType(selectedItem as Missivetype);
                    break;
                case CatalogType.ApprovalGoal:
                    Repository.DeleteApprovalGoal(selectedItem as Approvalgoal);
                    break;
                case CatalogType.ApprovalState:
                    Repository.DeleteApprovalState(selectedItem as Approvalstate);
                    break;
                case CatalogType.Department:
                    Repository.DeleteDepartment(selectedItem as Department);
                    break;
                case CatalogType.Yearreportcolor:
                    Repository.DeleteYearreportcolor(selectedItem as Yearreportcolor);
                    break;
                default:
                    throw new ArgumentException("Тип справочника не определен!");
            }
            
            Objects.Remove(selectedItem);
            ViewMediator.NotifyColleagues(RequestRepository.CATALOG_CHANGED, CatalogType);
            OnPropertyChanged("CollectionView");
            OnPropertyChanged("IsModified");
        }

        private RelayCommand addCommand;
        public ICommand AddCommand
        {
            get
            {
                return addCommand ??
                          (addCommand = new RelayCommand(Add, x => CanAdd));
                
            }
        }

        private void UpdateAfterAddOrEdit()
        {

            if (this is HierarchicalCatalogViewModel)
            {
               LoadData();
               OnPropertyChanged("CollectionView");
            }
            ViewMediator.NotifyColleagues(RequestRepository.CATALOG_CHANGED, CatalogType);
        }

        private void Add(object o)
        {
            if (EditWindowShowed != null)
            {
                EditWindowShowed(this, new EditWindowShowedEventsArgs(CatalogType, ActionType.Add));

            }
        }

        /// <summary>
        /// Получает признак того, что пользователь добавил новые элементы в коллекцию
        /// </summary>
        public bool NewItemsInserted { get; set; }
        public void ClearEvents()
        {
            //PropertyChanged = null;
            CatalogTypeChanged = null;
            EditWindowShowed = null;
        }


        private RelayCommand editCommand;
        public ICommand EditCommand
        {
            get
            {
                return editCommand ??
                          (editCommand = new RelayCommand(Edit, x => CanEdit));

            }
        }

        private void Edit(object o)
        {
            OnPropertyChanged("IsModified");
            if (EditWindowShowed != null)
            {
                EditWindowShowed(this, new EditWindowShowedEventsArgs(CatalogType, ActionType.Edit));
                if (this is HierarchicalCatalogViewModel)
                {
                    LoadData();
                    OnPropertyChanged("CollectionView");
                }
                ViewMediator.NotifyColleagues(RequestRepository.CATALOG_CHANGED, CatalogType);
            }
        }

        public bool CanDelete
        {
            get
            {
                if (selectedItem != null)
                        if (selectedItem is IObjectId)
                            return ((selectedItem as IObjectId).Id != EntityBase.ReservedUndifinedOid && (selectedItem as IObjectId).Id!=EntityBase.ReservedSelfOid && !(selectedItem as IObjectId).IsWellKnownId());
                        else
                            return true;
                return false;
            }
        }

        public bool CanEdit
        {
            get { 
                  return selectedItem != null && 
                        (selectedItem is IObjectId) && 
                        ((selectedItem as IObjectId).Id != EntityBase.ReservedUndifinedOid && (selectedItem as IObjectId).Id!=EntityBase.ReservedSelfOid);
                }
        }

        public bool CanAdd
        {
            get
            {
                switch (CatalogType)
                {
                    case CatalogType.Yearreportcolor:
                        var list = objects as BindingList<Yearreportcolor>;
                        if (list != null)
                            return !(list.Any(p => p.Quarter == 1) && list.Any(p => p.Quarter == 2) &&
                                     list.Any(p => p.Quarter == 3) && list.Any(p => p.Quarter == 4));
                        return true;

                    default:
                        return true;
                }
            }
        }

        #endregion

        #region HelperMethods

        public void AddObject(object p)
        {
            switch (CatalogType)
            {
                case CatalogType.ContractType:
                    Repository.InsertContractType(p as Contracttype);
                    break;
                case CatalogType.ContractState:
                    Repository.InsertContractState(p as Contractstate);
                    break;
                case CatalogType.Nds:
                    Repository.InsertNds(p as Nds);
                    break;
                case CatalogType.Currency:
                    Repository.InsertCurrency(p as Currency);
                    break;
                case CatalogType.Degree:
                    Repository.InsertDegree(p as Degree);
                    break;
                case CatalogType.Worktype:
                    Repository.InsertWorktype(p as Worktype);
                    break;
                case CatalogType.Economefficiencytype:
                    Repository.InsertEconomefficiencytype(p as Economefficiencytype);
                    break;
                case CatalogType.Economefficiencyparameter:
                    Repository.InsertEconomefficiencyparameter(p as Economefficiencyparameter);
                    break;
                case CatalogType.Efficienceparametertype:
                    Repository.InsertEfficienceparametertype(p as Efficienceparametertype);
                    break;
                case CatalogType.Enterpriseauthority:
                    Repository.TryGetContext().Enterpriseauthorities.InsertOnSubmit(selectedItem as Enterpriseauthority);
                    break;
                case CatalogType.Ntpview:
                    Repository.InsertNtpview(p as Ntpview);
                    break;
                case CatalogType.Ntpsubview:
                    Repository.InsertNtpsubview(p as Ntpsubview);
                    break;
                case CatalogType.Contractortype:
                    Repository.InsertContractortype(p as Contractortype);
                    break;
                case CatalogType.Position:
                    Repository.InsertPosition(p as Position);
                    break;
                case CatalogType.Property:
                    Repository.InsertProperty(p as Property);
                    break;
                case CatalogType.Ndsalgorithm:
                    Repository.InsertNdsalgorithm(p as Ndsalgorithm);
                    break;
                case CatalogType.Troublesregistry:
                    Repository.InsertTroublesregistry(p as Troublesregistry);
                    break;
                case CatalogType.Currencymeasure:
                    Repository.InsertCurrencymeasure(p as Currencymeasure);
                    break;
                case CatalogType.Contractor:
                    Repository.InsertContractor(p as Contractor);
                    break;
                case CatalogType.Region:
                    Repository.InsertRegion(p as Region);
                    break;
                case CatalogType.Role:
                    Repository.InsertRole(p as Role);
                    break;
                case CatalogType.Prepaymentdocumenttype:
                    Repository.InsertPrepaymentdocumenttype(p as Prepaymentdocumenttype);
                    break;
                case CatalogType.Authority:
                    Repository.InsertAuthority(p as Authority);
                    break;
                case CatalogType.Acttype:
                    Repository.InsertActtype(p as Acttype);
                    break;
                case CatalogType.Contractorposition:
                    Repository.InsertContractorposition(p as Contractorposition);
                    break;
                case CatalogType.Person:
                    Repository.InsertPerson(p as Person);
                    break;
                case CatalogType.Employee:
                    Repository.InsertEmployee(p as Employee);
                    break;
                case CatalogType.Trouble:
                    Repository.InsertTrouble(p as Trouble);
                    break;
                case CatalogType.Functionalcustomer:
                    Repository.InsertFunctionalcustomer(p as Functionalcustomer);
                    break;
                case CatalogType.Funccustomerperson:
                    Repository.InsertFunccustomerperson(p as Funccustomerperson);
                    break;
                case CatalogType.Functionalcustomertype:
                    Repository.InsertFunctionalcustomertype(p as Functionalcustomertype);
                    break;
                case CatalogType.Document:
                    Repository.InsertDocument(p as Document);
                    break;
                case CatalogType.Transferacttype:
                    Repository.InsertTransferacttype(p as Transferacttype);
                    break;
                case CatalogType.Transferacttypedocument:
                    Repository.InsertTransferacttypedocument(p as Transferacttypedocument);
                    break;
                case CatalogType.Contractorpropertiy:
                    Repository.InsertContractorpropertiy(p as Contractorpropertiy);
                    break;
                case CatalogType.Sightfuncpersonscheme:
                    Repository.InsertSightfuncpersonscheme(p as Sightfuncpersonscheme);
                    break;
                case CatalogType.Location:
                    Repository.InsertLocation(p as Location);
                    break;
                case CatalogType.MissiveType:
                    Repository.InsertMissiveType(p as Missivetype);
                    break;
                case CatalogType.ApprovalGoal:
                    Repository.InsertApprovalGoal(p as Approvalgoal);
                    break;
                case CatalogType.ApprovalState:
                    Repository.InsertApprovalState(p as Approvalstate);
                    break;
                case CatalogType.Department:
                    Repository.InsertDepartment(p as Department);
                    break;
                case CatalogType.Yearreportcolor:
                    Repository.InsertYearreportcolor(p as Yearreportcolor);
                    break;
                default:
                    throw new ArgumentException("Тип справочника не определен!");
            }

            Objects.Add(p);
            NewItemsInserted = true;
            OnPropertyChanged("CollectionView");
            OnPropertyChanged("IsModified");
        }

        public void IsModifiedChanged()
        {
            OnPropertyChanged("IsModified");
        }

        public void SelectedItemChanged()
        {
            OnPropertyChanged("SelectedItem");
        }

        public void IsObjectsChanged()
        {
            OnPropertyChanged("Objects");
            OnPropertyChanged("CollectionView");
        }

        public void LoadData()
        {
            switch (CatalogType)
            {
                case CatalogType.ContractType:
                    objects = new BindingList<Contracttype>(Repository.Contracttypes);
                    break;
                case CatalogType.ContractState:
                    objects = new BindingList<Contractstate>(Repository.States);
                    break;
                case CatalogType.Nds:
                    objects = new BindingList<Nds>(Repository.Nds);
                    break;
                case CatalogType.Currency:
                    objects = new BindingList<Currency>(Repository.Currencies);
                    break;
                case CatalogType.Degree:
                    objects = new BindingList<Degree>(Repository.Degrees);
                    break;
                case CatalogType.Worktype:
                    objects = new BindingList<Worktype>(Repository.Worktypes);
                    break;
                case CatalogType.Economefficiencytype:
                    objects = new BindingList<Economefficiencytype>(Repository.Economefficiencytypes);
                    break;
                case CatalogType.Economefficiencyparameter:
                    objects = new BindingList<Economefficiencyparameter>(Repository.Economefficiencyparameters);
                    break;
                case CatalogType.Efficienceparametertype:
                    objects = new BindingList<Efficienceparametertype>(Repository.Efficienceparametertypes);
                    break;
                case CatalogType.Ntpview:
                    objects = new BindingList<Ntpview>(Repository.Ntpviews);
                    break;
                case CatalogType.Ntpsubview:
                    objects = new BindingList<Ntpsubview>(Repository.Ntpsubviews);
                    break;
                case CatalogType.Contractortype:
                    objects = new BindingList<Contractortype>(Repository.Contractortypes);
                    break;
                case CatalogType.Position:
                    objects = new BindingList<Position>(Repository.Positions);
                    break;
                case CatalogType.Property:
                    objects = new BindingList<Property>(Repository.Properties);
                    break;
                case CatalogType.Ndsalgorithm:
                    objects = new BindingList<Ndsalgorithm>(Repository.Ndsalgorithms);
                    break;
                case CatalogType.Troublesregistry:
                    objects = new BindingList<Troublesregistry>(Repository.TroublesRegistry);
                    break;
                case CatalogType.Currencymeasure:
                    objects = new BindingList<Currencymeasure>(Repository.Currencymeasures);
                    break;
                case CatalogType.Contractor:
                    objects = new BindingList<Contractor>(Repository.Contractors);
                    break;
                case CatalogType.Region:
                    objects = new BindingList<Region>(Repository.Regions);
                    break;
                case CatalogType.Role:
                    objects = new BindingList<Role>(Repository.Roles);
                    break;
                case CatalogType.Prepaymentdocumenttype:
                    objects = new BindingList<Prepaymentdocumenttype>(Repository.Prepaymentdocumenttypes);
                    break;
                case CatalogType.Authority:
                    objects = new BindingList<Authority>(Repository.Authorities);
                    break;
                case CatalogType.Enterpriseauthority:
                    objects = new BindingList<Enterpriseauthority>(Repository.Enterpriseauthorities);
                    break;
                case CatalogType.Acttype:
                    objects = new BindingList<Acttype>(Repository.Acttypes);
                    break;
                case CatalogType.Person:
                    objects = new BindingList<Person>(Repository.Persons);
                    break;
                case CatalogType.Contractorposition:
                    objects = new BindingList<Contractorposition>(Repository.Contractorpositions);
                    break;
                case CatalogType.Employee:
                    objects = new BindingList<Employee>(Repository.Employees.OrderBy(x=>x.Level).ToList());
                    break;
                case CatalogType.Trouble:
                    objects = new BindingList<Trouble>(Repository.Troubles.OrderBy(x => x.Num).ToList());
                    break;
                case CatalogType.Functionalcustomer:
                    var fss = new BindingList<Functionalcustomer>(Repository.Functionalcustomers).OrderBy (x => x.Level);
                    objects = new BindingList<Functionalcustomer>();
                    foreach (Functionalcustomer f in fss)
                    {
                        if (f.Level == 0)
                        {
                            objects.Add(f); // если объект первого уровня иерерархии - просто добавляем его
                        }
                        else
                        {
                            objects.Insert(objects.IndexOf(f.Parent) + 1, f); // если какого-либо нижнего уровня - закидываем к нему
                        }

                    }
                    break;
                case CatalogType.Funccustomerperson:
                    objects = new BindingList<Funccustomerperson>(Repository.Funccustomerpersons);
                    break;
                case CatalogType.Functionalcustomertype:
                    objects = new BindingList<Functionalcustomertype>(Repository.Functionalcustomertypes);
                    break;
                case CatalogType.Document:
                    objects = new BindingList<Document>(Repository.Documents);
                    break;
                case CatalogType.Transferacttype:
                    objects = new BindingList<Transferacttype>(Repository.Transferacttypes);
                    break;
                case CatalogType.Transferacttypedocument:
                    objects = new BindingList<Transferacttypedocument>(Repository.Transferacttypedocuments);
                    break;
                case CatalogType.Contractorpropertiy:
                    objects = new BindingList<Contractorpropertiy>(Repository.Contractorpropertiies);
                    break;
                case CatalogType.Sightfuncpersonscheme:
                    objects = new BindingList<Sightfuncpersonscheme>(Repository.Sightfuncpersonschemes);
                    break;
                case CatalogType.Location:
                    objects = new BindingList<Location>(Repository.Locations);
                    break;
                case CatalogType.MissiveType:
                    objects = new BindingList<Missivetype>(Repository.MissiveTypes);
                    break;
                case CatalogType.ApprovalGoal:
                    objects = new BindingList<Approvalgoal>(Repository.ApprovalGoals);
                    break;
                case CatalogType.ApprovalState:
                    objects = new BindingList<Approvalstate>(Repository.ApprovalStates);
                    break;
                case CatalogType.Department:
                    var dss = new BindingList<Department>(Repository.Departments).OrderBy(x => x.Level);
                    objects = new BindingList<Department>();
                    foreach (Department d in dss)
                    {
                        if (d.Level == 0)
                        {
                            objects.Add(d); // если объект первого уровня иерерархии - просто добавляем его
                        }
                        else
                        {
                            objects.Insert(objects.IndexOf(d.Parent) + 1, d); // если какого-либо нижнего уровня - закидываем к нему
                        }

                    }
                    break;
                case CatalogType.Yearreportcolor:
                    objects = new BindingList<Yearreportcolor>(Repository.Yearreportcolors);
                    break;



            }

            CurrentType = Objects.GetType().GetGenericArguments().FirstOrDefault();

            if (CatalogTypeChanged != null)
            {
                CatalogTypeChanged(this, new CatalogTypeEventsArgs(CatalogType));
            }

        }

        public void RefreshCatalog()
        {
            switch (CatalogType)
            {
                case CatalogType.ContractType:
                    Repository.Refresh(System.Data.Linq.RefreshMode.KeepCurrentValues, Repository.Contracttypes);
                    break;
                case CatalogType.ContractState:
                    Repository.Refresh(System.Data.Linq.RefreshMode.KeepCurrentValues, Repository.States);
                    break;
                case CatalogType.Nds:
                    Repository.Refresh(System.Data.Linq.RefreshMode.KeepCurrentValues, Repository.Nds);
                    break;
                case CatalogType.Currency:
                    Repository.Refresh(System.Data.Linq.RefreshMode.KeepCurrentValues, Repository.Currencies);
                    break;
                case CatalogType.Degree:
                    Repository.Refresh(System.Data.Linq.RefreshMode.KeepCurrentValues, Repository.Degrees);
                    break;
                case CatalogType.Worktype:
                    Repository.Refresh(System.Data.Linq.RefreshMode.KeepCurrentValues, Repository.Worktypes);
                    break;
                case CatalogType.Economefficiencytype:
                    Repository.Refresh(System.Data.Linq.RefreshMode.KeepCurrentValues, Repository.Economefficiencytypes);
                    break;
                case CatalogType.Economefficiencyparameter:
                    Repository.Refresh(System.Data.Linq.RefreshMode.KeepCurrentValues, Repository.Economefficiencyparameters);
                    break;
                case CatalogType.Efficienceparametertype:
                    Repository.Refresh(System.Data.Linq.RefreshMode.KeepCurrentValues, Repository.Efficienceparametertypes);
                    break;
                case CatalogType.Ntpview:
                    Repository.Refresh(System.Data.Linq.RefreshMode.KeepCurrentValues, Repository.Ntpviews);
                    break;
                case CatalogType.Ntpsubview:
                    Repository.Refresh(System.Data.Linq.RefreshMode.KeepCurrentValues, Repository.Ntpsubviews);
                    break;
                case CatalogType.Contractortype:
                    Repository.Refresh(System.Data.Linq.RefreshMode.KeepCurrentValues, Repository.Contracttypes);
                    break;
                case CatalogType.Position:
                    Repository.Refresh(System.Data.Linq.RefreshMode.KeepCurrentValues, Repository.Positions);
                    break;
                case CatalogType.Property:
                    Repository.Refresh(System.Data.Linq.RefreshMode.KeepCurrentValues, Repository.Properties);
                    break;
                case CatalogType.Ndsalgorithm:
                    Repository.Refresh(System.Data.Linq.RefreshMode.KeepCurrentValues, Repository.Ndsalgorithms);
                    break;
                case CatalogType.Troublesregistry:
                    Repository.Refresh(System.Data.Linq.RefreshMode.KeepCurrentValues, Repository.TroublesRegistry);
                    break;
                case CatalogType.Currencymeasure:
                    Repository.Refresh(System.Data.Linq.RefreshMode.KeepCurrentValues, Repository.Currencymeasures);
                    break;
                case CatalogType.Contractor:
                    Repository.Refresh(System.Data.Linq.RefreshMode.KeepCurrentValues, Repository.Contractors);
                    break;
                case CatalogType.Region:
                    Repository.Refresh(System.Data.Linq.RefreshMode.KeepCurrentValues, Repository.Regions);
                    break;
                case CatalogType.Role:
                    Repository.Refresh(System.Data.Linq.RefreshMode.KeepCurrentValues, Repository.Roles);
                    break;
                case CatalogType.Prepaymentdocumenttype:
                    Repository.Refresh(System.Data.Linq.RefreshMode.KeepCurrentValues, Repository.Prepaymentdocumenttypes);
                    break;
                case CatalogType.Authority:
                    Repository.Refresh(System.Data.Linq.RefreshMode.KeepCurrentValues, Repository.Authorities);
                    break;
                case CatalogType.Enterpriseauthority:
                    Repository.Refresh(System.Data.Linq.RefreshMode.KeepCurrentValues, Repository.Enterpriseauthorities);
                    break;
                case CatalogType.Acttype:
                    Repository.Refresh(System.Data.Linq.RefreshMode.KeepCurrentValues, Repository.Acttypes);
                    break;
                case CatalogType.Person:
                    Repository.Refresh(System.Data.Linq.RefreshMode.KeepCurrentValues, Repository.Persons);
                    break;
                case CatalogType.Contractorposition:
                    Repository.Refresh(System.Data.Linq.RefreshMode.KeepCurrentValues, Repository.Contractorpositions);
                    break;
                case CatalogType.Employee:
                    Repository.Refresh(System.Data.Linq.RefreshMode.KeepCurrentValues, Repository.Employees);
                    break;
                case CatalogType.Trouble:
                    Repository.Refresh(System.Data.Linq.RefreshMode.KeepCurrentValues, Repository.Troubles);
                    break;
                case CatalogType.Functionalcustomer:
                    Repository.Refresh(System.Data.Linq.RefreshMode.KeepCurrentValues, Repository.Functionalcustomers);
                    break;
                case CatalogType.Funccustomerperson:
                    Repository.Refresh(System.Data.Linq.RefreshMode.KeepCurrentValues, Repository.Funccustomerpersons);
                    break;
                case CatalogType.Functionalcustomertype:
                    Repository.Refresh(System.Data.Linq.RefreshMode.KeepCurrentValues, Repository.Functionalcustomertypes);
                    break;
                case CatalogType.Document:
                    Repository.Refresh(System.Data.Linq.RefreshMode.KeepCurrentValues, Repository.Documents);
                    break;
                case CatalogType.Transferacttype:
                    Repository.Refresh(System.Data.Linq.RefreshMode.KeepCurrentValues, Repository.Transferacttypes);
                    break;
                case CatalogType.Transferacttypedocument:
                    Repository.Refresh(System.Data.Linq.RefreshMode.KeepCurrentValues, Repository.Transferacttypedocuments);
                    break;
                case CatalogType.Contractorpropertiy:
                    Repository.Refresh(System.Data.Linq.RefreshMode.KeepCurrentValues, Repository.Contractorpropertiies);
                    break;
                case CatalogType.Sightfuncpersonscheme:
                    Repository.Refresh(System.Data.Linq.RefreshMode.KeepCurrentValues, Repository.Sightfuncpersonschemes);
                    break;
                case CatalogType.Location:
                    Repository.Refresh(System.Data.Linq.RefreshMode.KeepCurrentValues, Repository.Locations);
                    break;
                case CatalogType.MissiveType:
                    Repository.Refresh(System.Data.Linq.RefreshMode.KeepCurrentValues, Repository.MissiveTypes);
                    break;
                case CatalogType.ApprovalGoal:
                    Repository.Refresh(System.Data.Linq.RefreshMode.KeepCurrentValues, Repository.ApprovalGoals);
                    break;
                case CatalogType.ApprovalState:
                    Repository.Refresh(System.Data.Linq.RefreshMode.KeepCurrentValues, Repository.ApprovalStates);
                    break;
                case CatalogType.Department:
                    Repository.Refresh(System.Data.Linq.RefreshMode.KeepCurrentValues, Repository.Departments);
                    break;
                case CatalogType.Yearreportcolor:
                    Repository.Refresh(System.Data.Linq.RefreshMode.KeepCurrentValues, Repository.Yearreportcolors);
                    break;
            }           
        
        }

        #endregion

        public override bool IsClosable
        {
            get { return true; }
        }

        public Type CurrentType { get; set; }

        public Visibility IsHierarchical
        {
            get
            {
                if (this is HierarchicalCatalogViewModel)
                    return Visibility.Visible;
                else
                    return Visibility.Collapsed;
            }
        }

        public bool FilterItem(object item, object value)
        {
            var pi = item.GetType().GetProperties();
            foreach (var propertyInfo in pi)
            {
                //берем все простые свойства, которые можно читать и писать, кроме ID и ссылки на родителя
                if (propertyInfo.CanRead && propertyInfo.CanWrite && propertyInfo.Name != "Id" && !propertyInfo.PropertyType.IsGenericType && item.GetType() != propertyInfo.PropertyType)
                {
                    var val = propertyInfo.GetValue(item, null);

                    if (val != null)
                    {
                        if (val.ToString().ToLower().Contains(value.ToString().ToLower()))
                            return true;
                    }
                }
            }

            return false;
        }
    }
}
