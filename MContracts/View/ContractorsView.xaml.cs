using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MCDomain.Model;
using MContracts.Controls.Dialogs;
using MContracts.ViewModel;
using CommonBase;

namespace MContracts.View
{
    /// <summary>
    /// Interaction logic for ContractorsView.xaml
    /// </summary>
    public partial class ContractorsView : UserControl
    {
        public ContractorsView()
        {
            InitializeComponent();
        }

        void NewContractorCreateRequestHandler(object sender, EventParameterArgs<Contractor> e )
        {
            var dlg = new ContractorDesignerDialog();
            var vm = DataContext as ContractorsViewModel;
            Contract.Assert(vm != null);
            var newContractor = e.Parameter??new Contractor();    
            var viewModel = new ContractorsDesignerViewModel(vm.Repository) {ContractObject = vm.ContractObject, ContractorObject = newContractor};
            dlg.DataContext = viewModel;
            e.Parameter = dlg.ShowDialog().GetValueOrDefault() ? viewModel.ContractorObject : null;
        }

        private void UserControl_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            var oldCtx = e.OldValue as ContractorsViewModel;
            var newCtx = e.NewValue as ContractorsViewModel;
            oldCtx.Do(x => x.NewContractorCreateRequest -= NewContractorCreateRequestHandler);
            newCtx.Do(x => x.NewContractorCreateRequest += NewContractorCreateRequestHandler);
        }

        private void TextBoxEx_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter || e.Key == Key.F3)
            {
                (DataContext as ContractorsViewModel).ContractorNamePartChanged();
            }

            if (lbxContractors.SelectedItem != null)
              lbxContractors.ScrollIntoView(lbxContractors.SelectedItem);
        }

        private void ListBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if ((DataContext as ContractorsViewModel).RemoveContractorCommand.CanExecute(this))
                (DataContext as ContractorsViewModel).RemoveContractorCommand.Execute(this);
        }

        private void lbxContractors_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if ((DataContext as ContractorsViewModel).AddContractorCommand.CanExecute(this))
                (DataContext as ContractorsViewModel).AddContractorCommand.Execute(this);
        }
    }
}
