using System.Diagnostics.Contracts;

namespace CommonBase.Progress
{
    /// <summary>
    /// Позволяет уведомлять о прогрессе за счёт поддержки IProgressReporter
    /// </summary>
    public class ReportCurrentProgress : ProgressCounter
    {
        /// <summary>
        /// Получает IProgressReporter через который осуществляется уведомление о прогрессе
        /// </summary>
        public IProgressReporter ProgressReporter { get; private set; }
        /// <summary>
        /// Создаёт экземпляр ReportCurrentProgress для поддержки уведомления о прогрессе
        /// </summary>
        /// <param name="reporter">Объект IProgressReporter через который осуществялются уведомления </param>
        /// <param name="count">Полное число шагов в работе</param>
        public ReportCurrentProgress(IProgressReporter reporter, int count) : base(count)
        {
            Contract.Requires(reporter != null);
            Contract.Requires(count > 0);
            ProgressReporter = reporter;
        }

        /// <summary>
        /// Уведомить о прогрессе через заданный IProgressReporter
        /// </summary>
        public void ReportProgress()
        {
            ProgressReporter.ReportProgress(CurrentProgress);
        }

    }
}