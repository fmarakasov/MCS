using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using CommonBase;
using MCDomain.Model;
using MContracts.Controls.Dialogs;
using MContracts.ViewModel;
using Telerik.Windows.Controls;

namespace MContracts.View
{
    /// <summary>
    /// Логика взаимодействия для UserControl1.xaml
    /// </summary>
    public partial class DisposalContentView: UserControl
    {
        public DisposalContentView()
        {
            InitializeComponent();
        }


   


        public DisposalContentViewModel ViewModel
        {
            get { return DataContext as DisposalContentViewModel; }
        }



        private RadGridView dg;
        private void ContractRespGrid_LoadingRowDetails(object sender, Telerik.Windows.Controls.GridView.GridViewRowDetailsEventArgs e)
        {
            dg = e.DetailsElement.FindName("StageRespGrid") as RadGridView;
            if (dg != null)
            {
                dg.ItemsSource = ViewModel.CurrentContractStageResponsibleDTOBindingList;
                SetContextItems(dg);
            }
        }

        private void SetContextItems(RadGridView dg)
        {
            var vm = DataContext.CastTo<DisposalContentViewModel>();
            vm.StageContextItems = dg.Items;
        }

        private void cmbxContractdoc_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dg != null)
                dg.ItemsSource = ViewModel.CurrentContractStageResponsibleDTOBindingList;
        }

        private void ContractRespGrid_RowActivated(object sender, Telerik.Windows.Controls.GridView.RowEventArgs e)
        {
            e.Row.BringIntoView();
        }

        private void ContractRespGrid_UnloadingRowDetails(object sender, Telerik.Windows.Controls.GridView.GridViewRowDetailsEventArgs e)
        {
            //ViewModel.CurrentStageResponsibleDTO = null;
        }

        private void ContractRespGrid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            ViewModel.CurrentStageResponsibleDTO = null;
        }

        private void StageRespGrid_SelectionChanged(object sender, SelectionChangeEventArgs e)
        {
            if (dg != null&&dg.SelectedItem != null)
              dg.ScrollIntoView(dg.SelectedItem);
        }

        private void ContractRespGrid_SelectionChanged(object sender, SelectionChangeEventArgs e)
        {
            if (ContractRespGrid.SelectedItem != null)
              ContractRespGrid.ScrollIntoView(ContractRespGrid.SelectedItem);
        }

        private void StageRespGrid_Loaded(object sender, RoutedEventArgs e)
        {
            if (dg != null)
                SetContextItems(dg);
        }

        private void StageRespGrid_Filtered(object sender, Telerik.Windows.Controls.GridView.GridViewFilteredEventArgs e)
        {
            if (dg != null)
                SetContextItems(dg);
        }




    }
}
