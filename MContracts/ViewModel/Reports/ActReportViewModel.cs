using System;
using System.Globalization;
using System.Linq;
using MCDomain.DataAccess;
using MCDomain.Model;

namespace MContracts.ViewModel.Reports
{
    public class ActReportViewModel: WordReportViewModel
    {
        public ActReportViewModel(IContractRepository contractRepository)
            : base(contractRepository)
        {
        
        }

       
        public override string DisplayName
        {
            get
            {
                return "Акт сдачи-приёмки работ";
            }
            protected set
            {
                base.DisplayName = value;
            }
        }

        public override string ReportFileName
        {
            get { return "ActReport"; }
        }

        public Act CurrentAct { get; set; }


        private void BuildPerson()
        {

            if (CurrentAct.ContractObject.Contractor != null)
            {
                Person pers = (CurrentAct.ContractObject.Contractor.Persons.FirstOrDefault(p => p.Actsignauthority && p.Contractheadauthority && p.Valid) ??
                               CurrentAct.ContractObject.Contractor.Persons.FirstOrDefault(p => p.Contractheadauthority&&p.Valid)) ??
                              CurrentAct.ContractObject.Contractor.Persons.FirstOrDefault(p => p.Actsignauthority && p.Valid);


                if (pers != null)
                {
                    ReplaceText("[ContractorPerson]", pers.PostName(CurrentAct.ContractObject.Contractor));
                    ReplaceText("[ContractorPersonPost]", pers.Contractorposition.ToString());
                    ReplaceText("[ContractorPersonName]", pers.GetShortFullName());
                }
                else
                {
                    ReplaceText("[ContractorPerson]", "<нет данных>");
                    ReplaceText("[ContractorPersonPost]", "<нет данных>");
                    ReplaceText("[ContractorPersonName]", "<нет данных>");
                }
            }
            else
            {
                ReplaceText("[ContractorPerson]", "<нет данных>");
                ReplaceText("[ContractorPersonPost]", "<нет данных>");
                ReplaceText("[ContractorPersonName]", "<нет данных>");
            }

        }


        private void BuildSignAuthorities()
        {
            if (CurrentAct.ContractObject.Functionalcustomercontracts.Any())
            {
                Functionalcustomer fc = CurrentAct.ContractObject.Functionalcustomercontracts[0].Functionalcustomer;
                Funccustomerperson p = fc.Funccustomerpersons.FirstOrDefault(x => x.Person.Valid && x.Person.Actsignauthority);


                if (p != null)
                {
                    ReplaceText("[FZPost]", p.Person.Contractorposition.ToString());
                    ReplaceText("[FZPostName]", p.Person.GetShortFullName());

                }
                else
                {
                    ReplaceText("[FZPost]", "<нет данных>");
                    ReplaceText("[FZPostName]", "<нет данных>");
                }
                
                // если заказчик - департамент, возвращаем первое способное на подпись лицо
                if (fc.Level == 0) // департамент
                {
                    ReplaceText("[FZDepartmentPost]", "");
                    ReplaceText("[FZDepartmentName]", "");
                }
                else
                {
                    if (fc.Parent != null)
                    {
                        var functionalcustomer = fc.Parent as Functionalcustomer;
                        if (functionalcustomer != null)
                            p = functionalcustomer.Funccustomerpersons.FirstOrDefault(x => x.Person.Valid && x.Person.Actsignauthority);
                        if (p != null)
                        {
                            ReplaceText("[FZDepartmentPost]", p.Person.Contractorposition.ToString());
                            ReplaceText("[FZDepartmentName]", p.Person.GetShortFullName());
                        }
                        else
                        {
                            ReplaceText("[FZDepartmentPost]", "<нет данных>");
                            ReplaceText("[FZDepartmentName]", "<нет данных>");
                        }
                    }
                    else
                    {
                        ReplaceText("[FZDepartmentPost]", "<нет данных>");
                        ReplaceText("[FZDepartmentName]", "<нет данных>");
                    }
                }
            }
            else
            {
                ReplaceText("[FZDepartmentPost]", "<нет данных>");
                ReplaceText("[FZDepartmentName]", "<нет данных>");
                ReplaceText("[FZPost]", "<нет данных>");
                ReplaceText("[FZPostName]", "<нет данных>");
            }
        }

        private void BuildStages()
        {
            for (int i = 0; i <= CurrentAct.Stages.Count - 1; i++)
            {
                ReplaceText("[StageNum]", CurrentAct.Stages[i].Num.ToString(CultureInfo.InvariantCulture));
                ReplaceText("[StageName]", CurrentAct.Stages[i].Subject.ToString(CultureInfo.InvariantCulture));

                ReplaceText("[StageCost]", CurrentAct.Stages[i].StageMoneyModel.Factor.PureValue.ToString("N2"));
                
                if (i < CurrentAct.Stages.Count - 1) AddTableRow();
            }

            if (CurrentAct.ActMoney.Price.HasValue) 
            {
              ReplaceText("[StageCostSum]", CurrentAct.ActMoney.Factor.PureValue.ToString("N2"));            
              ReplaceText("[StageCostSumWords]", CurrentAct.ActMoney.Currency.MoneyInWords(CurrentAct.ActMoney.Factor.PureValue, true, false, true));
            }
            else
            {
              ReplaceText("[StageCostSum]", "<нет данных>");
              ReplaceText("[StageCostSumWords]", "<нет данных>");
            }

            //([NDSPercent]) – NDSValue ([NDSValueWords]).

            


        }

        private void AddTableRow()
        {
            Word.ActiveDocument.Tables[2].Rows[Word.ActiveDocument.Tables[2].Rows.Count - 1].Select();
            Word.Selection.InsertRowsBelow(1);
            Word.Selection.TypeText("Этап [StageNum]");
            Word.Selection.MoveRight(Microsoft.Office.Interop.Word.WdUnits.wdCharacter, 1);
            Word.Selection.TypeText("[StageName]");
            Word.Selection.MoveRight(Microsoft.Office.Interop.Word.WdUnits.wdCharacter, 1);
            Word.Selection.TypeText("[StageCost]");
        }


        protected override void BuildReport()
        {
            if (CurrentAct.ContractObject.Contractor != null)
            {
                ReplaceText("[ContractorName]", CurrentAct.ContractObject.Contractor.ToString());
                ReplaceText("[ContractorData]", CurrentAct.ContractObject.Contractor.ContractorFullDesc);
                
            }
            else
            {
                ReplaceText("[ContractorName]", "<нет данных>");
                ReplaceText("[ContractorData]", "<нет данных>");
            }

            ReplaceText("[ActNum]", CurrentAct.Num.ToString(CultureInfo.InvariantCulture));

            ReplaceText("[ActDate]",
                        CurrentAct.Signdate.HasValue
                            ? string.Format("{0: dd MMMM yyyy}", CurrentAct.Signdate)
                            : "<нет данных>");
            ReplaceText("[ContractNum]", CurrentAct.ContractObject.Num.ToString(CultureInfo.InvariantCulture));
            ReplaceText("[ContractName]", CurrentAct.ContractObject.Subject);
            ReplaceText("[ContractDate]", string.Format("{0: dd.MM.yyyy}", CurrentAct.ContractObject.Approvedat));
            
            BuildPerson();
            
          
            BuildStages();

            ReplaceText("[NDSPercent]", CurrentAct.Nds != null ? CurrentAct.Nds.ToString() : "<нет даннных>");


            if (CurrentAct.ActMoney.Price.HasValue)
            {
                ReplaceText("[NDSValue]", CurrentAct.ActMoney.Factor.NdsValue.ToString("N2"));
                ReplaceText("[NDSValueWords]", CurrentAct.ActMoney.Currency.MoneyInWords(CurrentAct.ActMoney.Factor.NdsValue, true, false, true));
            }
            else
            {
                ReplaceText("[NDSValue]", "<нет данных>");
                ReplaceText("[NDSValueWords]", "<нет данных>");
            }



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
                ReplaceText("[AdvanceSum]", "<нет данных>");
                ReplaceText("[AdvanceSumWords]", "<нет данных>");
            }

            if (CurrentAct.TransferSumMoney.Price.HasValue)
            {
                ReplaceText("[ActCost]", CurrentAct.TransferSumMoney.Factor.WithNdsValue.ToString("N2"));
                ReplaceText("[ActCostWords]", CurrentAct.TransferSumMoney.Currency.MoneyInWords(CurrentAct.TransferSumMoney.Factor.WithNdsValue, true, false, true));
                ReplaceText("[ActCostNDSValue]", CurrentAct.TransferSumMoney.Factor.NdsValue.ToString("N2"));
                ReplaceText("[ActCostNDSValueWords]", CurrentAct.TransferSumMoney.Currency.MoneyInWords(CurrentAct.TransferSumMoney.Factor.NdsValue, true, false, true));
            }
            else
            {
                ReplaceText("[ActCost]", "<нет данных>");
                ReplaceText("[ActCostWords]", "<нет данных>");
                ReplaceText("[ActCostNDSValue]", "<нет данных>");
                ReplaceText("[ActCostNDSValueWords]", "<нет данных>");
            }

            ReplaceText("[Year]",
                        CurrentAct.Signdate.HasValue
                            ? String.Format("{0: yyyy}", CurrentAct.Signdate.Value)
                            : String.Format("{0: yyyy}", DateTime.Now));


            BuildSignAuthorities();
            ReplaceText("N/A", "<нет данных>");
            
        }
    }
}
