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
using System.Windows.Shapes;
using MCDomain.DataAccess;
using MCDomain.Model;
using MContracts.Classes;
using MContracts.Dialogs;
using MContracts.ViewModel;

namespace MContracts.Controls.Dialogs
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class AddDisposalWindow : Window
    {

        public AddDisposalViewModel viewModel = null;

        public AddDisposalWindow(IContractRepository repository)
        {
            InitializeComponent();
            viewModel = new AddDisposalViewModel(repository);
            DataContext = viewModel;

        }

        public void SaveDisposal()
        {
            viewModel.SaveCommand.Execute(viewModel.Disposal);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }


        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        public DisposalContentViewModel DisposalContentViewModel
        {
            get { return disposalContentView1.DataContext as DisposalContentViewModel; }
            set { disposalContentView1.DataContext = value; }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //disposalContentView1.DataContext = new DisposalContentViewModel(viewModel.Repository){Disposal = viewModel.Disposal, ContractObject = viewModel.SelectedContract};
        }


    }
}
