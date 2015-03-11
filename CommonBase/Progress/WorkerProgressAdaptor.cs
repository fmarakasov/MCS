using System.ComponentModel;
using System.Diagnostics.Contracts;

namespace CommonBase.Progress
{
    /// <summary>
    /// Адаптирует BackgroundWorker к интерфейсу IProgressReporter
    /// </summary>
    public class WorkerProgressAdaptor : IProgressReporter
    {
        private readonly BackgroundWorker _worker;
        private readonly object _userState;

        /// <summary>
        /// Создаёт экземпляр WorkerProgressAdaptor для адаптации заданного BackgroundWorker к интерфейсу IProgressReporter
        /// </summary>
        /// <param name="worker">Объект BackgroundWorker</param>
        /// <param name="userState">Объект, который будет передаваться в метод ReportProgress объекта BackgroundWorker</param>
        public WorkerProgressAdaptor(BackgroundWorker worker, object userState)
        {
            Contract.Assert(worker != null, "worker != null"); 
            _worker = worker;
            _worker.WorkerReportsProgress = true;
           _userState = userState;
        }
        /// <summary>
        /// Вызывает событие OnProgressChanged BackgroundWorker. 
        /// </summary>
        /// <param name="percents">Проценты выполнения работы</param>
        public void ReportProgress(int percents)
        {
           _worker.ReportProgress(percents, _userState);
        }
    }
}
 