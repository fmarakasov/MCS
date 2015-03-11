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
using UOW;

namespace McReports.ViewModel
{
    public class HandingWorkReportViewModel : ExcelReportViewModel
    {


        public override int StartPosition
        {
            get { return 4; }
        }

        protected override string MinExcelColumn
        {
            get { return "A"; }
        }

        protected virtual string MaxExcelColumn
        {
            get { return "I"; }
        }


         public HandingWorkReportViewModel(IContractRepository contractRepository)
            : base(contractRepository)
        {         
            Year = DateTime.Now.Year;
        }

         protected override void OnPeriodChanged()
         {
             Year = Period.End.Year;
             base.OnPeriodChanged();
         }

         

         public Quarters CurrentQuarter { get; set; }


         public override string DisplayName
         {
             get
             {
                 return "Справка о сдаче работ";
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
                 return string.Format("Справка о сдаче работ за {0} год", Year.ToString());
                 
             }
         }


        [InputParameter(typeof(int), "Год")]
        public int Year { get; set; }

        protected override DateTime StartPeriod
        {
            get { return DateTimeExtensions.GetFirstQuarterDay(Year, (int) CurrentQuarter); }
        }

        protected override DateTime EndPeriod
        {
            get { return DateTimeExtensions.GetLastQuarterDay(Year, (int) CurrentQuarter); }
        }

        private static Worksheet ContractTypeTailWorkSheet
        {
            get { return Excel.Worksheets["ContractTypeTail"]; }
        }

        private static Worksheet ConclusionWorkSheet
        {
            get { return Excel.Worksheets["ConclusionTail"]; }
        }

        private List<HandingWorkDTO> _contracts
        {
            get
            {
                DateTime startQuarter = DateTimeExtensions.GetFirstQuarterDay(Year, (int)CurrentQuarter);
                DateTime endQuarter = DateTimeExtensions.GetLastQuarterDay(Year, (int)CurrentQuarter);

                IEnumerable<Contractdoc> contractdocs = ActiveContractDocs;
                if (contractdocs != null && contractdocs.Any())
                {
                    var e =
                           contractdocs.Where(cd => cd.Stages.Any()).Aggregate(new List<HandingWorkDTO>(), (a, c) =>
                           {
                               a.AddRange(c.Stages.Where(st => st.IsLeaf()&&st.GetConditionOnDate(CurrentDate) == StageCondition.Closed&&st.Act.Signdate.HasValue&&st.Act.Signdate.Value.Between(startQuarter, endQuarter)).Distinct().Select(stage =>
                                                          {
                                                             return
                                                                new HandingWorkDTO()
                                                                  {
                                                                     Year = Year,
                                                                     QuarterInt = (int)CurrentQuarter,
                                                                     Quarter = CurrentQuarter,
                                                                     Stage = stage,
                                                                     ContractorType = (c.Contractor != null && c.Contractor.Contractortype != null)? c.Contractor.Contractortype :
                                                                     UnitOfWork.Repository<Contractortype>().ToList().FirstOrDefault(x=>x.WellKnownType == WellKnownContractorTypes.Other),


                                                                     ContractType = c.Contracttype ?? UnitOfWork.Repository<Contracttype>().ToList().FirstOrDefault(x=>x.WellKnownType == WellKnownContractTypes.Undefined),
                                                                     StageNum = stage.Num ?? "",
                                                                     Price = stage.StageMoneyModel.National.Factor.WithNdsValue - 
                                                                                (stage.SubContractsStages == null ? 0 : (stage.SubContractsStages.Sum(scs => scs.PriceOnPeriodWithNDS(StartPeriod, EndPeriod)))),

                                                                     AccompilePrice = stage.SubContractsStages == null ? 0 : 
                                                                        (stage.SubContractsStages.Sum(scs => 
                                                                         scs.PriceOnPeriodWithNDS(StartPeriod, EndPeriod))),

                                                                     FullPrice = stage.StageMoneyModel.National.Factor.WithNdsValue,
                                                                     ContractNum = c.Num ?? string.Empty,
                                                                     ActName = stage.Act == null ? string.Empty : stage.Act.ToString(),
                                                                     Actnum = stage.Act != null ? stage.Act.Num : "",
                                                                     ActSignDate = stage.Act == null? null: stage.Act.Signdate
                                                                 };
                                                          }));
                               return a;
                           });

                    return e;
                }
                return null; 
            }
        }


        protected override string ReportFileName
        {
            get { return "HandingWorkReport_6"; }
        }


        public override string MainWorkSheetName
        {
            get { return "Подписано в " + ((int)CurrentQuarter).ToString() + " кв"; }
        }

        protected override void BuildReport()
        {
            
            for (int i = 1; i <= 4; i++)
            {
                CurrentButtomPosition = StartPosition;
                CurrentQuarter = (Quarters)i;

                SetHeader();

                //При создании этой колеекции сразу выставляем параменры по умолчанию. Если не задан Contractor выставляем в Другие, если не задан ContractorType, задаем Прочие (чтоб все они попали в одну группу и не выбрасовала исключение)
                IEnumerable<HandingWorkDTO> contractStages = _contracts;

                //Группировка по типу контрагента
                IEnumerable<IGrouping<Contractortype, HandingWorkDTO>> contractorTypeGroups =
                    contractStages.GroupBy(x => x.ContractorType, x => x).OrderBy(x => x.Key.Reportorder);
                foreach (IGrouping<Contractortype, HandingWorkDTO> contractorGroup in contractorTypeGroups)
                {
                    //Группировка по типу договора
                    IEnumerable<IGrouping<Contracttype, HandingWorkDTO>> contractTypeGroups =
                        contractorGroup.GroupBy(x => x.ContractType, x => x).OrderBy(x => x.Key.Reportorder);
                    foreach (IGrouping<Contracttype, HandingWorkDTO> contractTypeGroup in contractTypeGroups)
                    {
                        BuildContractType(contractorGroup.Key, contractTypeGroup.Key);
                        int rownum = 1;
                        IEnumerable<IGrouping<string, HandingWorkDTO>> contractContractGroups =
                            contractTypeGroup.GroupBy(x => x.ContractNum, x => x).OrderBy(x => x.Key);
                        foreach (IGrouping<string, HandingWorkDTO> contractGroup in contractContractGroups)
                        {
                            IEnumerable<IGrouping<string, HandingWorkDTO>> contractScheduleGroups =
                                        contractGroup.GroupBy(x => x.ScheduleAppNum, x => x).OrderBy(x => x.Key);

                            foreach (var contractScheduleGroup in contractScheduleGroups)
                            {
                                foreach (HandingWorkDTO stage in contractScheduleGroup.OrderBy(c => c, new CompareHandingWorkDTO()))
                                {
                                    BuildDetail(stage, rownum);
                                    rownum++;
                                }   
                            }
                        }

                        BuildContractTypeTail(contractTypeGroup, contractorGroup.Key);
                    }


                }
                BuildConclusionTail(contractStages);
                SetPrintingArea(ActiveWorksheet);
            }

            CurrentQuarter = (Quarters) 1;
            MainWorkSheet.Activate();
        }

        protected override void SetHeader()
        {
            MainWorkSheet.Cells.Replace("#Year#", Year);
            MainWorkSheet.Cells.Replace("#Quarter#", ((int)CurrentQuarter).ToString());
            MainWorkSheet.Cells.Replace("#Today#", CurrentDate.ToString("dd.MM.yyyy"));
            MainWorkSheet.Activate();
         
        }
        
        private void BuildContractType(Contractortype ContractorType, Contracttype ContractType)
        {
            Range r = CopyRowRange(ContractTypeWorkSheet, 1);
            r.Replace("#ContractType#", ContractType.Name ?? "");
            r.Replace("#ContractorType#", NameInAblative(ContractorType.Name) ?? "");
            r.Rows.AutoFit();
        }

        private void BuildDetail(HandingWorkDTO stage, int num)
        {
            Range r = CopyRowRange(DetailWorkSheet, 1);
            r.Replace("#Num#", num.ToString());
            r.Replace("#ContractNum#", stage.ContractNum);
            r.Replace("#StageNum#", stage.StageNum);
            r.Replace("#FullPrice#", stage.FullPrice.ToString(NumberFormat));
            r.Replace("#AccompilePrice#", stage.AccompilePrice.ToString(NumberFormat));
            r.Replace("#Price#", stage.Price.ToString(NumberFormat));
            r.Replace("#Contractor#",
                      stage.Stage.SubContractsStages.SelectMany(s => s.Schedule.Schedulecontracts)
                           .Where(s => s.Contractdoc.Contractor != null)
                           .Select(s => s.Contractdoc.Contractor)
                           .Aggregate("", (s, next) => s + next.ToString() + "\n"));
            r.Replace("#ActNum#", stage.Actnum);
            r.Replace("#ActDate#", stage.ActSignDate.HasValue ? stage.ActSignDate.Value.ToString("dd.MM.yyyy") : "");

            r.Rows.AutoFit();
        }

       private void BuildContractTypeTail(IGrouping<Contracttype, HandingWorkDTO> contractTypeGroup, Contractortype contractorType)
        {
            Range r = CopyRowRange(ContractTypeTailWorkSheet, 1);
            r.Replace("#ContractType#", contractTypeGroup.Key.Name ?? "");
            r.Replace("#ContractorType#", contractorType.Name ?? "");

            r.Replace("#FullPrice#", contractTypeGroup.Sum(x => x.FullPrice).ToString(NumberFormat));
            r.Replace("#AccompilePrice#", contractTypeGroup.Sum(x => x.AccompilePrice).ToString(NumberFormat));
            r.Replace("#Price#", contractTypeGroup.Sum(x => x.Price).ToString(NumberFormat));
            r.Rows.AutoFit();
        }

        private void BuildConclusionTail(IEnumerable<HandingWorkDTO> contractStages)
        {
            Range r = CopyRowRange(ConclusionWorkSheet, 1);

            r.Replace("#Quarter#", ((int)CurrentQuarter).ToString());

            if (contractStages != null)
            {
                r.Replace("#FullPrice#", contractStages.Sum(x => x.FullPrice).ToString(NumberFormat));
                r.Replace("#AccompilePrice#", contractStages.Sum(x => x.AccompilePrice).ToString(NumberFormat));
                r.Replace("#Price#", contractStages.Sum(x => x.Price).ToString(NumberFormat));
            }
            r.Rows.AutoFit();


        }
     
    }
}
