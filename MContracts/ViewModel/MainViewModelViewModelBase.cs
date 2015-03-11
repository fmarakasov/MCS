using MCDomain.DataAccess;
using McUIBase.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UIShared.ViewModel;

namespace MContracts.ViewModel
{

    /// <summary>
    /// Модель представления с доступом к модели представления главного окна приложения
    /// </summary>
    public abstract class MainViewModelViewModelBase : RepositoryViewModel
    {
        MainWindowViewModel _mainViewModel;

        /// <summary>
        /// Получает или устанавливает модель представления главного окна приложения
        /// </summary>
        public MainWindowViewModel MainViewModel
        {
            get
            {
                return _mainViewModel;
            }
            set
            {
                if (_mainViewModel == value) return;
                _mainViewModel = value;
                OnMainViewModelChanged();
            }
        }

        protected virtual void OnMainViewModelChanged()
        {
            OnPropertyChanged(() => MainViewModel);
        }

        public MainViewModelViewModelBase(IContractRepository repository, ViewModelBase owner = null) : base(repository, owner)
        {
        }
    }
}
