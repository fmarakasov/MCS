using System;
using System.ComponentModel;
using System.Diagnostics.Contracts;
using System.Runtime.Serialization;

namespace MCDomain.Model
{
    public enum ErrorSeverity
    {
        [Description("Критическая")]
        Critical=1,
        [Description("Предупреждение")]
        Warning=2,
        [Description("Подсказка")]
        Hint=3,
        [Description("Не содержит ошибок")]
        None=4
    }

    public class ErrorState : IComparable, IComparable<ErrorState>
    {
        private const long CodeUndef = -1;
        public ErrorSeverity Severity { get; private set; }
        public string Message { get; private set; }
        public long Code { get; private set; }
        public ErrorState(ErrorSeverity severity, string message, long code = CodeUndef)
        {
            Severity = severity;
            Message = message;
            Code = code;
        }

        public int CompareTo(object obj)
        {
            Contract.Assert(obj is ErrorState);
            return CompareTo(obj as ErrorState);
        }

        public int CompareTo(ErrorState other)
        {
            Contract.Assert(other != null);
            return Severity.CompareTo(other.Severity);

        }
    }
}
