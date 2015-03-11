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
using MCDomain.Importer;

namespace MContracts.Controls.Dialogs
{
    /// <summary>
    /// диалог сохранения схемы импорта
    /// </summary>
    public partial class SaveSchemeDialog : Window
    {
        public SaveSchemeDialog()
        {
            InitializeComponent();
        }

        private void btnOk_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        private ImportingScheme _scheme;
        public ImportingScheme Scheme
        {
            get
            {
                return _scheme;
            }

            set
            {
                if (_scheme == value) return;
                _scheme = value;
                txtSchemeName.Text = _scheme.SchemeName;
            }

        }

        public string NewSchemeName
        {
            get
            {
                return txtSchemeName.Text.Trim();
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private void txtSchemeName_TextChanged(object sender, TextChangedEventArgs e)
        {
            btnOk.IsEnabled = (txtSchemeName.Text.Trim() != "");
        }

    }
}
