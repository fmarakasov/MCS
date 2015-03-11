using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

using MCDomain.DataAccess;
using Microsoft.Office.Interop.Excel;
using MCDomain.Model;
using Range = Microsoft.Office.Interop.Excel.Range;

namespace MContracts.ViewModel.Reports
{
    public abstract class BaseReportViewModel : RepositoryViewModel
    {
        public int Measure
        {
            get { return 1000; }
        
        }

        public virtual void SetReport() { }

        protected virtual void BuildReport() { }

        protected virtual NumberFormatInfo NumberFormat
        {
            get { return CultureInfo.CurrentCulture.NumberFormat ; }
        }

        public abstract string ReportFileName { get; }

        private ITemplateProvider _templateProvider;

        public ITemplateProvider TemplateProvider
        {
            get { return _templateProvider ?? (_templateProvider = new DefaultTemplateProvider()); }
            set { _templateProvider = value; }
        }

        private IEnumerable<IContractStateData> _inputcontractdocs; 
        /// <summary>
        /// входной набор договоров
        /// </summary>
        public IEnumerable<IContractStateData> InputContractdocs
        {
            get
            {
                var contractRepositoryViewBasedViewModel = MainViewModel.ActiveWorkspace as ContractRepositoryViewBasedViewModel;

                if (contractRepositoryViewBasedViewModel != null)
                    return _inputcontractdocs ??
                           (_inputcontractdocs =
                            contractRepositoryViewBasedViewModel.ActualContextItems.Cast
                                <IContractStateData>());
                else
                    return null;
            }
        }

        //Входят договора подлежащие сдаче в отчетном периоде, просроченные и ожидающие выполнения (последние с суммой 0)
        private IEnumerable<Contractdoc> _activeContractdocs;
 

        public IEnumerable<Contractdoc> ActiveContractDocs
        {
            get
            {
                if (_activeContractdocs != null) return _activeContractdocs;
                _activeContractdocs = Repository.Contracts.Where(p => (InputContractdocs.Count(x => x.Id == p.Id) > 0));
                return _activeContractdocs;
            }
           
        }

        /// <summary>
        /// нужно или нет запрашивать входные параметры
        /// </summary>
        public bool NeedsInputParameters { get; set; }

        /// <summary>
        /// Получает описание свойств, которые являются входными параметрами отчёта
        /// </summary>
        public IEnumerable<ReportParameter> Parameters
        {
            get
            {
                var pi = GetType().GetProperties();
                return from propertyInfo in pi
                       let attr = propertyInfo.GetCustomAttributes(typeof (InputParameterAttribute), true)
                       where attr.Length == 1
                       select new ReportParameter((InputParameterAttribute) attr[0], propertyInfo.Name);
            }
        }


        public string ReportTemplate
        {
            get { return TemplateProvider.GetTemplate(ReportFileName); }
        }

       protected BaseReportViewModel(IContractRepository repository)
            : base(repository)
       {
           NeedsInputParameters = false;
       }

       protected override void Save()
       {

       }

       protected override bool CanSave()
       {
           return false;
       }

     

       protected virtual void SetHeader()
       { }

      


    }



    public abstract class ExcelReportViewModel : BaseReportViewModel
    {

        protected virtual string MaxExcelColumn
        {
            get { return "Z"; }
        }


        public static int GetLongStringRowsCount(string s, int rowwidth, int defaultcount)
        {
            int iRowCount = s.Length/rowwidth;
            if (iRowCount < defaultcount) iRowCount = defaultcount;
            return iRowCount;
        }


        public override void SetReport()
        {
            Excel.Workbooks.Add(ReportTemplate);
            #if DEBUG
                Excel.Visible = true;
            #endif

            BuildReport();      
            MainWorkSheet.Activate();
            Excel.ActiveWindow.Activate();     
            Excel.Visible = true;
        }

        public void AddWorksheet(string name)
        {
            Worksheet w = Excel.Worksheets.Add(After: MainWorkSheet);
            w.Name = name;
        }

        public void AddWorksheetCopy(Worksheet sh, string name)
        {
            sh.Copy(Before: MainWorkSheet);

            Worksheet w = Excel.Worksheets.Item[MainWorkSheet.Index - 1] as Worksheet;
            w.Name = name;
        }


        protected override void OnDispose()
        {
            _excel = null;
            base.OnDispose();
        }

        public int DefaultRowHeight
        {
            get { return 23; }
        }

        private static Application _excel;

        public static Application Excel
        {
            get { return _excel ?? (_excel = new Application()); }
        }
        
        protected Worksheet MainWorkSheet
        {
            get
            {
                 return Excel.Worksheets[MainWorkSheetName];
            }
        }

        protected Worksheet ContractorWorkSheet
        {
            get
            {
                return Excel.Worksheets[ContractorWorkSheetName];
            }
        }

        protected Worksheet ContractTypeWorkSheet
        {
            get
            {
                return Excel.Worksheets[ContractTypeWorkSheetName];
            }
        }

        protected Worksheet DetailWorkSheet
        {
            get
            {
                return Excel.Worksheets[Detail];
            }
        }

        protected static Worksheet ActiveWorksheet
        {
            get { return Excel.ActiveSheet; }
        }

        protected Range CopyRowRange(Worksheet sourceSheet, int rangeHeight)
        {
            Range resultRange = ActiveWorksheet.Range["A" + CurrentButtomPosition, MaxExcelColumn + (CurrentButtomPosition + rangeHeight)];//[CurrentButtomPosition + ":" + CurrentButtomPosition + rangeHeight];
            sourceSheet.Range["1:" + rangeHeight].Copy(resultRange);
            CurrentButtomPosition += rangeHeight;
            return resultRange;
        }







        protected virtual DateTime StartPeriod
        {
            get { return DateTime.Now; }
        }

        protected virtual DateTime EndPeriod
        {
            get { return DateTime.Now; }
        }

        public DateTime CurrentDate
        {
            get
            {
                return DateTime.Today > EndPeriod ? EndPeriod : DateTime.Today;
            }
        }



        public virtual string MainWorkSheetName
        {
            get { return "MainReport"; }
        }

        public virtual string ContractorWorkSheetName
        {
            get { return "ContractorType"; }
        }

        public virtual string ContractTypeWorkSheetName
        {
            get { return "ContractType"; }
        }

        public virtual string Detail
        {
            get { return "Detail"; }
        }

        public virtual int CurrentButtomPosition { get; set; }

        protected ExcelReportViewModel(IContractRepository repository)
            : base(repository)
        {
            
        }

        
        //Возвращает имя конрагента в творительном падеже
        public string NameInAblative(string name)
        {
            if (name == null) return null;
            string res = name + " ";
            res = res.Replace("ие ", "ими ");
            res = res.Replace("ии ", "иями ");
            res = res.Trim();
            return res;
        }

        /// <summary>
        /// Если параметр слишком велик, то его вставляем по частям
        /// </summary>
        /// <param name="fieldName">Название поля, которое надо заменить (#StageName#)</param>
        /// <param name="fieldValue">Поле ,которое надо вставить по частям</param>
        /// <param name="range"> </param>
        /// <returns>остаток строки, который меньше максималььно допустимого предела (255)</returns>
        protected void InsertBigFieldIntoReport(string fieldName, string fieldValue, Range range)
        {
            const int maxLenth = 250;

            


            while (fieldValue.Count() > maxLenth)
            {
                int maxindex = maxLenth - fieldName.Count();
                range.Replace(fieldName, fieldValue.Substring(0, maxindex) + fieldName);
                fieldValue = fieldValue.Remove(0, maxindex);
            }

            range.Replace(fieldName, fieldValue);

        }
    }

    public abstract class WordReportViewModel : BaseReportViewModel
    {
        protected WordReportViewModel(IContractRepository repository)
            : base(repository)
        {
            
        }

        protected override void OnDispose()
        {
            _word = null;
            base.OnDispose();
        }


        private static Microsoft.Office.Interop.Word.Application _word;

        public static Microsoft.Office.Interop.Word.Application Word
        {
            get { return _word ?? (_word = new Microsoft.Office.Interop.Word.Application()); }
        }

        private Microsoft.Office.Interop.Word.Document _doc;
        public Microsoft.Office.Interop.Word.Document CurrentDocument
        {
            get { return _doc; }
        }

        public override void SetReport()
        {
            _doc = Word.Documents.Add(ReportTemplate);
            BuildReport();
            Word.ActiveWindow.Activate();
            Word.Visible = true;
            _word = null;
        }

        protected void ReplaceText(string text, string replacetext)
        {
            Word.Selection.Find.ClearFormatting();
            Word.Selection.Find.Replacement.ClearFormatting();
            Word.Selection.Find.Text = text;
            Word.Selection.Find.MatchCase = true;
            Word.Selection.Find.MatchWholeWord = true;
            Word.Selection.Find.Forward = true;
            Word.Selection.Find.Replacement.Text = replacetext;
            Word.Selection.Find.Execute(text, true, true, false, false, false, true, 
                Microsoft.Office.Interop.Word.WdFindWrap.wdFindContinue, false, replacetext, 
                Microsoft.Office.Interop.Word.WdReplace.wdReplaceAll, false, false, false, false); 
        }
    }

    public class ReportParameter
    {
        public InputParameterAttribute Attribute { get; private set; }
        public string PropertyName { get; private set; }
        public ReportParameter(InputParameterAttribute attribute, string propertyName)
        {
            Attribute = attribute;
            PropertyName = propertyName;
        }
    }
}
