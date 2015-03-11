using System.ComponentModel;
using UIShared.ViewModel;
using CommonBase;

namespace UIShared.Works
{
    public class ViewModelBackgroundWorker : BackgroundWorker
    {
        /// <summary>
        /// Получает или устанавливает объект модели представления, с которым связан экземпляр ViewModelBackgroundWorker
        /// </summary>
        public ViewModelBase ActiveViewModel { get; set; }

        protected override void OnDoWork(DoWorkEventArgs e)
        {
            ActiveViewModel.Do(x=>x.IsBusy = true); 
            base.OnDoWork(e);
        }
        protected override void OnRunWorkerCompleted(RunWorkerCompletedEventArgs e)
        {
            ActiveViewModel.Do(x=>x.IsBusy = false);
            base.OnRunWorkerCompleted(e);
        }
    }
}
