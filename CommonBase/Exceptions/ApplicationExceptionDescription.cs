using System;

namespace CommonBase.Exceptions
{
    /// <summary>
    /// Фильтр для ApplicationException
    /// </summary>
    public class ApplicationExceptionDescription : IExceptionDescriptionResolver
    {
        public bool GetDescription(Exception exception, ref ExceptionDescription description)
        {
            if (!(exception is ApplicationException)) return false;
            description.Content = exception.Message;
            description.Icon = IconDescription.Information;
            description.Instruction = string.Empty;
            description.ShowFooter = false;
            return true;
        }
    }
}