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
    public class ContractPeriodReport_3_ViewModel:ExcelReportViewModel
    {
        public ContractPeriodReport_3_ViewModel(IContractRepository contractRepository)
            : base(contractRepository)
        {
            PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler(ContractRegisterReport_3_ViewModel_PropertyChanged);
            Year = DateTime.Today.Year;
            PrevYear = Year - 1;
        }

        void ContractRegisterReport_3_ViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "MainViewModel")
            {
                if (MainViewModel != null)
                {
                    Year = MainViewModel.SelectedFilterEndDate.Year;
                    PrevYear = MainViewModel.SelectedFilterStartDate.Year;
                }
                else
                {
                    Year = DateTime.Today.Year;
                    PrevYear = Year - 1;
                }
            }
        }

        [InputParameter(typeof(int), "Год")]
        public int Year { get; set; }

        public int PrevYear { get; set; }


        public override string DisplayName
        {
            get
            {
                return "Текущая справка на переходный период";
            }
            protected set
            {
                base.DisplayName = value;
            }
        }

        public override string ReportFileName
        {
            get { return "ContractPepiodInformationTemplate_3"; }
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

        /// <summary>
        /// начало предыдущего периода - за который рассчитывается остаток на квартал
        /// </summary>
        private DateTime StartPrevPeriod
        {
            get { return DateTimeExtensions.GetFirstQuarterDay(PrevYear, 4); }
        }

        /// <summary>
        /// конец предыдущего периода - за который рассчитывается остаток на квартал
        /// </summary>
        private DateTime EndPrevPeriod
        {
            get { return DateTimeExtensions.GetLastYearDay(PrevYear); }
        }

        private List<ContractDto> _contracts
        {
            get
            {
                IEnumerable<Contractdoc> contractdocs = ActiveContractDocs;

                //contractdocs = contractdocs.Where(c => c.Num == "129-11-02");
                if (contractdocs != null && contractdocs.Any())
                {

                    var e =
                        contractdocs.Aggregate(new List<ContractDto>(), (a, c) =>
                        {
                            a.Add(new ContractDto()
                            {
                                Year = Year,
                                ContractorType = (c.Contractor != null) && (c.Contractor.Contractortype != null) ? c.Contractor.Contractortype : Repository.Contractortypes.FirstOrDefault(ct => ct.WellKnownType == WellKnownContractorTypes.Other),
                                ContractType = c.Contracttype ?? Repository.Contracttypes.FirstOrDefault(ct => ct.WellKnownType == WellKnownContractTypes.Other),
                                DeputyDirector = c.Directors(true),
                                DisposalFullName = c.Disposal != null ? c.Disposal.ToString() : string.Empty,
                                ContractNum = c.Num == null ? string.Empty : " № " + c.Num,
                                Subject = new StringBuilder().Append("\n").Append(c.FullSortName).Append("\n").ToString(),


                                //Заказчик 
                                //В столбце «Заказчик» должно отражаться Наименование организации заказчика (FunctionalCustomer) 
                                //Для договоров с ОАО «Газпром» должно отражаться название управления и (или) название департамента функционального заказчика (DepartamentName)

                                FunctionalCustomer = c.Customer,

                                //Цена, запланированная на текущий год
                                Price = c.GetPriceForPeriodWithNDS(StartPeriod, EndPeriod, Measure) - c.GetPriceForPeriodWithNDS(StartPrevPeriod, EndPrevPeriod, Measure),

                                //Цена по  подписанным договорам
                                SignedContractsPrice = c.Contractstate == null ? 0 : !c.Contractstate.IsSigned ? 0 :
                                                       c.GetPriceForPeriodWithNDS(StartPeriod, EndPeriod, Measure) - c.GetPriceForPeriodWithNDS(StartPrevPeriod, EndPrevPeriod, Measure),

                                // Цена по договорам с соисиполнителями
                                CoworkersPrice = c.Contractstate == null ? 0 : !c.Contractstate.IsSigned ? 0 :
                                                 c.GetCoworkersPriceForPeriodWithNDS(StartPeriod, EndPeriod, Measure) - c.GetCoworkersPriceForPeriodWithNDS(StartPrevPeriod, EndPrevPeriod, Measure),

                                PrevPrice = c.GetPriceForPeriodWithNDS(StartPrevPeriod, EndPrevPeriod, Measure),
                                
                                PrevSignedContractsPrice = c.Contractstate == null? 0: !c.Contractstate.IsSigned ? 0: c.GetPriceForPeriodWithNDS(StartPrevPeriod, EndPrevPeriod, Measure),
                               
                                PrevCoworkersPrice = c.Contractstate == null ? 0 : !c.Contractstate.IsSigned ? 0 :
                                                     c.GetCoworkersPriceForPeriodWithNDS(StartPrevPeriod, EndPrevPeriod, Measure),

                                ContractCondition = c.ContractConditionComment,

                                ContractorTypeID =  (c.Contractor == null ? (int) WellKnownContractorTypes.Gazprom : c.Contractor.Contractortype == null ? (int)WellKnownContractorTypes.Gazprom : !c.Contractor.Contractortype.Reportorder.HasValue ? (int)WellKnownContractorTypes.Gazprom : c.Contractor.Contractortype.Reportorder.Value),

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
                IEnumerable<IGrouping<Contracttype, ContractDto>> contractTypeGroups = contractorGroup.GroupBy(x => x.ContractType, x => x).OrderBy(x => x.Key.Reportorder);
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
            MainWorkSheet.Rows.AutoFit();
        }
        
        protected override void SetHeader()
        {
            decimal price = _contracts.Sum(x => x.Price);
            decimal sighPrice = _contracts.Sum(x => x.SignedContractsPrice);
            decimal coworkersPrice = _contracts.Sum(x => x.CoworkersPrice);
            decimal prevprice = _contracts.Sum(x => x.PrevPrice);
            decimal prevsighprice = _contracts.Sum(x => x.PrevSignedContractsPrice);
            decimal prevcoworkersprice = _contracts.Sum(x => x.PrevCoworkersPrice);

            MainWorkSheet.Cells.Replace("#Year#", Year);
            MainWorkSheet.Cells.Replace("#PrevYear#", PrevYear);
            MainWorkSheet.Cells.Replace("#Today#", CurrentDate.ToString(("dd.MM.yyyy")));

            MainWorkSheet.Cells.Replace("#TotalPrevPrice#", (prevprice).ToString(NumberFormat));
            MainWorkSheet.Cells.Replace("#PrevYearPrice#", (prevsighprice).ToString(NumberFormat));
            MainWorkSheet.Cells.Replace("#TotalPrevSignPrice#", (prevcoworkersprice).ToString(NumberFormat));

            MainWorkSheet.Cells.Replace("#TotalPrice#", (price).ToString(NumberFormat));
            MainWorkSheet.Cells.Replace("#TotalSignPrice#", (sighPrice).ToString(NumberFormat));
            MainWorkSheet.Cells.Replace("#TotalCoworkerplanprice#", (coworkersPrice).ToString(NumberFormat));

        }

        private void BuildContractorType(IGrouping<Contractortype, ContractDto> contractortype)
        {
            Range r = CopyRowRange(ContractorWorkSheet, 1);
            r.Replace("#ContractorType#", contractortype.Key.Name ?? "");

            r.Replace("#ContractorTypePrevPrice#", contractortype.Sum(x => x.PrevPrice).ToString(NumberFormat));
            r.Replace("#ContractorTypePrevYearPrice#", contractortype.Sum(x => x.PrevSignedContractsPrice).ToString(NumberFormat));
            r.Replace("#ContractorTypePrevSignPrice#", contractortype.Sum(x => x.PrevCoworkersPrice).ToString(NumberFormat));

            r.Replace("#ContractorTypePrice#", contractortype.Sum(x => x.Price).ToString(NumberFormat));
            r.Replace("#ContractorTypeSignPrice#", contractortype.Sum(x => x.SignedContractsPrice).ToString(NumberFormat));
            r.Replace("#ContractorCoworkerplanprice#", contractortype.Sum(x => x.CoworkersPrice).ToString(NumberFormat));
         
        }

        private void BuildContractType(Contractortype contractortype, IGrouping<Contracttype, ContractDto> contracttype)
        {
            Range r = CopyRowRange(ContractTypeWorkSheet, 1);
            r.Replace("#ContractType#", contracttype.Key.Name ?? "");
            r.Replace("#ContractorType#", NameInAblative(contractortype.Name) ?? "");

            r.Replace("#ContractTypePrevPrice#", contracttype.Sum(x => x.PrevPrice).ToString(NumberFormat));
            r.Replace("#ContractTypePrevSignPrice#", contracttype.Sum(x => x.PrevSignedContractsPrice).ToString(NumberFormat));
            r.Replace("#ContractTypePrevCoworkerplanprice#", contracttype.Sum(x => x.PrevCoworkersPrice).ToString(NumberFormat));

            r.Replace("#ContractTypePrice#", contracttype.Sum(x => x.Price).ToString(NumberFormat));
            r.Replace("#ContractTypeSignPrice#", contracttype.Sum(x => x.SignedContractsPrice).ToString(NumberFormat));
            r.Replace("#ContractTypeCoworkerplanprice#", contracttype.Sum(x => x.CoworkersPrice).ToString(NumberFormat));
        }
        
        private void BuildContract(ContractDto contractdoc)
        {
            Range r = CopyRowRange(DetailWorkSheet, 1);
            r.Replace("#Curator#", contractdoc.CuratorEmployee);
            r.Replace("#Directors#", contractdoc.DeputyDirector);
            r.Replace("#Disposal#", contractdoc.DisposalFullName);

            //Если поле слишком велико - вставляем по частям
            InsertBigFieldIntoReport("#ContractName#", contractdoc.Subject, r);
            InsertBigFieldIntoReport("#Customer#", contractdoc.FunctionalCustomer, r);

            r.Replace("#PrevPrice#", (contractdoc.PrevPrice).ToString(NumberFormat));
            r.Replace("#PrevSignPrice#", (contractdoc.PrevSignedContractsPrice).ToString(NumberFormat));
            r.Replace("#PrevCoworkerplanprice#", contractdoc.PrevCoworkersPrice.ToString(NumberFormat));

            r.Replace("#Price#", (contractdoc.Price).ToString(NumberFormat));
            r.Replace("#SignPrice#", (contractdoc.SignedContractsPrice).ToString(NumberFormat));
            r.Replace("#Coworkerplanprice#", contractdoc.CoworkersPrice.ToString(NumberFormat));

            InsertBigFieldIntoReport("#Information#", contractdoc.ContractCondition, r);

            r.Rows.AutoFit();
        }

       
    }
}
