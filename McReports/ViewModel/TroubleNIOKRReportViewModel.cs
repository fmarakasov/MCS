using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CommonBase.Progress;
using McReports.Common;
using McReports.DTO;
using MCDomain.Common;
using MCDomain.DataAccess;
using MCDomain.Model;
using CommonBase;
using Microsoft.Office.Interop.Excel;

namespace McReports.ViewModel
{
    public class TroubleNIOKRReportViewModel : ExcelReportViewModel
    {
        private int Defaultwidth = 70;

        public override int DefaultRowHeight
        {
            get { return 15; }
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
            get
            {
                return "E";
            }
        }

        public int  PrevYear
        {
            get { return Year - 1; }
        }

        public TroubleNIOKRReportViewModel(IContractRepository contractRepository)
            : base(contractRepository)
        {
            Year = DateTime.Today.Year;
            
        }


        protected override void OnPeriodChanged()
        {
            Year = Period.End.Year;
            base.OnPeriodChanged();
        }


        [InputParameter(typeof(int), "Год")]
        public int Year { get; set; }

        public override string DisplayName
        {
            get
            {
                return "НИОКР, выполненные ОАО 'Газпром промгаз'";
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
                return string.Format("НИОКР, выполненные ОАО Газпром промгаз в {0}-{1} гг., по приоритетным проблемам ОАО Газпром", PrevYear.ToString(), Year.ToString());
            }
        }

        protected override string ReportFileName
        {
            get { return "TroubleNIOKRReport"; }
        }

        protected DateTime StartPrevPeriod
        {
            get { return DateTimeExtensions.GetFirstYearDay(PrevYear); }
        }

        protected DateTime EndPrevPeriod
        {
            get { return DateTimeExtensions.GetLastYearDay(PrevYear); }
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

        private List<TroubleContractDto> _results
        {
            get
            {
                IEnumerable<Contractdoc> contractdocs = ActiveContractDocs;



                if (contractdocs != null && contractdocs.Any())
                {
                    var sr = contractdocs.SelectMany(cc => cc.Stages).Where(cs => cs.GetConditionOnDate(StartPrevPeriod) != StageCondition.Closed && cs.Stageresults.Any(ss => ss.Efparameterstageresults.Any(e => e.Economefficiencyparameter != null)));

                    var r = sr.Aggregate
                                        (new List<TroubleContractDto>(), (a, c) =>
                                        {
                                            a.Add(new TroubleContractDto()
                                            {
                                                Contractdoc = c.ContractObject,
                                                Stage = c,
                                                PrevyearItem = (c.GetConditionOnDate(EndPrevPeriod) == StageCondition.Closed)?c.Stageresults.Count:0,
                                                PrevyearMoney = (c.GetConditionOnDate(EndPrevPeriod) == StageCondition.Closed) ? c.StageMoneyModel.Factor.National.WithNdsValue / 1000 : 0,
                                                CurrentyearItem = (c.GetConditionOnDate(EndPrevPeriod) != StageCondition.Closed && c.GetConditionOnDate(EndPeriod) == StageCondition.Closed) ? c.Stageresults.Count : 0,
                                                CurrentyearMoney = (c.GetConditionOnDate(EndPrevPeriod) != StageCondition.Closed && c.GetConditionOnDate(EndPeriod) == StageCondition.Closed) ? c.StageMoneyModel.Factor.National.WithNdsValue / 1000 : 0

                                            });
                                            return a;
                                        });

                    return r;
                }
                throw new NoReportDataException();
            }
        }





        protected override void BuildReport()
        {


            CurrentButtomPosition = StartPosition;

            SetHeader();

     

            if (_results.Any())
            {


                var troubles = _results.SelectMany(c => c.Contractdoc.Contracttroubles).Select(c => c.Trouble).Distinct().OrderBy(t => t.Num);
                
                var reporter = CreateProgressReporter(2*troubles.Count());
                reporter.ReportProgress();



                BuildPart(WellKnownContractorTypes.Gazprom, troubles,  reporter, 1);
                BuildPart(WellKnownContractorTypes.Subsidiary, troubles, reporter, 2);

                SetPrintingArea(ActiveWorksheet);
            }
        }

        protected override void SetHeader()
        {
            //Для ускорения вводим диапозон ячеек для замены (задаем диапозон больше, чем требуется)
            Range r = MainWorkSheet.Range["A1", "E2"];
            r.Replace("#Year#", Year);
            r.Replace("#PrevYear#", PrevYear);
        }

        private void BuildPart(WellKnownContractorTypes contractortype, IEnumerable<Trouble> troubles, ReportCurrentProgress reporter, int inum)
        {
  

            IEnumerable<TroubleContractDto> contracttroubles = _results;
            var troublelist = contracttroubles.Where(
                            c =>
                                c.Contractdoc.Contractorcontractdocs.Any(
                                    x =>
                                    x.Contractor != null && x.Contractor.Contractortype != null &&
                                    x.Contractor.Contractortype.WellKnownType == contractortype && 
                                    x.Contractdoc.Contracttroubles.Any())).Distinct();


            BuildGroup(contractortype, troublelist);
            int i = 1;
            foreach (var t in troubles)
            {

                reporter.ReportProgress();
                BuildResult(t, troublelist, inum, i);
                i++;
                // Уведомление о продвижении на один шаг
                reporter.Next();
            }
        }


        private void BuildGroup(WellKnownContractorTypes contractortype, IEnumerable<TroubleContractDto> troublelist)
        {

            string sGroupname = "";
            string sNum = "";
            switch (contractortype)
            {
                case WellKnownContractorTypes.Gazprom:
                    {
                        sGroupname = "Количество и стоимость результатов НИОКР, выполненных по заказу ОАО \"Газпром\"";
                        sNum = "1";
                        break;
                    }
                case WellKnownContractorTypes.Subsidiary:
                    {
                        sGroupname =
                            "Количество и стоимость результатов НИОКР, выполненных по заказу  дочерних обществ ОАО \"Газпром\"";
                        sNum = "2";
                        break;
                    }
            }
            Range r = CopyRowRange(GroupWorkSheet, 2);
            r.Replace("#TroubleNum#", sNum);
            r.Replace("#TroubleName#", sGroupname);
            r.Replace("#PrevYearItem#", troublelist.Sum(x => x.PrevyearItem));
            r.Replace("#PrevYearMoney#", troublelist.Sum(x => x.PrevyearMoney).ToString(NumberFormat));
            r.Replace("#YearItem#", troublelist.Sum(x => x.CurrentyearItem));
            r.Replace("#YearMoney#", troublelist.Sum(x => x.CurrentyearMoney).ToString(NumberFormat));

            var iRowCount = GetLongStringRowsCount(sGroupname, Defaultwidth, 1);
            r.RowHeight = DefaultRowHeight * iRowCount ;
        }

        private void BuildResult(Trouble trouble, IEnumerable<TroubleContractDto> troublelist, int mainnum, int num)
        {
            Range r = CopyRowRange(DetailWorkSheet, 2);
            
            r.Replace("#TroubleNum#", string.Format( "  {0}.{1}", mainnum.ToString(), num.ToString()));
            InsertBigFieldIntoReport("#TroubleName#", trouble.ToString(), r);
            r.Replace("#PrevYearItem#", troublelist.Where(x => x.Contractdoc.Contracttroubles.Any(c => c.Trouble.Id == trouble.Id)).Distinct().Sum(x => x.PrevyearItem));
            r.Replace("#PrevYearMoney#", troublelist.Where(x => x.Contractdoc.Contracttroubles.Any(c => c.Trouble.Id == trouble.Id)).Distinct().Sum(x => x.PrevyearMoney).ToString(NumberFormat));
            r.Replace("#YearItem#", troublelist.Where(x => x.Contractdoc.Contracttroubles.Any(c => c.Trouble.Id == trouble.Id)).Distinct().Sum(x => x.CurrentyearItem));
            r.Replace("#YearMoney#", troublelist.Where(x => x.Contractdoc.Contracttroubles.Any(c => c.Trouble.Id == trouble.Id)).Distinct().Sum(x => x.CurrentyearMoney).ToString(NumberFormat));

            var iRowCount = GetLongStringRowsCount(trouble.ToString(), Defaultwidth, 1);
            r.RowHeight = DefaultRowHeight * iRowCount;
         }

        protected Worksheet GroupWorkSheet
        {
            get
            {
                return Excel.Worksheets["Group"];
            }
        }



    }


}
