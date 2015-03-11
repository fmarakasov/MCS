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

namespace MContracts.Controls.Dialogs
{
    /// <summary>
    /// Interaction logic for PropertyDialog.xaml
    /// </summary>
    public partial class PropertyDialog : Window
    {
        public PropertyDialog()
        {
            InitializeComponent();
        }

        public object PropertyObject
        {
            get { return objectPropertyDialog.SelectedObject; }
            set { objectPropertyDialog.SelectedObject = value; }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }
        
    }
}
