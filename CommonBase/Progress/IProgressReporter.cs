namespace CommonBase.Progress
{
    /// <summary>
    /// Определяет типы, поддерживающие уведомления о прогрессе выполнения операции
    /// </summary>
    public interface IProgressReporter
    {
        /// <summary>
        /// Вызывает уведомление об изменнии в прогрессе выполнения работы
        /// </summary>
        /// <param name="percents">Текущее выполнение работы в процентах</param>
        void ReportProgress(int percents);
    }

    /// <summary>
    /// Пустая реалзация интерфейса с уведомлением о прогрессе
    /// </summary>
    public class NullProgressReporter : IProgressReporter
    {
        /// <summary>
        /// Получает единственный экземпляр NullProgressReporter
        /// </summary>
        public static readonly IProgressReporter Instance = new NullProgressReporter();

        private NullProgressReporter()
        {
        }

        public void ReportProgress(int percents)
        {
            
        }
    }
}