using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using MCDomain.DataAccess;
using MContracts.ViewModel;

namespace MContracts.Controls.Dialogs
{

    public partial class AddOrderWindow : Window
    {

        private readonly AddOrderViewModel _viewModel = null;
        public AddOrderViewModel ViewModel
        {
            get
            {
                return _viewModel;
            }
        }

        public AddOrderWindow(IContractRepository repository)
        {
            InitializeComponent();
            _viewModel = new AddOrderViewModel(repository);
            //viewModel.DoAfterOkPressed += btnShowResponsiblesList_Click;
            DataContext = _viewModel;

        }


        public void SaveOrder()
        {
            _viewModel.SaveCommand.Execute(_viewModel.Order);
        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }



        private void ResponsiblesDataGrid_SelectionChanged(object sender, Telerik.Windows.Controls.SelectionChangeEventArgs e)
        {
            if (ResponsiblesDataGrid.SelectedItem != null)
                ResponsiblesDataGrid.ScrollIntoView(ResponsiblesDataGrid.SelectedItem);
        }

        private void GridViewComboBoxColumn_ContextMenuClosing(object sender, ContextMenuEventArgs e)
        {

        }

        private void GridViewComboBoxColumn_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
           
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ViewModel.SendPropertyChanged("SelectedDepartment");
            ViewModel.SendPropertyChanged("DepartmentEmployees");
            ViewModel.SendPropertyChanged("Departments");
            ViewModel.SendPropertyChanged("Contracttypes");
        }
    }
}
