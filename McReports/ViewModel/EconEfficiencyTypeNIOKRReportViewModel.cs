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
    public class EconEfficiencyTypeNIOKRReportViewModel : ExcelReportViewModel
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

        public EconEfficiencyTypeNIOKRReportViewModel(IContractRepository contractRepository)
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
                return "Эффект от использования результатов разработок, выполненных ОАО \"Газпром промгаз\"";
            }
            protected set
            {
                base.DisplayName = value;
            }
        }

        protected override string OutputFileName
        {
            get { return "Эффект от использования результатов разработок, выполненных ОАО \"Газпром промгаз\""; }
        }

        protected override string ReportFileName
        {
            get { return "EconEffectNIOKRReport"; }
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

        private List<ComparativeStageresultDto> _results
        {
            get
            {
                IEnumerable<Contractdoc> contractdocs = ActiveContractDocs;



                if (contractdocs != null && contractdocs.Any())
                {
                    var sr = contractdocs.SelectMany(cc => cc.Stages).Where(cs => cs.GetConditionOnDate(StartPrevPeriod) != StageCondition.Closed && cs.Stageresults.Any(ss => ss.Efparameterstageresults.Any(e => e.Economefficiencyparameter != null))).SelectMany(x => x.Stageresults);

                    var r = sr.Aggregate
                                        (new List<ComparativeStageresultDto>(), (a, c) =>
                                        {
                                            a.Add(new ComparativeStageresultDto()
                                            {
                                                
                                                Stage = c.Stage,
                                                Stageresult = c,
                                                Contractdoc = c.Stage.ContractObject,

                                                PrevyearItem = (c.Stage.GetConditionOnDate(EndPrevPeriod) == StageCondition.Closed) ? 1 : 0,
                                                PrevyearMoney = (c.Stage.GetConditionOnDate(EndPrevPeriod) == StageCondition.Closed) ? (c.Stage.StageMoneyModel.Factor.National.WithNdsValue/ c.Stage.Stageresults.Count()) / 1000 : 0,
                                                CurrentyearItem = (c.Stage.GetConditionOnDate(EndPrevPeriod) != StageCondition.Closed && c.Stage.GetConditionOnDate(EndPeriod) == StageCondition.Closed) ? 1 : 0,
                                                CurrentyearMoney = (c.Stage.GetConditionOnDate(EndPrevPeriod) != StageCondition.Closed && c.Stage.GetConditionOnDate(EndPeriod) == StageCondition.Closed) ? (c.Stage.StageMoneyModel.Factor.National.WithNdsValue / c.Stage.Stageresults.Count()) / 1000 : 0,

                                                PrevEconEffectMln = (c.Stage.GetConditionOnDate(EndPrevPeriod) == StageCondition.Closed) ? c.Efparameterstageresults.Where(e=>e.Economefficiencyparameter.WellKnownType == WellKnownEconomicEfficiencyParameters.EconEffMln && e.Value.HasValue).Sum(s=>1000*s.Value.Value):0,
                                                EconEffectMln = (c.Stage.GetConditionOnDate(EndPrevPeriod) != StageCondition.Closed && c.Stage.GetConditionOnDate(EndPeriod) == StageCondition.Closed) ? c.Efparameterstageresults.Where(e => e.Economefficiencyparameter.WellKnownType == WellKnownEconomicEfficiencyParameters.EconEffMln && e.Value.HasValue).Sum(s => 1000*s.Value.Value) : 0

                                            });
                                            return a;
                                        });

                    return r;
                }
                return null;
            }
        }



        /*

  protected override void BuildReport()
  {


      CurrentButtomPosition = StartPosition;

      SetHeader();

     

      if (_results.Any())
      {


          var efftypes = _results.Select(c => c.Stageresult.Economefficiencytype).Distinct().OrderBy(t => t.Name);
                
          var reporter = CreateProgressReporter(2*efftypes.Count());
          reporter.ReportProgress();


          IList<WellKnownContractorTypes> lst = new List<WellKnownContractorTypes>();
          lst.Add(WellKnownContractorTypes.Gazprom);
          BuildFinPart(lst, efftypes,  reporter, 1);
          lst.Add(WellKnownContractorTypes.Subsidiary);
          BuildEconEffPart(WellKnownContractorTypes.Subsidiary, efftypes, reporter, 2);

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

  
  private void BuildFinPart(IEnumerable<WellKnownContractorTypes> contractortypes, IEnumerable<Efficienceparametertype> efftypes, ReportCurrentProgress reporter, int inum)
  {

      var effresults = _results.Where(
          c =>
          c.Contractdoc.Contractorcontractdocs.Any(
              x =>
              x.Contractor != null && x.Contractor.Contractortype != null &&
              contractortypes.Any(t => x.Contractor.Contractortype.WellKnownType == t))).Distinct();


      BuildGroup(contractortypes, effresults);
      int i = 1;
      foreach (var t in efftypes)
      {

          reporter.ReportProgress();
          BuildFinResult(t, effresults, inum, i);
          i++;
          // Уведомление о продвижении на один шаг
          reporter.Next();
      }
  }


  private void BuildEconEffPart(IEnumerable<WellKnownContractorTypes> contractortypes, IEnumerable<Efficienceparametertype> efftypes, ReportCurrentProgress reporter, int inum)
  {
  

      var effresults = _results.Where(
          c =>
          c.Contractdoc.Contractorcontractdocs.Any(
              x =>
              x.Contractor != null && x.Contractor.Contractortype != null &&
              contractortypes.Any(t => x.Contractor.Contractortype.WellKnownType == t))).Distinct();


      BuildGroup(contractortypes, effresults);
      int i = 1;
      foreach (var t in efftypes)
      {

          reporter.ReportProgress();
          BuildResult(t, effresults, inum, i);
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
  */

        protected Worksheet GroupWorkSheet
        {
            get
            {
                return Excel.Worksheets["Group"];
            }
        }



    }


}
