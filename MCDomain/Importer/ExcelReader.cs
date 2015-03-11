using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Office.Interop.Excel;
using System.Diagnostics.Contracts;
using System.IO;
using MCDomain.Common;
using CommonBase;



namespace MCDomain.Importer
{
    public class ExcelReader: BaseReader
    {

        class SheetNotFoundException : Exception
        {
            public SheetNotFoundException(string message) : base(message) { }
        }

        class ExcelDataSourceNotSetException : Exception
        {
            public ExcelDataSourceNotSetException(string message) : base(message) { }
        }

        public ExcelReader()
        {
            Clear();
            AcceptingExtensions.Add(".xls");
            AcceptingExtensions.Add(".xlsx");
        }

        ~ExcelReader()
        {
            //Clear();
        }

        /// <summary>
        /// приложение Excel
        /// </summary>
        private Microsoft.Office.Interop.Excel.Application _app;
        /// <summary>
        /// книга, открытая в Excel
        /// </summary>
        private Microsoft.Office.Interop.Excel.Workbook _wb;
        public override void Clear()
        {
            _currentRange = null;
            _sh = null;
            if (_wb != null)
            {
                _wb.Close(false);
                _wb = null;
            }
            
            base.Clear();
            if (_app != null) _app.Quit();
            _app = null;
        }

        private Microsoft.Office.Interop.Excel.Workbook InitializeApp(ref Microsoft.Office.Interop.Excel.Application app)
        {
           Contract.Ensures(app != null);

           if (app != null)
           {
               app.Quit();
               app = null;
           }
                      
           app = new Microsoft.Office.Interop.Excel.Application();
           app.Visible = false;
           app.DisplayAlerts = false;
           
           if (!File.Exists(FileName))
           {
               Clear();
               throw new FileNotFoundException("Файл не найден");
           }
           
            return app.Workbooks.Open(FileName, false, false);
        }
        
        /// <summary>
        /// открыть книгу 
        /// </summary>
        public override void Open()
        {
           Contract.Ensures(_app != null); 
           Contract.Ensures(_wb != null);
           ClearFileElements(); 
           _wb = InitializeApp(ref _app);
        }

        /// <summary>
        /// закрыть книгу и открытое приложение
        /// </summary>
        public override void Close() {
            if (_wb != null) ActiveWorkBook.Close();
            Clear();
        }

        /// <summary>
        /// открытая сейчас книга
        /// </summary>
        public Workbook ActiveWorkBook {
            get { return _wb; }
        }

        private Worksheet _sh;

        public Worksheet ActiveWorkSheet
        {
            get {
                  Contract.Assert(ActiveWorkBook != null);

                  if (_sh == null && ActiveWorkBook != null) _sh = ActiveWorkBook.ActiveSheet;
                  return _sh; 
            }
        }

        /// <summary>
        /// все листы книги Excel
        /// </summary>
        public Sheets Worksheets
        {
            get
            {
                if (ActiveWorkBook != null)
                  return ActiveWorkBook.Worksheets;
                else
                  return null;
            }
        }

        private List<FileElement> _fileelements;

        protected override IEnumerable<INamed> GetFileElements() 
        {

            if (_fileelements == null)
            {
                _fileelements = new List<FileElement>();

                if (Worksheets != null)
                {
                    foreach (Worksheet sh in Worksheets)
                    {
                        _fileelements.Add(new FileElement(sh.Name));
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
        /// позволяет автоматически определить конечную строку
        /// </summary>
        /// <param name="finishCol"></param>
        /// <param name="finishRow"></param>
        protected override void DefineFinishCell(ref int? finishCol, ref int? finishRow)
        {
            Contract.Ensures(finishCol >= StartColumn);
            Contract.Ensures(finishRow >= StartRow);
            
            if (_currentRange == null)
                ReadCurrentRegion();

            finishCol = CurrentRange.Column + CurrentRange.Columns.Count - 1;
            finishRow = CurrentRange.Row + CurrentRange.Rows.Count - 1;

            
        }

        private void ReadCurrentRegion()
        {
            Contract.Requires(StartColumn > 0);
            Contract.Requires(StartRow > 0);
            Contract.Ensures(_currentRange != null);
            Contract.Ensures(_currentRange.Column == StartColumn);
            Contract.Ensures(_currentRange.Row == StartRow);

            _currentRange = ActiveWorkSheet.Cells[StartRow, StartColumn].CurrentRegion;

            int iFinRow = 0; int iFinCol = 0;
            // дальше проверяем - нам не нужен текущий регион выше указанного нами левого верхнего угла
            if ((!FinishRow.HasValue) || (FinishRow.Value < StartRow.Value))
                iFinRow = _currentRange.Row + _currentRange.Rows.Count - 1;
            else 
                iFinRow = FinishRow.HasValue?FinishRow.Value:_currentRange.Row + _currentRange.Rows.Count - 1;

            if ((!FinishColumn.HasValue) || (FinishColumn.Value < StartColumn.Value))
                iFinCol = _currentRange.Column + _currentRange.Columns.Count - 1;
            else
                iFinCol = FinishColumn.HasValue ? FinishColumn.Value : _currentRange.Column + _currentRange.Columns.Count - 1;


            if ((iFinRow >= StartRow) && (iFinCol >= StartColumn))
            {
                _currentRange = ActiveWorkSheet.Range[ActiveWorkSheet.Cells[StartRow, StartColumn],
                                                      ActiveWorkSheet.Cells[iFinRow, iFinCol]];
            }

        }

        private Microsoft.Office.Interop.Excel.Range _currentRange;
        public Microsoft.Office.Interop.Excel.Range CurrentRange 
        {
            get {
                   if (_currentRange == null) ReadCurrentRegion();
                   return _currentRange; }
        }


       

        protected override void InternalRead(int rowcount, int colcount)
        {

            RunInitSteps(2*rowcount);
            base.InternalRead(rowcount, colcount);
            // читаем эксель файл
            int i;
            int j;

            for (i = 1; i <= rowcount; i++)
            {
                Cells[i - 1].RowHeight = (int)CurrentRange.Rows[i].RowHeight;
                for (j = 1; j <= colcount; j++)
                {
                    if (CurrentRange.Cells[i, j].Value != null)
                    {
                        Cells[i - 1][j - 1].Value = CurrentRange.Cells[i, j].Value.ToString();
                        Cells[i - 1][j - 1].ColumnWidth = (int)CurrentRange.Cells[i, j].ColumnWidth;
                        if (Cells[i - 1][j - 1].ColumnWidth <= 0)
                           Cells[i - 1][j - 1].ColumnWidth = 10;  
                    }
                    else
                    {
                        Cells[i - 1][j - 1].Value = "";
                        Cells[i - 1][j - 1].ColumnWidth = (int)CurrentRange.Cells[i, j].ColumnWidth;
                        if (Cells[i - 1][j - 1].ColumnWidth <= 0)
                            Cells[i - 1][j - 1].ColumnWidth = 10;  

                    }
                }

                RunNextStep();
            }

        }

        /// <summary>
        /// читаем из файла - на выходе таблица Cells
        /// </summary>
        public override void Read ()
        {
            InternalRead(CurrentRange.Rows.Count, CurrentRange.Columns.Count);
        }


        /// <summary>
        /// задает параметры для конвертора
        /// </summary>
        /// <param name="parameters">объект с параметрами</param>
        public override void AcceptParameters(IReaderCommonParameters parameters)
        {

            Contract.Ensures(StartRow == parameters.StartRow);
            Contract.Ensures(StartColumn == parameters.StartColumn);

            if (ActiveWorkBook == null) throw new ExcelDataSourceNotSetException("Источник данных (книга Microsoft Excel) не задан");
            if (parameters.ActiveFileElementIndex > _wb.Sheets.Count) throw new SheetNotFoundException("Лист с указанным индексом " + parameters.ActiveFileElementIndex.ToString() + " не найден");

            GetFileElements();
            base.AcceptParameters(parameters);
            _sh = _wb.Sheets[parameters.ActiveFileElementIndex];
           
            

         
            ReadCurrentRegion();
        }

        /// <summary>
        /// сохраняет параметры конвертора в объект
        /// </summary>
        /// <param name="parameters">объект с параметрами</param>
        public override void SaveParameters(IReaderCommonParameters parameters)
        {
            base.SaveParameters(parameters);
        }

        /// <summary>
        /// показать приложение 
        /// </summary>
        public override void ShowApp()
        {
            base.ShowApp();

            Microsoft.Office.Interop.Excel.Application __app = null;
            InitializeApp(ref __app);
            __app.Visible = true;
        }
    }
}
