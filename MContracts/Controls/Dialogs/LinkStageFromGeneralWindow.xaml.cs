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

namespace MContracts.Controls.Dialogs
{
    /// <summary>
    /// Interaction logic for LinkStageFromGeneralWindow.xaml
    /// </summary>
    public partial class LinkStageFromGeneralWindow : Window
    {
        public LinkStageFromGeneralViewModel viewModel { get; set; }
        
        public LinkStageFromGeneralWindow()
        {
            InitializeComponent();
        }

        public LinkStageFromGeneralWindow(IContractRepository Repository)
        {
            InitializeComponent();
            viewModel = new LinkStageFromGeneralViewModel(Repository);
            DataContext = viewModel;
            Loaded += new RoutedEventHandler(LinkStageFromGeneralWindow_Loaded);
        }

        void LinkStageFromGeneralWindow_Loaded(object sender, RoutedEventArgs e)
        {
            Mover.TabControl = Wizzard;
            Mover.ParentWindow = this;
            Mover.ForwardButton.IsEnabled = false;
            Mover.FinalAction = () =>
                                    {
                                        if (viewModel.LinkStagesCommand.CanExecute(null))
                                        {
                                            viewModel.LinkStagesCommand.Execute(null);
                                            String Error = viewModel.Error;
                                            if (Error != String.Empty)
                                            {
                                                AppMessageBox.Show(viewModel.Error, MessageBoxButton.OK, MessageBoxImage.Error);
                                            }
                                            else
                                            {
                                                this.Close();
                                            }
                                        }
                                    };
        }

        private void CheckBox_Loaded(object sender, RoutedEventArgs e)
        {
            var stage = ((sender as CheckBox).DataContext as SelectedStage).Stage;
            (sender as CheckBox).IsChecked = viewModel.CheckContaintsInSubgeneralhierarchis(stage);
            (sender as CheckBox).IsEnabled = viewModel.HasAddedInSubgeneralhierarchi(stage);
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Mover.ForwardButton.IsEnabled = true;
            viewModel.SubContract = (sender as ComboBox).SelectedItem as Contractdoc;
        }

    }
}
