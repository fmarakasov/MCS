using System.Windows;

namespace MContracts.Controls.Dialogs
{
    /// <summary>
    /// Логика взаимодействия для ContractSelectDialog.xaml
    /// </summary>
    public partial class ContractSelectDialog : Window
    {
        public ContractSelectDialog()
        {
            InitializeComponent();
         
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }
        
    }
}
