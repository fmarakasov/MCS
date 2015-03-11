using System;
using System.Reflection;

namespace AppExceptions
{
    /// <summary>
    /// Определяет типы для получения описания визуального представления исключения
    /// </summary>
    public interface IExceptionDescriptionResolver
    {
        /// <summary>
        /// Получает описание визуального представления исключений 
        /// </summary>
        /// <param name="exception">Исключение</param>
        /// <param name="description">Возвращаемое описание визуального представления исключений</param>
        /// <returns>Истина, если фильтр смог подобрать описание для заданного исключения и вернул его в объекте description</returns>
        bool GetDescription(Exception exception, ref ExceptionDescription description);
    }
}