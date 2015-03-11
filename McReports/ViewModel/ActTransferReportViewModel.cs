using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MCDomain.DataAccess;
using MCDomain.Model;
using UOW;

namespace McReports.ViewModel
{
    public class ActTransferReportViewModel : WordReportViewModel
    {
        public ActTransferReportViewModel(IContractRepository repository) : base(repository)
        {
           
        }

        protected override string OutputFileName
        {
            get
            {
                return "Акт приёмки и передачи договоров";
            }
        }


        public long TransferActId
        {
            get; set;
        }

        private Transferact Act
        {
           get { return  UnitOfWork.Repository<Transferact>().Single((x) => x.Id == TransferActId); }
        }


        protected override string ReportFileName
        {
            get { return "ActTransfer"; }
        }

        public override string DisplayName
        {
            get
            {
                return "Акт приёмки и передачи договоров";
            }
            protected set
            {
                base.DisplayName = value;
            }
        }



        private void AddTableRow()
        {
            Word.ActiveDocument.Tables[1].Rows[Word.ActiveDocument.Tables[1].Rows.Count].Select();
            Word.Selection.InsertRowsBelow(1);
            Word.Selection.TypeText("[n]");
            Word.Selection.MoveRight(Microsoft.Office.Interop.Word.WdUnits.wdCharacter, 1);
            Word.Selection.TypeText("[contractName] от [contractDate]");
            Word.Selection.MoveRight(Microsoft.Office.Interop.Word.WdUnits.wdCharacter, 1);
            Word.Selection.TypeText("[ContractorName]");
            Word.Selection.MoveRight(Microsoft.Office.Interop.Word.WdUnits.wdCharacter, 1);
            Word.Selection.TypeText("[Price]");
            Word.Selection.MoveRight(Microsoft.Office.Interop.Word.WdUnits.wdCharacter, 1);
            Word.Selection.TypeText("[DocumentName]");
            Word.Selection.MoveRight(Microsoft.Office.Interop.Word.WdUnits.wdCharacter, 1);
            Word.Selection.TypeText("[PageCount]");  			
        }

        protected override void BuildReport()
        {
            int i = 1;
            int n = 0;
           
            // Шапка
            //номер акта приемки и передачи договоров
            ReplaceText("[ActNum]", Act.Num.HasValue        
                ? Act.Num.GetValueOrDefault().ToString()
                : "-");

            //дата составления акта
            ReplaceText("[ActDate]", Act.Signdate.HasValue  
                ? Act.Signdate.GetValueOrDefault().ToShortDateString()
                : DateTime.Today.ToShortDateString()); 
            
            //ReplaceText("[PostManContract]", );   //должность сотрудника договорного отдела
            //ReplaceText("[NameManContract]", );   //ФИО сотрудника договрного отдела
            //ReplaceText("[NameChief]", );         //ФИО начальника отдела имущественных отношений и соц. развития
            //ReplaceText("[NumOrder]", );          //№ приказа, на основании которого составляется акт приемки-передачи
            //ReplaceText("[DayOrder]", );          //день
            //ReplaceText("[MonthOrder]", );        //месяц
            //ReplaceText("[YearOrder]", );         //год приказа 
           
            //Таблица
            var grouped = Act.Contracttranactdocs.GroupBy(x => x.Contractdoc);

            foreach (var contract in grouped)
            {
                ReplaceText("[n]", i.ToString());
                if ((contract.Key.Internalnum != null) && contract.Key.Approvedat.HasValue)
                {
                    ReplaceText("[contractName]", contract.Key.Internalnum);
                    ReplaceText("[contractDate]", contract.Key.Approvedat.GetValueOrDefault().ToShortDateString());
                }
                else
                {
                    ReplaceText("[contractName] от [contractDate]","-");
                }

                if (contract.Key.Price.HasValue&&contract.Key.Measure!=null)
                {
                    ReplaceText("[Price]", contract.Key.ToFactor(1000).ToString());
                }
                else ReplaceText("[Price]", contract.Key.Price.HasValue ? contract.Key.Price.Value.ToString() : "-");
                         

                ReplaceText("[ContractorName]", contract.Key.Contractor.Name.Any()
                    ? contract.Key.Contractor.Name
                    : "-");

                int j = 1; 
                foreach (var contracttranactdoc in contract)
                {
                   ReplaceText("[DocumentName]", contracttranactdoc.Document.Name);
                   Word.ActiveDocument.Tables[1].Cell(Word.ActiveDocument.Tables[1].Rows.Count,5).Select();
                   if (j<contract.Count()) 
                        Word.Selection.TypeText(Word.Selection.Text.Substring(0, Word.Selection.Text.Length-1) + "[DocumentName]");
                   ReplaceText("[PageCount]", contracttranactdoc.Pagescount.HasValue
                        ? contracttranactdoc.Pagescount.ToString()
                        : "-");
                   Word.ActiveDocument.Tables[1].Cell(Word.ActiveDocument.Tables[1].Rows.Count,6).Select();
                   if (j < contract.Count()) 
                        Word.Selection.TypeText(Word.Selection.Text.Substring(0, Word.Selection.Text.Length - 1) + "[PageCount]");

                   ++j; //чтобы не добавлять лишние [DocumentName] и [PageCount]
                   ++n;
                }

                if (i < grouped.Count()) AddTableRow();
                ++ i; //чтобы не добавлять лишние строки в таблице
            }
            ReplaceText("[№]", n.ToString()); //количество переданных документов
        }
    }
}
