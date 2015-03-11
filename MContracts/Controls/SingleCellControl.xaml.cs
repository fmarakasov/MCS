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
using CommonBase;
using CommonBase;

namespace MContracts.Controls
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class SingleCellControl: UserControl
    {
        private Cell _cell;

        public Cell Cell
        {
            get { return _cell; }
            set
            {
                if (_cell == value) return;
                _cell = value;
            }
        }
        
        public SingleCellControl(Cell cell)
        {
            InitializeComponent();
            _cell = cell;
            DataContext = _cell;
        }

        private void tbCell_GotFocus(object sender, RoutedEventArgs e)
        {
            tbCell.Background = Brushes.LightGray;
        }

        private void tbCell_LostFocus(object sender, RoutedEventArgs e)
        {
            tbCell.Background = Brushes.Ivory;
        }
    }
}
