using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using CommonBase;
using MCDomain.DataAccess;
using MCDomain.Model;
using MContracts.Classes;
using MContracts.Controls.Dialogs;
using MContracts.ViewModel;
using MContracts.ViewModel.Helpers;
using UIShared.Common;
using McUIBase.Factories;

namespace MContracts.View
{
    /// <summary>
    /// Interaction logic for ActsView.xaml
    /// </summary>
    public partial class ActsView : UserControl
    {
        private ActsViewModel _prevContext;
        public ActsView()
        {
            InitializeComponent();
        }

        private ActsViewModel ViewModel
        {
            get { return DataContext.NullCastTo<ActsViewModel>(); }
        }


        public void RecalcAggregates(object sender, EventArgs e)
        {
            ActsDg.CalculateAggregates();
        }


        private void UserControl_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {

            e.DependencyPropertyAction<ActsViewModel>((old, newVal) =>
                {
                    old.Do(x => x.StatusChanged -= RecalcAggregates);
                    old.Do(x => x.RequestShowActDesigner -= Viewmodel_RequestShowActDesigner);
                    old.Do(x => x.RefusedPrintAct -= RefusedActPrintHandler);
                    old.Do(x => x.RequestActDate -= OnRequestActDate);

                    newVal.Do(x => x.StatusChanged += RecalcAggregates);
                    newVal.Do(x => x.RequestShowActDesigner += Viewmodel_RequestShowActDesigner);
                    newVal.Do(x => x.RefusedPrintAct += RefusedActPrintHandler);
                    newVal.Do(x => x.SelectedActs = ActsDg.SelectedItems);
                    newVal.Do(x => x.RequestActDate += OnRequestActDate);
                });
        }

        private DateTime? _prevDate;
        private bool? _prevIsSigned;

        private void OnRequestActDate(object sender, EventParameterArgs<ActStatusArgs> args)
        {
            var dlg = new SelectDateDialog
                {
                    SignDatePicker = {SelectedDate = _prevDate},
                    SignState = {IsChecked = _prevIsSigned}
                };

            var res = dlg.ShowDialog();
            if (!res.GetValueOrDefault()) return;

            _prevDate = dlg.SignDatePicker.SelectedDate;
            _prevIsSigned = dlg.SignState.IsChecked;
            args.Parameter.IsSigned = _prevIsSigned;
            args.Parameter.SignDate = _prevDate;
        }

        private void RefusedActPrintHandler(object sender, EventArgs e)
        {
            AppMessageBox.Show(
                    "Для того, чтобы сформировать акт для печати, требуется задать его тип. Это можно сделать в окне редакции акта.", MessageBoxButton.OK, MessageBoxImage.Warning);
            
        }

        private void Viewmodel_RequestShowActDesigner(object sender, EventParameterArgs<ActDto> e)
        {
            using (ActDesignerViewModel vm = CreateActDesgnerViewModel(e.Parameter))
            {
                var dlg = new ActDesignerDialog {DataContext = vm};
                dlg.ShowDialog();
            }
        }

        private ActDesignerViewModel CreateActDesgnerViewModel(ActDto act)
        {
            var vm = new ActDesignerViewModel(RepositoryFactory.CreateContractRepository());

            vm.CurrentContract = vm.Repository.GetContractdoc(ViewModel.ContractObject.Id);

            
            vm.CurrentAct = act.IsNull() ? ViewModel.CreateNewAct(vm) : vm.Repository.TryGetContext().Acts.Single(x => x.Id == act.Id);
         
            vm.CurrentSchedule =
                vm.Repository.TryGetContext().Schedulecontracts.Single(x => x.Id == ViewModel.SelectedSchedule.Id).
                    Schedule;

     
            return vm;
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ViewModel == null) return;
            ViewModel.SendPropertyChanged("StagePriceWithNDSColumnTitle");
            ViewModel.SendPropertyChanged("StagePriceWithNoNDSColumnTitle");
            ViewModel.SendPropertyChanged("NDSColumnTitle");
            ViewModel.SendPropertyChanged("PrepaymentColumnTitle");
            ViewModel.SendPropertyChanged("TransferColumnTitle");
        }


        private void ActsDG_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (ViewModel.EditActCommand.CanExecute(this))
                ViewModel.EditActCommand.Execute(this);
        }

        
    }
}