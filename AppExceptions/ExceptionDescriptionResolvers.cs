using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WindowsTaskDialog;

namespace AppExceptions
{
    public class ExceptionDescriptionResolvers
    {
        public static string GetMessage(Exception exception)
        {
            var stringBuilder = new StringBuilder();
            var ex = exception;
            while (ex != null)
            {
                stringBuilder.Append(ex.Message);
                stringBuilder.Append("\n");
                ex = ex.InnerException;
            }
            return stringBuilder.ToString();
        }

        public IList<IExceptionDescriptionResolver> Filters { get; private set; }   
        public ExceptionDescriptionResolvers()
        {
            Filters = new List<IExceptionDescriptionResolver>();
        }

        public ExceptionDescription GetDescription(Exception exception)
        {
            var description = new ExceptionDescription()
                {
                    Content = GetMessage(exception),
                    Icon = TaskDialogIcon.Error,
                    Title =string.Format("{0} - [{1} - {2}]", ExceptionFilterManager.Instance.AssemblyTitle,
                                              ExceptionFilterManager.Instance.FileVersion,
                                              ExceptionFilterManager.Instance.AssemblyVersion),
                    Instruction = "Зарегистрирована ошибка",
                    ShowFooter = true

                };
            
            return Filters.Any(exceptionFilter => exceptionFilter.GetDescription(exception, ref description)) ? description : description;
        }
    }
}