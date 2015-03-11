using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MCDomain.DataAccess;
using MCDomain.Model;
using McReports.Common;
using McReports.DTO;
using Microsoft.Office.Interop.Excel;
using MCDomain.Common;
using CommonBase;

namespace McReports.ViewModel
{
   

    /// <summary>
    /// Отчет 4. Текущий тематический план на X квартал
    /// </summary>
    public class ContractQuarterPlanRepotViewModel : ExcelReportViewModel 
    {

        private int defaultstagerowwidth = 60;
        private int defaultcontractrowwidth = 320;

        public override int StartPosition
        {
            get { return 6; }
        }

        protected override string MinExcelColumn
        {
            get { return "C"; }
        }

        public ContractQuarterPlanRepotViewModel(IContractRepository contractRepository): base(contractRepository)
        {
            SetDefaultDates();
        }

        private void SetDefaultDates()
        {
            Year = DateTime.Now.Year;
            Quarter = Quarters.Первый;
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

       

        public Worksheet StageWorkSheet
        {
            get { return Excel.Worksheets["Detail"]; }
        }

        public Worksheet BaseStageWorkSheet
        {
            get { return Excel.Worksheets["BaseStage"]; }
        }

        public Worksheet ScheduleContractWorkSheet
        {
            get { return Excel.Worksheets["ScheduleContract"]; }
        }


        private static Worksheet ContractTailWorkSheet
        {
            get { return Excel.Worksheets["ContractTail"]; }
        }

        private static Worksheet ContractTypeTailWorkSheet
        {
            get { return Excel.Worksheets["ContractTypeTail"]; }
        }

        private static Worksheet ContractorTypeTailWorkSheet
        {
            get { return Excel.Worksheets["ContractorTypeTail"]; }
        }


        public Worksheet GetWorkSheetByName(string workSheetName)
        {
            return Excel.Worksheets[workSheetName];
        }
        
        protected override string ReportFileName
        {
            get { return "ContractQuarterPlanReport_4"; }
        }

        protected override DateTime StartPeriod
        {
            get { return DateTimeExtensions.GetFirstQuarterDay(Year, (int) Quarter); }
        }

        protected override DateTime EndPeriod
        {
            get { return DateTimeExtensions.GetLastQuarterDay(Year, (int) Quarter); }
        }

        public override string DisplayName
        {
            get
            {
                return "Текущий тематический план на квартал";
               
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
                return string.Format("Текущий тематический план на {0} квартал {1} года", Quarter.ToString(), Year.ToString());
            }
        }


        private List<QuarterPlanContractDto> _contracts
        {
            get
            {
                var contractdocs = ActiveContractDocs.Where(cd => cd.Schedulecontracts != null && cd.Stages != null && cd.Stages.Any());

                if (contractdocs.Any())
                {
                    var e =
                           contractdocs.Aggregate(new List<QuarterPlanContractDto>(), (a, c) =>
                                        {
                                            a.AddRange(c.Stages.Distinct().Where(st => st.Price > 0 &&
                                                       // состояние хотя бы одного из этапоа было или неопределенное или активное или просроченное
                                                       st.GetConditionForPeriod(StartPeriod, EndPeriod) == StageCondition.Overdue ||
                                                       st.GetConditionForPeriod(StartPeriod, EndPeriod) == StageCondition.HaveToEnd  ||
                                                       (st.GetConditionForPeriod(StartPeriod, EndPeriod) == StageCondition.Closed && st.GetConditionOnDate(StartPeriod) != StageCondition.Closed) ||
                                                       (st.Approvalstate != null && st.Approvalstate.WellKnownType == WellKnownApprovalstates.WaitingForSignature &&
                                                        st.Statedate.HasValue && st.Statedate.Value.Between(StartPeriod, EndPeriod))).OrderBy(st => st.NumWithSchedulenum, new NaturSortComparer<Stage>()).Select(x =>
                                                       {
                                                           return
                                                               new QuarterPlanContractDto()
                                                               {
                                                                   Year = Year,

                                                                   Quarter = (int)Quarter,

                                                                   Contract = c,
                                                                   Schedulecontract = c.Schedulecontracts.FirstOrDefault(sc => sc.Schedule != null && x.Schedule != null && sc.Schedule.Id == x.Schedule.Id),

                                                                   ContractorType = (c.Contractor != null) && (c.Contractor.Contractortype != null) ? c.Contractor.Contractortype : Repository.Contractortypes.FirstOrDefault(ct => ct.WellKnownType == WellKnownContractorTypes.Other),

                                                                   ContractType = c.Contracttype ?? Repository.Contracttypes.FirstOrDefault(ct => ct.WellKnownType == WellKnownContractTypes.Undefined),

                                                                   Stage = x,

                                                                   Directors = c.DirectorsAndChiefs(true),

                                                                   ResponsiblePeople = c.GetResponsibleNameForReports(),

                                                                   ContractState = c.Contractstate != null ? c.Contractstate.Name ?? "" : "",

                                                                   StageNum = x.Num ?? "",

                                                                   StageName = x.Subject ?? "",

                                                                   StagePrice = x.StageMoneyModel.National.Factor.WithNdsValue / Measure,

                                                                   //TransitionPrice = PriceOnPreviousQuarters(x, Year, (int)Quarter, Measure),

                                                                   //TransitionAccomplicePrice = x.SubContractsStages != null ? (x.SubContractsStages.Sum(scs => PriceOnPreviousQuarters(scs, Year, (int)Quarter, Measure))) : 0,

                                                                   TransitionPrice = x.IsLeaf() ? x.GetPlanConditionForPeriod(StartPeriod, EndPeriod) == StageCondition.Overdue ? x.StageMoneyModel.Factor.National.WithNdsValue / Measure : 0 : 0,

                                                                   TransitionAccomplicePrice = x.IsLeaf() ? x.GetPlanConditionForPeriod(StartPeriod, EndPeriod) == StageCondition.Overdue ? x.GetOverdueCoworkersPriceForPeriodWithNDS(StartPeriod, EndPeriod, Measure) : 0 : 0,

                                                                   Month1Price = x.IsLeaf() ? x.PriceOnPeriodWithNDS(DateTimeExtensions.GetFirstMonthDay(Year, (int)Quarter * 3 - 2), DateTimeExtensions.GetLastMonthDay(Year, (int)Quarter * 3 - 2), Measure) + x.PriceOnPeriodForWaitingForSignature(DateTimeExtensions.GetFirstMonthDay(Year, (int)Quarter * 3 - 2), DateTimeExtensions.GetLastMonthDay(Year, (int)Quarter * 3 - 2), Measure) : 0,

                                                                   Month2Price = x.IsLeaf() ? x.PriceOnPeriodWithNDS(DateTimeExtensions.GetFirstMonthDay(Year, (int)Quarter * 3 - 1), DateTimeExtensions.GetLastMonthDay(Year, (int)Quarter * 3 - 1), Measure) + x.PriceOnPeriodForWaitingForSignature(DateTimeExtensions.GetFirstMonthDay(Year, (int)Quarter * 3 - 1), DateTimeExtensions.GetLastMonthDay(Year, (int)Quarter * 3 - 1), Measure) : 0,

                                                                   Month3Price = x.IsLeaf() ? x.PriceOnPeriodWithNDS(DateTimeExtensions.GetFirstMonthDay(Year, (int)Quarter * 3), DateTimeExtensions.GetLastMonthDay(Year, (int)Quarter * 3), Measure) + x.PriceOnPeriodForWaitingForSignature(DateTimeExtensions.GetFirstMonthDay(Year, (int)Quarter * 3), DateTimeExtensions.GetLastMonthDay(Year, (int)Quarter * 3), Measure) : 0,

                                                                   AccompilePrice =   x.IsLeaf() ? x.SubContractsStages == null ? 0 :
                                                                                           (x.SubContractsStages.Sum(scs => scs.PlanPriceOnPeriodWithNDS(DateTimeExtensions.GetFirstQuarterDay(Year, (int)Quarter), DateTimeExtensions.GetLastQuarterDay(Year, (int)Quarter), Measure))):0,

                                                                   ActStatus = x.Act == null ? false : !x.Act.Status.HasValue ? false : x.Act.Status.Value == 1,

                                                                   ContractorTypeID = c.Contractor == null ? 0 : c.Contractor.Contractortype == null ? 0 : !c.Contractor.Contractortype.Reportorder.HasValue ? 0 : c.Contractor.Contractortype.Reportorder.Value,

                                                                   ContractTypeID = c.Contracttype == null ? 0 : !c.Contracttype.Reportorder.HasValue ? 0 : c.Contracttype.Reportorder.Value,

                                                                   ContractNumSubject = c.FullName

                                                               };
                                                       }));
                                            return a;
                                        });

                    return e;
                }
                return null;
            }
        }

        //private decimal PriceOnPreviousQuarters(Stage stage, int year, int quarter, int measure)
        //{
        //    return !DateTimeExtensions.HasPereviousQuarters(quarter) ? 0
        //               : stage.PriceOnPeriodWithNDS(DateTimeExtensions.GetFirstQuarterDay(year, 1),
        //                                     DateTimeExtensions.GetLastQuarterDay(year, quarter - 1), measure);
        //}



        private void BuildChiefPages()
        {
            // для каждого руководителя создаем по странице
            var prvtctrcts = _contracts;
            IEnumerable<Employee> emps = prvtctrcts.Where(r => r.Stage.ChiefEmployee != null).Select(c => c.Stage.ChiefEmployee).Distinct().OrderBy(e => e.ToString());

            IEnumerable<QuarterPlanContractDto> econtracts;

            foreach (var e in emps)
            {
                econtracts = prvtctrcts.Where(c => c.Stage.ChiefEmployee != null && c.Stage.ChiefEmployee.Id == e.Id);
                BuildPage(econtracts, e.ToString());
            }

            MainWorkSheet.Activate();

        }

        private void BuildPage(IEnumerable<QuarterPlanContractDto> quarterPlanContractStages, string newpagename = "")
        {


            if (newpagename != string.Empty)
            {
                AddWorksheetCopy(MainWorkSheet, newpagename);
            }

            CurrentButtomPosition = StartPosition;
            //Группировка по тип контрагента
            IEnumerable<IGrouping<Contractortype, QuarterPlanContractDto>> contractorTypeGroups =
                quarterPlanContractStages.GroupBy(x => x.ContractorType, x => x).OrderBy(x => x.Key.Reportorder);
            foreach (IGrouping<Contractortype, QuarterPlanContractDto> contractorGroup in contractorTypeGroups)
            {
                BuildContractorType(contractorGroup.Key);

                //Группировка по типу договора
                IEnumerable<IGrouping<Contracttype, QuarterPlanContractDto>> contractTypeGroups =
                    contractorGroup.GroupBy(x => x.ContractType, x => x).OrderBy(x => x.Key.Reportorder);
                foreach (IGrouping<Contracttype, QuarterPlanContractDto> contractTypeGroup in contractTypeGroups)
                {
                    BuildContractType(contractorGroup.Key, contractTypeGroup.Key);

                    //ГРуппировка по договору
                    IEnumerable<IGrouping<Contractdoc, QuarterPlanContractDto>> contractGroups =
                        contractTypeGroup.GroupBy(x => x.Contract, x => x);
                    foreach (IGrouping<Contractdoc, QuarterPlanContractDto> contractGroup in contractGroups)
                    {
                        BuildContract(contractGroup.Key);
                        var schedulecontractGroups = contractGroup.GroupBy(x => x.Schedulecontract, x => x);
                        if (schedulecontractGroups.Count() == 1)
                        {

                            foreach (QuarterPlanContractDto stage in contractGroup)
                            {
                                BuildStage(stage);
                            }

                        }
                        else
                        {
                            foreach (var sch in schedulecontractGroups.OrderBy(sss => sss.Key.Schedule.Worktype.Name))
                            {
                                BuildScheduleContract(sch.Key);

                                foreach (QuarterPlanContractDto stage in sch)
                                {
                                    BuildStage(stage);
                                }
                            }
                        }

                        BuildContractTail(contractGroup);
                    }
                    BuildContractTypeTail(contractTypeGroup, contractorGroup.Key);

                }
                BuildContractorTypeTail(contractorGroup);
            }
            BuildStatistics(quarterPlanContractStages);
            SetPrintingArea(ActiveWorksheet);
        }

        protected override void BuildReport()
        {

            //При создании этой колеекции сразу выставляем параменры по умолчанию. Если не задан Contractor выставляем в Другие, если не задан ContractorType, задаем Прочие (чтоб все они попали в одну группу и не выбрасовала исключение)
            IEnumerable<QuarterPlanContractDto> quarterPlanContractStages = _contracts;

            SetHeader();
            BuildChiefPages();
            MainWorkSheet.Activate();
            BuildPage(_contracts);

            CurrentButtomPosition = 6;

        

            
        }

        protected override void SetHeader()
        {
            string month1;
            string month2;
            String month3;
            DateTimeExtensions.GetMonthByQuarter(Quarter, out month1, out month2, out month3);
            MainWorkSheet.Cells.Replace("#Year#", Year);
            MainWorkSheet.Cells.Replace("#Quarter#", ((int)Quarter).ToString());
            MainWorkSheet.Cells.Replace("#PreviousQuarter#", ((int)DateTimeExtensions.GetPreviousQuarters(Quarter)).ToString());

            if (Quarter != Quarters.Первый)
                MainWorkSheet.Cells.Replace("#YearPostfix#", "");
            else
                MainWorkSheet.Cells.Replace("#YearPostfix#", (Year - 1).ToString() + " года");

            MainWorkSheet.Cells.Replace("#Month1#", month1);
            MainWorkSheet.Cells.Replace("#Month2#", month2);
            MainWorkSheet.Cells.Replace("#Month3#", month3);


        }

        private void BuildContractorType(Contractortype ContractorGroup)
        {
            Range r = CopyRowRange(ContractorWorkSheet, 1);
            r.Replace("#ContractorType#", ContractorGroup.Name ?? "");
            r.Rows.AutoFit();
        }

        private void BuildContractType(Contractortype ContractorType, Contracttype ContractType)
        {
            Range r = CopyRowRange(ContractTypeWorkSheet, 1);
            r.Replace("#ContractType#", ContractType.Name ?? "");
            r.Replace("#ContractorType#", NameInAblative(ContractorType.Name) ?? "");
            r.Rows.AutoFit();
        }

        public override string Detail
        {
            get { return "Contract"; }
        }

        private void BuildContract(Contractdoc contract)
        {
            Range r = CopyRowRange(DetailWorkSheet, 1);
            if (contract != null)
            {

                r.Replace("#Directors#", contract.DirectorsAndChiefs(true));
                r.Replace("#ContractState#",
                           contract.Contractstate == null ? string.Empty : contract.Contractstate.Name ?? "");

                InsertBigFieldIntoReport("#ContractSubject#", contract.FullSortName, r);

                int iRowCount = GetLongStringRowsCount(contract.FullSortName, defaultcontractrowwidth, 0) + 1;

                if (contract.FullContractorName != string.Empty)
                {

                    iRowCount = iRowCount + contract.FullContractorName.Split('\n').Count();
                    int iSubIndex = r.Cells[1, 5].Value.ToString().IndexOf("#");
                    r.Replace("#Contractor#", contract.FullContractorName);
                    r.Characters[Start: iSubIndex].Font.Bold = false;

                }
                else
                {
                    r.Replace("#Contractor#", "");
                }

                r.RowHeight = DefaultRowHeight * (iRowCount + 2);
            }
        }
        
        private void BuildStage(QuarterPlanContractDto stage)
        {
            //Если это не лист
            if (!stage.Stage.IsLeaf()) BuildParentStage(stage);
            else BuildLeafStage(stage);
        }

        private void BuildParentStage(QuarterPlanContractDto stage)
        {
            Range r = CopyRowRange(BaseStageWorkSheet, 1);
            r.Replace("#Directors#", stage.Directors);
            r.Replace("#ContractState#", stage.ContractState);
            r.Replace("#ResponsiblePeople#", stage.ResponsiblePeople);
            r.Replace("#StageNum#", " " + stage.StageNum);

            //Может выдать исключение, так как слишком большая строка
            // r.Replace("#StageName#", stage.Subject ?? "");
            InsertBigFieldIntoReport("#StageName#", stage.StageName, r);
            r.Rows.AutoFit();
        }

        private void BuildScheduleContract(Schedulecontract schedulecontract)
        {
            Range r = CopyRowRange(ScheduleContractWorkSheet, 1);
            if (schedulecontract != null)
            {
                r.Replace("#ScheduleNum#", schedulecontract.Appnum);
                if (schedulecontract.Schedule != null && schedulecontract.Schedule.Worktype != null)
                    r.Replace("#WorkTypeName#", schedulecontract.Schedule.Worktype.ToString());
                else
                    r.Replace("#WorkTypeName#", "");

            }

        }

        private int GetPriceColumnIndex(QuarterPlanContractDto stage)
        {
            int iClosedAt = 0;
            if (stage.TransitionPrice > 0) iClosedAt = 9;
            if (stage.Month1Price > 0) iClosedAt = 11;
            if (stage.Month2Price > 0) iClosedAt = 12;
            if (stage.Month3Price > 0) iClosedAt = 13;
            
            return iClosedAt;
        }

        private void BuildLeafStage(QuarterPlanContractDto stage)
        {
            Range r = CopyRowRange(StageWorkSheet, 1);
            r.Replace("#Directors#", stage.Directors);
            r.Replace("#ContractState#", stage.ContractState);
            r.Replace("#ResponsiblePeople#", stage.ResponsiblePeople);
            //Ставим пробел, чтоб не менял на дату при экспорте (вводим 1.1 он меняет на 1 янв.)
            r.Replace("#StageNum#", " " + stage.StageNum);

            //StageName слишком большой и иногда выдает исключение
            InsertBigFieldIntoReport("#StageName#", stage.StageName, r);

            //Вставляем строку с форматированным числом (напр. 5 688,26)
            r.Replace("#StagePrice#", (stage.StagePrice).ToString(NumberFormat));
            r.Replace("#PreviousPrice#", (stage.TransitionPrice).ToString(NumberFormat));
            r.Replace("#PreviousAccomplicePrice#", (stage.TransitionAccomplicePrice).ToString(NumberFormat));
            r.Replace("#Month1Price#", (stage.Month1Price).ToString(NumberFormat));
            r.Replace("#Month2Price#", (stage.Month2Price).ToString(NumberFormat));
            r.Replace("#Month3Price#", (stage.Month3Price).ToString(NumberFormat));
            r.Replace("#AccomplicePrice#", (stage.AccompilePrice).ToString(NumberFormat));

            var iClosingAt = GetPriceColumnIndex(stage);
            SetColorByApprovalState(stage.Stage, iClosingAt, r);

            var iRowCount = GetLongStringRowsCount(stage.StageName, defaultstagerowwidth, 4);
            r.RowHeight = DefaultRowHeight * iRowCount;
        }

        private void BuildContractTail(IGrouping<Contractdoc, QuarterPlanContractDto> contract)
        {
            Range r = CopyRowRange(ContractTailWorkSheet, 2);

            r.Replace("#ContractPrice#", contract.Sum(x => x.StagePrice).ToString(NumberFormat));
            r.Replace("#ContractPreviousPrice#", contract.Sum(x => x.TransitionPrice).ToString(NumberFormat));
            r.Replace("#ContractPreviousAccomplicePrice#", contract.Sum(x => x.TransitionAccomplicePrice).ToString(NumberFormat));
            r.Replace("#ContractMonth1Price#", contract.Sum(x => x.Month1Price).ToString(NumberFormat));
            r.Replace("#ContractMonth2Price#", contract.Sum(x => x.Month2Price).ToString(NumberFormat));
            r.Replace("#ContractMonth3Price#", contract.Sum(x => x.Month3Price).ToString(NumberFormat));
            r.Replace("#ContractAccomplicePrice#", contract.Sum(x=>x.AccompilePrice).ToString(NumberFormat));
            r.Rows.AutoFit();
        }

        private void BuildContractTypeTail(IGrouping<Contracttype, QuarterPlanContractDto> contractTypeGroup, Contractortype contractorType)
        {
            Range r = CopyRowRange(ContractTypeTailWorkSheet, 2);

            r.Replace("#ContractType#", contractTypeGroup.Key.Name ?? "");
            r.Replace("#ContractorType#", NameInAblative(contractorType.Name ?? ""));

            r.Replace("#ContractTypePrice#", contractTypeGroup.Sum(x => x.StagePrice).ToString(NumberFormat));
            r.Replace("#ContractTypePreviousPrice#", contractTypeGroup.Sum(x => x.TransitionPrice).ToString(NumberFormat));
            r.Replace("#ContractTypePreviousAccomplicePrice#", contractTypeGroup.Sum(x => x.TransitionAccomplicePrice).ToString(NumberFormat));
            r.Replace("#ContractTypeMonth1Price#", contractTypeGroup.Sum(x => x.Month1Price).ToString(NumberFormat));
            r.Replace("#ContractTypeMonth2Price#", contractTypeGroup.Sum(x => x.Month2Price).ToString(NumberFormat));
            r.Replace("#ContractTypeMonth3Price#", contractTypeGroup.Sum(x => x.Month3Price).ToString(NumberFormat));
            r.Replace("#ContractTypeAccomplicePrice#", contractTypeGroup.Sum(x => x.AccompilePrice).ToString(NumberFormat));
            r.Rows.AutoFit();
        }

        private void BuildContractorTypeTail(IGrouping<Contractortype, QuarterPlanContractDto> contractorTypeGroup)
        {
            Range r = CopyRowRange(ContractorTypeTailWorkSheet, 2);

            r.Replace("#ContractorType#", contractorTypeGroup.Key.Name ?? "");

            r.Replace("#ContractorTypePrice#", contractorTypeGroup.Sum(x => x.StagePrice).ToString(NumberFormat));
            r.Replace("#ContractorTypePreviousPrice#", contractorTypeGroup.Sum(x => x.TransitionPrice).ToString(NumberFormat));
            r.Replace("#ContractorTypePreviousAccomplicePrice#", contractorTypeGroup.Sum(x => x.TransitionAccomplicePrice).ToString(NumberFormat));
            r.Replace("#ContractorTypeMonth1Price#", contractorTypeGroup.Sum(x => x.Month1Price).ToString(NumberFormat));
            r.Replace("#ContractorTypeMonth2Price#", contractorTypeGroup.Sum(x => x.Month2Price).ToString(NumberFormat));
            r.Replace("#ContractorTypeMonth3Price#", contractorTypeGroup.Sum(x => x.Month3Price).ToString(NumberFormat));
            r.Replace("#ContractorTypeAccomplicePrice#", contractorTypeGroup.Sum(x => x.AccompilePrice).ToString(NumberFormat));
            r.Rows.AutoFit();
        }


        private void BuildStatisticsHeader()
        {

            string month1;
            string month2;
            String month3;
            DateTimeExtensions.GetMonthByQuarter(Quarter, out month1, out month2, out month3);

            Worksheet statisticsHeaderWorkSheet = GetWorkSheetByName("StatisticsHeader");

            Range r = CopyRowRange(statisticsHeaderWorkSheet, 5);

            r.Replace("#Quarter#", ((int)Quarter).ToString());
            r.Replace("#PreviousQuarter#", ((int)DateTimeExtensions.GetPreviousQuarters(Quarter)).ToString());
            r.Replace("#Month1#", month1);
            r.Replace("#Month2#", month2);
            r.Replace("#Month3#", month3);
        }

        private void BuildStatisticsOverall(IEnumerable<QuarterPlanContractDto> quarterPlanContractStages)
        {
            Worksheet statisticsOverallWorksheet = GetWorkSheetByName("StatisticsOverall");

            Range r = CopyRowRange(statisticsOverallWorksheet, 3);

            r.Replace("#StagePrice#", quarterPlanContractStages.Sum(x => x.StagePrice).ToString(NumberFormat));
            r.Replace("#PreviousPrice#", quarterPlanContractStages.Sum(x => x.TransitionPrice).ToString(NumberFormat));
            r.Replace("#PreviousAccomplicePrice#", quarterPlanContractStages.Sum(x => x.TransitionAccomplicePrice).ToString(NumberFormat));

            r.Replace("#Month1Price#", quarterPlanContractStages.Sum(x => x.Month1Price).ToString(NumberFormat));
            r.Replace("#Month2Price#", quarterPlanContractStages.Sum(x => x.Month2Price).ToString(NumberFormat));
            r.Replace("#Month3Price#", quarterPlanContractStages.Sum(x => x.Month3Price).ToString(NumberFormat));

            r.Replace("#AccomplicePrice#", quarterPlanContractStages.Sum(x => x.AccompilePrice).ToString(NumberFormat));
            

            r.Rows.AutoFit();
        }

        private void BuildStatisticsForContractortype(int igroupnum, IGrouping<Contractortype, QuarterPlanContractDto> contractorType)
        {
            Worksheet statisticsContractortypeWorksheet = GetWorkSheetByName("StatisticsForContractortype");

            Range r = CopyRowRange(statisticsContractortypeWorksheet, 3);


            r.Replace("#ContractorTypeGroupNum#", igroupnum.ToString());
            r.Replace("#ContractorTypeGroup#", NameInAblative(contractorType.Key.Name ?? ""));

            r.Replace("#StagePrice#", contractorType.Sum(x => x.StagePrice).ToString(NumberFormat));
            r.Replace("#PreviousPrice#", contractorType.Sum(x => x.TransitionPrice).ToString(NumberFormat));
            r.Replace("#PreviousAccomplicePrice#", contractorType.Sum(x => x.TransitionAccomplicePrice).ToString(NumberFormat));

            r.Replace("#Month1Price#", contractorType.Sum(x => x.Month1Price).ToString(NumberFormat));
            r.Replace("#Month2Price#", contractorType.Sum(x => x.Month2Price).ToString(NumberFormat));
            r.Replace("#Month3Price#", contractorType.Sum(x => x.Month3Price).ToString(NumberFormat));

            r.Replace("#AccomplicePrice#", contractorType.Sum(x => x.AccompilePrice).ToString(NumberFormat));
            


            r.Rows.AutoFit();
        }

        private void BuildStatisticsForContracttype(int imaingroupnum, int igroupnum, IGrouping<Contracttype, QuarterPlanContractDto> contractType, string contractortypename)
        {
            Worksheet statisticsContracttypeWorksheet = GetWorkSheetByName("StatisticsForContracttype");

            Range r = CopyRowRange(statisticsContracttypeWorksheet, 3);

            // #ContractorTypeGroupNum#.#ContractTypeGroupnum#. #ContractTypeGroup# по договорам с #ContractorTypeGroup#, в т.ч.		



            r.Replace("#ContractorTypeGroupNum#", imaingroupnum.ToString());
            r.Replace("#ContractTypeGroupNum#", igroupnum.ToString());
            r.Replace("#ContractTypeGroup#", contractType.Key.Name);
            r.Replace("#ContractorTypeGroup#", NameInAblative(contractortypename ?? ""));


            r.Replace("#StagePrice#", contractType.Sum(x => x.StagePrice).ToString(NumberFormat));
            r.Replace("#PreviousPrice#", contractType.Sum(x => x.TransitionPrice).ToString(NumberFormat));
            r.Replace("#PreviousAccomplicePrice#", contractType.Sum(x => x.TransitionAccomplicePrice).ToString(NumberFormat));

            r.Replace("#Month1Price#", contractType.Sum(x => x.Month1Price).ToString(NumberFormat));
            r.Replace("#Month2Price#", contractType.Sum(x => x.Month2Price).ToString(NumberFormat));
            r.Replace("#Month3Price#", contractType.Sum(x => x.Month3Price).ToString(NumberFormat));

            r.Replace("#AccomplicePrice#", contractType.Sum(x => x.AccompilePrice).ToString(NumberFormat));
            


            r.Rows.AutoFit();
        }


        protected void BuildStatistics(IEnumerable<QuarterPlanContractDto> quarterPlanContractStages)
        {

            CurrentButtomPosition++;
            BuildStatisticsHeader();
            BuildStatisticsOverall(quarterPlanContractStages);

            int iGroupNum = 1;
            int iContractorNum;

            IEnumerable<IGrouping<Contractortype, QuarterPlanContractDto>> contractorTypeGroups = quarterPlanContractStages.GroupBy(x => x.ContractorType, x => x).OrderBy(x => x.Key.Reportorder);
            foreach (IGrouping<Contractortype, QuarterPlanContractDto> contractorGroup in contractorTypeGroups)
            {

                BuildStatisticsForContractortype(iGroupNum, contractorGroup);

                IEnumerable<IGrouping<Contracttype, QuarterPlanContractDto>> contractTypeGroups = contractorGroup.GroupBy(x => x.ContractType, x => x);
                iContractorNum = 1;
                foreach (IGrouping<Contracttype, QuarterPlanContractDto> contractTypeGroup in contractTypeGroups)
                {
                    BuildStatisticsForContracttype(iGroupNum, iContractorNum, contractTypeGroup, contractorGroup.Key.Name);
                    iContractorNum++;
                }
                iGroupNum++;
            }
        }
     
    }
}
 