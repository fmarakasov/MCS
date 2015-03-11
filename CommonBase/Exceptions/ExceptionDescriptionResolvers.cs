using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommonBase.Exceptions
{
    /// <summary>
    /// Класс управления фильтрами исключений. Обеспечивает получение описания исключения через метод GetMessage
    /// </summary>
    public class ExceptionDescriptionResolvers
    {

        public IList<IExceptionDescriptionResolver> Filters { get; private set; }   
        public ExceptionDescriptionResolvers()
        {
            Filters = new List<IExceptionDescriptionResolver>();
        }

        /// <summary>
        /// Получает объект ExceptionDescription с описанием для заданного исключения, произведя поиск соответствия в коллекции зарегистрированных фильтрах исключений
        /// </summary>
        /// <param name="exception">Исключение</param>
        /// <returns>Объект ExceptionDescription</returns>
        public ExceptionDescription GetDescription(Exception exception)
        {
            var description = new ExceptionDescription
                {
                    Content = exception.AggregateMessages(),
                    Icon = IconDescription.Critical,
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