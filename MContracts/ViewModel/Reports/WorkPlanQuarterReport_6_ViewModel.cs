using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MCDomain.Common;
using MCDomain.DataAccess;
using MCDomain.Model;
using MContracts.Classes;


namespace MContracts.ViewModel.Reports
{
    class WorkPlanQuarterReport_6_ViewModel : ExcelReportViewModel
    {
        public WorkPlanQuarterReport_6_ViewModel(IContractRepository contractRepository)
            : base(contractRepository)
        {
            Year = DateTime.Now.Year;
            Quarter = Quarters.Первый;
        }

        [InputParameter(typeof(int), "Год")]
        public int Year { get; set; }

        [InputParameter(typeof(Quarters), "Квартал")]
        public Quarters Quarter { get; set; }


        public override string DisplayName
        {
            get
            {
                return "План работ по видим работ на Х квартал";
            }
            protected set
            {
                base.DisplayName = value;
            }
        }

        //private List<QuarterPlanContractDto> _contracts
        //{
        //    get
        //    {
        //        var contractdocs = GetActiveContractDocs().Where(cd => cd.Schedulecontracts != null && cd.Schedulecontracts.Count == 1 && cd.Schedulecontracts.First().Schedule != null && cd.Schedulecontracts.First().Schedule.Stages != null && cd.Schedulecontracts.First().Schedule.Stages.Count > 0);
        //        // contractdocs = null;
        //        if (contractdocs != null && contractdocs.Count() > 0)
        //        {
        //            var e =
        //                   contractdocs.Aggregate(new List<QuarterPlanContractDto>(), (a, b) =>
        //                   {
        //                       a.AddRange(b.Schedulecontracts.First().Schedule.Stages.Select(x =>
        //                       {
        //                           return
        //                               new QuarterPlanContractDto()
        //                               {
        //                                   Year = Year,

        //                                   Quarter = (int)Quarter,

        //                                   Directors = b.Department != null ? (b.Department.Director != null ? b.Department.Director.GetShortFullName() : "") + "\n" + (b.Department.Manager != null ? b.Department.Manager.GetShortFullName() : "") : "",

        //                                   ResponsiblePeople = b.Responsibles.Count != 0 ? b.Responsibles.Aggregate(string.Empty, (r, next) => r + next.Employee.GetShortFullName() + " \n") : "",

        //                                   ContractState = b.Contractstate != null ? b.Contractstate.Name : "",

        //                                   StageNum = x.Num ?? "",

        //                                   StageName = x.Subject ?? "",

        //                                   StagePrice = x.PriceValue,

        //                                   TransitionPrice = x.GetConditionOnDate(DateTimeExtensions.GetLastQuarterDay(Year, (int)Quarter)) == StageCondition.Closed ? PriceOnPreviousQuarters(x, Year, (int)Quarter) : 0,

        //                                   TransitionAccomplicePrice = x.SubContractsStages != null ? (x.SubContractsStages.Where(scs => scs.GetConditionOnDate(DateTimeExtensions.GetLastQuarterDay(Year, (int)Quarter)) == StageCondition.Closed).Sum(scs => PriceOnPreviousQuarters(scs, Year, (int)Quarter))) : 0,

        //                                   Month1Price = x.GetConditionOnDate(DateTimeExtensions.GetLastQuarterDay(Year, (int)Quarter)) == StageCondition.Closed ? x.PriceOnMonth(Year, (int)Quarter * 3 - 2) : 0,

        //                                   Month2Price = x.GetConditionOnDate(DateTimeExtensions.GetLastQuarterDay(Year, (int)Quarter)) == StageCondition.Closed ? x.PriceOnMonth(Year, (int)Quarter * 3 - 1) : 0,

        //                                   Month3Price = x.GetConditionOnDate(DateTimeExtensions.GetLastQuarterDay(Year, (int)Quarter)) == StageCondition.Closed ? x.PriceOnMonth(Year, (int)Quarter * 3) : 0,

        //                                   AccompilePrice = x.SubContractsStages != null ? (x.SubContractsStages.Where(scs => scs.GetConditionOnDate(DateTimeExtensions.GetLastQuarterDay(Year, (int)Quarter)) == StageCondition.Closed).Sum(scs => scs.PriceOnQuarter(Year, (int)Quarter))) : 0,

        //                                   ActStatus = x.Act != null ? x.Act.Status ?? 0 : 0,

        //                                   ContractorType = b.Contractor != null ? b.Contractor.Name : "",

        //                                   ContractType = b.Contracttype != null ? b.Contracttype.Name : "",

        //                                   ContractNumSubject = (b.Internalnum != null ? "Договор № " + b.Internalnum + "\"" : "") + (b.Subject != null ? b.Subject + "\"" : "") + (b.Internalnum != null || b.Subject != null ? "\n" : "") +
        //                                                         (b.Approvedat != null ? "Утвержден " + b.Approvedat.Value.ToString("dd.MM.yyyy") + "\n" : "") +
        //                                                        (b.Contractor != null ? "Заказчик - " + b.Contractor.Name + ", \n" : "") +
        //                                                         (b.Functionalcustomercontracts != null && b.Schedulecontracts.Count > 0 ? "Функциональный заказчик - " + b.Functionalcustomercontracts.Aggregate(string.Empty, (fc, next) => fc + next.Functionalcustomer.Name + "\n") : ""),

        //                               };
        //                       }));
        //                       return a;
        //                   });

        //            return e;
        //        }
        //        return null;
        //    }
        //}



        /// <summary>
        /// В список договоров входят
        /// 
        ///a) договоры, по которым, запланированы работы на
        ///   текущий год; -  ContractCondition.NormalActive
        ///
        /// b) договоры, работы по которым были запланированы на предыдущие периоды, 
        ///   но не были завершенные до начала текущего года; -  ContractCondition.TransparentActive
     
        /// </summary>
        /// <returns>Договоры, соответствующие условию</returns>


        private IEnumerable<Contractdoc> GetActiveContractDocs()
        {
            DateTime startYear = DateTimeExtensions.GetFirstYearDay(Year);
            DateTime endYear = DateTimeExtensions.GetLastYearDay(Year);

            return Repository.Contracts.Where(
                //Необходимо, чтоб договор имел календарный план с запланированными этапами
                x => x.Schedulecontracts != null && x.Schedulecontracts.Count > 0 &&
                     x.Stages != null &&
                     x.Stages.Count() > 0 && x.IsGeneral).Where(
                     x =>
                         // На заданный период были запланированы работы
                         // Работы не были закончены к периоду
                        (x.Stages.FirstOrDefault(st =>
                                    (st.Startsat.HasValue && st.Startsat.Value >= startYear && st.Startsat.Value <= endYear) ||
                                    (st.Endsat.HasValue && st.Endsat.Value >= startYear && st.Endsat.Value <= endYear) ||
                                     //Работы не завершены  к заданному периоду
                                    (st.Endsat.HasValue && st.Endsat.Value <= startYear && (st.Act == null || !st.Act.Status.HasValue || (st.Act.Status.Value == 0)))) != null));
        }

        protected override void BuildReport()
        {
            throw new NotImplementedException();
        }

        public override string ReportFileName
        {
            get { throw new NotImplementedException(); }
        }

       
    }
}