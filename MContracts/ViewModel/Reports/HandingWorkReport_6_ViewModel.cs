using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MCDomain.DataAccess;
using MCDomain.Model;
using MContracts.Classes;
using MContracts.DTO;

using Microsoft.Office.Interop.Excel;
using MCDomain.Common;

namespace MContracts.ViewModel.Reports
{
    class HandingWorkReport_6_ViewModel : ExcelReportViewModel
    {
         public HandingWorkReport_6_ViewModel(IContractRepository contractRepository)
            : base(contractRepository)
        {
            PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler(HandingWorkReport_6_ViewModel_PropertyChanged);
            Year = DateTime.Now.Year;
        }

         void HandingWorkReport_6_ViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
         {
             if (e.PropertyName == "MainViewModel")
             {
                 if (MainViewModel != null)
                     Year = MainViewModel.SelectedFilterEndDate.Year;
                 else
                     Year = DateTime.Today.Year;
             }
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
                if (contractdocs != null && contractdocs.Count() > 0)
                {
                    var e =
                           contractdocs.Where(cd => cd.Stages.Count() > 0&&cd.Stages.Count(s => (s.Act == null||s.Act.ContractObject == cd))>0).Aggregate(new List<HandingWorkDTO>(), (a, c) =>
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
                                                                     Repository.Contractortypes.FirstOrDefault(x=>x.WellKnownType == WellKnownContractorTypes.Other),


                                                                     ContractType = c.Contracttype ?? Repository.Contracttypes.FirstOrDefault(x=>x.WellKnownType == WellKnownContractTypes.Other),
                                                                     StageNum = stage.Num ?? "",
                                                                     Price = stage.StageMoneyModel.National.Factor.WithNdsValue - 
                                                                                (stage.SubContractsStages == null ? 0 : (stage.SubContractsStages.Sum(scs => scs.PriceOnPeriodWithNDS(StartPeriod, EndPeriod)))),

                                                                     AccompilePrice = stage.SubContractsStages == null ? 0 : 
                                                                        (stage.SubContractsStages.Sum(scs => 
                                                                         scs.PriceOnPeriodWithNDS(StartPeriod, EndPeriod))),

                                                                     FullPrice = stage.StageMoneyModel.National.Factor.WithNdsValue,
                                                                     ContractNum = c.Internalnum ?? string.Empty,
                                                                     ActName = stage.Act == null ? string.Empty : stage.Act.ToString(),
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


        public override string ReportFileName
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
                CurrentButtomPosition = 4;
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
                        foreach (HandingWorkDTO stage in contractTypeGroup.OrderBy(c => c, new CompareHandingWorkDTO()))
                        {
                            BuildDetail(stage, rownum);
                            rownum++;
                        }

                        BuildContractTypeTail(contractTypeGroup, contractorGroup.Key);
                    }


                }
                BuildConclusionTail(contractStages);

            }
        }

        protected override void SetHeader()
        {
            MainWorkSheet.Cells.Replace("#Year#", Year);
            MainWorkSheet.Cells.Replace("#Quarter#", CurrentQuarter);
            MainWorkSheet.Cells.Replace("#Today#", CurrentDate.ToString("dd.MM.yyyy"));
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
            r.Replace("#ActName#", stage.ActName);

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

            r.Replace("#Quarter#", DateTimeExtensions.QuarterToRoman(CurrentQuarter));

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
