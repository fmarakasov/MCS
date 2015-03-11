#region

using System;
using System.ComponentModel;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Windows;
using System.Windows.Forms;
using CommonBase;
using MCDomain.DataAccess;
using MContracts.Classes;
using MContracts.Controls;
using MContracts.Controls.Dialogs;
using MContracts.ViewModel;
using McUIBase.Factories;
using McUIBase.ViewModel;
using WindowsTaskDialog;

#endregion

namespace MContracts
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        /// <summary>
        /// Получает экземпляр модели представления для главного окна
        /// </summary>
        private static MainWindowViewModel ViewModel
        {
            get { return MainWindowViewModel.Instance; }
        }

       

        private void MainWindowInitialized(object sender, EventArgs e)
        {
            // Настройка обработчика события закрытия модели представления
            EventHandler handler = null;
            handler = delegate
                {
                    ViewModel.Closable.RequestClose -= handler;
                    Close();
                };
            ViewModel.Closable.RequestClose += handler;
            
            // Установка модели представления для окна
            DataContext = ViewModel;

            ViewModel.QueryCloseWorkspace += ViewModelQueryCloseWorkspace;

            Loaded += MainWindowLoaded;
            // Настройка обработчика команды Редактора связей 
            ViewModel.RequestEditRelation += ViewModelRequestEditRelation;
            // Настройка обработчика отказа от создания договора, если создаётся СД к СД
            ViewModel.ContractCreateRefused += ViewModelContractCreateRefused;
            // Настройка обработчика визуализации диалога сохранения изменений в рабочей области
            ViewModel.RequestSaveModifiedWorkspace += ViewModelRequestSaveModifiedWorkspace;
            // Настройка обработчика закрытия главного окна
            Closed += MainWindowClosed;
            // Настройка обработчика выбора генерального плана для ДС к субподрядному
            ViewModel.SelectGeneralContractToBindStages += ViewModelSelectGeneralContractToBindStages;
            // Настройка обработчика предупреждения об удалении договора
            ViewModel.RequestDeleteContract += ViewModelRequestDeleteContract;
      

            // Попытка подключения к репозиторию вызовет запрос имени пользователя и пароля
            // через заданный провайдер. В случае успеха, это соединение будет использовано 
            // на протяжении работы системы
            ContextFactoryService.Instance.Connect();
        }

      

        /// <summary>
        /// Вызов диалога для выбора версии генерального договора для ДС к СД
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void ViewModelSelectGeneralContractToBindStages(object sender,
                                                                 EventParameterArgs<SelectContractdocArgs> e)
        {
            using (var vm = new SelectContractDialogViewModel
                {
                    // новое ДС к СД
                    NewContract = e.Parameter.NewSubAgreement,
                    // коллекция договоров, среди которых требуется выбрать версию генерального
                    Contracts = e.Parameter.Contracts
                })
            {
                var dlg = new ContractSelectDialog
                    {
                        DataContext = vm
                    };
                var res = dlg.ShowDialog();

                // Если пользователь выбрал договор и он не null
                // записать в выходной параметр события генеральный договор 
                if (res.GetValueOrDefault() && vm.General != null)
                {
                    e.Parameter.General = vm.General;
                }
            }
        }

        private void MainWindowClosed(object sender, EventArgs e)
        {
            ViewModel.Dispose();
        }

        private static void ViewModelRequestSaveModifiedWorkspace(object sender, EventParameterArgs<bool> e)
        {
            e.Parameter =
                AppMessageBox.Show("Перед продолжением необходимо сохранить изменения. Сохранить и продолжить?",
                                MessageBoxButton.YesNo, MessageBoxImage.Question) ==
                MessageBoxResult.Yes;
        }

        private static void ViewModelContractCreateRefused(object sender, ContractArgs e)
        {
            AppMessageBox.Show(
                "Создание договора с соисполнителями для договора с соисполнителями не предусмотрено. Однако вы можете создать дополнительное соглашение для существующего договора с соисполнителями, указав к какой редакции генерального договора он будет относиться.",
                MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private static void ViewModelRequestEditRelation(object sender, ContractArgs e)
        {
            using (
                var contractRelationsEditorViewModel =
                    new ContractRelationsEditorViewModel(RepositoryFactory.CreateContractRepository())
                        {WrappedDomainObject = ViewModel.SelectedContract, MainViewModel = ViewModel})
            {
                var editor = new ContractRelationsEditor
                    {
                        DataContext = contractRelationsEditorViewModel
                    };

                editor.ShowDialog();
            }
        }

        private static void ViewModelQueryCloseWorkspace(object sender, CancelEventArgs e)
        {
            var ws = (sender as WorkspaceViewModel);
            Contract.Assert(ws != null, "ws != null");
            if (ws.IsModified)
            {
                ws.IsActive = true;

                var typeViewModel = String.Empty;
                if (ws is ContractViewModel)
                    typeViewModel = "Документ";
                if (ws is CatalogViewModel)
                    typeViewModel = "Справочник";

                var mbRes =
                    AppMessageBox.Show(
                        string.Format("{1} {0} был изменён. Сохранить изменения перед закрытием?", ws.DisplayName,
                                      typeViewModel),
                        MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                switch (mbRes)
                {
                    case MessageBoxResult.Yes:
                        if (!ws.SaveCommand.CanExecute(null))
                        {
                            AppMessageBox.Show(
                                string.Format(
                                    "{0} не может быть сохранён прямо сейчас, так как содержит ошибки, которые должны быть исправлены. Перейдите на документ и посмотрите сообщения об ошибках.",
                                    typeViewModel),
                                MessageBoxButton.OK, MessageBoxImage.Information);
                            e.Cancel = true;
                        }
                        else
                        {
                            ViewModel.SaveWorkspace(ws);
                        }
                        break;
                    case MessageBoxResult.Cancel:
                        e.Cancel = true;
                        break;
                }
            }

            if (ws is ContractViewModel)
            {
                ViewModel.ShowContractRepositoryCommand.Execute(null);
            }
        }

        private void MainWindowLoaded(object sender, RoutedEventArgs e)
        {
            ViewModel.ShowActRepository();
            ViewModel.ShowContractRepository(0);
            SetupContractRepositoryViewModel();
        }

        private void SetupContractRepositoryViewModel()
        {
            var workspace = ViewModel.Workspaces.FirstOrDefault(x => x is ContractRepositoryViewBasedViewModel)
                            as ContractRepositoryViewBasedViewModel;

            if (workspace == null) return;

            var vm = new FilterContractsViewModel(workspace.Repository);
            filterContracts1.DataContext = vm;
            vm.MainViewModel = ViewModel;
            vm.ApplySelectedFilter();
        }

        private static void ViewModelRequestDeleteContract(object sender, DeleteContractArgs e)
        {
            //const int DELETE = 1;
            //const int CANCEL = 2;

            var dlg = new TaskDialog
                {
                    Content = string.Format(
                        "Продолжение выполнения приведёт к удалению  договора №{0} и связанных с ним данных. Продолжить?",
                        e.ContractState.Internalnum),
                    MainIcon = TaskDialogIcon.Information,
                    WindowTitle = "Удаление договора",
                    CommonButtons = TaskDialogCommonButtons.Yes | TaskDialogCommonButtons.No
                };
            e.Cancel = (TaskDialogCommonButtons) dlg.Show(IntPtr.Zero) == TaskDialogCommonButtons.No;
        }

        #region MenuBuilding

      

        #endregion

      
      
    }
}