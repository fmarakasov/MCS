using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.Contracts;
using System.Windows;
using CommonBase;
using MCDomain.Model;
using UIShared.Commands;
using UIShared.ViewModel;



namespace MContracts.ViewModel
{
    /// <summary>
    /// Определяет модель представления для коллекции договоров
    /// </summary>
    public class ContractsListViewModel : ViewModelBase
    {

        private readonly ObservableCollection<Contractdoc> _contracts = new ObservableCollection<Contractdoc>();

        private string _imageResourceName;
        private string _tooltip;
        private string _title;

        public ContractsListViewModel(ViewModelBase owner = null):base(owner)
        {

        }

        /// <summary>
        /// Получает коллекцию договоров
        /// </summary>
        public IEnumerable<Contractdoc> Contracts
        {
            get { return _contracts; }
        }


        public void LoadFrom(IEnumerable<Contractdoc> source)
        {
            Contract.Requires(source != null);
            _contracts.Clear();
            _contracts.AddRange(source);
            
            OnPropertyChanged(() => Contracts);
            OnPropertyChanged(() => ListVisibility);
        }

        public virtual void SendPropertyChanged(System.String propertyName)
        {
            OnPropertyChanged(propertyName);
        }

        /// <summary>
        /// Получает или устанавливает строку подсказки
        /// </summary>
        public string Tooltip
        {
            get { return _tooltip; }
            set
            {
                if (_tooltip == value) return;
                _tooltip = value;
                OnPropertyChanged(()=>Tooltip);
            }
        }

        /// <summary>
        /// Получает или устанавливает строку ресурса иконки
        /// </summary>
        public string ImageResourceName
        {
            get { return _imageResourceName; }
            set
            {
                if (_imageResourceName == value) return;
                _imageResourceName = value;
                OnPropertyChanged(()=>ImageResourceName);
            }
        }

        /// <summary>
        /// Получает или устанавливает заголовок
        /// </summary>
        public string Title
        {
            get { return _title; }
            set
            {
                if (_title == value) return;
                _title = value;
                OnPropertyChanged(()=>Title);
            }
        }

        public Visibility ListVisibility
        {
            get
            {
                return Classes.Converters.BoolToVisibilityConverter.BoolToVisibility(_contracts.Count > 0,
                                                                                                Visibility.Visible,
                                                                                               Visibility.Collapsed);
            }
        }
    }
}