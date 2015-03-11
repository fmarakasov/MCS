using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;
using MCDomain.DataAccess;
using MCDomain.Model;
using UIShared.Commands;
using System.Diagnostics.Contracts;
using MContracts.DTO;
using MediatorLib;
using MContracts.ViewModel.Helpers;
using MContracts.Classes;
using CommonBase;

namespace MContracts.ViewModel
{
    /// <summary>
    /// Модель представления для ввода данных по НТП
    /// </summary>
    public class NtpProblemViewModel : ContractdocBaseViewModel
    {
        private RelayCommand<Contracttrouble> _removeTroubleCommand;
        private RelayCommand<Trouble> _addTroubleCommand;
        private RelayCommand<String> _addTroubleCommandByNumber;
        private ObservableCollection<Contracttrouble> _contractTroublesBindingList;

        public event EventHandler NotFoundTrouble;
        
        public NtpProblemViewModel(IContractRepository repository):base(repository)
        {
            ViewMediator.Register(this);
        }

        #region Overrides of RepositoryViewModel

        /// <summary>
        /// Переопределите для задания логики сохранения изменений в модели
        /// </summary>
        protected override void Save()
        {
           // Repository.SubmitChanges();
        }

        /// <summary>
        /// Переопределите для проверки возможности сохранения модели
        /// </summary>
        /// <returns></returns>
        protected override bool CanSave()
        {
            return true;
        }

        #endregion

        public override string Error
        {
            get
            {
                return null;
            }
        }

        /// <summary>
        /// Получает команду добавления проблемы в список
        /// </summary>
        public ICommand AddTroubleCommand
        {
            get
            {
                return _addTroubleCommand ??
                       (_addTroubleCommand = new RelayCommand<Trouble>(AddTrouble, x => CanAddTrouble));
            }
        }

        /// <summary>
        /// Получает команду добавления проблемы в список
        /// </summary>
        public ICommand AddTroubleCommandByNumber
        {
            get
            {
                return _addTroubleCommandByNumber ??
                       (_addTroubleCommandByNumber = new RelayCommand<String>(AddTroubleByNumber, x => CanAddTroubleByNumber));
            }
        }

        /// <summary>
        /// Получает команду удаления проблемы из списка
        /// </summary>
        public ICommand RemoveTroubleCommand
        {
            get
            {
                if (_removeTroubleCommand == null)
                    _removeTroubleCommand = new RelayCommand<Contracttrouble>(RemoveTrouble, x => CanRemoveTrouble);
                return _removeTroubleCommand;
            }
        }
        
        /// <summary>
        /// Получает коллекцию проблем договора
        /// </summary>
        public ObservableCollection<Contracttrouble> ContractTroublesBindingList
        {
            get
            {
                Contract.Requires(ContractObject != null);
                if (_contractTroublesBindingList == null)
                {
                    _contractTroublesBindingList =
                        new ObservableCollection<Contracttrouble>(ContractObject.Contracttroubles);
                }
                return _contractTroublesBindingList;
            }
        }

        public Visibility VisibilityTreeView
        {
            get { return SelectedTroublesRegistry != null ? Visibility.Visible : Visibility.Hidden; }
        }

        private void AddTrouble(Trouble trouble)
        {
            Contract.Requires(ContractObject != null);
            
            if (trouble == null) return;
            if (!_contractTroublesBindingList.Any(x => x.Troubleid == trouble.Id))
            {
                Contracttrouble CT = new Contracttrouble()
                                         {
                                             Trouble = trouble,
                                             Contractdocid = ContractObject.Id
                                         };
                ContractTroublesBindingList.Add(CT);
                Repository.InsertTroublecontract(CT);
            }
        }

        private void AddTroubleByNumber(String Number)
        {
            Contract.Requires(ContractObject != null);

            if (Number == String.Empty) return;

            Number = Number.Trim();
            Number = Number.Replace(',', '.');
            
            if (!_contractTroublesBindingList.Any(x => x.Trouble.Num == Number))
            {
                var trouble = SelectedTroublesRegistry.Troubles.FirstOrDefault(x => x.Num == Number);
                if (trouble != null)
                {
                    Contracttrouble CT = new Contracttrouble()
                                             {
                                                 Trouble = trouble,
                                                 Contractdocid = ContractObject.Id
                                             };
                    ContractTroublesBindingList.Add(CT);
                    Repository.InsertTroublecontract(CT);
                }
                else
                {
                    if (NotFoundTrouble != null)
                        NotFoundTrouble(this, EventArgs.Empty);
                }
            }
        }

        private void RemoveTrouble(Contracttrouble trouble)
        {
            Contract.Requires(ContractObject != null);
            
            if (trouble == null) return;
            if (ContractTroublesBindingList.Contains(trouble))
            {
                ContractTroublesBindingList.Remove(trouble);
                Repository.DeleteTroublecontract(trouble);
            }
        }
        
        /// <summary>
        /// Получает признак возможности добавления проблемы
        /// </summary>
        public bool CanAddTrouble
        {
            get { return SelectedTroublesRegistry != null; }
        }

        /// <summary>
        /// Получает признак возможности добавления проблемы
        /// </summary>
        public bool CanAddTroubleByNumber
        {
            get { return SelectedTroublesRegistry != null; }
        }

        /// <summary>
        /// Получает признак возможности удаления проблемы
        /// </summary>
        public bool CanRemoveTrouble
        {
            get { return true; }
        }

        private ObservableCollection<Troublesregistry> _troublesregistres;
        
        /// <summary>
        /// Получает доступ к коллекции элементов реестра проблем
        /// </summary>
        public ObservableCollection<Troublesregistry> Troublesregistres
        {
            get
            {
                if (_troublesregistres == null)
                    _troublesregistres =
                        new ObservableCollection<Troublesregistry>(Repository.TroublesRegistry);
                return _troublesregistres;
            }
        }

        private Troublesregistry _selectedTroublesRegistry;
        public Troublesregistry SelectedTroublesRegistry
        {
            get
            {
                return _selectedTroublesRegistry;
            }
            set
            {
                _selectedTroublesRegistry = value;
                OnPropertyChanged("Troubles");
                OnPropertyChanged("ContractTroublesBindingList");
                OnPropertyChanged("VisibilityTreeView");
            }
        }

        /// <summary>
        /// Получает доступ к коллекции проблем верхнего уровня в зависимости от выбранного реестра проблем
        /// </summary>
        public ObservableCollection<Trouble> Troubles
        {
            get
            {
                if (_selectedTroublesRegistry != null)
                {
                    return
                        new ObservableCollection<Trouble>(
                            Repository.Troubles.Where(
                                x => x.ParentTrouble == null && x.Troubleregistryid == _selectedTroublesRegistry.Id).OrderBy(p => p.Num, new NaturSortComparer<Trouble>()));
                }
                return null;
            }
        }

        [MediatorMessageSink(RequestRepository.CATALOG_CHANGED, ParameterType = typeof(CatalogType))]
        public void CatalogChanged(CatalogType c)
        {
            if (c == CatalogType.Trouble)
            {
                Repository.Refresh(System.Data.Linq.RefreshMode.OverwriteCurrentValues, Repository.Troubles);
                OnPropertyChanged("Troubles");
            }
        }
    }
}
