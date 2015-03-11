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
using MContracts.ViewModel;

namespace MContracts.View
{
    /// <summary>
    /// Interaction logic for PrepaymentView.xaml
    /// </summary>
    public partial class PrepaymentView : UserControl
    {
        private PrepaymentViewModel ViewModel { get { return DataContext as PrepaymentViewModel; } }

        public PrepaymentView()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.PrepaymentsList.AddNew();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            prepaymentsGrid.CancelEdit();
            if (prepaymentsGrid.SelectedItem != null)
            {
                ViewModel.PrepaymentsList.Remove(prepaymentsGrid.SelectedItem);
            }

        }
    }
}
