using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Office.Interop.Word;
using System.Diagnostics.Contracts;
using System.IO;
using System.Collections;
using MCDomain.Common;
using CommonBase;




namespace MCDomain.Importer
{
    class TableNotFoundException:Exception
    {
        public TableNotFoundException(string message): base(message) {}
    }

    class WordDataSourceNotSetException : Exception
    {
        public WordDataSourceNotSetException(string message) : base(message) { }
    }

    public class WordReader: BaseReader, IReaderCommonParameters
    {
        public WordReader()
        {
            Clear();
            AcceptingExtensions.Add(".doc");
            AcceptingExtensions.Add(".docx");
            AcceptingExtensions.Add(".rtf");
        }

        ~WordReader()
        {
           // Clear();
        }

        /// <summary>
        /// запущенное приложение Word
        /// </summary>
        private Microsoft.Office.Interop.Word.Application _app;
        /// <summary>
        /// открытый документ Word
        /// </summary>
        private Microsoft.Office.Interop.Word.Document _doc;

        private Microsoft.Office.Interop.Word.Document InitializeApp(ref Microsoft.Office.Interop.Word.Application app)
        {
            Contract.Ensures(app != null);
            if (app != null)
            {
                app.Quit();             
                app = null;
            }

            app = new Microsoft.Office.Interop.Word.Application();
            app.Visible = false;
            app.DisplayAlerts = WdAlertLevel.wdAlertsNone;

            if (!File.Exists(FileName))
            {
                Clear();
                throw new FileNotFoundException("Файл не найден");
            }
            return app.Documents.Open(FileName, false, true);
        }


        /// <summary>
        /// открыть документ
        /// </summary>
        public override void Open()
        {
            Contract.Ensures(_doc != null);
            ClearFileElements(); 
            _doc = InitializeApp(ref _app);
        }


        /// <summary>
        /// текущий документ
        /// </summary>
        public Microsoft.Office.Interop.Word.Document ActiveDocument
        {
            get { return _doc; }
        }
        
        /// <summary>
        /// закрыть документ и открытое приложение
        /// </summary>
        public override void Close()
        {
            if (_doc != null) ActiveDocument.Close(false);
            Clear();
        }

        /// <summary>
        /// все таблицы в документе MSWord
        /// </summary>
        public Tables DocumentTables
        {
            get 
            {
                if (ActiveDocument != null)
                    return ActiveDocument.Tables;
                else
                    return null;
 
            }
        }

        private Microsoft.Office.Interop.Word.Table _tbl;
        /// <summary>
        /// текущая (выбранная пользователем или, по умолчанию, первая, таблица в документе)
        /// </summary>
        public Microsoft.Office.Interop.Word.Table ActiveTable
        {
            get
            {
                Contract.Requires(ActiveDocument != null);

                if (_tbl == null)
                {
                    if (ActiveDocument.Tables.Count > 0) _tbl = ActiveDocument.Tables[1];
                    else throw new TableNotFoundException("В документе отсутствуют таблицы");
                }
                return _tbl;

            }
     
            set
            {
                Contract.Requires(value != null);
                Contract.Ensures(_tbl == value);
                _tbl = value;
            }
         }


        protected override void DefineFinishCell(ref int? finishCol, ref int? finishRow)
        {
            if (ActiveTable != null)
            {
                finishRow = ActiveTable.Rows.Count;
            }
            else
            {
                finishRow = 0;
                throw new TableNotFoundException("Таблиц в файле не найдено");
            }
        }

        /// <summary>
        /// прочитать содержимое таблицы
        /// </summary>
        /// <param name="rowcount">количество строк</param>
        /// <param name="colcount">количество столбцов</param>
        protected override void InternalRead(int rowcount, int colcount)
        {

            base.InternalRead(rowcount, colcount);
            // читаем документ
            int i;
            int j;
            int mi;
            int mj;

            i = StartRow.HasValue ? StartRow.Value : 1;
            mi = 1;
         
            int? fc = FinishColumn;
            int? fr = FinishRow;
            DefineFinishCell(ref fc, ref fr);
            if (fr < FinishRow) FinishRow = fr;

            RunInitSteps(2*((FinishRow.HasValue?FinishRow.Value : i) - i + 1));

            while (i <= FinishRow.Value)
            {
                mj = 1;
                j = 1;

                Cells[mi - 1].RowHeight = 100;//(int)ActiveTable.Rows[i].Height;
                while (mj <= colcount)
                {
                    Cells[mi - 1][mj - 1].Value = ActiveTable.Cell(i, j).Range.Text;
                    Cells[mi - 1][mj - 1].ColumnWidth = 20;// (int)ActiveTable.Cell(i, j).Width;
                    if (ActiveTable.Cell(i, j).Next == null) break;
                    mj++;
                    j = ActiveTable.Cell(i, j).Next.ColumnIndex;
                }
                i++;
                mi++;
                RunNextStep();
            }
        }

        /// <summary>
        /// читаем из файла - на выходе таблица Cells
        /// </summary>
        public override void Read()
        {
            int _rc = !(FinishRow - StartRow + 1).HasValue ? 1 : (FinishRow - StartRow + 1).Value;
            InternalRead(_rc, ActiveTable.Columns.Count);
        }

        /// <summary>
        /// очистить состояние объекта
        /// </summary>
        public override void Clear()
        {
            _tbl = null;
            if (_doc != null)
            {
              _doc.Close(false);
              _doc = null;
            }
            base.Clear();
            if (_app != null) _app.Quit(false);
            _app = null;
        }

        /// <summary>
        /// задает параметры для конвертора
        /// </summary>
        /// <param name="parameters">объект с параметрами</param>
        public override void AcceptParameters(IReaderCommonParameters parameters) 
        {
            Contract.Ensures(StartRow == parameters.StartRow);
            if (ActiveDocument == null) throw new WordDataSourceNotSetException("Источник данных (документ Word) не задан");
            if (parameters.ActiveFileElementIndex > _doc.Tables.Count) throw new TableNotFoundException("Таблица с указанным индексом " + parameters.ActiveFileElementIndex.ToString() + " не найдена");

      
            GetFileElements();
            base.AcceptParameters(parameters);
            _tbl = _doc.Tables[parameters.ActiveFileElementIndex];

        }




        private List<FileElement> _fileelements;

        protected override IEnumerable<INamed> GetFileElements()
        {

            if (_fileelements == null)
            {
                _fileelements = new List<FileElement>();

                if (DocumentTables != null)
                {
                    int i = 0;
                    foreach (Table tbl in DocumentTables)
                    {
                        i++;
                        _fileelements.Add(new FileElement("Таблица " + i.ToString()));
                    }
                }
            }
            return _fileelements;
        }

        public override void ClearFileElements()
        {
            if (_fileelements != null)
                _fileelements = null;

        }

        /// <summary>
        /// столбец, которым начинается область данных в файле - для word - без надобности
        /// </summary>
        public new int? StartColumn
        {
            get
            {
                return null;
            }
            set { }
        }


        /// <summary>
        /// столбец, которым заканчивается область данных в файле - для word - без надобности
        /// </summary>
        public new int? FinishColumn
        {
            get
            {
                return null;
            }
            set {}
        }

        /// <summary>
        /// показать приложение 
        /// </summary>
        public override void ShowApp()
        {
            base.ShowApp();
            Microsoft.Office.Interop.Word.Application __app = null;
            InitializeApp(ref __app);
            __app.Visible = true;
        }
    }
}
