using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MCDomain.Common;
using MCDomain.DataAccess;
using MCDomain.Model;
using MContracts.Classes;
using MContracts.DTO;
using Microsoft.Office.Interop.Excel;
using System.Drawing;


namespace MContracts.ViewModel.Reports
{
    /// <summary>
    /// Отчет 1. Тематический план на год
    /// </summary>
    public class ContractYearPlanReport_1_ViewModel:ExcelReportViewModel
    {


        private int defaultstagerowwidth = 60;
        private int defaultcontractrowwidth = 320;


        public Contractortype DefaultContractorType
        {
            get { return Repository.Contractortypes.FirstOrDefault(x => x.WellKnownType == WellKnownContractorTypes.Other); }
        }

        public Contracttype DefaultContractType
        {
            get { return Repository.Contracttypes.FirstOrDefault(x => x.WellKnownType == WellKnownContractTypes.Other); }
        }

        public ContractYearPlanReport_1_ViewModel(IContractRepository contractRepository)
            : base(contractRepository)
        {
            PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler(ContractYearPlanReport_1_ViewModel_PropertyChanged);
            NeedsInputParameters = false;
        }

        /// <summary>
        /// начало предыдущего периода - за который рассчитывается остаток на квартал
        /// </summary>
        private DateTime StartPrevPeriod
        {
            get { return DateTimeExtensions.GetFirstYearDay(PrevYear); }
        }

        /// <summary>
        /// конец предыдущего периода - за который рассчитывается остаток на квартал
        /// </summary>
        private DateTime EndPrevPeriod
        {
            get { return DateTimeExtensions.GetLastYearDay(PrevYear); }
        }


        void ContractYearPlanReport_1_ViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "MainViewModel")
            {
                if (MainViewModel != null)
                {
                    Year = MainViewModel.SelectedFilterEndDate.Year;
                    PrevYear = Year - 1;
                }
                else
                {
                    Year = DateTime.Today.Year;
                    PrevYear = Year - 1;
                }
            }
        }
        
        public override string DisplayName
        {
            get
            {
                return "Текущий тематический план на год";
            }
            protected set
            {
                base.DisplayName = value;
            }
        }

        [InputParameter(typeof (int), "Год")]
        public int Year{ get; set; }
        
        public int PrevYear { get; set; }
        
        public override string ReportFileName
        {
            get { return "YearPlanReport_1"; }
        }

        protected override DateTime StartPeriod
        {
            get { return DateTimeExtensions.GetFirstYearDay(Year); }
        }
        
        protected override DateTime EndPeriod
        {
            get { return DateTimeExtensions.GetLastYearDay(Year); }
        }
        
        private List<YearPlanContractDto> _contracts
        {
            get
            {
               IEnumerable<Contractdoc> contractdocs = ActiveContractDocs;
               if (contractdocs != null && contractdocs.Count() > 0)
               {
                   var e =
                          contractdocs.Where(cd => cd.HasSchedule && cd.Stages != null && cd.Stages.Any()).
                          Aggregate(new List<YearPlanContractDto>(), (a, c) =>
                                        {
                                            a.AddRange(c.Stages.Distinct().Where(st =>
                                                  //st.GetConditionForPeriod(StartPeriod, EndPeriod) == StageCondition.Undefined ||
                                                  st.GetConditionForPeriod(StartPeriod, EndPeriod) == StageCondition.HaveToEnd ||
                                                  st.GetConditionForPeriod(StartPeriod, EndPeriod) == StageCondition.Overdue ||
                                                  (st.GetConditionForPeriod(StartPeriod, EndPeriod) == StageCondition.Closed&&st.GetConditionOnDate(StartPeriod)!=StageCondition.Closed)).OrderBy(st => st.Startsat).Select(st => new YearPlanContractDto()
                                                      {
                                                          Year = Year,

                                                          Contract = c,

                                                          ContractorType = (c.Contractor != null) && (c.Contractor.Contractortype != null) ? c.Contractor.Contractortype : Repository.Contractortypes.FirstOrDefault(ct => ct.WellKnownType == WellKnownContractorTypes.Other),

                                                          ContractType = c.Contracttype ?? Repository.Contracttypes.FirstOrDefault(ct => ct.WellKnownType == WellKnownContractTypes.Other),

                                                          Stage = st,

                                                          StageResponsiblePeople = st.GetResponsibleNameForReports(),

                                                          StageDirectors = st.Directors(false),

                                                          StageCurator = st.CuratorEmployee!=null?st.CuratorEmployee.ToString():string.Empty,

                                                          ContractState = c.Contractstate == null ? string.Empty : c.Contractstate.Name ?? "",

                                                          StageNum = st.Num ?? "",

                                                          StageName = st.Subject ?? string.Empty,

                                                          StagePrice =  st.IsLeaf()?st.StageMoneyModel.Factor.National.WithNdsValue/Measure:0,

                                                          PrevYearPrice = st.IsLeaf()?st.GetPlanConditionForPeriod(StartPeriod, EndPeriod) == StageCondition.Overdue ?  st.StageMoneyModel.Factor.National.WithNdsValue/Measure : 0 : 0,
                                                          PrevYearCoworkersPrice = st.IsLeaf()?st.GetPlanConditionForPeriod(StartPeriod, EndPeriod) == StageCondition.Overdue ? st.GetOverdueCoworkersPriceForPeriodWithNDS(StartPeriod, EndPeriod, Measure) : 0 : 0,

                                                          // По этапу, запланированному на текущий год цена работы должна отображаться в столбце, соответствующий месяцу окончания работ согласно календарному плану.
                                                          // По этапу, переходящему с предыдущего года цена работы должна отображаться в столбце, соответствующем последнему месяцу первого квартала
                                                          //Т.е если этап просрочен - его цену выводим в март, на другие периоды его цена будет 0.

                                                          JanuaryPrice = st.IsLeaf()?st.PlanPriceOnPeriodWithNDS(DateTimeExtensions.GetFirstMonthDay(Year, 1), DateTimeExtensions.GetLastMonthDay(Year, 1),Measure):0,
                                                          FebuaryPrice = st.IsLeaf() ? st.PlanPriceOnPeriodWithNDS(DateTimeExtensions.GetFirstMonthDay(Year, 2), DateTimeExtensions.GetLastMonthDay(Year, 2), Measure) : 0,
                                                          MarchPrice = st.IsLeaf() ? st.PlanPriceOnPeriodWithNDS(DateTimeExtensions.GetFirstMonthDay(Year, 3), DateTimeExtensions.GetLastMonthDay(Year, 3), Measure) : 0,


                                                          FirstQuarterAccomplice = st.IsLeaf()?st.SubContractsStages == null ? 0 :
                                                                                                                                     (st.SubContractsStages.Sum(scs => scs.IsLeaf()?scs.PriceOnPeriodWithNDS(DateTimeExtensions.GetFirstQuarterDay(Year, 1), DateTimeExtensions.GetLastQuarterDay(Year, 1), Measure):0)):0,

                                                          AprilPrice = st.IsLeaf() ? st.PlanPriceOnPeriodWithNDS(DateTimeExtensions.GetFirstMonthDay(Year, 4), DateTimeExtensions.GetLastMonthDay(Year, 4), Measure) : 0,
                                                          MayPrice = st.IsLeaf() ? st.PlanPriceOnPeriodWithNDS(DateTimeExtensions.GetFirstMonthDay(Year, 5), DateTimeExtensions.GetLastMonthDay(Year, 5), Measure) : 0,
                                                          JunePrice = st.IsLeaf() ? st.PlanPriceOnPeriodWithNDS(DateTimeExtensions.GetFirstMonthDay(Year, 6), DateTimeExtensions.GetLastMonthDay(Year, 6), Measure) : 0,

                                                          SecondQuarterAccomplice = st.IsLeaf()?st.SubContractsStages == null ? 0 :
                                                                                                                                      (st.SubContractsStages.Sum(scs =>  scs.IsLeaf()?scs.PriceOnPeriodWithNDS(DateTimeExtensions.GetFirstQuarterDay(Year, 2), DateTimeExtensions.GetLastQuarterDay(Year, 2), Measure):0)):0,

                                                          JulyPrice = st.IsLeaf() ? st.PlanPriceOnPeriodWithNDS(DateTimeExtensions.GetFirstMonthDay(Year, 7), DateTimeExtensions.GetLastMonthDay(Year, 7), Measure) : 0,
                                                          AugustPrice = st.IsLeaf() ? st.PlanPriceOnPeriodWithNDS(DateTimeExtensions.GetFirstMonthDay(Year, 8), DateTimeExtensions.GetLastMonthDay(Year, 8), Measure) : 0,
                                                          SeptemberPrice = st.IsLeaf() ? st.PlanPriceOnPeriodWithNDS(DateTimeExtensions.GetFirstMonthDay(Year, 9), DateTimeExtensions.GetLastMonthDay(Year, 9), Measure) : 0,

                                                          ThirdQuarterAccomplice = st.IsLeaf()?st.SubContractsStages == null ? 0 :
                                                                                                                                     (st.SubContractsStages.Sum(scs =>  scs.IsLeaf()?scs.PriceOnPeriodWithNDS(DateTimeExtensions.GetFirstQuarterDay(Year, 3), DateTimeExtensions.GetLastQuarterDay(Year, 3), Measure):0)):0,

                                                          OctoberPrice = st.IsLeaf() ? st.PlanPriceOnPeriodWithNDS(DateTimeExtensions.GetFirstMonthDay(Year, 10), DateTimeExtensions.GetLastMonthDay(Year, 10), Measure) : 0,
                                                          NovemberPrice = st.IsLeaf() ? st.PlanPriceOnPeriodWithNDS(DateTimeExtensions.GetFirstMonthDay(Year, 11), DateTimeExtensions.GetLastMonthDay(Year, 11), Measure) : 0,
                                                          DecemberPrice = st.IsLeaf() ? st.PlanPriceOnPeriodWithNDS(DateTimeExtensions.GetFirstMonthDay(Year, 12), DateTimeExtensions.GetLastMonthDay(Year, 12), Measure) : 0,

                                                          FouthQuarterAccomplice = st.IsLeaf()?st.SubContractsStages == null ? 0 :
                                                                                                                                     (st.SubContractsStages.Sum(scs =>  scs.IsLeaf()?scs.PriceOnPeriodWithNDS(DateTimeExtensions.GetFirstQuarterDay(Year, 4), DateTimeExtensions.GetLastQuarterDay(Year, 4), Measure):0)):0,


                                                          ContractorTypeID = c.Contractor == null ? 0 : c.Contractor.Contractortype == null ? 0 : !c.Contractor.Contractortype.Reportorder.HasValue ? 0 : c.Contractor.Contractortype.Reportorder.Value,

                                                          ContractTypeID = c.Contracttype == null ? 0 : !c.Contracttype.Reportorder.HasValue ? 0 : c.Contracttype.Reportorder.Value,

                                                          ContractNumSubject = c.FullName ?? "",

                                                      }));
                                            return a;
                                        });

                   return e;
               }
                return null;
            } 
            
        }

       
        private void BuildChiefPages()
        {
            // для каждого руководителя создаем по странице
            IEnumerable<Employee> emps = _contracts.Where(r=>r.Stage.ChiefEmployee != null).Select(c => c.Stage.ChiefEmployee).Distinct().OrderBy(e=>e.ToString());
            
            IEnumerable<YearPlanContractDto> econtracts;
            
            foreach (var e in emps)
            {
                econtracts = _contracts.Where(c => c.Stage.ChiefEmployee != null&&c.Stage.ChiefEmployee.Id == e.Id);
                BuildPage(econtracts,  e.ToString());
            }

            MainWorkSheet.Activate();

        }

        private void BuildPage(IEnumerable<YearPlanContractDto> yearPlanContractStages, string newpagename = "")
        {
           

            if (newpagename != string.Empty)
            {
               AddWorksheetCopy(MainWorkSheet, newpagename);
            }

            CurrentButtomPosition = 6;
            //Группировка по тип контрагента
            IEnumerable<IGrouping<Contractortype, YearPlanContractDto>> contractorTypeGroups = yearPlanContractStages.GroupBy(x => x.ContractorType, x => x).OrderBy(x => x.Key.Reportorder);
            foreach (IGrouping<Contractortype, YearPlanContractDto> contractorGroup in contractorTypeGroups)
            {
                BuildContractorType(contractorGroup.Key);

                //Группировка по типу договора
                IEnumerable<IGrouping<Contracttype, YearPlanContractDto>> contractTypeGroups = contractorGroup.GroupBy(x => x.ContractType, x => x);
                foreach (IGrouping<Contracttype, YearPlanContractDto> contractTypeGroup in contractTypeGroups)
                {
                    BuildContractType(contractorGroup.Key, contractTypeGroup.Key);

                    //ГРуппировка по договору
                    IEnumerable<IGrouping<Contractdoc, YearPlanContractDto>> contractGroups = contractTypeGroup.GroupBy(x => x.Contract, x => x);
                    foreach (IGrouping<Contractdoc, YearPlanContractDto> contractGroup in contractGroups)
                    {
                        BuildContract(contractGroup.Key);

                        foreach (YearPlanContractDto stage in contractGroup.OrderBy(x => x.StageNum, HierarchicalNumberingComparier.Instance))
                        {
                            BuildStage(stage);
                        }

                        BuildContractTail(contractGroup);
                    }

                    BuildContractTypeTail(contractTypeGroup, contractorGroup.Key);

                }
                BuildContractorTypeTail(contractorGroup);
            }

            BuildStatistics(yearPlanContractStages);
        }

        protected void BuildStatistics(IEnumerable<YearPlanContractDto> yearPlanContractStages)
        {
            BuildStatisticsHeader();
            BuildStatisticsOverall(yearPlanContractStages);

            int iGroupNum = 1;
            int iContractorNum;

            IEnumerable<IGrouping<Contractortype, YearPlanContractDto>> contractorTypeGroups = yearPlanContractStages.GroupBy(x => x.ContractorType, x => x).OrderBy(x => x.Key.Reportorder);
            foreach (IGrouping<Contractortype, YearPlanContractDto> contractorGroup in contractorTypeGroups)
            {

                BuildStatisticsForContractortype(iGroupNum, contractorGroup);

                IEnumerable<IGrouping<Contracttype, YearPlanContractDto>> contractTypeGroups = contractorGroup.GroupBy(x => x.ContractType, x => x);
                iContractorNum = 1;
                foreach (IGrouping<Contracttype, YearPlanContractDto> contractTypeGroup in contractTypeGroups)
                {
                    BuildStatisticsForContracttype(iGroupNum, iContractorNum, contractTypeGroup, contractorGroup.Key.Name);
                    iContractorNum++;
                }
                iGroupNum++;
            }
        }
    
        protected override void BuildReport()
        {
            //При создании этой колеекции сразу выставляем параменры по умолчанию. Если не задан Contractor выставляем в Другие, если не задан ContractorType, задаем Прочие (чтоб все они попали в одну группу и не выбрасовала исключение)
            //IEnumerable<YearPlanContractDto> yearPlanContractStages = _contracts;
            //BuildPage(yearPlanContractStages, true);
            SetHeader();
            BuildChiefPages();
            MainWorkSheet.Activate();
            BuildPage(_contracts);
        }

        private void BuildContractorType(Contractortype contractorGroup)
        {
            Range r = CopyRowRange(ContractorWorkSheet, 1);
            r.Replace("#ContractorType#", contractorGroup.Name ?? "");
        }

        private void BuildContractType(Contractortype contractorType,Contracttype contractType)
        {
            Range r = CopyRowRange(ContractTypeWorkSheet, 1);
            r.Replace("#ContractType#", contractType.Name ?? "");
            r.Replace("#ContractorType#", NameInAblative(contractorType.Name) ?? "");
        }

        private void BuildContract(Contractdoc contract)
        {
            Range r = CopyRowRange(DetailWorkSheet, 1);
            if (contract != null)
            {
                
                r.Replace("#Directors#", contract.Directors(false));
                r.Replace("#DirectorsOnly#", contract.DirectorEmployee != null?contract.DirectorEmployee.ToString():string.Empty);
                r.Replace("#Curator#", contract.CuratorEmployee != null ? contract.CuratorEmployee.ToString() : String.Empty);

                r.Replace("#ContractState#",
                           contract.Contractstate == null ? string.Empty : contract.Contractstate.Name.ToLower() ?? "");



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

                r.RowHeight = DefaultRowHeight*(iRowCount+2);

            }
        }

        private void BuildStage(YearPlanContractDto stage)
        {
            //Если это не лист
            if (!stage.Stage.IsLeaf())  BuildParentStage(stage);
            else BuildLeafStage(stage);
        }

        private void BuildParentStage(YearPlanContractDto stage)
        {
            Range r=  CopyRowRange(BaseStageWorkSheet, 1);
            if (stage != null)
            {
                r.Replace("#DirectorsOnly#", stage.StageDirector);
                r.Replace("#Curator#", stage.StageCurator);

                r.Replace("#Directors#", stage.StageDirectors);
                r.Replace("#ContractState#", stage.ContractState.ToLower());
                r.Replace("#ResponsiblePeople#", stage.StageResponsiblePeople);
                r.Replace("#StageNum#", " " + stage.StageNum ?? "");

                //Может выдать исключение, так как слишком большая строка
                // r.Replace("#StageName#", stage.Subject ?? "");
                InsertBigFieldIntoReport("#StageName#", stage.StageName, r);

                var iRowCount = GetLongStringRowsCount(stage.StageName, defaultstagerowwidth, 4) + 1;
                r.RowHeight = DefaultRowHeight * iRowCount;
            }
            
        }

        private int GetPriceColumnIndex(YearPlanContractDto stage)
        {
            int iClosedAt = 0;
            if (stage.PrevYearPrice > 0) iClosedAt = 9;
            if (stage.JanuaryPrice > 0) iClosedAt = 11;
            if (stage.FebuaryPrice > 0) iClosedAt = 12;
            if (stage.MarchPrice > 0) iClosedAt = 13;
            if (stage.AprilPrice > 0) iClosedAt = 15;
            if (stage.MayPrice > 0) iClosedAt = 16;
            if (stage.JunePrice > 0) iClosedAt = 17;
            if (stage.JulyPrice > 0) iClosedAt = 19;
            if (stage.AugustPrice > 0) iClosedAt = 20;
            if (stage.SeptemberPrice > 0) iClosedAt = 21;
            if (stage.OctoberPrice > 0) iClosedAt = 23;
            if (stage.NovemberPrice > 0) iClosedAt = 24;
            if (stage.DecemberPrice > 0) iClosedAt = 25;

            return iClosedAt;
        }

        private int GetAccomplicePriceColumnIndex(YearPlanContractDto stage)
        {
            int iAccompliceClosedAt = 0;
            if (stage.PrevYearCoworkersPrice > 0) iAccompliceClosedAt = 10;
            if (stage.FirstQuarterAccomplice > 0) iAccompliceClosedAt = 14;
            if (stage.SecondQuarterAccomplice > 0) iAccompliceClosedAt = 18;
            if (stage.ThirdQuarterAccomplice > 0) iAccompliceClosedAt = 22;
            if (stage.FouthQuarterAccomplice > 0) iAccompliceClosedAt = 26;
            return iAccompliceClosedAt;
        }

        private void BuildLeafStage(YearPlanContractDto stage)
        {
            Range r = CopyRowRange(StageWorkSheet, 1);
            if (stage != null)
            {


                r.Replace("#DirectorsOnly#", stage.StageDirector);
                r.Replace("#Curator#", stage.StageCurator);
                r.Replace("#Directors#", stage.StageDirectors);
                r.Replace("#ContractState#", stage.ContractState.ToLower());
                r.Replace("#ResponsiblePeople#", stage.StageResponsiblePeople);

                //Ставим пробел, чтоб не менял на дату при экспорте (вводим 1.1 он меняет на 1 янв.)
                r.Replace("#StageNum#", " " + stage.StageNum);

                //StageName слишком большой и иногда выдает исключение
                InsertBigFieldIntoReport("#StageName#", stage.StageName, r);

                //Вставляем строку с форматированным числом (напр. 5 688,26)
                r.Replace("#StagePrice#", (stage.StagePrice).ToString(NumberFormat));

                // переходящие с предыдущего года и они же по соисполнителям
                r.Replace("#PrevYearPrice#", (stage.PrevYearPrice).ToString(NumberFormat));
                r.Replace("#PrevYearCoworkersPrice#", stage.PrevYearCoworkersPrice.ToString(NumberFormat));


                r.Replace("#JanuaryPrice#", (stage.JanuaryPrice).ToString(NumberFormat));
                r.Replace("#FebuaryPrice#", (stage.FebuaryPrice).ToString(NumberFormat));
                r.Replace("#MarchPrice#", (stage.MarchPrice).ToString(NumberFormat));
                r.Replace("#FirstAccoplicePrice#", (stage.FirstQuarterAccomplice).ToString(NumberFormat));


                r.Replace("#AprilPrice#", (stage.AprilPrice).ToString(NumberFormat));
                r.Replace("#MayPrice#", (stage.MayPrice).ToString(NumberFormat));
                r.Replace("#JunePrice#", (stage.JunePrice).ToString(NumberFormat));
                r.Replace("#SecondAccoplicePrice#", (stage.SecondQuarterAccomplice).ToString(NumberFormat));


                r.Replace("#JulyPrice#", (stage.JulyPrice).ToString(NumberFormat));
                r.Replace("#AugustPrice#", (stage.AugustPrice).ToString(NumberFormat));
                r.Replace("#SeptemberPrice#", (stage.SeptemberPrice).ToString(NumberFormat));
                r.Replace("#ThirdAccoplicePrice#", (stage.ThirdQuarterAccomplice).ToString(NumberFormat));
                r.Replace("#OctoberPrice#", (stage.OctoberPrice).ToString(NumberFormat));
                r.Replace("#NovemberPrice#", (stage.NovemberPrice).ToString(NumberFormat));
                r.Replace("#DecemberPrice#", (stage.DecemberPrice).ToString(NumberFormat));
                r.Replace("#FouthAccoplicePrice#", (stage.FouthQuarterAccomplice).ToString(NumberFormat));


                Quarters? q = stage.Stage.FinishingQuarter(Year);
                if (q != null)
                {
                    Yearreportcolor yc = Repository.Yearreportcolors.FirstOrDefault(x => x.UserQuarter == q);
                    if (yc != null)
                    {
                        var iClosedAt = GetPriceColumnIndex(stage);
                        if (iClosedAt > 0)
                        {
                            var range = r.Item[1, iClosedAt] as Range;
                            if (range != null)
                                range.Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.FromArgb(yc.WinColor.A, yc.WinColor.R, yc.WinColor.G, yc.WinColor.B));
                        }

                        iClosedAt = GetAccomplicePriceColumnIndex(stage);
                        if (iClosedAt > 0)
                        {
                            var range1 = r.Item[1, iClosedAt] as Range;
                            if (range1 != null)
                                range1.Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.FromArgb(yc.CoworkersWinColor.A, yc.CoworkersWinColor.R, yc.CoworkersWinColor.G, yc.CoworkersWinColor.B));
                        }
                    }
                    
                }
                var iRowCount = GetLongStringRowsCount(stage.StageName, defaultstagerowwidth, 4);
                r.RowHeight = DefaultRowHeight * iRowCount;
            }
        }
          



        private void BuildContractTail(IGrouping<Contractdoc, YearPlanContractDto> contract)
        {
            Range r=  CopyRowRange(ContractTailWorkSheet, 2);

            if (contract != null)
            {
                r.Replace("#ContractPrice#", contract.Sum(x=>x.StagePrice).ToString(NumberFormat));
                // переходящие с предыдущего года и они же по соисполнителям
                r.Replace("#PrevYearPrice#", contract.Sum(x => x.PrevYearPrice).ToString(NumberFormat));
                r.Replace("#PrevYearCoworkersPrice#", contract.Sum(x => x.PrevYearCoworkersPrice).ToString(NumberFormat));

                r.Replace("#JanuaryPrice#", contract.Sum(x=>x.JanuaryPrice).ToString(NumberFormat));
                r.Replace("#FebuaryPrice#", contract.Sum(x=>x.FebuaryPrice).ToString(NumberFormat));
                r.Replace("#MarchPrice#", contract.Sum(x=>x.MarchPrice).ToString(NumberFormat));
                r.Replace("#AprilPrice#", contract.Sum(x=>x.AprilPrice).ToString(NumberFormat));
                r.Replace("#MayPrice#", contract.Sum(x=>x.MayPrice).ToString(NumberFormat));
                r.Replace("#JunePrice#", contract.Sum(x=>x.JunePrice).ToString(NumberFormat));
                r.Replace("#JulyPrice#", contract.Sum(x=>x.JulyPrice).ToString(NumberFormat));
                r.Replace("#AugustPrice#", contract.Sum(x=>x.AugustPrice).ToString(NumberFormat));
                r.Replace("#SeptemberPrice#", contract.Sum(x=>x.SeptemberPrice).ToString(NumberFormat));
                r.Replace("#OctoberPrice#", contract.Sum(x=>x.OctoberPrice).ToString(NumberFormat));
                r.Replace("#NovemberPrice#", contract.Sum(x=>x.NovemberPrice).ToString(NumberFormat));
                r.Replace("#DecemberPrice#", contract.Sum(x=>x.DecemberPrice).ToString(NumberFormat));
                r.Replace("#FirstQuarterAccomplice#", contract.Sum(x=>x.FirstQuarterAccomplice).ToString(NumberFormat));
                r.Replace("#SecondQuarterAccomplice#", contract.Sum(x=>x.SecondQuarterAccomplice).ToString(NumberFormat));
                r.Replace("#ThirdQuarterAccomplice#", contract.Sum(x=>x.ThirdQuarterAccomplice).ToString(NumberFormat));
                r.Replace("#FouthQuarterAccomplice#", contract.Sum(x=>x.FouthQuarterAccomplice).ToString(NumberFormat));
            }
            r.Rows.AutoFit();
        }

        private void BuildContractTypeTail(IGrouping<Contracttype, YearPlanContractDto> contractType, Contractortype contractorType)
        {
            Worksheet contractTypeTailWorkSheet = GetWorkSheetByName("ContractTypeTail");
            Range r = CopyRowRange(contractTypeTailWorkSheet, 2);

            r.Replace("#ContractType#", contractType.Key.Name ?? "");
            r.Replace("#ContractorType#", NameInAblative(contractorType.Name) ?? "");

            r.Replace("#ContractPrice#", contractType.Sum(x => x.StagePrice).ToString(NumberFormat));

            // переходящие с предыдущего года и они же по соисполнителям
            r.Replace("#PrevYearPrice#", contractType.Sum(x => x.PrevYearPrice).ToString(NumberFormat));
            r.Replace("#PrevYearCoworkersPrice#", contractType.Sum(x => x.PrevYearCoworkersPrice).ToString(NumberFormat));

            r.Replace("#JanuaryPrice#", contractType.Sum(x => x.JanuaryPrice).ToString(NumberFormat));
            r.Replace("#FebuaryPrice#", contractType.Sum(x => x.FebuaryPrice).ToString(NumberFormat));
            r.Replace("#MarchPrice#", contractType.Sum(x => x.MarchPrice).ToString(NumberFormat));
            r.Replace("#AprilPrice#", contractType.Sum(x => x.AprilPrice).ToString(NumberFormat));
            r.Replace("#MayPrice#", contractType.Sum(x => x.MayPrice).ToString(NumberFormat));
            r.Replace("#JunePrice#", contractType.Sum(x => x.JunePrice).ToString(NumberFormat));
            r.Replace("#JulyPrice#", contractType.Sum(x => x.JulyPrice).ToString(NumberFormat));
            r.Replace("#AugustPrice#", contractType.Sum(x => x.AugustPrice).ToString(NumberFormat));
            r.Replace("#SeptemberPrice#", contractType.Sum(x => x.SeptemberPrice).ToString(NumberFormat));
            r.Replace("#OctoberPrice#", contractType.Sum(x => x.OctoberPrice).ToString(NumberFormat));
            r.Replace("#NovemberPrice#", contractType.Sum(x => x.NovemberPrice).ToString(NumberFormat));
            r.Replace("#DecemberPrice#", contractType.Sum(x => x.DecemberPrice).ToString(NumberFormat));
            r.Replace("#FirstQuarterAccomplice#", contractType.Sum(x => x.FirstQuarterAccomplice).ToString(NumberFormat));
            r.Replace("#SecondQuarterAccomplice#", contractType.Sum(x => x.SecondQuarterAccomplice).ToString(NumberFormat));
            r.Replace("#ThirdQuarterAccomplice#", contractType.Sum(x => x.ThirdQuarterAccomplice).ToString(NumberFormat));
            r.Replace("#FouthQuarterAccomplice#", contractType.Sum(x => x.FouthQuarterAccomplice).ToString(NumberFormat));

            r.Rows.AutoFit();
        }

        private void BuildContractorTypeTail(IGrouping<Contractortype, YearPlanContractDto> contractorType)
        {
            Worksheet contractorTypeTailWorkSheet = GetWorkSheetByName("ContractorTypeTail");
           
            Range r = CopyRowRange(contractorTypeTailWorkSheet, 2);

            r.Replace("#ContractorType#", NameInAblative(contractorType.Key.Name ?? ""));

            r.Replace("#ContractPrice#", contractorType.Sum(x => x.StagePrice).ToString(NumberFormat));

            // переходящие с предыдущего года и они же по соисполнителям
            r.Replace("#PrevYearPrice#", contractorType.Sum(x => x.PrevYearPrice).ToString(NumberFormat));
            r.Replace("#PrevYearCoworkersPrice#", contractorType.Sum(x => x.PrevYearCoworkersPrice).ToString(NumberFormat));

            r.Replace("#JanuaryPrice#", contractorType.Sum(x => x.JanuaryPrice).ToString(NumberFormat));
            r.Replace("#FebuaryPrice#", contractorType.Sum(x => x.FebuaryPrice).ToString(NumberFormat));
            r.Replace("#MarchPrice#", contractorType.Sum(x => x.MarchPrice).ToString(NumberFormat));
            r.Replace("#AprilPrice#", contractorType.Sum(x => x.AprilPrice).ToString(NumberFormat));
            r.Replace("#MayPrice#", contractorType.Sum(x => x.MayPrice).ToString(NumberFormat));
            r.Replace("#JunePrice#", contractorType.Sum(x => x.JunePrice).ToString(NumberFormat));
            r.Replace("#JulyPrice#", contractorType.Sum(x => x.JulyPrice).ToString(NumberFormat));
            r.Replace("#AugustPrice#", contractorType.Sum(x => x.AugustPrice).ToString(NumberFormat));
            r.Replace("#SeptemberPrice#", contractorType.Sum(x => x.SeptemberPrice).ToString(NumberFormat));
            r.Replace("#OctoberPrice#", contractorType.Sum(x => x.OctoberPrice).ToString(NumberFormat));
            r.Replace("#NovemberPrice#", contractorType.Sum(x => x.NovemberPrice).ToString(NumberFormat));
            r.Replace("#DecemberPrice#", contractorType.Sum(x => x.DecemberPrice).ToString(NumberFormat));
            r.Replace("#FirstQuarterAccomplice#", contractorType.Sum(x => x.FirstQuarterAccomplice).ToString(NumberFormat));
            r.Replace("#SecondQuarterAccomplice#", contractorType.Sum(x => x.SecondQuarterAccomplice).ToString(NumberFormat));
            r.Replace("#ThirdQuarterAccomplice#", contractorType.Sum(x => x.ThirdQuarterAccomplice).ToString(NumberFormat));
            r.Replace("#FouthQuarterAccomplice#", contractorType.Sum(x => x.FouthQuarterAccomplice).ToString(NumberFormat));
           
            r.Rows.AutoFit();
        }




        private void BuildStatisticsHeader()
        {
            Worksheet statisticsHeaderWorkSheet = GetWorkSheetByName("StatisticsHeader");

            Range r = CopyRowRange(statisticsHeaderWorkSheet, 1);
            r.Replace("#Year#", Year);
        }

        private void BuildStatisticsOverall(IEnumerable<YearPlanContractDto> yearPlanContractStages)
        {
            Worksheet statisticsOverallWorksheet = GetWorkSheetByName("StatisticsOverall");

            Range r = CopyRowRange(statisticsOverallWorksheet, 2);

            r.Replace("#StagePrice#", yearPlanContractStages.Sum(x => x.StagePrice).ToString(NumberFormat));
            r.Replace("#PrevYearPrice#", yearPlanContractStages.Sum(x => x.PrevYearPrice).ToString(NumberFormat));
            r.Replace("#PrevYearCoworkersPrice#", yearPlanContractStages.Sum(x => x.PrevYearCoworkersPrice).ToString(NumberFormat));
            
            r.Replace("#JanuaryPrice#", yearPlanContractStages.Sum(x => x.JanuaryPrice).ToString(NumberFormat));
            r.Replace("#FebuaryPrice#", yearPlanContractStages.Sum(x => x.FebuaryPrice).ToString(NumberFormat));
            r.Replace("#MarchPrice#", yearPlanContractStages.Sum(x => x.MarchPrice).ToString(NumberFormat));
            r.Replace("#AprilPrice#", yearPlanContractStages.Sum(x => x.AprilPrice).ToString(NumberFormat));
            r.Replace("#MayPrice#", yearPlanContractStages.Sum(x => x.MayPrice).ToString(NumberFormat));
            r.Replace("#JunePrice#", yearPlanContractStages.Sum(x => x.JunePrice).ToString(NumberFormat));
            r.Replace("#JulyPrice#", yearPlanContractStages.Sum(x => x.JulyPrice).ToString(NumberFormat));
            r.Replace("#AugustPrice#", yearPlanContractStages.Sum(x => x.AugustPrice).ToString(NumberFormat));
            r.Replace("#SeptemberPrice#", yearPlanContractStages.Sum(x => x.SeptemberPrice).ToString(NumberFormat));
            r.Replace("#OctoberPrice#", yearPlanContractStages.Sum(x => x.OctoberPrice).ToString(NumberFormat));
            r.Replace("#NovemberPrice#", yearPlanContractStages.Sum(x => x.NovemberPrice).ToString(NumberFormat));
            r.Replace("#DecemberPrice#", yearPlanContractStages.Sum(x => x.DecemberPrice).ToString(NumberFormat));

            r.Replace("#FirstAccoplicePrice#", yearPlanContractStages.Sum(x => x.FirstQuarterAccomplice).ToString(NumberFormat));
            r.Replace("#SecondAccoplicePrice#", yearPlanContractStages.Sum(x => x.FirstQuarterAccomplice).ToString(NumberFormat));
            r.Replace("#ThirdAccoplicePrice#", yearPlanContractStages.Sum(x => x.ThirdQuarterAccomplice).ToString(NumberFormat));
            r.Replace("#FouthAccoplicePrice#", yearPlanContractStages.Sum(x => x.FouthQuarterAccomplice).ToString(NumberFormat));

            
            r.Rows.AutoFit();    
        }
        
        private void BuildStatisticsForContractortype(int igroupnum, IGrouping<Contractortype, YearPlanContractDto> contractorType)
        {
            Worksheet statisticsContractortypeWorksheet = GetWorkSheetByName("StatisticsForContractortype");

            Range r = CopyRowRange(statisticsContractortypeWorksheet, 3);

            r.Replace("#ContractorTypeGroupNum#", igroupnum.ToString());
            r.Replace("#ContractorTypeGroup#", NameInAblative(contractorType.Key.Name ?? ""));

            r.Replace("#StagePrice#", contractorType.Sum(x => x.StagePrice).ToString(NumberFormat));
            r.Replace("#PrevYearPrice#", contractorType.Sum(x => x.PrevYearPrice).ToString(NumberFormat));
            r.Replace("#PrevYearCoworkersPrice#", contractorType.Sum(x => x.PrevYearCoworkersPrice).ToString(NumberFormat));

            r.Replace("#JanuaryPrice#", contractorType.Sum(x => x.JanuaryPrice).ToString(NumberFormat));
            r.Replace("#FebuaryPrice#", contractorType.Sum(x => x.FebuaryPrice).ToString(NumberFormat));
            r.Replace("#MarchPrice#", contractorType.Sum(x => x.MarchPrice).ToString(NumberFormat));
            r.Replace("#AprilPrice#", contractorType.Sum(x => x.AprilPrice).ToString(NumberFormat));
            r.Replace("#MayPrice#", contractorType.Sum(x => x.MayPrice).ToString(NumberFormat));
            r.Replace("#JunePrice#", contractorType.Sum(x => x.JunePrice).ToString(NumberFormat));
            r.Replace("#JulyPrice#", contractorType.Sum(x => x.JulyPrice).ToString(NumberFormat));
            r.Replace("#AugustPrice#", contractorType.Sum(x => x.AugustPrice).ToString(NumberFormat));
            r.Replace("#SeptemberPrice#", contractorType.Sum(x => x.SeptemberPrice).ToString(NumberFormat));
            r.Replace("#OctoberPrice#", contractorType.Sum(x => x.OctoberPrice).ToString(NumberFormat));
            r.Replace("#NovemberPrice#", contractorType.Sum(x => x.NovemberPrice).ToString(NumberFormat));
            r.Replace("#DecemberPrice#", contractorType.Sum(x => x.DecemberPrice).ToString(NumberFormat));
            r.Replace("#FirstAccoplicePrice#", contractorType.Sum(x => x.FirstQuarterAccomplice).ToString(NumberFormat));
            r.Replace("#SecondAccoplicePrice#", contractorType.Sum(x => x.SecondQuarterAccomplice).ToString(NumberFormat));
            r.Replace("#ThirdAccoplicePrice#", contractorType.Sum(x => x.ThirdQuarterAccomplice).ToString(NumberFormat));
            r.Replace("#FouthAccoplicePrice#", contractorType.Sum(x => x.FouthQuarterAccomplice).ToString(NumberFormat));

            r.Rows.AutoFit();
        }

        private void BuildStatisticsForContracttype(int imaingroupnum, int igroupnum, IGrouping<Contracttype, YearPlanContractDto> contractType, string contractortypename)
        {
            Worksheet statisticsContracttypeWorksheet = GetWorkSheetByName("StatisticsForContracttype");

            Range r = CopyRowRange(statisticsContracttypeWorksheet, 3);
            
            // #ContractorTypeGroupNum#.#ContractTypeGroupnum#. #ContractTypeGroup# по договорам с #ContractorTypeGroup#, в т.ч.		

		

            r.Replace("#ContractorTypeGroupNum#", imaingroupnum.ToString());
            r.Replace("#ContractTypeGroupNum#", igroupnum.ToString());
            r.Replace("#ContractTypeGroup#", contractType.Key.Name);
            r.Replace("#ContractorTypeGroup#", NameInAblative(contractortypename ?? ""));


            r.Replace("#StagePrice#", contractType.Sum(x => x.StagePrice).ToString(NumberFormat));
            r.Replace("#PrevYearPrice#", contractType.Sum(x => x.PrevYearPrice).ToString(NumberFormat));
            r.Replace("#PrevYearCoworkersPrice#", contractType.Sum(x => x.PrevYearCoworkersPrice).ToString(NumberFormat));

            r.Replace("#JanuaryPrice#", contractType.Sum(x => x.JanuaryPrice).ToString(NumberFormat));
            r.Replace("#FebuaryPrice#", contractType.Sum(x => x.FebuaryPrice).ToString(NumberFormat));
            r.Replace("#MarchPrice#", contractType.Sum(x => x.MarchPrice).ToString(NumberFormat));
            r.Replace("#AprilPrice#", contractType.Sum(x => x.AprilPrice).ToString(NumberFormat));
            r.Replace("#MayPrice#", contractType.Sum(x => x.MayPrice).ToString(NumberFormat));
            r.Replace("#JunePrice#", contractType.Sum(x => x.JunePrice).ToString(NumberFormat));
            r.Replace("#JulyPrice#", contractType.Sum(x => x.JulyPrice).ToString(NumberFormat));
            r.Replace("#AugustPrice#", contractType.Sum(x => x.AugustPrice).ToString(NumberFormat));
            r.Replace("#SeptemberPrice#", contractType.Sum(x => x.SeptemberPrice).ToString(NumberFormat));
            r.Replace("#OctoberPrice#", contractType.Sum(x => x.OctoberPrice).ToString(NumberFormat));
            r.Replace("#NovemberPrice#", contractType.Sum(x => x.NovemberPrice).ToString(NumberFormat));
            r.Replace("#DecemberPrice#", contractType.Sum(x => x.DecemberPrice).ToString(NumberFormat));
            r.Replace("#FirstAccoplicePrice#", contractType.Sum(x => x.FirstQuarterAccomplice).ToString(NumberFormat));
            r.Replace("#SecondAccoplicePrice#", contractType.Sum(x => x.SecondQuarterAccomplice).ToString(NumberFormat));
            r.Replace("#ThirdAccoplicePrice#", contractType.Sum(x => x.ThirdQuarterAccomplice).ToString(NumberFormat));
            r.Replace("#FouthAccoplicePrice#", contractType.Sum(x => x.FouthQuarterAccomplice).ToString(NumberFormat));


            r.Rows.AutoFit();
        }

        protected override void SetHeader()
        {
            MainWorkSheet.Cells.Replace("#Year#", Year);
        }

        public override string Detail
        {
            get { return "Contract"; }
        }

        public Worksheet GetWorkSheetByName(string workSheetName)
        {
            return Excel.Worksheets[workSheetName];
        }

        public Worksheet BaseStageWorkSheet
        {
            get { return Excel.Worksheets["BaseStage"]; }
        }

        public Worksheet StageWorkSheet
        {
            get { return Excel.Worksheets["Detail"]; }
        }

        private static Worksheet ContractTailWorkSheet
        {
            get { return Excel.Worksheets["ContractTail"]; }
        }
    }

  
}
