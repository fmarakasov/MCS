using System;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using MCDomain.DataAccess;
using MCDomain.Model;
using CommonBase;

namespace McReports.ViewModel
{
    public class Act1ReportViewModel: BaseActReportViewModel
    {
        public Act1ReportViewModel(IContractRepository contractRepository)
            : base(contractRepository)
        {
        
        }

        protected override string ReportFileName
        {
            get { return "Act1Report"; }
        }

        /// <summary>
        /// департаменты
        /// </summary>
        private void BuildDepartmentSignAutohrities()
        {
            var departments = CurrentAct.ContractObject.Functionalcustomercontracts.Select(f => f.Functionalcustomer).Where(fc => fc.Level == 0).ToList();
            var i = 0;
             
            foreach (var fc in departments)
            {
                var p = fc.Funccustomerpersons.FirstOrDefault(x => x.Person.Valid && x.Person.Actsignauthority);

                if (p != null && p.Person != null)
                {
                    ReplaceText("[FZDepartmentPost]",
                                p.Person.Contractorposition != null
                                    ? p.Person.Contractorposition.ToString()
                                    : "<должность не указана>");


                    ReplaceText("[FZDepartmentName]", p.Person.GetShortFullName());

                    i++;
                    if (i < departments.Count - 1) AddFunctionalcustomerDepartmentAutohrityRow();
                }
           }

            ReplaceText("[FZDepartmentPost]", "");
            ReplaceText("[FZDepartmentName]", "");

        }

        /// <summary>
        /// управления
        /// </summary>
        private void BuildSimpleSignAutohrities()
        {
            var departments = CurrentAct.ContractObject.Functionalcustomercontracts.Select(f => f.Functionalcustomer).Where(fc => fc.Level != 0).ToList();
            var i = 0;
             
            foreach (var fc in departments)
            {
                var p = fc.Funccustomerpersons.FirstOrDefault(x => x.Person.Valid && x.Person.Actsignauthority);

                if (p != null && p.Person != null)
                {
                    ReplaceText("[FZPost]",
                                p.Person.Contractorposition != null
                                    ? p.Person.Contractorposition.ToString()
                                    : "<должность не указана>");


                    ReplaceText("[FZName]", p.Person.GetShortFullName());

                    i++;
                    if (i < departments.Count - 1) AddFunctionalcustomerAutohrityRow();
                }
           }

            ReplaceText("[FZPost]", "");
            ReplaceText("[FZName]", "");

        }
        
        private void BuildDisposal()
        {
            var disposal =
                CurrentAct.Stages.Where(s => s.Disposal != null).Select(x => x.Disposal).FirstOrDefault(d => d.Responsibles.Any(r => r.Director != null&&r.Director.Employee != null));

            if (disposal != null)
            {
                var director =
                    disposal.Responsibles.FirstOrDefault(r => r.Director != null && r.Director.Employee != null);

                Debug.Assert(director != null, "director != null");
                ReplaceText("[SelfRespPost]",
                            director.DirectorEmployee.Post != null
                                ? director.DirectorEmployee.Post.Name
                                : "<должность не указана>");

                ReplaceText("[SelfRespName]", director.DirectorEmployee.GetShortFullName());
            }
            else
            {
                ReplaceText("[SelfRespPost]", "");
                ReplaceText("[SelfRespName]", "");
            }    
        }

        private void BuildSignAuthorities()
        {
            if (CurrentAct.ContractObject.Functionalcustomercontracts.Any())
            {
                BuildDepartmentSignAutohrities();
                BuildSimpleSignAutohrities();
            }
            else
            {
                ReplaceText("[FZDepartmentPost]", "");
                ReplaceText("[FZDepartmentName]", "");
                ReplaceText("[FZPost]", "");
                ReplaceText("[FZName]", "");
            }

            BuildDisposal();
        }

        protected override void BuildStages()
        {
            for (int i = 0; i <= CurrentAct.Stages.Count - 1; i++)
            {
                ReplaceText("[StageNum]", CurrentAct.Stages[i].Num.ToString(CultureInfo.InvariantCulture));
                ReplaceText("[StageName]", CurrentAct.Stages[i].Subject.ToString(CultureInfo.InvariantCulture));

                ReplaceText("[StageCost]", CurrentAct.Stages[i].StageMoneyModel.Factor.PureValue.ToString("N2"));
                
                if (i < CurrentAct.Stages.Count - 1) AddStagesTableRow();
            }

            if (CurrentAct.ActMoney.Price.HasValue) 
            {
              ReplaceText("[StageCostSum]", CurrentAct.ActMoney.Factor.PureValue.ToString("N2"));            
              ReplaceText("[StageCostSumWords]", CurrentAct.ActMoney.Currency.MoneyInWords(CurrentAct.ActMoney.Factor.PureValue, true, false, true));
            }
            else
            {
                ReplaceText("[StageCostSum]", NoDataString);
                ReplaceText("[StageCostSumWords]", NoDataString);
            }

            //([NDSPercent]) – NDSValue ([NDSValueWords]).

            


        }

        private void AddStagesTableRow()
        {
            Word.ActiveDocument.Tables[2].Rows[Word.ActiveDocument.Tables[2].Rows.Count - 1].Select();
            Word.Selection.InsertRowsBelow(1);
            Word.Selection.TypeText("Этап [StageNum]");
            Word.Selection.MoveRight(Microsoft.Office.Interop.Word.WdUnits.wdCharacter, 1);
            Word.Selection.TypeText("[StageName]");
            Word.Selection.MoveRight(Microsoft.Office.Interop.Word.WdUnits.wdCharacter, 1);
            Word.Selection.TypeText("[StageCost]");
        }


        private void AddFunctionalcustomerDepartmentAutohrityRow()
        {
            Word.ActiveDocument.Tables[3].Rows[Word.ActiveDocument.Tables[3].Rows.Count - 1].Select();
            Word.Selection.InsertRowsBelow(1);
            Word.Selection.MoveRight(Microsoft.Office.Interop.Word.WdUnits.wdCharacter, 1);
            Word.Selection.TypeText("[FZDepartmentPost]");
            Word.Selection.MoveRight(Microsoft.Office.Interop.Word.WdUnits.wdCharacter, 1);
            Word.Selection.TypeText("[FZDepartmentName]");
        }

        private void AddFunctionalcustomerAutohrityRow()
        {
            Word.ActiveDocument.Tables[6].Rows[Word.ActiveDocument.Tables[6].Rows.Count - 1].Select();
            Word.Selection.InsertRowsBelow(1);
            Word.Selection.MoveRight(Microsoft.Office.Interop.Word.WdUnits.wdCharacter, 1);
            Word.Selection.TypeText("[FZPost]");
            Word.Selection.MoveRight(Microsoft.Office.Interop.Word.WdUnits.wdCharacter, 1);
            Word.Selection.TypeText("[FZName]");
        }

        protected  void BuildAdvance()
        {
            if (CurrentAct.CreditedMoneyModel.Price.HasValue)
            {
                if (CurrentAct.CreditedMoneyModel.Factor.Price != null)
                {
                    ReplaceText("[AdvanceSum]", CurrentAct.CreditedMoneyModel.Factor.Price.Value.ToString("N2"));
                    if (CurrentAct.CreditedMoneyModel.Price.Value > 0)
                        ReplaceText("[AdvanceSumWords]", CurrentAct.CreditedMoneyModel.Currency.MoneyInWords(CurrentAct.CreditedMoneyModel.Factor.Price.Value, true, false, true));
                    else
                        ReplaceText("([AdvanceSumWords])", "");
                }
            }
            else
            {
                ReplaceText("[AdvanceSum]", NoDataString);
                ReplaceText("[AdvanceSumWords]", NoDataString);
            }

        }

        protected void BuildTransfer()
        {
            if (CurrentAct.TransferSumMoney.Price.HasValue)
            {
                ReplaceText("[ActCost]", CurrentAct.TransferSumMoney.Factor.WithNdsValue.ToString("N2"));
                ReplaceText("[ActCostWords]", CurrentAct.TransferSumMoney.Currency.MoneyInWords(CurrentAct.TransferSumMoney.Factor.WithNdsValue, true, false, true));
                ReplaceText("[ActCostNDSValue]", CurrentAct.TransferSumMoney.Factor.NdsValue.ToString("N2"));
                ReplaceText("[ActCostNDSValueWords]", CurrentAct.TransferSumMoney.Currency.MoneyInWords(CurrentAct.TransferSumMoney.Factor.NdsValue, true, false, true));
            }
            else
            {
                ReplaceText("[ActCost]", NoDataString);
                ReplaceText("[ActCostWords]", NoDataString);
                ReplaceText("[ActCostNDSValue]", NoDataString);
                ReplaceText("[ActCostNDSValueWords]", NoDataString);
            }

        }

        protected override void BuildReport()
        {
            base.BuildReport();
            BuildAdvance();
            BuildTransfer();
            BuildSignAuthorities();
        }

        protected override string ContractorData
        {
            get { return CurrentAct.ContractObject.Contractor.ContractorFullDesc; }
        }


    }
}
