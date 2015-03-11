using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics.Contracts;
using System.Linq;
using CommonBase;
using MCDomain.DataAccess;
using MCDomain.Model;
using UIShared.Commands;
using MContracts.Controls;
using UIShared.ViewModel;

namespace MContracts.ViewModel
{
    public class ContractorsViewModel : ContractorsViewModelBase
    {
        private readonly Contractortype _allContractortypesType = new Contractortype {Name = "Все"};
        private IBindingList _contractContractors;

        private IBindingList _enterpriseauthorities;
        private Contractorcontractdoc _selectedContractContractor;
        private Contractor _selectedContractor;

        public ContractorsViewModel(IContractRepository repository, ViewModelBase owner = null)
            : base(repository, owner)
        {
            SelectedContractortype = _allContractortypesType;
        }

        private IEnumerable<Contractor> _foundcontractors; 

        private string _contragentnamepart;
        private int iselectedindex;
        public string ContragentNamePart
        {
            get { return _contragentnamepart; }

            set
            {
                if (value != _contragentnamepart)
                {
                    _contragentnamepart = value;
                    _foundcontractors = Contractors.Where(c => c.Name.Contains(_contragentnamepart.Trim()));
                    SelectedContractor = _foundcontractors.FirstOrDefault();
                    iselectedindex = 0;
                }
                else
                {
                    SearchForward();   
                }

                OnPropertyChanged(()=>ContragentNamePart);
                OnPropertyChanged(()=>SelectedContractor);
            }
        }


        private void SearchForward()
        {
            if (_foundcontractors != null && _foundcontractors.Count() > 1)
            {
                iselectedindex++;
                if (iselectedindex >= _foundcontractors.Count()) iselectedindex = 0;
                SelectedContractor = _foundcontractors.ElementAtOrDefault(iselectedindex);
            }
        }

        protected override void OnContractortypesCreated()
        {
            Contractortypes.Insert(0, _allContractortypesType);
            base.OnContractortypesCreated();
        }

        protected override void OnSelectedContractortypeChanged()
        {
            OnPropertyChanged(() => Contractors);
            base.OnSelectedContractortypeChanged();
        }

        public void ContractorNamePartChanged()
        {
            OnPropertyChanged(()=>ContragentNamePart);
            SearchForward();
        }

        public string Caption
        {
            get { return GetCaption(ContractObject); }
        }

        public IEnumerable<Contractor> Contractors
        {
            get
            {
                return (SelectedContractortype == _allContractortypesType)
                           ? Repository.TryGetContext().Contractors.AsEnumerable()
                           : SelectedContractortype.With(x => x.Contractors);
            }
        }

        /// <summary>
        /// Получает коллекцию контрагентов договора
        /// </summary>
        public IBindingList ContractContractors
        {
            get
            {
                if (_contractContractors == null)
                {
                    _contractContractors = ContractObject.Contractorcontractdocs.GetNewBindingList();
                    _contractContractors.ListChanged += _contractContractors_ListChanged;
                }
                return _contractContractors;
            }
        }

        public IEnumerable<Person> Persons
        {
            get { return ContractContractors.Cast<Contractorcontractdoc>().SelectMany(x => x.Contractor.Persons).Distinct(); }
        }

        public IBindingList Enterpriseauthorities
        {
            get
            {
                return _enterpriseauthorities ??
                       (_enterpriseauthorities = Repository.TryGetContext().Enterpriseauthorities.GetNewBindingList());
            }
        }

        /// <summary>
        /// Получает выбранного пользователем контрагента
        /// </summary>
        public Contractor SelectedContractor
        {
            get { return _selectedContractor; }
            set
            {
                _selectedContractor = value;
                OnPropertyChanged(() => SelectedContractor);
            }
        }

        /// <summary>
        /// Получает выбранного пользователем контрагента
        /// </summary>
        public Contractorcontractdoc SelectedContractContractor
        {
            get { return _selectedContractContractor; }
            set
            {
                _selectedContractContractor = value;
                OnPropertyChanged(() => SelectedContractContractor);
            }
        }

        [ApplicationCommand("Выбрать контрагента", "/MContracts;component/Resources/arrow_back.png")]
        public RelayCommand AddContractorCommand
        {
            get { return new RelayCommand(x => AddContractor(), x => SelectedContractor != null); }
        }


        [ApplicationCommand("Исключить контрагента", "/MContracts;component/Resources/arrow_fwd.png")]
        public RelayCommand RemoveContractorCommand
        {
            get { return new RelayCommand(x => RemoveContractor(), x => SelectedContractContractor != null); }
        }

        [ApplicationCommand("Создать нового контрагента", "/MContracts;component/Resources/act_add.png",
            AppCommandType.Silent, "", SeparatorType.Before)]
        public RelayCommand NewContractorCommand
        {
            get { return new RelayCommand(x => CreateContractorThenAdd()); }
        }

        /// <summary>
        /// Получает заголовок элемента управления:
        /// Если договор генеральный, то исполнителем является Промгаз, а контрагент выступает заказчиком,
        /// если договор субподрядный, то Контрагент является исполнителем договора, а промгаз - Заказчиком 
        /// </summary>
        /// <param name="contractdoc">Договор</param>
        /// <returns>Строка заголовка, в зависимости от типа договора</returns>
        public static string GetCaption(Contractdoc contractdoc)
        {
            return contractdoc.Return(x => x.IsSubContract ? "Исполнители" : "Заказчик", "Контрагент");
        }

        private void _contractContractors_ListChanged(object sender, ListChangedEventArgs e)
        {
            OnPropertyChanged(() => Persons);
        }

        public event EventHandler<EventParameterArgs<Contractor>> NewContractorCreateRequest;

        public void OnNewContractorCreateRequest(EventParameterArgs<Contractor> e)
        {
            EventHandler<EventParameterArgs<Contractor>> handler = NewContractorCreateRequest;
            if (handler != null) handler(this, e);
        }

        private void CreateContractorThenAdd()
        {
            var args = new EventParameterArgs<Contractor>(null);
            OnNewContractorCreateRequest(args);
            if (args.Parameter != null)
            {
                AddContractor(args.Parameter);
            }
        }

        private void RemoveContractor()
        {
            // В коллекции контрагентов договора должен быть исключаемый контрагент 
            Contract.Assert(ContractContractors.Contains(SelectedContractContractor));

            ContractContractors.Remove(SelectedContractContractor);
        }

        private void AddContractor()
        {
            AddContractor(SelectedContractor);
        }

        private void AddContractor(Contractor contractor)
        {
            Contractorcontractdoc contractContractor =
                ContractContractors.Cast<Contractorcontractdoc>().FirstOrDefault(x => x.Contractor == contractor);

            if (contractContractor != null)
            {
                SelectedContractContractor = contractContractor;
                return;
            }

            var newItem = new Contractorcontractdoc
                {
                    Contractor = contractor
                };

            ContractContractors.Add(newItem);
        }

        protected override void Save()
        {
        }

        protected override bool CanSave()
        {
            return true;
        }
    }
}