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
using MContracts.ViewModel;

namespace MContracts.Controls.Dialogs
{
    
    
    /// <summary>
    /// Interaction logic for ClosingStagesWizzard.xaml
    /// </summary>
    public partial class ClosingStagesWizzard : Window
    {
        public ClosingStagesViewModel viewModel { get; set; }

        private bool flag;

        public ClosingStagesWizzard(IContractRepository _repository)
        {
            InitializeComponent();
            viewModel = new ClosingStagesViewModel(_repository);
            DataContext = viewModel;
            Loaded += new RoutedEventHandler(ClosingStagesWizzard_Loaded);
            this.Closed += new EventHandler(ClosingStagesWizzard_Closed);
        }

        void ClosingStagesWizzard_Closed(object sender, EventArgs e)
        {
            if (this.DialogResult != false)
            {
                switch (viewModel.SelectedAction)
                {
                    case SelectedAction.NoAct:
                        if (viewModel.DetachCommand.CanExecute(null))
                            viewModel.DetachCommand.Execute(null);
                        break;
                    case SelectedAction.ExistingAct:
                        if (viewModel.AttachExistingActCommand.CanExecute(null))
                            viewModel.AttachExistingActCommand.Execute(null);
                        break;
                    case SelectedAction.NewAct:
                        if (viewModel.AttachNewActCommand.CanExecute(null))
                            viewModel.AttachNewActCommand.Execute(null);
                        break;

                    default: throw new Exception("Не выбрано действие!");
                }
            }
        }

        void ClosingStagesWizzard_Loaded(object sender, RoutedEventArgs e)
        {
            Mover.TabControl = Wizzard;
            Mover.ParentWindow = this;
            Mover.FinalAction = () =>
                                    {
                                        this.DialogResult = true;
                                    };
        }

        void viewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "HasExit")
            {
                Mover.ForwardBtn.IsEnabled = viewModel.HasExit;
            }
        }

        private void CheckBox_Loaded(object sender, RoutedEventArgs e)
        {
            (sender as CheckBox).IsChecked = viewModel.ClosedStages.Contains((sender as CheckBox).DataContext as Stage);
        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            ActsComboBox.Visibility = System.Windows.Visibility.Visible;
            Mover.ForwardButton.IsEnabled = false;
            viewModel.SelectedAction = SelectedAction.ExistingAct;
        }

        private void RadioButton_Checked_1(object sender, RoutedEventArgs e)
        {
            ActsComboBox.Visibility = System.Windows.Visibility.Collapsed;
            Mover.ForwardButton.IsEnabled = true;
            viewModel.SelectedAction = SelectedAction.NewAct;
            Mover.ConvertNegativeForwardButton();
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            if (!viewModel.ClosedStages.Contains((sender as CheckBox).DataContext as Stage)) 
                viewModel.ClosedStages.Add((sender as CheckBox).DataContext as Stage);
        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            viewModel.ClosedStages.Remove((sender as CheckBox).DataContext as Stage);
        }

        private void RadioButton_Checked_2(object sender, RoutedEventArgs e)
        {
            Mover.ConvertRightForwardButton();
            viewModel.ExistingAct = null;
            viewModel.SelectedAction = SelectedAction.NoAct;
            Mover.ForwardBtn.IsEnabled = true;
        }

        private void ActsComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (viewModel.ExistingAct != null)
            {
                flag = true;
                Mover.ConvertRightForwardButton();
                Mover.ForwardBtn.IsEnabled = true;
            }
        }

        private void Wizzard_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((sender as TabControl).SelectedIndex == 1)
            {
                if (!flag)
                {
                    Mover.ForwardButton.IsEnabled = false;
                }
                else
                {
                    flag = false;
                }
            }
            if ((sender as TabControl).SelectedIndex == 2)
            {
                viewModel.PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler(viewModel_PropertyChanged);
                Mover.ForwardButton.IsEnabled = false;
            }
        }

    }
    
}
