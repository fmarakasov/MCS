using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using McReports.Common;
using McReports.DTO;
using MCDomain.DataAccess;
using MCDomain.Model;
using CommonBase;
using Microsoft.Office.Interop.Excel;

namespace McReports.ViewModel
{
    public class NIOKREconomicEffectReportViewModel : ExcelReportViewModel
    {

        public override int StartPosition
        {
            get { return 5; }
        }

        protected override string MinExcelColumn
        {
            get { return "A"; }
        }

        protected override string MaxExcelColumn
        {
            get
            {
                return "P";
            }
        }

        public NIOKREconomicEffectReportViewModel(IContractRepository contractRepository)
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
                return "Сведения о внедрении и использовании результатов НИОКР, для которых оценивается экономический эффект";
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

                return string.Format("Сведения о внедрении и использовании результатов НИОКР(ЭЭ) {0} год", Year.ToString());
            }
        }

        protected override string ReportFileName
        {
            get { return "NIOKREconEfficiencyReport"; }
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

        private List<StageresultDto> Results
        {
            get
            {
                var contractdocs = ActiveContractDocs.ToArray();



                if (contractdocs.Any())
                {
                    var sr = contractdocs.SelectMany(cc => cc.Stages)
                        .Where(cs => cs.GetConditionOnDate(StartPeriod) != StageCondition.Closed)
                        .SelectMany(ss => ss.Stageresults)
                        .Where(s => s.Efparameterstageresults
                            .Any(e => e.Economefficiencyparameter != null
                                && (e.Economefficiencyparameter.WellKnownType == WellKnownEconomicEfficiencyParameters.EconEff 
                                || e.Economefficiencyparameter.WellKnownType == WellKnownEconomicEfficiencyParameters.EconEffMln)));

                    var r = sr.Distinct().Aggregate
                                        (new List<StageresultDto>(), (a, c) =>
                                        {
                                            a.Add(new StageresultDto()
                                            {
                                                Id = c.Id,
                                                Stageresult = c,
                                                Name = new StringBuilder().AppendLine().Append(c.Name).AppendLine().ToString(),
                                                Contractdocnum = c.Stage.Contractdoc.Internalnum,
                                                Contractdocdate = c.Stage.Contractdoc.Approvedat.HasValue ? c.Stage.Contractdoc.Approvedat.Value.ToString("dd.MM.yyyy") : "",
                                                Contractorname = c.Stage.Contractdoc != null && c.Stage.Contractdoc.Contractor != null ? c.Stage.Contractdoc.Contractor.Name : "",
                                                FunctionalCustomerName = c.Stage.Contractdoc.FunctionalCustomers.ToString(),
                                                EconEffect = c.Efparameterstageresults.Any(ee => ee.Economefficiencyparameter != null && ee.Economefficiencyparameter.WellKnownType == WellKnownEconomicEfficiencyParameters.EconEff) ? ((c.Efparameterstageresults.FirstOrDefault(ee => ee.Economefficiencyparameter != null && ee.Economefficiencyparameter.WellKnownType == WellKnownEconomicEfficiencyParameters.EconEff)).Value.HasValue ? c.Efparameterstageresults.FirstOrDefault(ee => ee.Economefficiencyparameter != null && ee.Economefficiencyparameter.WellKnownType == WellKnownEconomicEfficiencyParameters.EconEff).Value.Value : 0) : 0,
                                                EconEffectMln = c.Efparameterstageresults.Any(ee => ee.Economefficiencyparameter != null && ee.Economefficiencyparameter.WellKnownType == WellKnownEconomicEfficiencyParameters.EconEffMln) ? ((c.Efparameterstageresults.FirstOrDefault(ee => ee.Economefficiencyparameter != null && ee.Economefficiencyparameter.WellKnownType == WellKnownEconomicEfficiencyParameters.EconEffMln)).Value.HasValue ? c.Efparameterstageresults.FirstOrDefault(ee => ee.Economefficiencyparameter != null && ee.Economefficiencyparameter.WellKnownType == WellKnownEconomicEfficiencyParameters.EconEffMln).Value.Value : 0) : 0,
                                                Stagename = new StringBuilder().AppendLine().Append(c.Stage.Subject).AppendLine().ToString(),
                                                IntroductionActDate = c.Actintroductiondate,
                                                IntroductionActNum = c.Actintroductionnum,
                                                EfficiencyComment = c.Efficiencycomment ?? "",
                                                Period = c.Efparameterstageresults.Any(ee => ee.Economefficiencyparameter != null && ee.Economefficiencyparameter.WellKnownType == WellKnownEconomicEfficiencyParameters.Period) ? (c.Efparameterstageresults.FirstOrDefault(ee => ee.Economefficiencyparameter != null && ee.Economefficiencyparameter.WellKnownType == WellKnownEconomicEfficiencyParameters.Period).Value.HasValue ? c.Efparameterstageresults.FirstOrDefault(ee => ee.Economefficiencyparameter != null && ee.Economefficiencyparameter.WellKnownType == WellKnownEconomicEfficiencyParameters.Period).Value.Value : 0) : 0,
                                            });
                                            return a;
                                        }

                              );


                    return r;
                }
                throw new NoReportDataException();
            }
        }





        protected override void BuildReport()
        {


            CurrentButtomPosition = StartPosition;

            SetHeader();

            IEnumerable<StageresultDto> stageresults = Results;
            if (stageresults.Any())
            {
                // Создать объект ReportCurrentProgress для поддержки уведомления. Полное число шагов равно числу договоров
                var reporter = CreateProgressReporter(stageresults.Count());
                reporter.ReportProgress();

                var i = 1;
                foreach (var sr in stageresults)
                {

                    reporter.ReportProgress();
                    BuildResult(sr, i++);
                    // Уведомление о продвижении на один шаг
                    reporter.Next();

                }
                SetPrintingArea(ActiveWorksheet);
            }
        }

        protected override void SetHeader()
        {
            //Для ускорения вводим диапозон ячеек для замены (задаем диапозон больше, чем требуется)
            var r = MainWorkSheet.Range["A1", "M4"];
            r.Replace("#Year#", Year);
            r.Replace("#Today#", CurrentDate.ToString(("dd.MM.yyyy")));
        }


        private void BuildResult(StageresultDto stageersult, int num)
        {
            var r = CopyRowRange(DetailWorkSheet, 1);
            r.Replace("#Num#", num.ToString());
            InsertBigFieldIntoReport("#NOIKRName#", stageersult.Stagename, r);
            InsertBigFieldIntoReport("#ResultName#", stageersult.Name, r);

            r.Replace("#ContractNum#", stageersult.Contractdocnum);
            r.Replace("#ContractDate#", stageersult.Contractdocdate);

            InsertBigFieldIntoReport("#Contractor#", stageersult.Contractorname, r);

            InsertBigFieldIntoReport("#FunctionalCustomer#", stageersult.FunctionalCustomerName, r);
            r.Replace("#Period#", stageersult.Period != 0 ? stageersult.Period.ToString(NumberFormat) : "");

            r.Replace("#ActNum#", stageersult.IntroductionActNum);
            r.Replace("#ActDate#", stageersult.IntroductionActDate.HasValue ? stageersult.IntroductionActDate.Value.ToString("d") : "");
            InsertBigFieldIntoReport("#EfficiencyComment#", stageersult.EfficiencyComment, r);

            r.Replace("#Coef#", (stageersult.EconEffectMln * 1000).ToString(NumberFormat));
            r.Replace("#FactCoef#", "");
            r.Replace("#YearCoef#", "");
            r.Replace("#YearFactCoef#", "");

            r.Rows.AutoFit();
        }

    }

}
