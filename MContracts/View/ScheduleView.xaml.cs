#region

using System;
using System.ComponentModel;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using MCDomain.DataAccess;
using MCDomain.Model;
using MContracts.Controls.Dialogs;
using MContracts.ViewModel;
using Telerik.Windows.Controls;
using Telerik.Windows.Controls.GridView;
using CommonBase;
using McUIBase.Factories;
using UIShared.Common;

#endregion

namespace MContracts.View
{
    /// <summary>
    ///   Interaction logic for ScheduleView.xaml
    /// </summary>
    public partial class ScheduleView : UserControl
    {
        public Guid ViewInstanceId { get; private set; }
        
        public ScheduleView()
        {
            InitializeComponent();
            ViewInstanceId = Guid.NewGuid();
        }

        private ScheduleViewModel Viewmodel
        {
            get { return DataContext as ScheduleViewModel; }
        }
        
        private void Viewmodel_DeletingStage(object sender, CancelEventArgs e)
        {
            if (StageDataGrid != null) StageDataGrid.CancelEdit();
        }

     
        
        private void ScheduleView_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {

            e.DependencyPropertyAction<ScheduleViewModel>((oldCtx, newCtx) =>
                {
                    oldCtx.Do(x => x.DeletingStage -= Viewmodel_DeletingStage);
                    oldCtx.Do(x => x.RequestApprovalStateEditor -= Viewmodel_RequestApprovalStateEditor);

                    oldCtx.Do(x => x.DisposeImporterDialog());

                    newCtx.Do(x => x.RequestApprovalStateEditor += Viewmodel_RequestApprovalStateEditor);
                    newCtx.Do(x => x.DeletingStage += Viewmodel_DeletingStage);

                });
          
        }

        private void Viewmodel_RequestApprovalStateEditor(object sender, EventParameterArgs<ISupportStateApproval> e)
        {
            Contract.Requires(e.Parameter !=null);
            var dlg = new ApprovalStateEditor();
            using (var vm = new ApprovalStateEditorViewModel(RepositoryFactory.CreateContractRepository()))
            {
                if (e.Parameter is Stage)
                {
                    var stage = e.Parameter as Stage;
                    vm.Approval = vm.Repository.TryGetContext().Stages.SingleOrDefault(x => x.Id == stage.Id);
                }
                else if (e.Parameter is Stageresult)
                {
                    var stageResult = e.Parameter as Stageresult;
                    vm.Approval = vm.Repository.TryGetContext().Stageresults.SingleOrDefault(x => x.Id == stageResult.Id);  
                }
            
                Contract.Assert(vm.Approval != null);
            
                dlg.DataContext = vm;
                dlg.ShowDialog();
            }
          
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            StageDataGrid.RowDetailsVisibilityMode = StageDataGrid.RowDetailsVisibilityMode == GridViewRowDetailsVisibilityMode.VisibleWhenSelected ? GridViewRowDetailsVisibilityMode.Collapsed : GridViewRowDetailsVisibilityMode.VisibleWhenSelected;
        }


        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Viewmodel != null)
            {
                (Viewmodel).SendPropertyChanged("StagePriceColumnTitle");
                (Viewmodel).SendPropertyChanged("StagePriceWithNoNDSColumnTitle");
                (Viewmodel).SendPropertyChanged("NDSColumnTitle");
                (Viewmodel).SendPropertyChanged("OverallColumnTitle");
            }
        }

        private void StageDataGrid_KeyDown(object sender, KeyEventArgs e)
        {
            if (Viewmodel == null) return;
            if (e.DeadCharProcessedKey == Key.Delete)
            {
                if (Viewmodel.DeleteStageCommand.CanExecute(null))
                    Viewmodel.DeleteStageCommand.Execute(null);
            }
        }


        private void TypeComboBox_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            if (Viewmodel == null) return;
            Viewmodel.SelectedResult.BeginEdit();

            var comboBox = sender as ComboBox;
            if (comboBox != null)
            {
                var obj = comboBox.SelectedItem;

                if (obj == null) return;
            }

            if (Viewmodel.SelectedResult.Efparameterstageresults.Any(x => x.Value != null))
            {
                if (
                    AppMessageBox.Show("Параметры уже заполнены. Вы действительно хотите продолжить?",
                                    MessageBoxButton.OKCancel, MessageBoxImage.Warning) ==
                    MessageBoxResult.OK)
                {
                    Viewmodel.CreateParameters();
                    Viewmodel.SelectedResult.EndEdit();
                }
                else
                {
                    Viewmodel.SelectedResult.CancelEdit();
                }
            }
            else
            {
                Viewmodel.CreateParameters();
            }

            Viewmodel.SelectedResult.EndEdit();
            Viewmodel.UpdateResult();
        }

        private void ScheduleDataGrid_CellEditEnded(object sender, GridViewCellEditEndedEventArgs e)
        {
            if (Viewmodel != null)
            Viewmodel.OnErrorChanged();
        }

        private void StageContextMenuOpned(object sender, RoutedEventArgs e)
        {
            var menu = sender as ContextMenu;
            Contract.Assert(menu != null);
            menu.DataContext = Viewmodel;
        }

        private void StageDataGrid_Filtered(object sender, GridViewFilteredEventArgs e)
        {
            Viewmodel.IsFiltered = e.ColumnFilterDescriptor.IsActive;
        }


        
    }
}