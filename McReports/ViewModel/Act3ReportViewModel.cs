using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using MCDomain.DataAccess;
using MCDomain.Model;

namespace McReports.ViewModel
{
    public class Act3ReportViewModel: BaseActReportViewModel
    {
        public Act3ReportViewModel(IContractRepository contractRepository)
            : base(contractRepository)
        {

        }

        private string SelectString(string value, string constStr, bool quoted = false)
        {
            return string.IsNullOrEmpty(value)
                       ? constStr + " N/A"
                       : string.Format("{0} {1}", constStr, quoted ? "\"" + value + "\"" : value);
        }

        protected override string ActDate
        {
            get
            {
                if  (CurrentAct.Signdate.HasValue)
                {
                    var s = string.Format("{0:dd MMMM yyyy} г.", CurrentAct.Signdate);
                    s = s.Substring(3);
                    return string.Format("\"{0:dd}\" " + s, CurrentAct.Signdate);
                }
                else 
                    return NoDataString;
            }
        }

        protected override string ContractorData
        {
            get
            {
                var sb = new StringBuilder();
                
                sb.Append(!String.IsNullOrWhiteSpace(CurrentAct.ContractObject.Contractor.Zip)
                              ? SelectString(CurrentAct.ContractObject.Contractor.Zip + ", " + CurrentAct.ContractObject.Contractor.Address, "Адрес: ")
                              : SelectString(CurrentAct.ContractObject.Contractor.Address, "Адрес: "));
                sb.Append((char) 13);
                sb.Append(SelectString(CurrentAct.ContractObject.Contractor.Account, "р/c № "));
                sb.Append((char) 13);
                sb.Append(SelectString(CurrentAct.ContractObject.Contractor.Bank, "в "));
                sb.Append((char) 13);
                sb.Append((char) 13);

                sb.Append(SelectString(CurrentAct.ContractObject.Contractor.Inn, "ИНН "));
                sb.Append((char) 13);
                sb.Append(SelectString(CurrentAct.ContractObject.Contractor.Kpp, "КПП "));
                sb.Append((char) 13);
                sb.Append(SelectString(CurrentAct.ContractObject.Contractor.Correspaccount, "к/c № "));
                sb.Append((char) 13);
                sb.Append((char) 13);
                sb.Append(SelectString(CurrentAct.ContractObject.Contractor.Bik, "БИК "));
                sb.Append((char) 13);
                return sb.ToString();
            }
        }

        private string EnterpriseAuthority
        {
            get
            {
                var sb = new StringBuilder();
                if (CurrentAct.Enterpriseauthority != null)
                {
                    if (CurrentAct.Enterpriseauthority.Authority != null) sb.Append(CurrentAct.Enterpriseauthority.Authority);
                    if (CurrentAct.Enterpriseauthority.Num != "")
                    {
                        sb.Append(" № ");
                        sb.Append(CurrentAct.Enterpriseauthority.Num);
                    }

                    if (CurrentAct.Enterpriseauthority.Validfrom.HasValue)
                    {
                        sb.Append(" от ");
                        sb.Append(CurrentAct.Enterpriseauthority.Validfrom.Value.ToShortDateString());
                        sb.Append(" г.");
                    }    
                }

                return sb.ToString();
            }
        }

        protected override void BuildStages()
        {
            for (int i = 0; i <= CurrentAct.Stages.Count - 1; i++)
            {
                ReplaceText("[StageNum]", CurrentAct.Stages[i].Num.ToString(CultureInfo.InvariantCulture));
                ReplaceText("[ObjNum]", CurrentAct.Stages[i].Code);
                ReplaceText("[StageName]", CurrentAct.Stages[i].Subject.ToString(CultureInfo.InvariantCulture));
                ReplaceText("[StageCost]", CurrentAct.Stages[i].StageMoneyModel.Factor.PureValue.ToString("N2"));
                ReplaceText("[Region]", CurrentAct.Region != null ? CurrentAct.Region.ToString():"");

                if (i < CurrentAct.Stages.Count - 1) AddStagesTableRow();
            }

            if (CurrentAct.ActMoney.Price.HasValue)
            {
                ReplaceText("[StageCostSum]", CurrentAct.ActMoney.Factor.PureValue.ToString("N2"));
                ReplaceText("[StageCostSumWords]", CurrentAct.ActMoney.Currency.MoneyInWords(CurrentAct.ActMoney.Factor.PureValue, true, false, true));
                ReplaceText("[NdsSum]", CurrentAct.ActMoney.Factor.NdsValue.ToString("N2"));
            }
            else
            {
                ReplaceText("[StageCostSum]", NoDataString);
                ReplaceText("[StageCostSumWords]", NoDataString);
                ReplaceText("[NdsSum]", NoDataString);
            }

        }

        private void AddStagesTableRow()
        {
            Word.ActiveDocument.Tables[3].Rows[Word.ActiveDocument.Tables[2].Rows.Count - 1].Select();
            Word.Selection.InsertRowsBelow(1);
            Word.Selection.TypeText("[StageNum]");
            Word.Selection.MoveRight(Microsoft.Office.Interop.Word.WdUnits.wdCharacter, 1);
            Word.Selection.TypeText("[ObjNum]");
            Word.Selection.MoveRight(Microsoft.Office.Interop.Word.WdUnits.wdCharacter, 1);
            Word.Selection.TypeText("[Region]");
            Word.Selection.MoveRight(Microsoft.Office.Interop.Word.WdUnits.wdCharacter, 1);
            Word.Selection.TypeText("[StageName]");
            Word.Selection.MoveRight(Microsoft.Office.Interop.Word.WdUnits.wdCharacter, 1);
            Word.Selection.TypeText("[StageCost]");
        }

        private string ContractorFullSpec
        {
            get
            {
                var sb = new StringBuilder();
                if (CurrentAct.ContractObject.IsAgreement)
                {
                    sb.Append("Дополнительному соглашению №");
                    sb.Append(CurrentAct.ContractObject.Agreementnum.HasValue
                                  ? CurrentAct.ContractObject.Agreementnum.Value.ToString()
                                  : "<не задан>");
                    sb.Append((char) 13);
                    sb.Append("к ");
                }
                else
                {
                    sb.Append("по ");
                }

                sb.Append("Договору ");
                Contractdoc.AddInternalnum("", CurrentAct.ContractObject.MainContract, sb);
                Contractdoc.AddApproved(CurrentAct.ContractObject.MainContract, sb);
                sb.Append(" г.");
                return sb.ToString();
            }
        }

        protected override void BuildHeader()
        {
            base.BuildHeader();
            ReplaceText("[EnterpriseAuthority]", EnterpriseAuthority);
            ReplaceText("[ContractFullSpec]", ContractorFullSpec);
            
        }


        protected override string ReportFileName
        {
            get { return "Act3Report"; }
        }

    }


}
