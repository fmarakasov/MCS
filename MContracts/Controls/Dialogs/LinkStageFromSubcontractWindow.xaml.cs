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
using MContracts.Classes;
using MContracts.ViewModel;
using MContracts.Classes.Converters;

namespace MContracts.Controls.Dialogs
{
    /// <summary>
    /// Interaction logic for LinkStageFromSubcontractWindow.xaml
    /// </summary>
    public partial class LinkStageFromSubcontractWindow : Window
    {
        public LinkStageFromSubcontractViewModel viewModel = null;

        public LinkStageFromSubcontractWindow(IContractRepository Repository)
        {
            InitializeComponent();
            viewModel = new LinkStageFromSubcontractViewModel(Repository);
            DataContext = viewModel;
            Loaded += Window_Loaded;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Mover.TabControl = Wizzard;
            Mover.ParentWindow = this;
            Mover.ForwardButton.IsEnabled = false;
            Mover.FinalAction = () =>
                                    {
                                        if (viewModel.LinkStagesCommand.CanExecute(null))
                                        {
                                            viewModel.LinkStagesCommand.Execute(null);
                                            this.Close();
                                        }
                                    };

        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            Mover.ForwardButton.IsEnabled = true;
        }

        private void StagesCB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

             viewModel.HasAddedInSubgeneralhierarchi();
             viewModel.TrySetGeneralStage((sender as ComboBox).SelectedItem as Stage);
             if (viewModel.Error != String.Empty)
             {
                AppMessageBox.Show(viewModel.Error, MessageBoxButton.OK, MessageBoxImage.Error);
                (sender as ComboBox).SelectedItem = null;
             }
             else
             {
                Mover.ForwardBtn.IsEnabled = true;
             }

        }

        private void Wizzard_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((sender as TabControl).SelectedIndex == (sender as TabControl).Items.Count - 1)
            {
                Mover.ForwardBtn.IsEnabled = false;
                Wizzard.SelectionChanged -= Wizzard_SelectionChanged;
            }
        }

        private void btnDiscardChoice_Click(object sender, RoutedEventArgs e)
        {
            StagesCB.SelectedItem = null;
            Mover.ForwardBtn.IsEnabled = true;
        }

    }
}
