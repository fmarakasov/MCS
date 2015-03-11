using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Office.Interop.Excel;
using MCDomain.DataAccess;
using MCDomain.Model;

namespace McReports.ViewModel
{
    public class InformationConcludedContracts_ViewModel : ExcelReportViewModel
    {
        public InformationConcludedContracts_ViewModel(IContractRepository contractRepository)
            : base(contractRepository)
        {
           
        }

        public override int StartPosition
        {
            get { return 7; }
        }

        protected override string MinExcelColumn
        {
            get { return "A"; }
        }

        protected override string MaxExcelColumn
        {
            get { return "T"; }
        }

        public override string DisplayName
        {
            get { return "Информация о заключенных договорах"; }
            protected set { base.DisplayName = value; }
        }

        protected override string OutputFileName
        {
            get
            {
                return "Информация о заключенных договорах";
            }
        }

        protected override string ReportFileName
        {
            get { return "InformationConcludedContracts"; }
        }

        public override string MainWorkSheetName
        {
            get { return ActiveWorksheet.Name; }
        }

        //находит договоры, утверждение которых, произошло в указанном диапазоне
        private IEnumerable<Contractdoc> SelectedContracts
        {
            get { return ActiveContractDocs; }
        }

        protected override void BuildReport()
        {
            // первая строка в таблице
            CurrentButtomPosition = StartPosition;

            //порядковый номер №п/п
            var i = 1;

            var reporter = CreateProgressReporter(SelectedContracts.Count());
            foreach (var item in SelectedContracts)
            {
                
                reporter.ReportProgress();                

                if (item != null)
                {
                    MainWorkSheet.Range["A" + CurrentButtomPosition.ToString(),
                    "T" + CurrentButtomPosition.ToString()].BorderAround2();

                    MainWorkSheet.Range["A" + CurrentButtomPosition.ToString(),
                    "T" + CurrentButtomPosition.ToString()].Borders[XlBordersIndex.xlInsideVertical].ColorIndex 
                    = XlColorIndex.xlColorIndexAutomatic;

                    MainWorkSheet.Cells[CurrentButtomPosition, 1] = i;
                                       
                    //ФИО руководителя
                    if (item.Contractor != null)
                    {
                        //наименование организации
                        MainWorkSheet.Cells[CurrentButtomPosition, 4] = item.Contractor.Name ?? "-";
                     
                        if (item.Contractor.Contractortype != null &&
                            item.Contractor.Contractortype.WellKnownType == WellKnownContractorTypes.Individual)
                        {
                            //ИНН
                            MainWorkSheet.Cells[CurrentButtomPosition, 2] = item.Contractor.Inn ?? "-";
                            //ФИО если физ. лицо
                            MainWorkSheet.Cells[CurrentButtomPosition, 6] = "-";
                            //серия и номер паспорта   
                            MainWorkSheet.Cells[CurrentButtomPosition, 7] = item.Contractor.PasportSeries != null &&
                                                                            item.Contractor.PasportNum != null
                                                                                ? item.Contractor.PasportSeries +
                                                                                  " " + item.Contractor.PasportNum
                                                                                : "-";
                        }
                        else
                        {
                            if (item.Person != null)
                            {
                                var sb = new StringBuilder();
                                sb.Append(item.Person.Familyname);
                                sb.Append(" ");
                                sb.Append(item.Person.Firstname);
                                sb.Append(" ");
                                sb.Append(item.Person.Middlename);
                                MainWorkSheet.Cells[CurrentButtomPosition, 6] = sb.ToString();
                            }
                            else MainWorkSheet.Cells[CurrentButtomPosition, 7] = "-";

                            MainWorkSheet.Cells[CurrentButtomPosition, 7] = "-";

                            //ОГРН
                            MainWorkSheet.Cells[CurrentButtomPosition, 3] = item.Contractor.Ogrn ?? "-";

                            //ОКВЭД
                            MainWorkSheet.Cells[CurrentButtomPosition, 5] = item.Contractor.Okved ?? "-";
                        }

                    }
                    //№ и дата договора (подписания)
                    if (item.Approvedat.HasValue)
                    {
                        var sb = new StringBuilder();
                        sb.Append(item.Num);

                        if (item.Approvedat.HasValue)
                        {
                            sb.Append(" от ");
                            sb.Append(item.Approvedat.Value.ToString("dd.MM.yyyy"));
                        }
                        MainWorkSheet.Cells[CurrentButtomPosition, 8] = sb.ToString();
                    }
                    //предмет договора
                    MainWorkSheet.Cells[CurrentButtomPosition, 9] = item.Subject ?? "-";          

                    MainWorkSheet.Range["A" + CurrentButtomPosition.ToString(),
                     "T" + CurrentButtomPosition.ToString()].RowHeight = +5;

                    var rh = MainWorkSheet.Range["A" + CurrentButtomPosition.ToString(),
                     "T" + CurrentButtomPosition.ToString()].Height;

                    //цена в млн. руб.
                    if (item.Price.HasValue && item.Measure != null)
                    {
                        var m = new MoneyModel(item.Ndsalgorithm, item.Nds, item.Currency, item.Measure, item.Price);

                        MainWorkSheet.Cells[CurrentButtomPosition, 10] = m.Factor.National.WithNdsValue/1000000;
                    }
                    else MainWorkSheet.Cells[CurrentButtomPosition, 10] = "-";


                    //срок действия (от даты принятия договора до окончания работ по нему)
                    MainWorkSheet.Cells[CurrentButtomPosition, 11] = ((item.Appliedat.HasValue)?item.Appliedat.Value.ToString("dd.MM.yyyy"):"-") + " - " + ((item.Endsat.HasValue) ? item.Endsat.Value.ToString("dd.MM.yyyy"):"-");

                    //иные условия
                    MainWorkSheet.Cells[CurrentButtomPosition, 12] = item.Description ?? "-";

                    //бенефициары
                    ++CurrentButtomPosition;
                    ++i;
                }

                MainWorkSheet.Rows.AutoFit();
                SetPrintingArea(MainWorkSheet);
                
                reporter.Next();

            }
        }
    }
}
