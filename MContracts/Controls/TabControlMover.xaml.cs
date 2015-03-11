using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using UIShared.Commands;

namespace MContracts.Controls.Dialogs
{
    /// <summary>
    /// Interaction logic for TabControlMover.xaml
    /// </summary>
    public partial class TabControlMover : UserControl
    {
        public TabControlMover()
        {
            InitializeComponent();
        }

        private TabControl tabControl;
        public TabControl TabControl
        {
            get
            {
                return tabControl;
            } 
            set
            {
                tabControl = value;
                this.DataContext = this;
            }
        }

        public Action FinalAction { get; set; }

        public Window ParentWindow { get; set; }

        private RelayCommand<object> forwardCommand;
        public ICommand ForwardCommand
        {
            get
            {
                return forwardCommand ??
                       (forwardCommand = new RelayCommand<object>(Forward, x => CanForward));
            }
        }

        public Button BackButton
        {
            get { return this.BackBtn; }
        }

        public Button ForwardButton
        {
            get { return this.ForwardBtn; }
        }

        public bool CanForward
        { 
            get { return true; }
        }

        public void ConvertRightForwardButton()
        {
            ForwardBtn.Content = "Завершить";
        }

        public void ConvertNegativeForwardButton()
        {
            ForwardBtn.Content = "Далее";
        }

        private void Forward(object content)
        {
            if (content.ToString() == "Далее")
            {
                int currentindex = TabControl.Items.IndexOf(TabControl.SelectedItem);
                TabControl.SelectedItem = TabControl.Items[currentindex + 1];
                if (TabControl.SelectedIndex == TabControl.Items.Count - 1)
                {
                    ConvertRightForwardButton();
                }
            }
            else
            {
                if (FinalAction != null)
                    FinalAction.Invoke();
            }
        }

        private ICommand backCommand;
        public ICommand BackCommand
        {
            get
            {
                return backCommand ??
                       (backCommand = new RelayCommand(Back, x => CanBack));
            }
        }

        public bool CanBack
        {
            get { return TabControl.Items.IndexOf(TabControl.SelectedItem) > 0; }
        }

        public void Back(object o)
        {
            int currentindex = TabControl.Items.IndexOf(TabControl.SelectedItem);
            TabControl.SelectedItem = TabControl.Items[currentindex - 1];
            ConvertNegativeForwardButton();
        }

        private void CancelBtn_Click(object sender, RoutedEventArgs e)
        {
            if (ParentWindow != null)
                ParentWindow.DialogResult = false;
        }
    }
}
