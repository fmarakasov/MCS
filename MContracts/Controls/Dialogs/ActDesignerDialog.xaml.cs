using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using MCDomain.Model;
using MContracts.Classes;
using MContracts.View;
using MContracts.ViewModel;
using MContracts.ViewModel.Helpers;
using CommonBase;
using McUIBase.Factories;
using UIShared.Common;

namespace MContracts.Controls.Dialogs
{
    /// <summary>
    /// Interaction logic for ActDesignerDialog.xaml
    /// </summary>
    public partial class ActDesignerDialog
    {
        public ActDesignerDialog()
        {
            InitializeComponent();
        }

        private void ActDesignerDialogLoadedHandler(object sender, RoutedEventArgs e)
        {

            listBoxOpnedStages.SelectionChanged += OpenStagesSelectionChanged;
            listBoxClosedStages.SelectionChanged += ClosedStagesSelectionChanged;

        }

        void ViewModel_QuerySaveChanges(object sender, EventParameterArgs<MessageBoxResult> e)
        {
            e.Parameter = AppMessageBox.Show("Перед печатью акта необходимо сохранить сделанные изменения. Продолжить?",
                                          MessageBoxButton.YesNo, MessageBoxImage.Question);
        }

        private static void ShowEditor<T>(CatalogType catalogType, Action perfomRefresh, Func<IEnumerable<T>> affectedCollection, Action<T> setValue, string title ) where T:IObjectId
        {
        

            using (var vm = new CatalogViewModel(RepositoryFactory.CreateContractRepository()) { CatalogType = catalogType })
            {  
                var dlg = new CatalogEditorDialog {Title = title, DataContext = vm};

                if (dlg.ShowDialog().GetValueOrDefault())
                {
                    perfomRefresh();

                    if (vm.NewItemsInserted)
                        setValue(
                            affectedCollection().FirstOrDefault(x => x.Id == affectedCollection().Max(p => p.Id)));
                }
            }
        }

        void ViewModel_RequestShowRegionsEditor(object sender, EventArgs e)
        {
            ShowEditor(CatalogType.Region, () => ViewModel.ReloadRegions(), () => ViewModel.Regions, (x) => ViewModel.
                                                                                                                CurrentAct.Region
                                                                                                            = x, "Регионы");
        }

        void ViewModel_RequestShowEnterpriseAuthorityEditor(object sender, EventArgs e)
        {
            ShowEditor(CatalogType.Enterpriseauthority, () => ViewModel.ReloadEnterpriseAuthority(), () => ViewModel.Enterpriseauthorities, (x) => ViewModel.
                                                                                                     CurrentAct.Enterpriseauthority
                                                                                                 = x, "Основания для организации");
        }

        void ViewModel_QueryCurrencyFailed(object sender, EventParameterArgs<Exception> e)
        {
            AppMessageBox.Show(
                string.Format("Запрос курса валют с сайта ЦБР не удался. Сообщение об ошибке: {0}", e.Parameter.Message),
                MessageBoxButton.OK, MessageBoxImage.Exclamation);
        }

        void ViewModel_RequestShowPaymentDialog(object sender, EventArgs e)
        {
            using (var vm = new PaymentDocumentsBaseViewModel(RepositoryFactory.CreateContractRepository(), null))
            {
                // Платёжки идут все по основной версии договора
                vm.ContractObject = vm.Repository.GetContractdoc(ViewModel.CurrentContract.MainContract.Id);

                var view = new PaymentsView {ContentExpander = {IsExpanded = true}};

                var shell = new DialogShell {Content = view, DataContext = vm};
                if (shell.ShowDialog().GetValueOrDefault())
                {
                    ViewModel.RefreshPaymnets();
                }
            }

        }

        void ViewModel_ForeignOrhantFound(object sender, EventParameterArgs<Stage> e)
        {
            AppMessageBox.Show("Обнаружен этап, все подэтапы которого были закрыты другим актом: " + e.Parameter);
        }

        public ActDesignerViewModel ViewModel
        {
            get
            {
                var vm = DataContext as ActDesignerViewModel;
                Contract.Assert(vm != null);
                return vm;
            }
            set
            {
                DataContext = value;
            }
        }

        public bool CanEditAct
        {
            set
            {
                CreateNewButton.IsEnabled = value;
                SaveAndCloseButton.IsEnabled = value;
            }
        }

        private void ClosedStagesSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Contract.Requires(sender is ListBox);
            ViewModel.SelectedClosedStagesCount = (sender as ListBox).SelectedItems.Count;
        }

        void OpenStagesSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Contract.Requires(sender is ListBox);
            ViewModel.SelectedOpenStagesCount = (sender as ListBox).SelectedItems.Count;

        }

        private static void SetupSortDescriptions(ObservableCollection<Stage> list )
        {
            Contract.Requires(list != null);
            var view1 = CollectionViewSource.GetDefaultView(list) as ListCollectionView;
            Contract.Assert(view1 != null, "view1 != null");
            view1.CustomSort = new StageComparier();

        }

        private void OpenStageDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (ViewModel.CloseSelectedStages.CanExecute(this))
                 ViewModel.CloseSelectedStages.Execute(this);
        }

        private void ClosedStageDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (ViewModel.OpenSelectedStages.CanExecute(this))
                ViewModel.OpenSelectedStages.Execute(this);
        }

        private void ListItemBorderLoaded(object sender, RoutedEventArgs e)
        {
            var g = (Border)sender;
            var b = new Binding("ActualWidth") {Source = listBoxClosedStages};

            g.SetBinding(FrameworkElement.WidthProperty, b);
        }

        private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (ApplyChanges())
            {
                ViewModel.SaveCommand.Execute(this);
                DialogResult = true;
            }
            else
            {
                ShowErrorCollection();
            }
        }

        private void ShowErrorCollection()
        {
            AppMessageBox.Show(string.Format("Перед продолжением устраните, пожалуйста, следующие ошибки: {0}",
                                          ViewModel.CurrentAct.Validate()));
        }

        private void AutoIncClicked(object sender, RoutedEventArgs e)
        {
            e.Handled = true;
        }


        /// <summary>
        /// Обработка Новый акт
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CreateNextActClickHandler(object sender, RoutedEventArgs e)
        {
            if (ViewModel.Repository.IsModified)
            {
                var result = AppMessageBox.Show("Сохранить изменения в акте перед созданием нового?", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                switch (result)
                {
                   
                    case MessageBoxResult.Cancel:
                        return;
                    case MessageBoxResult.Yes:
                        if (!ApplyChanges())
                        {
                            ShowErrorCollection();
                            return;
                        }
                        break;
                    case MessageBoxResult.No:
                        break;
                    default:
                        return;
                }

                CreateNextAct();
            }          
        }
        /// <summary>
        /// Создать новый акт 
        /// </summary>
        private void CreateNextAct()
        {
            ViewModel.CurrentAct = ViewModel.Repository.NewAct(ViewModel.CurrentContract);
            
            if (ViewModel.ActNumberAutoIncrement)
            {
                ViewModel.CurrentAct.Num =
                    ActsViewModel.GetNextActNumber(ActsViewModel.GetScheduleActs(ViewModel.CurrentSchedule).IntoActDto());
            }
        }

        private bool ApplyChanges()
        {
            if (!this.IsValid() || !ViewModel.SaveCommand.CanExecute(this)) return false;
            ViewModel.SaveChanges();
            return true;
        }

        private void ActDesignerUnloadedHandler(object sender, RoutedEventArgs e)
        {
            listBoxOpnedStages.SelectionChanged -= OpenStagesSelectionChanged;
            listBoxClosedStages.SelectionChanged -= ClosedStagesSelectionChanged;
        }



        private void ActDesignerDialogOnDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            e.DependencyPropertyAction<ActDesignerViewModel>((oldVal, newVal) =>
                {
                    oldVal.Do(x=>x.ForeignOrhantFound -= ViewModel_ForeignOrhantFound);
                    oldVal.Do(x=>x.RequestShowPaymentDialog -= ViewModel_RequestShowPaymentDialog);
                    oldVal.Do(x=>x.QueryCurrencyFailed -= ViewModel_QueryCurrencyFailed);
                    oldVal.Do(x=>x.RequestShowEnterpriseAuthorityEditor -= ViewModel_RequestShowEnterpriseAuthorityEditor);
                    oldVal.Do(x=>x.RequestShowRegionsEditor -= ViewModel_RequestShowRegionsEditor);
                    oldVal.Do(x=>x.QuerySaveChanges -= ViewModel_QuerySaveChanges);
                    newVal.Do(x=>x.ForeignOrhantFound += ViewModel_ForeignOrhantFound);
                    newVal.Do(x=>x.RequestShowPaymentDialog += ViewModel_RequestShowPaymentDialog);
                    newVal.Do(x=>x.QueryCurrencyFailed += ViewModel_QueryCurrencyFailed);
                    newVal.Do(x=>x.RequestShowEnterpriseAuthorityEditor += ViewModel_RequestShowEnterpriseAuthorityEditor);
                    newVal.Do(x=>x.RequestShowRegionsEditor += ViewModel_RequestShowRegionsEditor);
                    newVal.Do(x=>x.QuerySaveChanges += ViewModel_QuerySaveChanges);
                    newVal.Do(x=>x.SelectedClosedStages = listBoxClosedStages.SelectedItems);
                    newVal.Do(x=>x.SelectedOpenStages = listBoxOpnedStages.SelectedItems);
                    newVal.Do(x=>SetupSortDescriptions(x.OpenStages));
                    newVal.Do(x=>SetupSortDescriptions(x.ClosedStages));                
                });
        }
    }
}
