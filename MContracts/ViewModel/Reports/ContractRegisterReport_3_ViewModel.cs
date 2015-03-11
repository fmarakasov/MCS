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

namespace MContracts.ViewModel.Reports
{
    /// <summary>
    /// Отчет 2. Текущая справка по договорам
    /// </summary>
    public class ContractRegisterReport_2_ViewModel:ExcelReportViewModel
    {
        public ContractRegisterReport_2_ViewModel(IContractRepository contractRepository):base(contractRepository)
        {
            PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler(ContractRegisterReport_2_ViewModel_PropertyChanged);
            Year = DateTime.Today.Year;
        }

        void ContractRegisterReport_2_ViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName=="MainViewModel")
            {
                if (MainViewModel != null)
                    Year = MainViewModel.SelectedFilterEndDate.Year;
                else
                    Year = DateTime.Today.Year;
            }
        }


        [InputParameter(typeof(int), "Год")]
        public int Year { get; set; }

        public override string DisplayName
        {
            get
            {
                return "Справка по договорам";
            }
            protected set
            {
                base.DisplayName = value;
            }
        }

        public override string ReportFileName
        {
            get { return "ContractInformationTemplate_2"; }
        }

        protected override DateTime StartPeriod
        {
            get
            {
                 return DateTimeExtensions.GetFirstYearDay(Year);
            }
        }

        protected override DateTime EndPeriod
        {
            get
            {
                return DateTimeExtensions.GetLastYearDay(Year);
            }
        }

        private List<ContractDto> _contracts
        {
            get
            {
                IEnumerable<Contractdoc> contractdocs = ActiveContractDocs;

                //contractdocs = contractdocs.Where(c => c.Num == "129-11-02");
                if (contractdocs != null && contractdocs.Count() > 0)
                {
                    var e =
                        contractdocs.Aggregate(new List<ContractDto>(), (a, c) =>
                                                                            {a.Add(new ContractDto()
                                                                                {
                                                                                    Year = Year,
                                                                                    ContractorType = (c.Contractor != null) && (c.Contractor.Contractortype != null) ? c.Contractor.Contractortype : Repository.Contractortypes.FirstOrDefault(ct => ct.WellKnownType == WellKnownContractorTypes.Other),
                                                                                    ContractType = c.Contracttype ?? Repository.Contracttypes.FirstOrDefault(ct => ct.WellKnownType == WellKnownContractTypes.Other),
                                                                                    DeputyDirector = c.Directors(true),
                                                                                    DisposalFullName = c.Disposal != null?c.Disposal.ToString():string.Empty,
                                                                                    ContractNum = c.Num == null ? string.Empty : " № " + c.Num,
                                                                                    Subject = new StringBuilder().AppendLine().Append(c.FullSortName).AppendLine().ToString(),


                                                                                    //Заказчик 
                                                                                    //В столбце «Заказчик» должно отражаться Наименование организации заказчика (FunctionalCustomer) 
                                                                                    //Для договоров с ОАО «Газпром» должно отражаться название управления и (или) название департамента функционального заказчика (DepartamentName)

                                                                                    FunctionalCustomer = c.Customer,

                                                                                    //Цена, запланированная на текущий год
                                                                                    Price = c.GetPriceForPeriodWithNDS(StartPeriod, EndPeriod, Measure),

                                                                                    //Цена по  подписанным договорам
                                                                                    SignedContractsPrice = c.Contractstate == null ? 0 : !c.Contractstate.IsSigned ? 0 :
                                                                                                           c.GetPriceForPeriodWithNDS(StartPeriod, EndPeriod, Measure),

                                                                                    // Цена по договорам с соисиполнителями
                                                                                    CoworkersPrice = c.Contractstate == null ? 0 : !c.Contractstate.IsSigned ? 0 :
                                                                                                           c.GetCoworkersPriceForPeriodWithNDS(StartPeriod, EndPeriod, Measure),


                                                                                    ContractCondition = c.ContractConditionComment,

                                                                                    ContractorTypeID = (c.Contractor == null ? (int)WellKnownContractorTypes.Gazprom : c.Contractor.Contractortype == null ? (int)WellKnownContractorTypes.Gazprom : !c.Contractor.Contractortype.Reportorder.HasValue ? (int)WellKnownContractorTypes.Gazprom : c.Contractor.Contractortype.Reportorder.Value),

                                                                                    ContractTypeID = (c.Contracttype == null ? (int)WellKnownContractTypes.Other : !c.Contracttype.Reportorder.HasValue ? (int)WellKnownContractTypes.Other : c.Contracttype.Reportorder.Value),
                                                                                    CuratorEmployee = c.CuratorEmployee.GetShortNameForReports()
                                                                                });
                                                                                return a;
                                                                            });
                    return e;
                }
                return null;
            }
            }
        
        

    

        protected override void BuildReport()
        {
            CurrentButtomPosition = 6;

            SetHeader();

            IEnumerable<ContractDto> contractdocs = _contracts;

            //Группировка по тип контрагента
            IEnumerable<IGrouping<Contractortype, ContractDto>> contractorTypeGroups = contractdocs.GroupBy(x => x.ContractorType, x => x).OrderBy(x => x.Key.Reportorder);
            foreach (IGrouping<Contractortype, ContractDto> contractorGroup in contractorTypeGroups)
            {
                BuildContractorType(contractorGroup);

                //Группировка по типу договора
                IEnumerable<IGrouping<Contracttype, ContractDto>> contractTypeGroups = contractorGroup.GroupBy(x => x.ContractType, x => x).OrderBy(x=>x.Key.Reportorder);
                foreach (IGrouping<Contracttype, ContractDto> contractTypeGroup in contractTypeGroups)
                {
                    BuildContractType(contractorGroup.Key, contractTypeGroup);

                    //ГРуппировка по договору
                    foreach (ContractDto contract in contractTypeGroup.OrderBy(x => x.ContractNum))
                    {
                        BuildContract(contract);
                    }
                    
                }
            }
            
        }

        protected override void SetHeader()
        {
            decimal price = _contracts.Sum(x => x.Price);
            decimal sighPrice = _contracts.Sum(x => x.SignedContractsPrice);
            decimal coworkersPrice = _contracts.Sum(x => x.CoworkersPrice);
            //Для ускорения вводим диапозон ячеек для замены (задаем диапозон больше, чем требуется)
            Range r = MainWorkSheet.Range["A1", "M5"];
            r.Replace("#Year#", Year);
            r.Replace("#Today#", CurrentDate.ToString(("dd.MM.yyyy")));
            r.Replace("#TotalPrice#", (price).ToString(NumberFormat));
            r.Replace("#TotalSignPrice#", (sighPrice).ToString(NumberFormat));
            r.Replace("#TotalCoworkerplanprice#", (coworkersPrice).ToString(NumberFormat));
           
        }

        private void BuildContractorType(IGrouping<Contractortype, ContractDto> contractortype)
        {
            Range r = CopyRowRange(ContractorWorkSheet, 1);
            r.Replace("#ContractorType#", contractortype.Key.Name ?? "");
            r.Replace("#ContractorTypePrice#",contractortype.Sum(x => x.Price).ToString(NumberFormat));
            r.Replace("#ContractorTypeSignPrice#", contractortype.Sum(x => x.SignedContractsPrice).ToString(NumberFormat));
            r.Replace("#ContractorCoworkerplanprice#", contractortype.Sum(x => x.CoworkersPrice).ToString(NumberFormat));
        }

        private void BuildContractType(Contractortype contractortype, IGrouping<Contracttype, ContractDto> contracttype)
        {
            Range r = CopyRowRange(ContractTypeWorkSheet, 1);
            r.Replace("#ContractType#", contracttype.Key.Name ?? "");
            r.Replace("#ContractorType#", NameInAblative(contractortype.Name) ?? "");
            r.Replace("#ContractTypePrice#",contracttype.Sum(x => x.Price).ToString(NumberFormat));
            r.Replace("#ContractTypeSignPrice#", contracttype.Sum(x => x.SignedContractsPrice).ToString(NumberFormat));
            r.Replace("#ContractTypeCoworkerplanprice#", contracttype.Sum(x => x.CoworkersPrice).ToString(NumberFormat));
        }

        private void BuildContract(ContractDto contract)
        {
            Range r = CopyRowRange(DetailWorkSheet, 1);
            r.Replace("#Curator#", contract.CuratorEmployee);
            r.Replace("#Directors#", contract.DeputyDirector);
            r.Replace("#Disposal#", contract.DisposalFullName);

            //Если поле слишком велико - вставляем по частям
            InsertBigFieldIntoReport("#ContractName#", contract.Subject, r);
            InsertBigFieldIntoReport("#Customer#", contract.FunctionalCustomer,r);
            r.Replace("#Price#", (contract.Price).ToString(NumberFormat));
            r.Replace("#SignPrice#", (contract.SignedContractsPrice).ToString(NumberFormat));
            r.Replace("#Coworkerplanprice#", contract.CoworkersPrice.ToString(NumberFormat));

            InsertBigFieldIntoReport("#Information#", contract.ContractCondition,r);

            r.Rows.AutoFit();
        }

  

  
    }
}
