using System;
using System.Reflection;

namespace AppExceptions
{
    /// <summary>
    /// Проекция исключений типа TargetInvocationException
    /// Возвращает InnerException исключения, как сождержащий смысловую спецификацию исключения
    /// </summary>
    public class TargetInvocationExceptionMapper : IExceptionMapper
    {
        public bool MapException(Exception source, ref Exception target)
        {
            if (!(source is TargetInvocationException)) return false;
            target = source.InnerException;
            return true;
        }
    }
}