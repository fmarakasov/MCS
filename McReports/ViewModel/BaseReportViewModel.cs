using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using CommonBase;
using CommonBase.Progress;
using MCDomain.DataAccess;
using MCDomain.Model;
using McReports.Common;
using McUIBase.ViewModel;

namespace McReports.ViewModel
{
    public abstract class BaseReportViewModel : RepositoryViewModel
    {
        private IEnumerable<Contractdoc> _activeContractdocs;
        private int? _count;
        private IEnumerable<IContractStateData> _inputcontractdocs;
        private IProgressReporter _progressReporter;

        protected BaseReportViewModel(IContractRepository repository)
            : base(repository)
        {
            NeedsInputParameters = false;
        }

        public int Measure
        {
            get { return 1000; }
        }


        /// <summary>
        /// Показывает отчёт пользователю. Переопредилите метод в производном классе, что бы реализовать такое поведение
        /// </summary>
        public virtual void ShowReport()
        {
        }

        protected virtual NumberFormatInfo NumberFormat
        {
            get { return CultureInfo.CurrentCulture.NumberFormat; }
        }

        protected abstract string ReportFileName { get; }
        protected abstract string DefaultExt { get; }

        protected virtual string OutputFileName
        {
            get { return ReportFileName; }
        }


        /// <summary>
        ///     Получает полный путь к файле в который сохраняется отчёт
        /// </summary>
        protected string OutputFullFileName
        {
            get { return OutputFilePath(GetReportPath()); }
        }

        /// <summary>
        ///     Получает или устанавливает объект IProgressReporter для уведомления о прогрессе выполнения построения отчёта
        /// </summary>
        public IProgressReporter ProgressReporter
        {
            get { return _progressReporter ?? (_progressReporter = NullProgressReporter.Instance); }
            set { _progressReporter = value; }
        }

        /// <summary>
        /// Указывает, поддерживается ли моделью уведомление о прогрессе выполнния. Если свойство  
        /// установлено в true, но не производит уведомлений, то расчёт процентов выполнения игнорирует это представление, что может вызвать 
        /// некорректный расчёт, если строится сразу множество отчётов.
        /// </summary>
        public bool SupportProgressNotification { get; set; }
        /// <summary>
        ///     Получает или устанавливает провайдер шаблона для отчёта
        /// </summary>
        public ITemplateProvider TemplateProvider { get; set; }

        /// <summary>
        ///     Получает или устанавливает провайдер коллекции договоров для отчёта
        /// </summary>
        public IReportSourceProvider ReportSource { get; set; }

        /// <summary>
        ///     Получает коллекцию договоров для отчёта с использованием провайдера
        /// </summary>
        public IEnumerable<IContractStateData> InputContractdocs
        {
            get
            {
                Contract.Requires(ReportSource != null);
                return _inputcontractdocs ??
                       (_inputcontractdocs = ReportSource.Return(x => x.Source, new List<IContractStateData>()));
            }
        }

        /// <summary>
        ///     Получает число договоров в ActiveContractDocs
        /// </summary>
        protected int ContractsCount
        {
            get { return _count.HasValue ? _count.Value : (_count = ActiveContractDocs.Count()).Value; }
        }


        /// <summary>
        ///     Получает договора подлежащие сдаче в отчетном периоде,
        ///     просроченные и ожидающие выполнения (последние с суммой 0)
        /// </summary>
        public IEnumerable<Contractdoc> ActiveContractDocs
        {
            get
            {
                if (_activeContractdocs != null) return _activeContractdocs;
                _activeContractdocs = UnitOfWork.Repository<Contractdoc>().AsQueryable().Where(p => (InputContractdocs.Any(x => x.Id == p.Id)));
                return _activeContractdocs;
            }
        }

        /// <summary>
        ///     Получает или устанавливает признак необходимости запрашивать входные параметры
        /// </summary>
        public bool NeedsInputParameters { get; set; }

        /// <summary>
        ///     Получает описание свойств, которые являются входными параметрами отчёта
        /// </summary>
        public IEnumerable<ReportParameter> Parameters
        {
            get
            {
                var pi = GetType().GetProperties();
                return from propertyInfo in pi
                       let attr = propertyInfo.GetCustomAttributes(typeof (InputParameterAttribute), true)
                       where attr.Length == 1
                       select new ReportParameter((InputParameterAttribute) attr[0], propertyInfo.Name);
            }
        }


        /// <summary>
        ///     Получает полный путь к шаблону отчёта с использованием провайдера и свойства ReportFileName
        /// </summary>
        public string ReportTemplate
        {
            get
            {
                Contract.Requires(TemplateProvider != null);
                return TemplateProvider.GetTemplate(ReportFileName);
            }
        }

        public object ApplicationParameters { get; set; }

        /// <summary>
        ///     Создаёт экземпляр ReportCurrentProgress, который использует
        ///     заданный в свойстве ProgressReporter объект IProgressReporter для поддержки уведомлений о прогрессе
        ///     выполнения шагов построения отчёт. Если переданное в параметре число шагов в отчёте равно 0, то 
        ///     значение шагов выставляется в 1 и делается один шаг, что соответствует 100% выполненных шагов.
        /// </summary>
        /// <param name="count">Полное число шагов в построении отчёта</param>
        /// <returns>Экземпляр ReportCurrentProgress</returns>
        protected ReportCurrentProgress CreateProgressReporter(int count)
        {
            var realcount = count == 0 ? 1 : count;
            var reporter = new ReportCurrentProgress(ProgressReporter, realcount);
            if (count == 0) reporter.Next();
            return reporter;
        }

        public virtual void SetReport()
        {
        }

        protected virtual void BuildReport()
        {
        }

        protected string OutputFilePath(string userfolder)
        {
            var sPath = new StringBuilder();
            // добавляем дату, на которую формируется отчет
            sPath.Append(OutputFileName);
            sPath.Append(String.Format(" {0}", DateTime.Today.ToString("dd-MM-yyyy")));
            sPath.Append(DefaultExt);

            // удаляем запрещенные для имени файла символы
            string sResult = sPath.ToString();

            sPath = Path.GetInvalidFileNameChars()
                        .Where(c => sResult.IndexOf(c) > 0)
                        .Aggregate(sPath, (current, chr) => current.Replace(chr, ' '));


            return String.Format("{0}\\{1}", userfolder, sPath);
        }

        protected string GetReportPath()
        {
            var defaultPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

            var path = ApplicationParameters
                .With(x => x.GetPropertyValue<string>("UserReportFolder"))
                .If(p => !string.IsNullOrEmpty(p)).Return(p => p, defaultPath);
            return path;
        }

        protected override void Save()
        {
        }

        protected override bool CanSave()
        {
            return true;
        }


        protected virtual void SetHeader()
        {
        }
    }
}