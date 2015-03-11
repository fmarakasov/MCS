using System;

namespace MContracts.ViewModel
{
    /// <summary>
    /// Исключение связанное с ошибкой фиксации результатов в базе данных
    /// </summary>
    public class SubmitChangesFailed : ApplicationException
    {
        public SubmitChangesFailed(string message, Exception exception):base(message, exception)
        {
            
        }
    }
}