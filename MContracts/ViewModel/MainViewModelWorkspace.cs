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
    public abstract class MainViewModelWorkspace : WorkspaceViewModel
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

        protected override void OnIsActiveChanged()
        {
            // Если область активируется, то послать команду на восстановление состояние для View, 
            // в противном случае - сохранить состояние View
            if (IsActive)
            {
                System.Diagnostics.Debug.WriteLine("Модель представления InstanceId={0} активирована. this.ToString():{1}", InstanceId, ToString());
                RestoreState();
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("Модель представления InstanceId={0} деактивирована. this.ToString():{1}", InstanceId, ToString());
                SaveState();
            }
            base.OnIsActiveChanged();
        }


        protected MainViewModelWorkspace(IContractRepository repository)
            : base(repository)
        {
        }
    }
}
