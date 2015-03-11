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
using System.Diagnostics.Contracts;
using System.Collections.ObjectModel;

namespace MContracts.Controls
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class CellsGridControl: UserControl
    {

        ObservableCollection<Label> _numberlabels;

        private void ClearControls()
        {
           MainGrid.Children.Clear();
           HeaderControls.Clear();
           _crossControl = null;

        }
       
        public CellsGridControl()
        {
            InitializeComponent();
            NumberColumns = true;
            NumberRows = true;
            _numberlabels = new ObservableCollection<Label>();
        }

       

        private int _rowcount;
        public int RowCount
        {
            get { return _rowcount; }
        }

        private int _colcount;
        public int ColCount
        {
            get { return _colcount; }
        }

        private int _basecol;
        public int BaseCol
        {
            get { return _basecol; }
        }

        private int _baserow;
        public int BaseRow
        {
            get { return _baserow; }
        }

        private bool _numbercolumns;
        public bool NumberColumns
        {
            get { return _numbercolumns; }
            set
            {
                if (_numbercolumns == value) return;
                _numbercolumns = value;
            }
        }

        private bool _numberrows;
        public bool NumberRows
        {
            get { return _numberrows; }
            set
            {
                if (_numberrows == value) return;
                _numberrows = value;
            }
        }

        UIElement _crossControl;
        public UIElement CrossControl
        {
            get { return _crossControl; }
            set
            {
                Contract.Requires(NumberColumns && NumberRows, "Для того, чтобы выставить элемент интерфейса в нулевую ячейку таблицы, в таблице должны быть пронумерованы строки и столбцы");

                if (_crossControl != null) return;
                _crossControl = value;
                Grid.SetRow(_crossControl, 0);
                Grid.SetColumn(_crossControl, 0);
                MainGrid.Children.Add(_crossControl);
                
                // добавляем лэйбл
                Label lbl = new Label();
                MainGrid.Children.Add(lbl);
                _numberlabels.Add(lbl);
                Grid.SetRow(lbl, 0);
                Grid.SetColumn(lbl, 0);

                
                lbl.HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch;
                lbl.HorizontalContentAlignment = System.Windows.HorizontalAlignment.Center;
                lbl.FontSize = 10;
                lbl.VerticalAlignment = System.Windows.VerticalAlignment.Top;
                lbl.VerticalContentAlignment = System.Windows.VerticalAlignment.Top;
                
                lbl.Content = "Cхема импорта";
                
            }
        }

        UIElement _headerControl;
        public UIElement HeaderControl
        {
            get
            {
                return _headerControl;
            }

            set
            {
                Contract.Requires(NumberColumns, "Для того, чтобы выставить элемент интерфейса в нулевую строку таблицы, в таблице должны быть пронумерованы столбцы");

                if (_headerControl != null) return;
                _headerControl = value;
                Grid.SetRow(_headerControl, 0);
                Grid.SetColumn(_headerControl, 1);
                MainGrid.Children.Add(_headerControl);
            }
        }

        public void SetDimensions(int rowcount, int colcount, int basecol, int baserow)
        {
            _rowcount = rowcount;
            _colcount = colcount;
            _basecol = basecol;
            _baserow = baserow;

            int i; 
            RowDefinition rd;
            ColumnDefinition cd;
            MainGrid.RowDefinitions.Clear();
            MainGrid.ColumnDefinitions.Clear();
            ClearControls();
            
            // добавляем строки
            for (i = 0; i < RowCount + (NumberColumns?1:0); i++)
            {
                rd = new RowDefinition();
                MainGrid.RowDefinitions.Add(rd);
                rd.Height = new GridLength(32);
            }

       
            // добавляем столбцы
            for (i = 0; i < ColCount + (NumberRows?1:0); i ++)
            {
                cd = new ColumnDefinition();
                MainGrid.ColumnDefinitions.Add(cd);
                cd.Width = new GridLength(150);
            }


            
            // добавляем нумерацию строк и столбцов
            if (NumberRows)
            {

                for (i = 0; i <= RowCount; i++)
                {
                    SetNumberLabel(i, 0, i);
                }   
            } 

            if (NumberColumns)
            {

                for (i = 0; i <= ColCount; i++)
                {
                    SetNumberLabel(0, i, i);
                }
            }


            // добавляем сплиттеры 
            for (i = 1; i <= ColCount; i++)
            {
                SetColumnSplitter(i);
            }

            for (i = 1; i <= RowCount; i++)
            {
                SetRowSplitter(i);
            }

        }

        private void SetNumberLabel(int row, int col, int number)
        {

            Contract.Requires((row == 0) || (col == 0));

            Label lbl = new Label();
            MainGrid.Children.Add(lbl);
            _numberlabels.Add(lbl);
           
          
            lbl.HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch;
            lbl.HorizontalContentAlignment = System.Windows.HorizontalAlignment.Center;
            
            lbl.FontSize = 10;


            if (row == 0)
            {
               lbl.VerticalAlignment = System.Windows.VerticalAlignment.Top;
               lbl.VerticalContentAlignment = System.Windows.VerticalAlignment.Top;
               if (number != 0) lbl.Content = number.ToString() + "[" + (BaseCol + number - 1).ToString() + "]";
               lbl.Margin = new Thickness(0, 0, 0, 0); // место под спрлиттер
            }

            if (col == 0)
            {
                lbl.VerticalAlignment = System.Windows.VerticalAlignment.Stretch;
                lbl.VerticalContentAlignment = System.Windows.VerticalAlignment.Center;
                if (number != 0) lbl.Content = number.ToString() + "[" + (BaseRow + number - 1).ToString() + "]";
                lbl.Margin = new Thickness(0, 0, 0, 0); // место под спрлиттер
            }

            Grid.SetColumn(lbl, col);
            Grid.SetRow(lbl, row);

        }

        public void SetControl(int row, int col, UserControl uc, int width, int height)
        {
            MainGrid.Children.Add(uc);
            MainGrid.ColumnDefinitions[col + (NumberRows ? 1 : 0)].Width = new GridLength(width);
            MainGrid.RowDefinitions[row + (NumberColumns ? 1 : 0)].Height = new GridLength(height);
            Grid.SetColumn(uc, col + (NumberRows ? 1 : 0));
            Grid.SetRow(uc, row + (NumberColumns ? 1 : 0));
            uc.HorizontalAlignment =  System.Windows.HorizontalAlignment.Stretch;
            uc.VerticalAlignment = System.Windows.VerticalAlignment.Stretch;
            uc.Margin = new Thickness(0, 0, 0, 0);
        }

        List<UserControl> _headercontrols;
        public List<UserControl> HeaderControls
        {
            get
            {
                if (_headercontrols == null) _headercontrols = new List<UserControl>();
                return _headercontrols;
            }
        }

        public void SetHeaderControl(int col, UserControl uc, int width)
        {
            MainGrid.Children.Add(uc);
            MainGrid.RowDefinitions[0].Height = new GridLength(50);
            Grid.SetColumn(uc, col);
            Grid.SetRow(uc, 0);
            uc.HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch;
            uc.VerticalAlignment = System.Windows.VerticalAlignment.Bottom;
            uc.Margin = new Thickness(0, 15, 0, 0);
            HeaderControls.Add(uc);
        }

        private void SetColumnSplitter(int col)
        {
            GridSplitter splt = new GridSplitter();
            MainGrid.Children.Add(splt);
            Grid.SetColumn(splt, col);
            Grid.SetRowSpan(splt, RowCount + (NumberColumns ? 1 : 0));
            splt.Background = Brushes.LightGray;
            splt.HorizontalAlignment = HorizontalAlignment.Left;
            splt.VerticalAlignment = VerticalAlignment.Stretch;
            splt.Visibility = System.Windows.Visibility.Visible;
            splt.Width = 3;
            splt.ResizeBehavior = GridResizeBehavior.PreviousAndNext;
            splt.ResizeDirection = GridResizeDirection.Columns;
            Panel.SetZIndex(splt, 1);
        }

        private void SetRowSplitter(int row)
        {
            GridSplitter splt = new GridSplitter();
            MainGrid.Children.Add(splt);
            Grid.SetRow(splt, row);
            Grid.SetColumnSpan(splt, ColCount + (NumberRows ? 1 : 0));
            splt.Background = Brushes.LightGray;
            splt.HorizontalAlignment = HorizontalAlignment.Stretch;
            splt.VerticalAlignment = VerticalAlignment.Top;
            splt.Visibility = System.Windows.Visibility.Visible;
            splt.Height = 3;
            splt.ResizeBehavior = GridResizeBehavior.PreviousAndNext;
            splt.ResizeDirection = GridResizeDirection.Rows;
            Panel.SetZIndex(splt, 1);
        }

            

    }
}
