using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using MCDomain.Common;
using CommonBase;


namespace MCDomain.Importer
{

    public class FileElement : INamed
    {
        public FileElement(string name)
        {
            Name = name;
        }

        public string Name { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }

    public interface IReaderCommonParameters
    {
        int ActiveFileElementIndex { get; set; }
        /// <summary>
        /// начальная строка диапазона
        /// </summary>
        int? StartRow { get; set; }
        /// <summary>
        /// конечная строка диапазона
        /// </summary>
        int? FinishRow { get; set; }
        /// <summary>
        /// начальный столбец диапазона
        /// </summary>
        int? StartColumn { get; set; }
        /// <summary>
        /// конечный столбец диапазона
        /// </summary>
        int? FinishColumn { get; set; }
    }

    /// <summary>
    /// параметры конвертора со значениями по умолчанию
    /// </summary>
    public class ReaderCommonParameters : IReaderCommonParameters
    {
        public int ActiveFileElementIndex { get; set; }


        public int? StartRow { get; set; }

        public int? FinishRow { get; set; }

        public int? StartColumn { get; set; }


        public int? FinishColumn { get; set; }

        /// <summary>
        /// конструктор с параметрами по умолчанию
        /// </summary>
        public ReaderCommonParameters()
        {
            ActiveFileElementIndex = 1;
            StartRow = 1;
            StartColumn = 1;
            FinishRow = null;
            FinishColumn = null;
        }
        
        /// <summary>
        /// конструктор со всеми параметрами
        /// </summary>
        /// <param name="activeelementindex">индекс элемента: листа или таблицы</param>
        /// <param name="startrow">начальная строка</param>
        /// <param name="startcolumn">начальный столбец</param>
        /// <param name="finishrow">конечная строка</param>
        /// <param name="finishcolumn">конечный столбец</param>
        public ReaderCommonParameters(int activeelementindex, int? startrow, int? startcolumn, int? finishrow, int? finishcolumn)
        {
            ActiveFileElementIndex = activeelementindex;
            StartRow = startrow;
            StartColumn = startcolumn;
            FinishRow = finishrow;
            FinishColumn = finishcolumn;
        }

        public static bool operator == (ReaderCommonParameters lhs, ReaderCommonParameters rhs)
        {
            if ((lhs == null) || (rhs == null)) return lhs == rhs;
            return ((lhs.ActiveFileElementIndex == rhs.ActiveFileElementIndex) 
                && (lhs.StartColumn == rhs.StartColumn) && (lhs.StartRow == rhs.StartRow) && (lhs.FinishColumn == rhs.FinishColumn) &&
                (lhs.FinishRow == rhs.FinishRow));
        }

        public static bool operator !=(ReaderCommonParameters lhs, ReaderCommonParameters rhs)
        {
            if (((object)lhs == null) || ((object)rhs == null))
                return lhs != rhs;
            return !((lhs.ActiveFileElementIndex == rhs.ActiveFileElementIndex) && (lhs.StartColumn == rhs.StartColumn) && (lhs.StartRow == rhs.StartRow) && (lhs.FinishColumn == rhs.FinishColumn) && (lhs.FinishRow == rhs.FinishRow));
        }

        public override bool Equals(object value)
        {
            try
            {
                return this == (ReaderCommonParameters)value;
            }
            catch
            {
                return false;
            }
        }

        public override int GetHashCode()
        {
            return ActiveFileElementIndex.GetHashCode() + StartRow.GetHashCode() + StartColumn.GetHashCode();
        }


    }

    /// <summary>
    /// базовый класс конвертора
    /// </summary>
    public class BaseReader: IReaderCommonParameters
    {
        /// <summary>
        /// очистить состояние
        /// </summary>
        public virtual void Clear() 
        {
            _activefileelementindex = 1;
            _startcolumn = 1;
            _startrow = 1;
            _finishcolumn = null;
            _finishrow = null;
        }
        /// <summary>
        /// открыть файл
        /// </summary>
        public virtual void Open() { }
        public virtual void Close() { }
        private string _filename;
        /// <summary>
        /// имя файла, содержащего конвертируемые данные
        /// </summary>
        public string FileName
        {
            get { return _filename; }
            set
            {
                Contract.Requires(!string.IsNullOrEmpty(value), "Имя файла не должно быть пустой строкой");
                Contract.Ensures(_filename == value);

                if (_filename == value) return;
                _filename = value;
                Clear();
            }
        }

        private Cells _cells;
        /// <summary>
        /// таблица значений полученная из файла
        /// </summary>
        public Cells Cells
        {
            get { return _cells ?? (_cells = new Cells(1, 1)); }
        }

        /// <summary>
        /// прочитать файл в таблицу значений - шаблонный метод
        /// </summary>
        /// <param name="rowcount">количество строк в таблице</param>
        /// <param name="colcount">количество столбцов в таблице</param>
        protected virtual void InternalRead(int rowcount, int colcount)
        {
            Cells.Reset(rowcount, colcount);
        }

        /// <summary>
        /// читать файл - для вызова из интерфейса
        /// </summary>
        public virtual void Read() { }
   

        private List<string> _acceptingextensions;
        /// <summary>
        /// расширения, которые способен принимать данный ридер
        /// </summary>
        public List<string> AcceptingExtensions
        {
            get { return _acceptingextensions ?? (_acceptingextensions = new List<string>()); }
        }

        /// <summary>
        /// проверить - принимает или нет данное расширение
        /// </summary>
        /// <param name="extension">проверяемое расширение</param>
        /// <returns>результат проверки</returns>
        public bool AcceptExtension(string extension)
        {
            return (AcceptingExtensions.IndexOf(extension) > -1);
        }


        protected virtual IEnumerable<INamed> GetFileElements() { return null;  }

        public IEnumerable<INamed> FileElements
        {
            get { return GetFileElements(); }
        }

        public virtual  void ClearFileElements() {}

        int _activefileelementindex;
        public int ActiveFileElementIndex
        {
            get
            {
                return _activefileelementindex;
            }

            set
            {
                _activefileelementindex = value;
            }
        }

        
        private int? _startcolumn;
        /// <summary>
        /// колонка, с которой начинается область данных в файле
        /// </summary>
        public int? StartColumn
        {
            get { return _startcolumn; }
            set
            {
                Contract.Assert(value > 0);
                _startcolumn = value;
            }
        }

        private int? _startrow;
        /// <summary>
        /// строка, с которой начинается область данных в файле
        /// </summary>
        public int? StartRow
        {
            get { return _startrow; }
            set
            {
                Contract.Assert(value > 0);                
                _startrow = value;
            }
        }

        protected virtual void DefineFinishCell(ref int? finishCol, ref int? finishRow) { }

        private int? _finishcolumn;
        /// <summary>
        /// столбец, которым заканчивается область данных в файле
        /// </summary>
        public int? FinishColumn
        {
            get
            {
                if (_finishcolumn == null) DefineFinishCell(ref _finishcolumn, ref _finishrow);
                return _finishcolumn;
            }
            set
            {
                Contract.Assert(value > 0);
                _finishcolumn = value;

            }
        }

        private int? _finishrow;
        /// <summary>
        /// строка, которой заканчивается область данных в файле
        /// </summary>
        public int? FinishRow
        {
            get
            {
                if (_finishrow == null) DefineFinishCell(ref _finishcolumn, ref _finishrow);
                return _finishrow;
            }
            set
            {
                Contract.Assert(value > 0);
                _finishrow = value;
            }
        }


        public virtual void AcceptParameters(IReaderCommonParameters parameters) 
        {
            _activefileelementindex = parameters.ActiveFileElementIndex;
            _startrow = parameters.StartRow;
            _finishrow = parameters.FinishRow;
            _startcolumn = parameters.StartColumn;
            _finishcolumn = parameters.FinishColumn;
        
        }
        public virtual void SaveParameters(IReaderCommonParameters parameters) 
        {
            Contract.Ensures(_startrow == parameters.StartRow);
            Contract.Ensures(_finishrow == parameters.FinishRow);
            Contract.Ensures(_startcolumn == parameters.StartColumn);
            Contract.Ensures(_finishcolumn == parameters.FinishColumn);

            parameters.ActiveFileElementIndex = _activefileelementindex;
            parameters.StartRow = _startrow;
            parameters.FinishRow = _finishrow;
            parameters.StartColumn = _startcolumn;
            parameters.FinishColumn = _finishcolumn;
        }

        public virtual void ShowApp() { }

        public event EventHandler<ProgressEventArgs> InitSteps;
        public event EventHandler<ProgressEventArgs> NextStep;

        protected void RunInitSteps(int max)
        {
            if (InitSteps != null) InitSteps(this, new ProgressEventArgs { Maximum = max });
        }

        protected void RunNextStep()
        {
            if (NextStep != null) NextStep(this, null);
        }
    }
}
