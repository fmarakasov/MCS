using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using MCDomain.DataAccess;
using MCDomain.Model;
using MContracts.DTO;
using MContracts.Reports;


namespace MContracts.ViewModel
{
    /// <summary>
    /// Отчет 1. Тематический план на год
    /// </summary>
    public class ContractYearPlanReport1_ViewModel:ReportViewModel
    {
        public ContractYearPlanReport1_ViewModel(IContractRepository contractRepository)
            : base(contractRepository)
        {
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


        private const string PARAMETER_FIELD_NAME = "year";

        private void SetCurrentValuesForParamFields(ParameterFields parameterFields, ArrayList arrayList)
        {
            ParameterValues currentParameterValues = new ParameterValues();
            foreach (object submittedValue in arrayList)
            {
                ParameterDiscreteValue parameterDiscreteValue = new ParameterDiscreteValue();
                parameterDiscreteValue.Value = submittedValue.ToString();
                currentParameterValues.Add(parameterDiscreteValue);
            }
            ParameterField parameterField = parameterFields[PARAMETER_FIELD_NAME];
            parameterField.CurrentValues = currentParameterValues;
        }


        /// <summary>
        ///
        /// </summary>
        private List<YearPlanContractDto> _contracts
        {
            get
            {
                IEnumerable<Contractdoc> contractdocs = GetActiveContractDocs();
                

                int year = 2010;
                var e =
                    contractdocs.Where(cd=>cd.Schedulecontracts.Count == 1).Aggregate(new List<YearPlanContractDto>(), (a, b)=>
                                 {
                                     a.AddRange(b.Schedulecontracts.First().Schedule.Stages.Select(x =>
                                               {
                                                   return
                                                       new YearPlanContractDto()
                                                       {
                                                           Year=year,

                                                           Directors="ГДЕ ЖЕ ДИРЕКТОР И РУКОВОДИТЕЛЬ НАПРАВЛЕНИЯ???",

                                                           ResponsiblePeople = b.Disposals.Aggregate(string.Empty,
                                                                         (d, next) => d + next.Responsibles.Aggregate(string.Empty,
                                                                                   (r, n) => r + n.Employee.Customperson.FullName + " \n")),

                                                           ContractState = b.Contractstate.Name,

                                                           ContractNumSubject = "Договор № " + b.Internalnum + 
                                                                                "Утвержден " + b.Approvedat != null  ?  b.Approvedat.Value.ToString("dd.MM.yyyy") : "" + 
                                                                                 " \"" + b.Subject + "\"" + 
                                                                                 "Заказчик - " + b.Contractor.Name + 
                                                                                 "Функциональный заказчик - " + b.Functionalcustomercontracts.Aggregate(string.Empty,(fc, next) =>
                                                                                                                fc + next.Functionalcustomer.Name + "\n"), 
                                                                                 

                                                           StageNum = x.Num,

                                                           StageName = x.Subject,

                                                           StagePrice = x.Price.HasValue ? x.Price.Value : 0,





                                                       };
                                               }));
                                     return a;
                                 })
                                 
                                 ;
                return e;
            }
        }

        /// <summary>
        /// В список договоров входят
        /// 
        ///a) подписанные договоры, по которым, запланированы работы на
        ///   текущий год; -  ContractCondition.NormalActive
        ///
        /// b) подписанные договоры, работы по которым были запланированы на предыдущие периоды, 
        ///   но не были завершенные до начала текущего года; -  ContractCondition.TransparentActive
        ///
        /// c) договоры, находящиеся на стадии согласования, не зависимо от того, планируются 
        ///   по ним работы в текущем году или нет. - ?????
        /// </summary>
        /// <returns>Договоры, соответствующие условию</returns>

        private IEnumerable<Contractdoc> GetActiveContractDocs()
        {
           return Repository.Contracts.Where(
                x =>
                (x.Condition == ContractCondition.NormalActive || x.Condition == ContractCondition.TransparentActive ||
                 x.Condition == ContractCondition.TroubledActive)&&(x.IsGeneral));
        }

        public override ReportClass Report
        {
            get
            {
                var newReport = new ContractYearPlanReport_1();
               newReport.SetDataSource(_contracts);                
               return newReport;
            }
        }  
        
    }
}
