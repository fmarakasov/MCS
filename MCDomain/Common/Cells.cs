using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.Contracts;
using System.ComponentModel;

namespace MCDomain.Common
{
    /// <summary>
    /// ячейка 
    /// </summary>
    public class Cell : INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;
        public void SendPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }


        /// <summary>
        /// адрес ячейки - столбец
        /// </summary>
        public int Column
        {
            get { return CellRow.IndexOf(this); }
        }

        /// <summary>
        /// адрес ячейки - строка (берется из той строки-массива, к которой принадлежит ячейка)
        /// </summary>
        public int Row
        {
            get { return CellRow.Row; }
        }
        /// <summary>
        /// базовая строка, к которой принадлежит ячейка
        /// </summary>
        CellRow _cellRow;
        public CellRow CellRow
        {
            get { return _cellRow; }
        }
        /// <summary>
        /// конструктор
        /// </summary>
        /// <param name="cellRow">передается строка, к которой принадлежит ячейка</param>
        public Cell(CellRow cellRow)
        {
            _cellRow = cellRow;
        }


        private string _value;
        /// <summary>
        /// значение, записываемое в ячейку
        /// </summary>
        public string Value
        {
            get
            {
                return _value;
            }
            set
            {
                if (_value == value) return;
                _value = value;
                SendPropertyChanged("Value");
            }
        }

        public int RowHeight
        {
            get { return _cellRow.RowHeight; }
        }

        private int _columnWidth;
        public int ColumnWidth
        {
            get { return _columnWidth; }
            set
            {
                if (_columnWidth == value) return;
                _columnWidth = value;
            }
        }

    }

    /// <summary>
    /// строка таблицы, состоит из ячеек
    /// </summary>
    public class CellRow : List<Cell>
    {
        /// <summary>
        /// переопределенный индексатор - всегда возвращает ячейку
        /// </summary>
        /// <param name="index">индекс, соответствующий номеру столбца</param>
        /// <returns>возвращенная ячейка</returns>
        public new Cell this[int index]
        {
            get
            {
                Contract.Requires(index >= 0);
                if (index >= Count) Add(index);
                return base[index];
            }
            set
            {
                Contract.Requires(index < this.Count);
                Contract.Requires(index >= 0);
                base[index] = value;
            }
        }
        /// <summary>
        /// номер строки
        /// </summary>
        public int Row
        {
            get { return _cells.IndexOf(this); }
        }
        /// <summary>
        /// позволяет добавить ячейку в строку
        /// </summary>
        /// <param name="col">номер столбца</param>
        /// <returns>возвращает добавленную ячейку</returns>
        public Cell Add(int col)
        {

            Contract.Ensures(col < Count);
            int i;
            Cell c;
            for (i = Count; i <= col; i++)
            {
                if (i <= Count)
                {
                    c = new Cell(this);
                    Insert(i, c);
                }
            }
            return base[col];
        }

        private int _rowheight;
        /// <summary>
        /// высота строки
        /// </summary>
        public int RowHeight
        {
            get { return _rowheight; }
            set
            {
                if (_rowheight == value) return;
                _rowheight = value;
            }

        }

        private Cells _cells;
        /// <summary>
        /// двумерный массив ячеек, к которому принадлежит строка
        /// </summary>
        public Cells Cells
        {
            get { return _cells; }
        }

        public CellRow(Cells cells)
        {
            _cells = cells;
        }


    }

    /// <summary>
    /// двумерный массив ячеек
    /// </summary>
    public class Cells : List<CellRow>
    {

        /// <summary>
        /// переопределенный индексатор, всегда возвращает строку
        /// </summary>
        /// <param name="index">индекс, соответствующий номеру строки</param>
        /// <returns></returns>
        public new CellRow this[int index]
        {
            get
            {
                Contract.Requires(index >= 0);
                if (index >= Count) AddRow(index);
                return base[index];
            }

            set
            {
                Contract.Requires(index >= 0);
                Contract.Requires(index < this.Count);
                base[index] = value;
            }
        }
        /// <summary>
        /// добавляет строку (в случае, если предшествующие строки не существуют - добавляет и их) 
        /// </summary>
        /// <param name="row">номер строки</param>
        /// <returns>добавленная строка</returns>
        private CellRow AddRow(int row)
        {
            CellRow c;
            int i;

            for (i = 0; i <= row; i++)
            {
                c = new CellRow(this);
                Insert(i, c);
            }
            return base[row];
        }

        /// <summary>
        /// добавляет ячейку
        /// </summary>
        /// <param name="col">номер столбца</param>
        /// <param name="row">номер строки</param>
        /// <returns>добавленная ячейка</returns>
        private Cell Add(int col, int row)
        {
            Contract.Requires(col >= 0);
            Contract.Requires(row >= 0);

            if (this[row] == null) AddRow(row);
            if (this[row][col] == null) this[row].Add(col);
            return this[row][col];
        }

        /// <summary>
        /// конструктор - позволяет создать двумерный массив с ячейками (без пустот)
        /// </summary>
        /// <param name="rowcount">количество строк</param>
        /// <param name="colcount">количество столбцов</param>
        public Cells(int rowcount, int colcount)
        {
            _rowcount = rowcount;
            _colcount = colcount;
            for (int i = 0; i < rowcount; i++)
            {
                this[i].Add(colcount - 1);
            }
        }
        private int _rowcount;
        /// <summary>
        /// количество строк
        /// </summary>
        public int RowCount
        {
            get { return _rowcount; }
        }
        private int _colcount;
        /// <summary>
        /// количество столбцов
        /// </summary>
        public int ColCount
        {
            get { return _colcount; }
        }

        /// <summary>
        /// переназначить измерения
        /// </summary>
        /// <param name="rowcount">количеcтво строк</param>
        /// <param name="colcount">количество столбцов</param>
        public void Reset(int rowcount, int colcount)
        {
            Clear();
            _rowcount = rowcount;
            _colcount = colcount;
            for (int i = 0; i < rowcount; i++)
            {
                this[i].Add(colcount - 1);
            }
        }
    }
}
