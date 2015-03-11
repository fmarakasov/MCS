using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
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
    public class EfficientInformationReportViewModel : ExcelReportViewModel
    {

        public EfficientInformationReportViewModel(IContractRepository contractRepository)
            : base(contractRepository)
        {
            Year = DateTime.Now.Year;
        }
        
        [InputParameter(typeof(int), "Год")]
        public int Year { get; set; }

        protected static Worksheet ContractTypeTailWorkSheet
        {
            get { return Excel.Worksheets["ContractTypeTail"]; }
        }

        protected static Worksheet ContractorTypeTailWorkSheet
        {
            get { return Excel.Worksheets["ContractorTypeTail"]; }
        }

        protected static Worksheet ConclusionWorkSheet
        {
            get { return Excel.Worksheets["ConclusionTail"]; }
        }

        protected override string ReportFileName
        {
            get { return "YearEfficientInformationReport_7"; }
        }

        protected override DateTime StartPeriod { get { return DateTimeExtensions.GetFirstYearDay(Year); } }
        protected override DateTime EndPeriod { get { return DateTimeExtensions.GetLastYearDay(Year); } } 

        public override string DisplayName
        {
            get
            {
                return "Оперативные сведения о выполнении работ";

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
                return string.Format("Оперативные сведения о выполнении работ за {0} год", Year.ToString());
            }
        }

        /// <summary>
        ///
        /// </summary>
        private List<EfficientInformationDTO> _contracts
        {
            get
            {
              var contractdocs = GetActiveContractDocs().Where(cd => cd.HasSchedule && cd.Stages != null && cd.Stages.Any());

                if (contractdocs.Any())
                {
                    var e =
                           contractdocs.Aggregate(new List<EfficientInformationDTO>(), (a, b) =>
                           {
                               a.AddRange(b.Stages.Where(st => (st.Endsat.HasValue && st.Endsat.Value >= StartPeriod && st.Endsat.Value <= EndPeriod) ||
                                             //Работы не завершены  к заданному периоду
                                         (st.Endsat.HasValue && st.Endsat.Value <= StartPeriod && (st.Act == null || !st.Act.Signdate.HasValue || st.Act.Signdate.Value >= StartPeriod))).
                                         Distinct().Select(stage =>
                                         {
                                             return
                                                 new EfficientInformationDTO()
                                                 {
                                                     Year = Year,

                                                     ContractorType = (b.Contractor != null && b.Contractor.Contractortype != null) ? b.Contractor.Contractortype : 
                                                                       UnitOfWork.Repository<Contractortype>().AsQueryable()
                                                                       .FirstOrDefault(x=>x.WellKnownType == WellKnownContractorTypes.Other),

                                                     ContractType = b.Contracttype ?? UnitOfWork.Repository<Contracttype>().AsQueryable().FirstOrDefault(x=>x.WellKnownType== WellKnownContractTypes.Undefined),

                                                     
                                                     ContractNum = b.Num,

                                                     Customer = b.FunctionalCustomers,

                                                     NDS = b.ContractMoney.NdsValue,

                                                     Price = b.GetPriceForPeriodWithNDS(StartPeriod, EndPeriod, Measure),
                                                     
                                                     AccompilePrice = (stage.SubContractsStages != null && stage.SubContractsStages.Any()) ? 
                                                                      (stage.SubContractsStages.Sum(scs => scs.PriceOnPeriodWithNDS(StartPeriod, EndPeriod,Measure))):0,

                                                     ContractorTypeName = b.ContractorTypeName,

                                                     ContractTypeName = b.ContractTypeName,
                                                     
                                                 };
                                         }));
                               return a;
                           });

                    return e;
                }
                return null;
            }
        }

        private IEnumerable<Contractdoc> GetActiveContractDocs()
        {
            return UnitOfWork.Repository<Contractdoc>().AsQueryable().Where(
                //Необходимо, чтоб договор имел календарный план с запланированными этапами и был генеральным
                x => x.HasSchedule && x.Stages != null && x.Stages.Any() && x.IsGeneral).Select(x => x.Actual).Distinct().Where(x =>
                (
                    // состояние хотя бы одного из этапа было или неопределенное или подлежит сдаче или просроченное и договор был подписан
                               x.Stages.FirstOrDefault(st =>
                                   st.GetConditionForPeriod(StartPeriod, EndPeriod) == StageCondition.Undefined ||
                                   st.GetConditionForPeriod(StartPeriod, EndPeriod) == StageCondition.HaveToEnd ||
                                   st.GetConditionForPeriod(StartPeriod, EndPeriod) == StageCondition.Overdue) != null));
        }

        protected override void BuildReport()
        {
            CurrentButtomPosition = 5;
            SetHeader();

            //При создании этой колеекции сразу выставляем параменры по умолчанию. Если не задан Contractor выставляем в Другие, если не задан ContractorType, задаем Прочие (чтоб все они попали в одну группу и не выбрасовала исключение)
            IEnumerable<EfficientInformationDTO> contractStages = _contracts;

            //Группировка по типу контрагента
            IEnumerable<IGrouping<Contractortype, EfficientInformationDTO>> contractorTypeGroups = contractStages.GroupBy(x => x.ContractorType, x => x).OrderBy(x => x.Key.Reportorder);
            foreach (IGrouping<Contractortype, EfficientInformationDTO> contractorGroup in contractorTypeGroups)
            {
                BuildContractorType(contractorGroup.Key);

                //Группировка по типу договора
                IEnumerable<IGrouping<Contracttype, EfficientInformationDTO>> contractTypeGroups = contractorGroup.GroupBy(x => x.ContractType, x => x).OrderBy(x => x.Key.Reportorder);
                foreach (IGrouping<Contracttype, EfficientInformationDTO> contractTypeGroup in contractTypeGroups)
                {
                    BuildContractType(contractTypeGroup.Key);
                    int rownum = 1;
                    foreach (EfficientInformationDTO contract in contractTypeGroup)
                    {
                        BuildDetail(contract, rownum);
                        rownum++;
                    }
                    BuildContractTypeTail(contractTypeGroup, contractorGroup.Key);
               }

                BuildContractorTypeTail(contractorGroup);
            }
            BuildConclusionTail(contractStages);
        }

        protected override void SetHeader()
        {
            MainWorkSheet.Cells.Replace("#Period#", "");
            MainWorkSheet.Cells.Replace("#Year#", Year);
        }  
        
        protected virtual void BuildContractorType(Contractortype contractortype)
        {
            Range r = CopyRowRange(ContractTypeWorkSheet, 1);
            r.Replace("#ContractorType#", contractortype.Name ?? "");
            
            r.Rows.AutoFit();
        }

        protected void BuildContractType(Contracttype contractType)
        {
            Range r = CopyRowRange(ContractTypeWorkSheet, 1);
            r.Replace("#ContractType#", contractType.Name ?? "");
            r.Rows.AutoFit();
        }

        protected void BuildDetail(EfficientInformationDTO contract, int num)
        {
            Range r = CopyRowRange(DetailWorkSheet, 1);
            r.Replace("#Num#", num.ToString());
            r.Replace("#ContractNum#", contract.ContractNum);
            r.Replace("#Customer#", contract.Customer);
            r.Replace("#FullPrice#", contract.FullPrice.ToString(NumberFormat));
            r.Replace("#NDS#", contract.NDS.ToString(NumberFormat));
            r.Replace("#Price#", contract.Price.ToString(NumberFormat));
            r.Replace("#AccompilePrice#", contract.AccompilePrice.ToString(NumberFormat));
            r.Rows.AutoFit();
        }

        protected void BuildContractTypeTail(IGrouping<Contracttype, EfficientInformationDTO> contractTypeGroup, Contractortype contractorType)
        {
            Range r = CopyRowRange(ContractTypeTailWorkSheet, 1);

            r.Replace("#ContractType#", contractTypeGroup.Key.Name ?? "");
            r.Replace("#ContractorType#", NameInAblative(contractorType.Name) ?? "");

            r.Replace("#FullPrice#", contractTypeGroup.Sum(x => x.FullPrice).ToString(NumberFormat));
            r.Replace("#NDS#", contractTypeGroup.Sum(x => x.NDS).ToString(NumberFormat));
            r.Replace("#AccompilePrice#", contractTypeGroup.Sum(x => x.AccompilePrice).ToString(NumberFormat));
            r.Replace("#Price#", contractTypeGroup.Sum(x => x.Price).ToString(NumberFormat));
            r.Rows.AutoFit();
        }

        protected void BuildContractorTypeTail(IGrouping<Contractortype, EfficientInformationDTO> contractorGroup)
        {
            Range r = CopyRowRange(ContractorTypeTailWorkSheet, 1);

            r.Replace("#ContractorType#", contractorGroup.Key.Name ?? "");
            r.Replace("#FullPrice#", contractorGroup.Sum(x => x.FullPrice).ToString(NumberFormat));
            r.Replace("#NDS#", contractorGroup.Sum(x => x.NDS).ToString(NumberFormat));
            r.Replace("#AccompilePrice#", contractorGroup.Sum(x => x.AccompilePrice).ToString(NumberFormat));
            r.Replace("#Price#", contractorGroup.Sum(x => x.Price).ToString(NumberFormat));
            r.Rows.AutoFit();
        }

        protected void BuildConclusionTail(IEnumerable<EfficientInformationDTO> contracts)






















        {
            Range r = CopyRowRange(ConclusionWorkSheet, 1);
            if (contracts != null)
            {
                r.Replace("#FullPrice#", contracts.Sum(x => x.FullPrice).ToString(NumberFormat));
                r.Replace("#NDS#", contracts.Sum(x => x.NDS).ToString(NumberFormat));
                r.Replace("#AccompilePrice#", contracts.Sum(x => x.AccompilePrice).ToString(NumberFormat));
                r.Replace("#Price#", contracts.Sum(x => x.Price).ToString(NumberFormat));
            }
            r.Rows.AutoFit();
        }

       
    }


    public class EfficientInformationQuarterReport_7_ViewModel : EfficientInformationReportViewModel
    {
        public EfficientInformationQuarterReport_7_ViewModel(IContractRepository contractRepository) : base(contractRepository)
        {
            Quarter = Quarters.Первый;
        }

        [InputParameter(typeof(Quarters), "Квартал")]
        public Quarters Quarter { get; set; }

        protected override DateTime StartPeriod
        {
            get { return DateTimeExtensions.GetFirstQuarterDay(Year, (int)Quarter); }
        }

        protected override DateTime EndPeriod
        {
            get
            {
                return DateTimeExtensions.GetLastQuarterDay(Year, (int)Quarter);
            }
        }

        protected override void SetHeader()
        {
            MainWorkSheet.Cells.Replace("#Period#", Quarter.Description() + "квартал ");
            MainWorkSheet.Cells.Replace("#Year#", Year);
        }
    }

  

    public class EfficientInformationMonthReport_7_ViewModel : EfficientInformationReportViewModel
    {
        public EfficientInformationMonthReport_7_ViewModel(IContractRepository contractRepository)
            : base(contractRepository)
        {
            Month = Months.January;
        }

        [InputParameter(typeof(Months), "Месяц")]
        public Months Month { get; set; }

        protected override DateTime StartPeriod
        {
            get { return DateTimeExtensions.GetFirstMonthDay(Year, (int)Month); }
        }

        protected override DateTime EndPeriod
        {
            get
            {
                return DateTimeExtensions.GetLastMonthDay(Year, (int)Month);
            }
        }

        protected override void SetHeader()
        {
            MainWorkSheet.Cells.Replace("#Period#", Month.Description() + " ");
            MainWorkSheet.Cells.Replace("#Year#", Year);
        }
    }
}

