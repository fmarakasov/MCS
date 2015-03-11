using System.Globalization;
using MCDomain.DataAccess;

namespace McReports.ViewModel
{
    public class Act4ReportViewModel : BaseActReportViewModel
    {
        public Act4ReportViewModel(IContractRepository contractRepository)
            : base(contractRepository)
        {

        }

        protected override string ActDate
        {
            get
            {
                if (CurrentAct.Signdate.HasValue)
                {
                    var s = string.Format("{0:dd MMMM yyyy} г.", CurrentAct.Signdate);
                    s = s.Substring(3);
                    return string.Format("\"{0:dd}\" " + s, CurrentAct.Signdate);
                }
                return NoDataString;
            }
        }

        protected override string ContractDate
        {
            get
            {
                if (CurrentAct.ContractObject.Approvedat.HasValue)
                {
                    var s = string.Format("{0:dd MMMM yyyy} г.", CurrentAct.ContractObject.Approvedat);
                    s = s.Substring(3);
                    return string.Format("\"{0:dd}\" " + s, CurrentAct.ContractObject.Approvedat);
                }
                return NoDataString;
            }
        }



        protected override string ContractNum
        {
            get
            {
                return string.Format("№ {0}", CurrentAct.ContractObject.MainContract.Num.ToString(CultureInfo.InvariantCulture));
            }
        }





        protected override string ReportFileName
        {
            get { return "Act4Report"; }
        }

        protected override void BuildStages()
        {
            for (int i = 0; i <= CurrentAct.Stages.Count - 1; i++)
            {
                ReplaceText("[StageNum]", CurrentAct.Stages[i].Num.ToString(CultureInfo.InvariantCulture));
                ReplaceText("[StageName]", CurrentAct.Stages[i].Subject.ToString(CultureInfo.InvariantCulture));
                ReplaceText("[StageCost]", CurrentAct.Stages[i].StageMoneyModel.Factor.PriceWithNdsValue.ToString("N2"));
                ReplaceText("[StageNdsValue]", CurrentAct.Stages[i].StageMoneyModel.Factor.NdsValue.ToString("N2"));

                if (i < CurrentAct.Stages.Count - 1) AddStagesTableRow();
            }

            if (CurrentAct.ActMoney.Price.HasValue)
            {
                ReplaceText("[StageCostSum]", CurrentAct.ActMoney.Factor.PriceWithNdsValue.ToString("N2"));
                ReplaceText("[NdsSum]", CurrentAct.ActMoney.Factor.NdsValue.ToString("N2"));
            }
            else
            {
                ReplaceText("[StageCostSum]", NoDataString);
                ReplaceText("[NdsSum]", NoDataString);
            }

        }

        private void AddStagesTableRow()
        {
            Word.ActiveDocument.Tables[2].Rows[Word.ActiveDocument.Tables[2].Rows.Count - 1].Select();
            Word.Selection.InsertRowsBelow(1);
            Word.Selection.TypeText("[StageNum]");
            Word.Selection.MoveRight(Microsoft.Office.Interop.Word.WdUnits.wdCharacter, 1);
            Word.Selection.TypeText("[StageName]");
            Word.Selection.MoveRight(Microsoft.Office.Interop.Word.WdUnits.wdCharacter, 1);
            Word.Selection.TypeText("[StageCost]");
            Word.Selection.MoveRight(Microsoft.Office.Interop.Word.WdUnits.wdCharacter, 1);
            Word.Selection.TypeText("[StageNdsValue]");
        }

        protected override void BuildHeader()
        {
            base.BuildHeader();
            ReplaceText("[Region]", RegionName);
        }

        protected override void BuildReport()
        {
            base.BuildReport();
            ReplaceText("[Authority]", string.Empty);
        }


    }
}
