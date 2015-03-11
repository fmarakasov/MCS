using System;
using System.Collections.Generic;
using System.Globalization;
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
using MContracts.Classes;
using MContracts.ViewModel;
using MCDomain.Model;
using System.ComponentModel;
using MContracts.Classes.Converters;
using MContracts.ViewModel.Helpers;
using McReports.ViewModel;

namespace MContracts.View
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class DisposalsRepositoryView : UserControl
    {

        DisposalsRepositoryViewModel viewmodel; 
        
        public DisposalsRepositoryView()
        {
            InitializeComponent();
            Loaded += new RoutedEventHandler(ContractRepositoryView_Loaded);
        }


        void ContractRepositoryView_Loaded(object sender, RoutedEventArgs e)
        {
            viewmodel = DataContext as DisposalsRepositoryViewModel;
        }


    }
}
