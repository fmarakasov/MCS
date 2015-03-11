using System.Windows;

namespace MContracts.Controls.Dialogs
{
    /// <summary>
    /// Логика взаимодействия для PaymentDocumentsDialog.xaml
    /// </summary>
    public partial class PaymentDocumentsDialog : Window
    {
        public PaymentDocumentsDialog()
        {
            InitializeComponent();
            PaymentsViewCtrl.ContentExpander.IsExpanded = true;
        }

        private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
        	DialogResult = true;
        }
    }
}
