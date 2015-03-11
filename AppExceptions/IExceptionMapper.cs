using System;
using System.Collections.Generic;
using System.Linq;

namespace AppExceptions
{
    /// <summary>
    /// Определяет типы, которые могут проецировать один тип исключения в другой\
    /// Используется для предварительной обработки исключения, например, с целью вернуть на дальнейшую обработку не само исключение
    /// а вложенное.
    /// </summary>
    public interface IExceptionMapper
    {
        /// <summary>
        /// Проецирует объект исключения в другой
        /// </summary>
        /// <param name="source">Исключение</param>
        /// <param name="target">Конечное исключение</param>
        /// <returns>Истина, если проекция была применена</returns>
        bool MapException(Exception source, ref Exception target);
    }

    public class ExceptionMappers
    {
        public IList<IExceptionMapper> Mappers { get; private set; }
       
        public ExceptionMappers()
        {
            Mappers = new List<IExceptionMapper>();
        }
        public Exception MapException(Exception exception)
        {
            return Mappers.Any(exceptionMapper => exceptionMapper.MapException(exception, ref exception)) ? exception : exception;
        }
    }
}