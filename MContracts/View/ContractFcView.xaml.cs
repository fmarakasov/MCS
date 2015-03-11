using System;
using System.Collections.Generic;
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
using MContracts.ViewModel;

namespace MContracts.View
{
    /// <summary>
    /// Interaction logic for ContractFcView.xaml
    /// </summary>
    public partial class ContractFcView : UserControl
    {
        ContractFcViewModel Viewmodel { get { return DataContext as ContractFcViewModel; } }

        public ContractFcView()
        {
            InitializeComponent();
        }

        private void FunctionalCustomersTreeView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            AddCustomer(sender as TreeView);
        }

        private void AddCustomer(TreeView treeView)
        {
            Contract.Requires(Viewmodel != null);
            object item = treeView.SelectedItem;
            if (item != null)
                Viewmodel.AddFunctionalCustomerCommand.Execute(item);
        }

        private void CustomersDataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            RemoveCustomer(sender as DataGrid);
        }

        private void RemoveCustomer(DataGrid dataGrid)
        {
            Contract.Requires(Viewmodel != null);
            object item = dataGrid.SelectedItem;
            if (item != null)
                Viewmodel.RemoveFunctionalCustomerCommand.Execute(item);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            RemoveCustomer(CustomersDataGrid);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            AddCustomer(FunctionalCustomersTreeView);
        }

        private void Image_ImageFailed(object sender, ExceptionRoutedEventArgs e)
        {

        }

    }
}
