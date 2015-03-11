using System;
using WindowsTaskDialog;

namespace AppExceptions
{
    public class ApplicationExceptionDescription : IExceptionDescriptionResolver
    {
        public bool GetDescription(Exception exception, ref ExceptionDescription description)
        {
            if (!(exception is ApplicationException)) return false;
            description.Content = exception.Message;
            description.Icon = TaskDialogIcon.Information;
            description.Instruction = string.Empty;
            description.ShowFooter = false;
            return true;
        }
    }
}