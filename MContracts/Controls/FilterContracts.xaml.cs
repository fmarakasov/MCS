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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.ComponentModel;
using System.Collections.ObjectModel;
using MContracts.Classes.Filtering;
using MContracts.ViewModel;
using System.Collections;
using McReports.ViewModel;

namespace MContracts.Controls
{
    /// <summary>
    /// Interaction logic for SorterContracts.xaml
    /// </summary>
    public partial class FilterContracts : UserControl
    {

        public FilterContracts()
        {
            InitializeComponent();
            //ContractRepositoryViewModel.FiltersChanged += new EventHandler(ContractRepositoryViewModel_FiltersChanged);er(Filters_CollectionChanged);
        }

        private void UserControl_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (DataContext != null&&DataContext is FilterContractsViewModel)
            {
                (DataContext as FilterContractsViewModel).SendPropertyChanged("Filterstates");
                (DataContext as FilterContractsViewModel).SendPropertyChanged("SelectedFilterstate");
            }
        }
    }

}
