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

namespace MContracts.Controls
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class ImportingSchemeColumnDefiner : UserControl
    {
        private DefaultImportingScheme _scheme;
        public ImportingSchemeColumnDefiner(int colindex)
        {
            InitializeComponent();
            _scheme = DefaultImportingScheme.Scheme;
            DataContext = _scheme;
            _colindex = colindex;
        }

        private int _colindex;
        public int ColumnIndex
        {
            get { return _colindex; }
        }

        public ImportingSchemeItem SelectedItem
        {
            get { return (ImportingSchemeItem)cmbxColumn.SelectedItem; }
            set
            {
                if (value != null)
                    cmbxColumn.SelectedItem = _scheme.Items.GetItemByCode(value.Code);
                else
                    cmbxColumn.SelectedItem = null;
            }
        }

        private void Image_ImageFailed(object sender, ExceptionRoutedEventArgs e)
        {

        }

        private void btnCancelSelect_Click(object sender, RoutedEventArgs e)
        {
            cmbxColumn.SelectedItem = null;
        }
    }
}
