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
using MCDomain.Model;
using MContracts.ViewModel;
using Telerik.Windows.Controls;

namespace MContracts.Controls.Dialogs
{
    /// <summary>
    /// Логика взаимодействия для Window1.xaml
    /// </summary>
    public partial class SelectEmployeeWindow : Window
    {
        private readonly SelectEmployeeViewModel _viewModel;

        public  SelectEmployeeViewModel ViewModel
        {
            get { return _viewModel; }
        }



        public SelectEmployeeWindow(IContractRepository repository)
        {
            InitializeComponent();
            _viewModel = new SelectEmployeeViewModel(repository);
            DataContext = _viewModel;



        }

        private void ButtonClick(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        private void BtnCancelClick(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private void DepartmentsTreeView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (_viewModel.SelectedEmployee != null)
            {
                DialogResult = true;
            }
        }

        private void DepartmentsTreeView_Loaded(object sender, RoutedEventArgs e)
        {

        }


        private void TreeViewSelectedItemChanged(object sender, RoutedEventArgs e)
        {
            var item = sender as RadTreeViewItem;
            if (item != null)
            {
                item.BringIntoView();
                e.Handled = true;
            }
        }

        private void DepartmentsTreeView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //ViewModel.SelectedDepartment = DepartmentsTreeView.SelectedItem as Department;
            
        }




    }   
}
