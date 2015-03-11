using System.Windows;
using System.Windows.Controls;
using MContracts.ViewModel;

namespace MContracts.View
{
    /// <summary>
    /// Логика взаимодействия для UserControl1.xaml
    /// </summary>
    public partial class OrderRepositoryView : UserControl
    {
        private OrderRepositoryViewModel _viewmodel;
        public OrderRepositoryView()
        {
            InitializeComponent();
            Loaded += ContractRepositoryViewLoaded;
        }


        void ContractRepositoryViewLoaded(object sender, RoutedEventArgs e)
        {
            _viewmodel = DataContext as OrderRepositoryViewModel;
        }

    }
}
