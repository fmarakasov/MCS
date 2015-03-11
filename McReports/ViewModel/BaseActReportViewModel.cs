using System;
using System.Diagnostics.Contracts;
using System.Globalization;
using System.Linq;
using MCDomain.DataAccess;
using MCDomain.Model;
using CommonBase;


namespace McReports.ViewModel
{
    public class BaseActReportViewModel : WordReportViewModel
    {
        /// <summary>
        /// Текст для замены в случае отсутствия данных 
        /// </summary>
        protected const string NoDataString = "";

        public BaseActReportViewModel(IContractRepository contractRepository)
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

        protected override string OutputFileName
        {
            get
            {
                if (CurrentAct != null)
                    return string.Format("Акт сдачи-приемки работ {0} от {1} ({2})", CurrentAct.Num,
                                         String.Format("{0: dd.MM.yyyy}", CurrentAct.Signdate),
                                         CurrentAct.ContractObject);
                return base.OutputFileName;
            }
        }

        protected override string ReportFileName
        {
            get { return string.Empty; }
        }

        public Act CurrentAct { get; set; }

        private void BuildPerson()
        {
            Contract.Assert(CurrentAct != null);
            Contract.Assert(CurrentAct.ContractObject != null);

            var contractor = NoDataString;
            var personPost = NoDataString;
            var personName = NoDataString;

            if (CurrentAct.ContractObject.Contractor != null)
            {
                var pers =
                    (CurrentAct.ContractObject.Contractor.Persons.FirstOrDefault(
                        p => p.Actsignauthority && p.Contractheadauthority && p.Valid) ??
                     CurrentAct.ContractObject.Contractor.Persons.FirstOrDefault(p => p.Contractheadauthority && p.Valid)) ??
                    CurrentAct.ContractObject.Contractor.Persons.FirstOrDefault(p => p.Actsignauthority && p.Valid);


                if (pers != null)
                {
                    contractor = pers.PostName(CurrentAct.ContractObject.Contractor);
                    personPost = pers.Contractorposition.ToString();
                    personName = pers.GetShortFullName();
                }
            }

            ReplaceText("[ContractorPerson]", contractor);
            ReplaceText("[ContractorPersonPost]", personPost);
            ReplaceText("[ContractorPersonName]", personName);

        }

        protected virtual void  BuildStages()
        {
            
        }

        protected virtual string ContractorData
        {
            get { return CurrentAct.ContractObject.Contractor.ContractorFullDesc; }
        }
        
        protected virtual string ContractorName
        {
            get
            {
                Contract.Assert(CurrentAct != null);
                return CurrentAct.ContractObject.Contractor.Return(x => x.ToString(), NoDataString);
            }
        }

        protected virtual string ActNum
        {
            get
            {
                Contract.Assert(CurrentAct != null);
                return CurrentAct.Num.Return(x=>x.ToString(CultureInfo.InvariantCulture), NoDataString);
            }

        }

        protected virtual string ScheduleNum
        {
            get
            {
                Contract.Assert(CurrentAct != null);
                var sch = CurrentAct.Stages.Select(s => s.Schedule).SelectMany(sc => sc.Schedulecontracts).FirstOrDefault();
                if (sch != null && sch.Appnum.HasValue)
                {
                    return sch.Appnum.Value.ToString();
                }
                return string.Empty;
            }
        }

        protected virtual string ActDate
        {
            get
            {
                Contract.Assert(CurrentAct != null);
                return CurrentAct.Signdate.HasValue
                           ? string.Format("{0: dd MMMM yyyy}", CurrentAct.Signdate)
                           : NoDataString;
            }
        }

        protected virtual string RegionName
        {
            get
            {
                Contract.Assert(CurrentAct != null);
                return CurrentAct.Region.Return(x => x.ToString(), NoDataString);
            }
        }

        protected virtual string ContractNum
        {
            get
            {
                Contract.Assert(CurrentAct != null);
                return CurrentAct.ContractObject.Num.Return(x=>x.ToString(CultureInfo.InvariantCulture), NoDataString);
            }
        }

        protected virtual string ContractName
        {
            get
            {
                Contract.Assert(CurrentAct != null);
                return CurrentAct.ContractObject.Subject;
            }
        }

        protected virtual string ContractDate
        {
            get
            {
                Contract.Assert(CurrentAct != null);
                return string.Format("{0: dd.MM.yyyy}", CurrentAct.ContractObject.Approvedat);
            }
        }
        
        protected virtual string Year
        {
            get
            {
                Contract.Assert(CurrentAct != null);
                return CurrentAct.Signdate.HasValue
                           ? String.Format("{0: yyyy}", CurrentAct.Signdate.Value)
                           : String.Format("{0: yyyy}", DateTime.Now);
            }
        }

        protected virtual void BuildHeader()
        {
            BuildContractorHeader();

            ReplaceText("[ActNum]", ActNum);
            ReplaceText("[ActDate]", ActDate);

            ReplaceText("[ContractNum]", ContractNum);
            ReplaceText("[ContractName]", ContractName);
            ReplaceText("[ContractDate]", ContractDate);

            ReplaceText("[Year]", Year);


        }

        private void BuildContractorHeader()
        {
            var contractorName = NoDataString;
            var contractorData = NoDataString;

            if (CurrentAct.ContractObject.Contractor != null)
            {
                contractorName = ContractorName;
                contractorData = ContractorData;
            }

            ReplaceText("[ContractorName]", contractorName);
            ReplaceText("[ContractorData]", contractorData);
        }

        protected virtual void BuildNDS()
        {
            Contract.Assert(CurrentAct != null);
            ReplaceText("[NDSPercent]", CurrentAct.Nds != null ? CurrentAct.Nds.ToString() : NoDataString);

            var ndsVal = NoDataString;
            var ndsValueWords = NoDataString;

            if (CurrentAct.ActMoney.Price.HasValue)
            {
                ndsVal = CurrentAct.ActMoney.Factor.NdsValue.ToString("N2");
                ndsValueWords = CurrentAct.ActMoney.Currency.MoneyInWords(CurrentAct.ActMoney.Factor.NdsValue, true,
                                                                          false, true);
            }

            ReplaceText("[NDSValue]", ndsVal);
            ReplaceText("[NDSValueWords]", ndsValueWords);

        }


        protected override void BuildReport()
        {
            var reporter = CreateProgressReporter(5);
            BuildHeader();
            reporter.Next();
            reporter.ReportProgress();
            BuildPerson();
            reporter.Next();
            reporter.ReportProgress();
            BuildStages();
            reporter.Next();
            reporter.ReportProgress();
            BuildNDS();
            reporter.Next();
            reporter.ReportProgress();
            ReplaceText("N/A", NoDataString);
            reporter.Next();
            reporter.ReportProgress();
        }

    }
}
