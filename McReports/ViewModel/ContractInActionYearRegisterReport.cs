using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MCDomain.Common;
using MCDomain.DataAccess;
using MCDomain.Model;
using McReports.Common;
using McReports.DTO;
using Microsoft.Office.Interop.Excel;
using System.Drawing;
using CommonBase;


namespace McReports.ViewModel
{
    /// <summary>
    /// Реестр действующих договоров на 2013 год ( с учетом переходящих этапов ранее заключенных договоров)
    /// </summary>
    public class ContractInActionYearRegisterReportViewModel : ExcelReportViewModel
    {
        private const int Defaultstagerowwidth = 60;
        private const int Defaultcontractrowwidth = 320;



        public ContractInActionYearRegisterReportViewModel(IContractRepository contractRepository)
            : base(contractRepository)
        {
            NeedsInputParameters = false;
        }

        public override int StartPosition
        {
            get { return 3; }
        }

        protected override string MinExcelColumn
        {
            get { return "A"; }
        }

        protected override string MaxExcelColumn
        {
            get { return "AM"; }
        }

        protected override void OnPeriodChanged()
        {
            Year = Period.End.Year;
            base.OnPeriodChanged();
        }


        public override string DisplayName
        {
            get
            {
                return "Реестр действующих договоров на год (с учетом переходящих этапов ранее заключенных договоров)";
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
                return string.Format("Реестр действующих договоров на {0} год", Year.ToString());
            }
        }

        [InputParameter(typeof(int), "Год")]
        public int Year { get; set; }

        public int PrevYear
        {
            get { return Year - 1; }
        }

        protected override string ReportFileName
        {
            get { return "ContractInActionYearRegister_1"; }
        }

        protected override DateTime StartPeriod
        {
            get { return DateTimeExtensions.GetFirstYearDay(Year); }
        }

        protected override DateTime EndPeriod
        {
            get { return DateTimeExtensions.GetLastYearDay(Year); }
        }

        private IEnumerable<ContractInActionDTO> Contracts
        {
            get
            {
                var contractdocs = ActiveContractDocs.ToList();
                if (contractdocs.Any())
                {
                    var e =
                           contractdocs.Where(cd => cd.HasSchedule && cd.Stages != null && cd.Stages.Any()).OrderBy(x => x.Internalnum).
                           Aggregate(new List<ContractInActionDTO>(), (a, c) =>
                           {
                               a.AddRange(c.Stages.Distinct().Where(st => st.Price > 0 &&
                                     (st.GetConditionForPeriod(StartPeriod, EndPeriod) == StageCondition.HaveToEnd ||
                                      st.GetConditionForPeriod(StartPeriod, EndPeriod) == StageCondition.Overdue ||
                                      (st.GetConditionForPeriod(StartPeriod, EndPeriod) == StageCondition.Closed && st.GetConditionOnDate(StartPeriod) != StageCondition.Closed))).OrderBy(st => st.Num, new NaturSortComparer<Stage>()).Select(st => new ContractInActionDTO()
                                      {

                                          Year = Year,
                                          Contract = c,
                                          Schedulecontract = c.Schedulecontracts.FirstOrDefault(sc => sc.Schedule != null && st.Schedule != null && sc.Schedule.Id == st.Schedule.Id),
                                          Stage = st,
                                          Ordersuperviser = st.OrderSuperviserEmployee != null ? st.OrderSuperviserEmployee.ToString() : "",
                                          Director = st.DirectorEmployee != null?st.DirectorEmployee.ToString():"",
                                          Contractortype = c.Contractor != null?c.Contractor.ToString():"",
                                          Appnum = st.Schedulecontract != null?(st.Schedulecontract.Appnum.HasValue?st.Schedulecontract.Appnum.Value.ToString():""):"",
                                          Chief = st.Responsibles.Any(r => r.Roleid == (long)WellKnownRoles.ContractChief)?st.Responsibles.Where(r=>r.Roleid == (long)WellKnownRoles.ContractChief&&r.Employee != null).Select(r => r.Employee).Aggregate("", (s, next) => s + next.ToString() +"\n"):"", //бывает 2
                                          OrderResponsible = c.Contracttype != null ? c.Contracttype.Responsiblefororders.Where(r => r.Department == c.DisposalDepartment).Aggregate("", (s, next) => s + next.ToString() + "\n"):"",
                                          Contractnum = c.Internalnum,
                                          Contractdate = c.Startat,
                                          Agreementnum = c.Agreementnum.HasValue ? c.Agreementnum.Value.ToString():"",
                                          Contractstate = c.Contractstate != null ? c.Contractstate.ToString(): "",
                                          Contracttype  = c.ContractorTypeName,
                                          Customer = c.FunctionalCustomers,
                                          Subject = c.Subject,
                                          Stagenum = st.Num,
                                          Objectcode = st.Code,
                                          Stagesubject = st.Subject,
                                          Stagecost = st.StageMoneyModel.National.Factor.WithNdsValue,
                                          StageStartsAt = st.Startsat,
                                          StageFinishesAt = st.Endsat,
                                          Actdate = st.HasActOrParentHasAct?(st.Act != null?st.Act.Signdate:null):(st.ParentAct != null?st.ParentAct.Signdate:null),
                                          Actnum = st.HasActOrParentHasAct?(st.Act != null?st.Act.Num:null):(st.ParentAct != null?st.ParentAct.Num:null),
                                          Actcost = st.HasActOrParentHasAct?(st.Act != null? st.Act.ActMoney.National.Factor.WithNdsValue:0):(st.ParentAct != null? st.ParentAct.ActMoney.National.Factor.WithNdsValue:0),
                                          Actstate = st.Approvalstate != null? st.Approvalstate.Name:"",
                                          Subcontractcost = st.SubContractsStages.Sum(sst => sst.StageMoneyModel.National.Factor.WithNdsValue),
                                          Contractconditioncomment = c.ContractConditionComment
                                          }));
                               return a;
                           });

                    return e;
                }
                return null;
            }

        }




        private void BuildPage(IEnumerable<ContractInActionDTO> yearPlanContractStages, string newpagename = "")
        {

            var reporter = CreateProgressReporter(yearPlanContractStages.Count());
            
            IEnumerable<IGrouping<Contractdoc, ContractInActionDTO>> contractGroups = Contracts.GroupBy(x => x.Contract, x => x).OrderBy(x => x.Key.Internalnum);
            foreach (var c in contractGroups)
            {

                IEnumerable<IGrouping<Schedulecontract, ContractInActionDTO>> scheduleGroups = c.GroupBy(x => x.Schedulecontract, x => x).OrderBy(x => x.Key.Appnum);

                foreach (var sch in scheduleGroups)
                {

                    foreach (var st in sch)
                    {
                        reporter.ReportProgress();
                        if (!st.Stage.SubContractsStages.Any())
                        if (!st.Stage.SubContractsStages.Any())
                        {
                            BuildStage(st, null);
                        }
                        else
                        {
                            foreach (var subst in st.Stage.SubContractsStages)
                            {
                                BuildStage(st, subst);
                            }
                        }
                        reporter.Next();
                    }
                }
            }
        }

        
        protected override void BuildReport()
        {
            //При создании этой колеекции сразу выставляем параменры по умолчанию. Если не задан Contractor выставляем в Другие, если не задан ContractorType, задаем Прочие (чтоб все они попали в одну группу и не выбрасовала исключение)
            //IEnumerable<ContractInActionDTO> yearPlanContractStages = _contracts;
            //BuildPage(yearPlanContractStages, true);
            SetHeader();
            MainWorkSheet.Activate();
            CurrentButtomPosition = StartPosition;
            BuildPage(Contracts);
            BuildTail();
            SetPrintingArea(Excel.ActiveSheet);
        }


        private void BuildStage(ContractInActionDTO stage, Stage substage)
        {
            Range r = CopyRowRange(DetailWorkSheet, 1);

            if (stage != null)
            {
                //Excel.Visible = true;
                //r.Select();
                r.Replace("#Ordersuperviser#", stage.Ordersuperviser);
                r.Replace("#Director#", stage.Director);
                r.Replace("#ContractorType#", stage.Contractortype);
                r.Replace("#AppNum#", stage.Appnum);
                r.Replace("#Chief#", stage.Chief);
                r.Replace("#OrderResponsible#", stage.OrderResponsible);
                r.Replace("#ContractNum#", stage.Contractnum);
                r.Replace("#AgreementNum#", stage.Agreementnum);
                r.Replace("#ContractDate#", stage.Contractdate.ToString());
                r.Replace("#ContractState#", stage.Contractstate);
                r.Replace("#ContractType#", stage.Contractortype);
                InsertBigFieldIntoReport("#Customer#", stage.Customer, r);
                InsertBigFieldIntoReport("#Subject#", stage.Subject, r);
                r.Replace("#StageNum#", " " + stage.Stagenum);
                r.Replace("#ObjectCode#", stage.Objectcode);
                InsertBigFieldIntoReport("#StageSubject#", stage.Stagesubject, r);
                r.Replace("#StageCost#", stage.Stagecost.ToString(NumberFormat));
                r.Replace("#StageStartsAt#", stage.StageStartsAt.HasValue ? stage.StageStartsAt.Value.ToString("dd.MM.yyyy"):"");
                r.Replace("#StageFinishesAt#", stage.StageFinishesAt.HasValue ? stage.StageFinishesAt.Value.ToString("dd.MM.yyyy"):"");
                r.Replace("#ActDate#", stage.Actdate.HasValue? stage.Actdate.Value.ToString("dd.MM.yyyy"):"");
                r.Replace("#ActNum#", stage.Actnum);
                r.Replace("#ActCost#", stage.Actcost.ToString(NumberFormat));
                r.Replace("#OwnCost#", stage.Owncost.ToString(NumberFormat));
                r.Replace("#ActState#", stage.Actstate);
                r.Replace("#ContractComment#", stage.Contractconditioncomment);


                if (substage != null)
                {
                    InsertBigFieldIntoReport("#Subcontactor#",
                                             (substage.ContractObject.Contractor != null)
                                                 ? substage.ContractObject.Contractor.ToString()
                                                 : "", r);
                    r.Replace("#SubcontractNum#", substage.ContractObject.Internalnum);
                    r.Replace("#SubcontractDate#",
                              substage.ContractObject.Approvedat.HasValue
                                  ? substage.ContractObject.Approvedat.Value.ToString("dd.MM.yyyy")
                                  : "");
                    r.Replace("#SubcontractState#",
                              substage.ContractObject.Contractstate != null
                                  ? substage.ContractObject.Contractstate.ToString()
                                  : "");
                    r.Replace("#SubcontractCost#",
                              substage.StageMoneyModel.National.Factor.WithNdsValue.ToString(NumberFormat));
                    r.Replace("#SubcontractStageStartsAt#", substage.Startsat.HasValue?substage.Startat.Value.ToString("dd.MM.yyyy"):"");
                    r.Replace("#SubcontractStageFinishesAt#", substage.Endsat.HasValue?substage.Endsat.Value.ToString("dd.MM.yyyy"):"");
                    r.Replace("#SubcontractActDate#",
                              (substage.Act != null && substage.Act.Issigned.HasValue && substage.Act.Issigned.Value
                                   ? (substage.Act.Signdate.HasValue ? substage.Act.Signdate.Value.ToString("dd.MM.yyyy") : "")
                                   : ""));
                    r.Replace("#SubcontractActNum#",
                              (substage.Act != null && substage.Act.Issigned.HasValue && substage.Act.Issigned.Value
                                   ? substage.Act.Num
                                   : ""));
                    r.Replace("#SubcontractActCost#",
                              (substage.Act != null && substage.Act.Issigned.HasValue && substage.Act.Issigned.Value
                                   ? substage.Act.ActMoney.National.Factor.WithNdsValue.ToString(NumberFormat)
                                   : ""));
                    r.Replace("#FromOwnCostPercent#",
                              (substage.StageMoneyModel.National.Factor.WithNdsValue*100/stage.Stagecost).ToString(
                                  NumberFormat));

                    r.Replace("#SubContractComment#", substage.ContractObject.ApprovalContractConditionComment);

                }
                else
                {
                    r.Replace("#Subcontactor#", "");
                    r.Replace("#SubcontractNum#", "");
                    r.Replace("#SubcontractDate#","");
                    r.Replace("#SubcontractState#","");
                    r.Replace("#SubcontractCost#","");
                    r.Replace("#SubcontractStageStartsAt#", "");
                    r.Replace("#SubcontractStageFinishesAt#", "");
                    r.Replace("#SubcontractActDate#","");
                    r.Replace("#SubcontractActNum#","");
                    r.Replace("#SubcontractActCost#","");
                    r.Replace("#FromOwnCostPercent#","");
                    r.Replace("#SubContractComment#", "");
                }


                SetColorByApprovalState(stage.Stage, 15, r);



                r.Rows.AutoFit();
                var iRowCount = GetLongStringRowsCount(stage.Stage.Subject, Defaultstagerowwidth, 4);
                r.RowHeight = DefaultRowHeight * (iRowCount + 2);

            }
        }

        private void BuildTail()
        {
            //#SumStagesCost#					
            //#SumActCost#					
            //#SumSubstageCost#						 
            //#SumSubstagesActCost# 
            Range r = CopyRowRange(ContractTailWorkSheet, 1);
            r.Replace("#SumStagesCost#", Contracts.Select(c => c.Stagecost).Sum().ToString(NumberFormat));
            r.Replace("#SumActCost#", Contracts.Select(c => c.Actcost).Sum().ToString(NumberFormat));
            r.Replace("#SumSubstageCost#", Contracts.Select(c => c.Subcontractcost).Sum().ToString(NumberFormat));
            r.Replace("#SumSubstagesActCost#", Contracts.Select(c => c.Subcontractactcost).Sum().ToString(NumberFormat));
        }


        protected override void SetHeader()
        {
            MainWorkSheet.Cells.Replace("#Year#", Year);
        }

     
        public Worksheet GetWorkSheetByName(string workSheetName)
        {
            return Excel.Worksheets[workSheetName];
        }


        private static Worksheet ContractTailWorkSheet
        {
            get { return Excel.Worksheets["Tail"]; }
        }
    }


}
