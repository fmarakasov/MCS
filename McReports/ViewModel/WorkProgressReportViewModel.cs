using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using MCDomain.DataAccess;
using MCDomain.Model;
using CommonBase;
using McReports.Common;
using Microsoft.Office.Interop.Excel;
using McReports.DTO;

namespace McReports.ViewModel
{
    public class WorkProgressReportViewModel : ExcelReportViewModel
    {

        private string AllContractGroupsSheetName = "Все";
        public WorkProgressReportViewModel(IContractRepository contractRepository)
            : base(contractRepository)
        {
            Year = DateTime.Now.Year;
            Quarter = Quarters.Первый;
        }


        public override int StartPosition
        {
            get { return 9; }
        }

        protected override string MinExcelColumn
        {
            get { return "A"; }
        }

        protected override string MaxExcelColumn
        {
            get { return "Y"; }
        }


        protected override void OnPeriodChanged()
        {
            Year = Period.End.Year;
            Quarter = Period.End.GetQuarterByDate();

            base.OnPeriodChanged();
        }


        [InputParameter(typeof(int), "Год")]
        public int Year { get; set; }

        [InputParameter(typeof(Quarters), "Квартал")]
        public Quarters Quarter { get; set; }



        public override string DisplayName
        {
            get
            {
                return "План работ по видам работ на квартал";
            }
            protected set
            {
                base.DisplayName = value;
            }
        }

        protected override string OutputFileName
        {
            get
            {
                return string.Format("План работ по видам работ на {0} квартал {1} года", Quarter.Description(), Year.ToString(CultureInfo.InvariantCulture));
            }
        }
        public Worksheet DetailWorksheet
        {
            get
            {
                return Excel.Worksheets["Detail"];
            }
        }


        protected override string ReportFileName
        {
            get
            {
                return "WorkProgressReport";
            }
        }


        protected override DateTime StartPeriod
        {
            get { return DateTimeExtensions.GetFirstQuarterDay(Year, (int)Quarter); }
        }

        protected override DateTime EndPeriod
        {
            get { return DateTimeExtensions.GetLastQuarterDay(Year, (int)Quarter); }
        }


        private IEnumerable<WorkProgressDTO> _worksinprogress
        {
            get
            {
                IEnumerable<Contractdoc> contractdocs = ActiveContractDocs.Where(c => c.Contracttype != null && c.Stages.Any(s => s.GetConditionOnDate(EndPeriod) == StageCondition.HaveToEnd || s.GetConditionOnDate(EndPeriod) == StageCondition.Overdue));

                //contractdocs = contractdocs.Where(c => c.Num == "129-11-02");
                if (contractdocs.Any() )
                {
                    var e =
                        contractdocs.Aggregate(new List<WorkProgressDTO>(), (a, c) =>
                        {
                            a.Add(new WorkProgressDTO
                            {
                                Year = this.Year,
                                Quarter = this.Quarter,
                                Contractdoc = c,
                                Contracttype = c.Contracttype,
                                Directors = c.Directors,
                                OverallPlan = c.GetPriceForPeriodWithNDS(StartPeriod, EndPeriod, Measure),
                                CoworkersPlan = c.GetCoworkersPriceForPeriodWithNDS(StartPeriod, EndPeriod, Measure),

                                SignedOverallPlan = c.Contractstate == null ? 0 : !c.Contractstate.IsSigned ? 0 :
                                  c.GetPriceForPeriodWithNDS(StartPeriod, EndPeriod, Measure),

                                SignedCoworkersPlan = c.Contractstate == null ? 0 : !c.Contractstate.IsSigned ? 0 :
                                                       c.GetCoworkersPriceForPeriodWithNDS(StartPeriod, EndPeriod, Measure),

                                OverallFact =  c.GetFactPriceForPeriodWithNDS(StartPeriod, EndPeriod, Measure), 
                                CoworkersFact = c.GetFactCoworkersPriceForPeriodWithNDS(StartPeriod, EndPeriod, Measure), 

                                OverallWaitingForSignature = c.GetApprovalStatePriceForPeriodWithNDS(StartPeriod, EndPeriod, WellKnownApprovalstates.WaitingForSignature, Measure),
                                CoworkersWaitingForSignature = c.GetApprovalStateCoworkersPriceForPeriodWithNDS(StartPeriod, EndPeriod, WellKnownApprovalstates.WaitingForSignature, Measure),

                                OverallUIR = c.GetApprovalStatePriceForPeriodWithNDS(StartPeriod, EndPeriod, WellKnownApprovalstates.UIR, Measure),
                                CoworkersUIR = c.GetApprovalStateCoworkersPriceForPeriodWithNDS(StartPeriod, EndPeriod, WellKnownApprovalstates.UIR, Measure),


                                OverallFZ = c.GetApprovalStatePriceForPeriodWithNDS(StartPeriod, EndPeriod, WellKnownApprovalstates.OnTheFunctionalCustomerSide, Measure),
                                CoworkersFZ = c.GetApprovalStateCoworkersPriceForPeriodWithNDS(StartPeriod, EndPeriod, WellKnownApprovalstates.OnTheFunctionalCustomerSide, Measure),

                                OverallCompletion = c.GetApprovalStatePriceForPeriodWithNDS(StartPeriod, EndPeriod, WellKnownApprovalstates.AtTheFinishingState, Measure),
                                CoworkersCompletion = c.GetApprovalStateCoworkersPriceForPeriodWithNDS(StartPeriod, EndPeriod, WellKnownApprovalstates.AtTheFinishingState, Measure),

                                OverallNoCompletion = c.GetApprovalStatePriceForPeriodWithNDS(StartPeriod, EndPeriod, WellKnownApprovalstates.NoCompletion, Measure),
                                CoworkersNoCompletion = c.GetApprovalStateCoworkersPriceForPeriodWithNDS(StartPeriod, EndPeriod, WellKnownApprovalstates.NoCompletion, Measure)
                            });
                            return a;
                        });
                    return e;
                }

                throw new NoReportDataException();
                
            }
        }



        

        protected override void BuildReport()
        {


            

            IEnumerable<Contracttype> ctypes = _worksinprogress.Select(w => w.Contracttype).Distinct();

            var reporter = CreateProgressReporter(ctypes.Count() + 1);
            reporter.ReportProgress();

            foreach (var contractTypeGroup in ctypes)
            {
                CurrentButtomPosition = StartPosition;
                var currentworksinprogress = _worksinprogress.Where(w => w.Contracttype == contractTypeGroup);
                BuildPage(currentworksinprogress, contractTypeGroup);
                reporter.Next();
                reporter.ReportProgress();
            }

            CurrentButtomPosition = StartPosition;
            BuildPage(_worksinprogress, null);
            reporter.Next();
            reporter.ReportProgress();            
            BuildContractTail();

        }

        private void BuildPage(IEnumerable<WorkProgressDTO> _currentworksinprogress, Contracttype _contracttype)
        {
            BuildContractType(_currentworksinprogress, _contracttype);

            IEnumerable<IGrouping<string, WorkProgressDTO>> directorsgroups = _currentworksinprogress.GroupBy(x => x.Directors, x => x).OrderBy(x => x.Key);

            foreach (IGrouping<string, WorkProgressDTO> directorsgroup in directorsgroups)
            {
                BuildDetail(directorsgroup);
            }

            SetPrintingArea(ActiveWorksheet);
        }

       



        private Contracttype _firstcontracttype;
        private Contracttype _currentcontracttype;
        private Contracttype CurrentContracttype
        {
            get
            {

                return _currentcontracttype;
            }
            set
            {
                if (_currentcontracttype == null) _firstcontracttype = value;
                _currentcontracttype = value;
            }
        }

        public override string MainWorkSheetName
        {
            get { return CurrentContracttype != null ? CurrentContracttype.Shortname : AllContractGroupsSheetName; }
        }




        private void BuildContractTail()
        {
            Excel.Worksheets[DefaultMainWorkSheetName].Visible = XlSheetVisibility.xlSheetHidden;

            foreach (Worksheet sh in Excel.Worksheets)
            {
                if (sh.Cells.Find(0, MainWorkSheet.Cells[1, 1], XlFindLookIn.xlValues, XlLookFor.xlLookForFormulas, XlLookAt.xlWhole) != null)
                {
                    sh.Cells.Replace(0, "", XlLookAt.xlWhole);
                }
            }

            if (_firstcontracttype != null) CurrentContracttype = _firstcontracttype;
        }

        private void MakeCellColored(Range r, int colorindex, WellKnownApprovalstates state)
        {

            Approvalstate tmpApprovalState =
                    Repository.ApprovalStates.FirstOrDefault(
                        p => ((WellKnownApprovalstates)p.Id == state));

            var range = r.Item[1, colorindex] as Range;
            if (range != null && tmpApprovalState != null && tmpApprovalState.Color != null)
            {

                range.Interior.Color =
                    System.Drawing.ColorTranslator.ToOle(
                        System.Drawing.Color.FromArgb(tmpApprovalState.WinColor.A,
                                                      tmpApprovalState.WinColor.R,
                                                      tmpApprovalState.WinColor.G,
                                                      tmpApprovalState.WinColor.B));
            }

        }
        
        private void BuildDetail(IGrouping<string, WorkProgressDTO> directorsgroup)
        {
            Range r = CopyRowRange(DetailWorkSheet, 1);
            // 				#OverallCompletion#	#CoworkersCompletion#	#OwnCompletion#	#OverallNoCompletion#	#CoworkersNoCompletion#	#OwnNoCompletion#

            r.Replace("#Directors#", directorsgroup.Key);
            r.Replace("#OverallPlan#", directorsgroup.Sum(w => w.OverallPlan).ToString(NumberFormat));
            r.Replace("#CoworkersPlan#", directorsgroup.Sum(w => w.CoworkersPlan).ToString(NumberFormat));
            r.Replace("#OwnPlan#", directorsgroup.Sum(w => w.OwnPlan).ToString(NumberFormat));

            r.Replace("#SignedOverallPlan#", directorsgroup.Sum(w => w.SignedOverallPlan).ToString(NumberFormat));
            r.Replace("#SignedCoworkersPlan#", directorsgroup.Sum(w => w.SignedCoworkersPlan).ToString(NumberFormat));
            r.Replace("#SignedOwnPlan#", directorsgroup.Sum(w => w.SignedOwnPlan).ToString(NumberFormat));

            decimal colorsum = directorsgroup.Sum(w => w.OverallFact);
            int iColorIndex = 8;
            r.Replace("#OverallFact#", colorsum.ToString(NumberFormat));
            MakeCellColored(r, iColorIndex, WellKnownApprovalstates.Signed);
            
            r.Replace("#CoworkersFact#", directorsgroup.Sum(w => w.CoworkersFact).ToString(NumberFormat));
            r.Replace("#OwnFact#", directorsgroup.Sum(w => w.OwnFact).ToString(NumberFormat));

            iColorIndex += 3;
            colorsum = directorsgroup.Sum(w => w.OverallWaitingForSignature);
            r.Replace("#OverallWaitingForSignature#", colorsum.ToString(NumberFormat));
            MakeCellColored(r, iColorIndex, WellKnownApprovalstates.WaitingForSignature);
            r.Replace("#CoworkersWaitingForSignature#", directorsgroup.Sum(w => w.CoworkersWaitingForSignature).ToString(NumberFormat));
            r.Replace("#OwnWaitingForSignature#", directorsgroup.Sum(w => w.OwnWaitingForSignature).ToString(NumberFormat));

            iColorIndex += 3;
            colorsum = directorsgroup.Sum(w => w.OverallUIR);
            r.Replace("#OverallUIR#", colorsum.ToString(NumberFormat));
            MakeCellColored(r, iColorIndex, WellKnownApprovalstates.UIR);
            r.Replace("#CoworkersUIR#", directorsgroup.Sum(w => w.CoworkersUIR).ToString(NumberFormat));
            r.Replace("#OwnUIR#", directorsgroup.Sum(w => w.OwnUIR).ToString(NumberFormat));


            iColorIndex += 3;
            colorsum = directorsgroup.Sum(w => w.OverallFZ);
            r.Replace("#OverallFZ#", colorsum.ToString(NumberFormat));
            MakeCellColored(r, iColorIndex, WellKnownApprovalstates.OnTheFunctionalCustomerSide);
            r.Replace("#CoworkersFZ#", directorsgroup.Sum(w => w.CoworkersFZ).ToString(NumberFormat));
            r.Replace("#OwnFZ#", directorsgroup.Sum(w => w.OwnFZ).ToString(NumberFormat));

            iColorIndex += 3;
            colorsum = directorsgroup.Sum(w => w.OverallCompletion);
            r.Replace("#OverallCompletion#", colorsum.ToString(NumberFormat));
            MakeCellColored(r, iColorIndex, WellKnownApprovalstates.AtTheFinishingState);
            r.Replace("#CoworkersCompletion#", directorsgroup.Sum(w => w.CoworkersCompletion).ToString(NumberFormat));
            r.Replace("#OwnCompletion#", directorsgroup.Sum(w => w.OwnCompletion).ToString(NumberFormat));

            iColorIndex += 3;
            colorsum = directorsgroup.Sum(w => w.OverallNoCompletion);
            r.Replace("#OverallNoCompletion#", colorsum.ToString(NumberFormat));
            MakeCellColored(r, iColorIndex, WellKnownApprovalstates.NoCompletion);
            
            r.Replace("#CoworkersNoCompletion#", directorsgroup.Sum(w => w.CoworkersNoCompletion).ToString(NumberFormat));
            r.Replace("#OwnNoCompletion#", directorsgroup.Sum(w => w.OwnNoCompletion).ToString(NumberFormat));
            
            r.Rows.AutoFit();


            
        }



        private void BuildContractType(IEnumerable<WorkProgressDTO> _contracttypeworksinprogtess, Contracttype _contracttype)
        {
            
            // текущий тип договора
            CurrentContracttype = _contracttype;
            // на основе листа Main создать новый лист
            string sSheetName = (_contracttype != null) ? _contracttype.Shortname : AllContractGroupsSheetName;
            AddWorksheetCopy(Excel.Worksheets[DefaultMainWorkSheetName], sSheetName, Excel.Worksheets[DefaultMainWorkSheetName]);
            MainWorkSheet.Activate();

            MainWorkSheet.Cells.Replace("#WorkType#", sSheetName);
            MainWorkSheet.Cells.Replace("#Quarter#", ((int)Quarter).ToString(CultureInfo.InvariantCulture));
            MainWorkSheet.Cells.Replace("#Year#", Year);
            MainWorkSheet.Cells.Replace("#ReportDate#", CurrentDate.ToString("dd.MM.yyyy"));


            // формируется итоговая строка
            MainWorkSheet.Cells.Replace("#OverallPlan#", _contracttypeworksinprogtess.Sum(w => w.OverallPlan).ToString(NumberFormat));
            MainWorkSheet.Cells.Replace("#CoworkersPlan#", _contracttypeworksinprogtess.Sum(w => w.CoworkersPlan).ToString(NumberFormat));
            MainWorkSheet.Cells.Replace("#OwnPlan#", _contracttypeworksinprogtess.Sum(w => w.OwnPlan).ToString(NumberFormat));

            MainWorkSheet.Cells.Replace("#SignedOverallPlan#", _contracttypeworksinprogtess.Sum(w => w.SignedOverallPlan).ToString(NumberFormat));
            MainWorkSheet.Cells.Replace("#SignedCoworkersPlan#", _contracttypeworksinprogtess.Sum(w => w.SignedCoworkersPlan).ToString(NumberFormat));
            MainWorkSheet.Cells.Replace("#SignedOwnPlan#", _contracttypeworksinprogtess.Sum(w => w.SignedOwnPlan).ToString(NumberFormat));

            MainWorkSheet.Cells.Replace("#OverallFact#", _contracttypeworksinprogtess.Sum(w => w.OverallFact).ToString(NumberFormat));
            MainWorkSheet.Cells.Replace("#CoworkersFact#", _contracttypeworksinprogtess.Sum(w => w.CoworkersFact).ToString(NumberFormat));
            MainWorkSheet.Cells.Replace("#OwnFact#", _contracttypeworksinprogtess.Sum(w => w.OwnFact).ToString(NumberFormat));

            MainWorkSheet.Cells.Replace("#OverallWaitingForSignature#", _contracttypeworksinprogtess.Sum(w => w.OverallWaitingForSignature).ToString(NumberFormat));
            MainWorkSheet.Cells.Replace("#CoworkersWaitingForSignature#", _contracttypeworksinprogtess.Sum(w => w.CoworkersWaitingForSignature).ToString(NumberFormat));
            MainWorkSheet.Cells.Replace("#OwnWaitingForSignature#", _contracttypeworksinprogtess.Sum(w => w.OwnWaitingForSignature).ToString(NumberFormat));

            MainWorkSheet.Cells.Replace("#OverallUIR#", _contracttypeworksinprogtess.Sum(w => w.OverallUIR).ToString(NumberFormat));
            MainWorkSheet.Cells.Replace("#CoworkersUIR#", _contracttypeworksinprogtess.Sum(w => w.CoworkersUIR).ToString(NumberFormat));
            MainWorkSheet.Cells.Replace("#OwnUIR#", _contracttypeworksinprogtess.Sum(w => w.OwnUIR).ToString(NumberFormat));

            MainWorkSheet.Cells.Replace("#OverallFZ#", _contracttypeworksinprogtess.Sum(w => w.OverallFZ).ToString(NumberFormat));
            MainWorkSheet.Cells.Replace("#CoworkersFZ#", _contracttypeworksinprogtess.Sum(w => w.CoworkersFZ).ToString(NumberFormat));
            MainWorkSheet.Cells.Replace("#OwnFZ#", _contracttypeworksinprogtess.Sum(w => w.OwnFZ).ToString(NumberFormat));

            MainWorkSheet.Cells.Replace("#OverallCompletion#", _contracttypeworksinprogtess.Sum(w => w.OverallCompletion).ToString(NumberFormat));
            MainWorkSheet.Cells.Replace("#CoworkersCompletion#", _contracttypeworksinprogtess.Sum(w => w.CoworkersCompletion).ToString(NumberFormat));
            MainWorkSheet.Cells.Replace("#OwnCompletion#", _contracttypeworksinprogtess.Sum(w => w.OwnCompletion).ToString(NumberFormat));

            MainWorkSheet.Cells.Replace("#OverallNoCompletion#", _contracttypeworksinprogtess.Sum(w => w.OverallNoCompletion).ToString(NumberFormat));
            MainWorkSheet.Cells.Replace("#CoworkersNoCompletion#", _contracttypeworksinprogtess.Sum(w => w.CoworkersNoCompletion).ToString(NumberFormat));
            MainWorkSheet.Cells.Replace("#OwnNoCompletion#", _contracttypeworksinprogtess.Sum(w => w.OwnNoCompletion).ToString(NumberFormat));

            

        }


       
    }
}