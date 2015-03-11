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
using MCDomain.DataAccess;
using MCDomain.Model;
using MContracts.ViewModel;

namespace MContracts.View
{
    /// <summary>
    /// Interaction logic for StageResultsView.xaml
    /// </summary>
    public partial class StageResultsView : UserControl
    {
        private StageResultsViewModel Viewmodel { get { return DataContext as StageResultsViewModel; } }
        
        public StageResultsView()
        {
            InitializeComponent();
        }





        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((sender as ComboBox).SelectedItem != null)
              Viewmodel.AddResult((sender as ComboBox).SelectedItem as Stage);
        }

        private void TypeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Viewmodel != null)
            {

                Viewmodel.SelectedResult.BeginEdit();

                var obj = (sender as ComboBox).SelectedItem;

                if (obj == null) return;

                if (Viewmodel.SelectedResult.Efparameterstageresults.Where(x => x.Value != null).Count() > 0)
                {
                    if (AppMessageBox.Show("Параметры уже заполнены. Вы действительно хотите продолжить?", MessageBoxButton.OKCancel, MessageBoxImage.Warning) == MessageBoxResult.OK)
                    {
                        Viewmodel.CreateParameters();
                        Viewmodel.SelectedResult.EndEdit();
                    }
                    else
                    {
                        Viewmodel.SelectedResult.CancelEdit();
                    }
                }
                else
                {
                    Viewmodel.CreateParameters();
                }

                Viewmodel.SelectedResult.EndEdit();
                Viewmodel.UpdateResult();
            }
        }
    }
}
