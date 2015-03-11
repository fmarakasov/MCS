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
    /// Interaction logic for NtpProblemView.xaml
    /// </summary>
    public partial class NtpProblemView : UserControl
    {
        NtpProblemViewModel Viewmodel { get { return DataContext as NtpProblemViewModel; } }
        
        public NtpProblemView()
        {
            InitializeComponent();
            Loaded += new RoutedEventHandler(NtpProblemView_Loaded);
        }

        void NtpProblemView_Loaded(object sender, RoutedEventArgs e)
        {
            Viewmodel.NotFoundTrouble += new EventHandler(Viewmodel_NotFoundTrouble);
        }

        void Viewmodel_NotFoundTrouble(object sender, EventArgs e)
        {
            AppMessageBox.Show("Проблема с таким номером не найдена", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void TroublesDataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            RemoveTrouble(sender as DataGrid);
        }

        private void RemoveTrouble(DataGrid dataGrid)
        {
            Contract.Requires(Viewmodel != null);
            object item = dataGrid.SelectedItem;
            if (item != null)
                Viewmodel.RemoveTroubleCommand.Execute(item);
        }

        private void TroublesTreeView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            AddTrouble(sender as TreeView);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            AddTrouble(TroublesTreeView);
        }

        private void AddTrouble(TreeView treeView)
        {
            Contract.Requires(Viewmodel != null);
            object item = treeView.SelectedItem;
            if (item != null)
                Viewmodel.AddTroubleCommand.Execute(item);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            RemoveTrouble(TroublesDataGrid);
        }

    }
}
