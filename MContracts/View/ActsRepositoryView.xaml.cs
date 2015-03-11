using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using MContracts.ViewModel;
using MCDomain.Model;
using Telerik.Windows.Controls;

namespace MContracts.View
{
    /// <summary>
    /// Interaction logic for ContractRepositoryView.xaml
    /// </summary>
    public partial class ActsRepositoryView : UserControl
    {

        ActRepositoryViewModel viewmodel;

        public ActsRepositoryView()
        {
            InitializeComponent();
            Loaded += new RoutedEventHandler(ContractRepositoryView_Loaded);
        }

        void ContractRepositoryView_Loaded(object sender, RoutedEventArgs e)
        {
            viewmodel = DataContext as ActRepositoryViewModel;
        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (viewmodel.CanCreateAct)
            {
                viewmodel.actsViewModel.ContractObject = ((sender as Button).DataContext as CollectionViewGroup).Name as Contractdoc;
                viewmodel.CreateActCommand.Execute(null);
            }
        }

        private void ActsDataGrid_SelectedCellsChanged(object sender, Telerik.Windows.Controls.GridView.GridViewSelectedCellsChangedEventArgs e)
        {
            if (viewmodel == null) return;
            if (sender == null) return;

            viewmodel.SelectedActs.Clear();

            if ((sender as RadGridView).SelectedItems.Count == 1)
            {
                viewmodel.SelectedAct = (sender as RadGridView).SelectedItems[0] as ActRepositoryEntity;

                if (viewmodel.SelectedAct.Act.ContractObject != null)
                {
                    viewmodel.actsViewModel.ContractObject = viewmodel.SelectedAct.Act.ContractObject;
                    viewmodel.SelectedContract = viewmodel.SelectedAct.Act.ContractObject;
                }
            }
            else
            {
                foreach (var obj in (sender as RadGridView).SelectedItems)
                {
                    viewmodel.SelectedActs.Add(obj as ActRepositoryEntity);
                }
            }
        }

    }

}
