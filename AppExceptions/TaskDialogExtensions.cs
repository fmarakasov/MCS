using CommonBase.Exceptions;
using WindowsTaskDialog;

namespace AppExceptions
{
    /// <summary>
    /// Класс расширений с методами преобразования из IconDescription в TaskDialogIcon
    /// </summary>
    public static class TaskDialogExtensions
    {
        /// <summary>
        /// Преобразует объект IconDescription в соответствующий ему TaskDialogIcon
        /// </summary>
        /// <param name="source">Объект IconDescription</param>
        /// <returns>Объект TaskDialogIcon</returns>
        public static TaskDialogIcon ToTaskDialogIcon(this IconDescription source)
        {
            switch (source)
            {
                case IconDescription.Information:
                    return TaskDialogIcon.Information;
                case IconDescription.Warning:
                    return TaskDialogIcon.Warning;
                case IconDescription.Critical:
                    return TaskDialogIcon.Error;
                case IconDescription.Question:
                    return TaskDialogIcon.Information;
                default:
                    return TaskDialogIcon.None;
            }
        }
    }
}