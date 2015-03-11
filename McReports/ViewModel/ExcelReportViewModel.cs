using System;
using System.Linq;
using MCDomain.DataAccess;
using MCDomain.Model;
using Microsoft.Office.Interop.Excel;
using CommonBase;
using System.Text;
using System.IO;

namespace McReports.ViewModel
{
    public abstract class ExcelReportViewModel : DateRangeReportViewModel
    {

        protected virtual string MinExcelColumn
        {
            get { return "A"; }
        }

        protected virtual string MaxExcelColumn
        {
            get { return "Z"; }
        }

        protected override string DefaultExt
        {
            get { return ".xls"; }
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
            //MainWorkSheet.Activate();
            //Excel.ActiveWindow.Activate();
            //Save();
            //Excel.Visible = true;
            
        }

        public void AddWorksheet(string name)
        {
            Worksheet w = Excel.Worksheets.Add(After: MainWorkSheet);
            w.Name = name;
            w = null;
        }

        public void AddWorksheetCopy(Worksheet sh, string name, Worksheet before = null)
        {
            if (before == null) before = MainWorkSheet;
            sh.Copy(Before: before);

            Worksheet w = Excel.Worksheets.Item[before.Index - 1] as Worksheet;

            w.Name = name;
            w = null;
        }

        public override void ShowReport()
        {
            MainWorkSheet.Activate();
            Excel.ActiveWindow.Activate();
            Excel.Visible = true;
            base.ShowReport();
        }
        protected override void OnDispose()
        {

            _excel = null;
            base.OnDispose();
        }

        public virtual int DefaultRowHeight
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

        public string DefaultMainWorkSheetName
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

        public virtual int StartPosition 
        { 
            get { return 1; }
        }

        protected ExcelReportViewModel(IContractRepository repository)
            : base(repository)
        {
            
        }

        
        //¬озвращает им€ конрагента в творительном падеже
        public string NameInAblative(string name)
        {
            if (name == null) return null;
            string res = name + " ";
            res = res.Replace("ие ", "ими ");
            res = res.Replace("ии ", "и€ми ");
            res = res.Trim();
            return res;
        }

        /// <summary>
        /// ≈сли параметр слишком велик, то его вставл€ем по част€м
        /// </summary>
        /// <param name="fieldName">Ќазвание пол€, которое надо заменить (#StageName#)</param>
        /// <param name="fieldValue">ѕоле ,которое надо вставить по част€м</param>
        /// <param name="range"> </param>
        /// <returns>остаток строки, который меньше максималььно допустимого предела (255)</returns>
        protected void InsertBigFieldIntoReport(string fieldName, string fieldValue, Range range, int boldcharstart = 0, int boldcharcount = 0)
        {
            const int maxLenth = 250;

            var activecell = range.Find(fieldName);


            while (fieldValue.Count() > maxLenth)
            {
                int maxindex = maxLenth - fieldName.Count();
                range.Replace(fieldName, fieldValue.Substring(0, maxindex) + fieldName);
                fieldValue = fieldValue.Remove(0, maxindex);
            }

            
            range.Replace(fieldName, fieldValue);
            
            if (boldcharstart > 0 && boldcharcount >= boldcharstart ) activecell.Characters[boldcharstart, boldcharcount].Font.Bold = true;

        }

        protected override void Save()
        {
            Excel.ActiveWorkbook.SaveAs(OutputFullFileName);
        }


        protected void SetPrintingArea(Worksheet sheet)
        {
            sheet.PageSetup.PrintArea = string.Format("${0}${1}:${2}${3}", 
                                                      MinExcelColumn, "1",
                                                      MaxExcelColumn, CurrentButtomPosition.ToString());
        }


        public void SetColorByApprovalState(Stage st, int columnindex, Range r)
        {
            Approvalstate tmpApprovalState = null;
            DateTime? tmpApprovalDate = null;
            if (st.Act != null && st.Act.Issigned.HasValue && st.Act.Issigned.Value && st.Act.Signdate.HasValue && st.Act.Signdate.Value.Between(StartPeriod, EndPeriod))
            {
                tmpApprovalState =
                    Repository.ApprovalStates.FirstOrDefault(
                        p => ((WellKnownApprovalstates)p.Id == WellKnownApprovalstates.Signed));

                tmpApprovalDate = st.Act.Signdate;
            }
            else
            {
                tmpApprovalState = st.Approvalstate;
                tmpApprovalDate = st.Statedate;
            }

            if (tmpApprovalState != null && tmpApprovalDate.HasValue && tmpApprovalDate.Value.Between(StartPeriod, EndPeriod))
            {

                if (columnindex > 0)
                {
                    var range = r.Item[1, columnindex] as Range;
                    if (range != null)
                    {

                        range.Interior.Color =
                            System.Drawing.ColorTranslator.ToOle(
                                System.Drawing.Color.FromArgb(tmpApprovalState.WinColor.A,
                                                              tmpApprovalState.WinColor.R,
                                                              tmpApprovalState.WinColor.G,
                                                              tmpApprovalState.WinColor.B));
                    }
                }
            }
        }
    }
}