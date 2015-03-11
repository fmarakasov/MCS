using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using Ninject;
using PostSharp.Aspects;

namespace MCDomain.AOP
{
    
    public interface ICoreLogger
    {
        void Write(string logMessage);
    }

    [Serializable]
    public class DefaultLogger : ICoreLogger
    {
        public void Write(string logMessage)
        {
            Logger.Write(logMessage);
        }
    }

    
    [Serializable]
    public class LogAttribute : OnMethodBoundaryAspect
    {
        private bool _hasEntered;
        private int _margin;

        ICoreLogger _logger;

        public ICoreLogger Logger { get { return _logger; } set { _logger = value; } }

        public LogAttribute(Type loggerType)
        {
            Logger = (ICoreLogger)Activator.CreateInstance(loggerType);
            if (Logger == null) throw new ArgumentException("The loggerType must be ICoreLogger type");
        }

        private string Spaces()
        {
            if (_margin <= 0) return string.Empty;
            StringBuilder sb = new StringBuilder(_margin);
            for (int i = 0; i < _margin; ++i) sb.Append("\t");
            return sb.ToString();
        }

        private string Format(string format, params object[] values)
        {
            return Spaces() + string.Format(format, values);
        }

        private bool HasEntered()
        {
            var initial = _hasEntered;
            _hasEntered = true;
            return initial;
        }

        private string GetInstanceName(object instance)
        {
            var toString = instance.ToString();
            if (string.IsNullOrEmpty(toString))
                toString = instance.GetType().Name;
            return toString;
        }

        public override void OnEntry(MethodExecutionArgs args)
        {
       
            if (HasEntered()) return;
            ++_margin;
                        
            Logger.Write(Format("Entry: \'{0}.{1}\'", GetInstanceName(args.Instance), args.Method.Name));
            
      

            if (args.Arguments.Any())
                Logger.Write(Format("\tArguments: \'{0}\'", args.Arguments.Aggregate((workingSentence, next) =>
                                                  next + " " + workingSentence)));
            _hasEntered = false;

            base.OnEntry(args);
        }
        
        public override void OnExit(MethodExecutionArgs args)
        {
            
                        if (HasEntered()) return;
                                                       

                        Logger.Write(Format("OnExit: \'{1}.{0}\'", args.Method.Name, args.Instance));
                       // Logger.Write(string.Format("----------------END OF CALL \'{0}.{1}\'--------------", args.Instance, args.Method.Name));
                        _hasEntered = false;

                        --_margin;
            base.OnExit(args);
            
        }
         

        public override void OnException(MethodExecutionArgs args)
        {
            if (HasEntered()) return;
            Logger.Write(Format("\tExited with exception \'{0}\'  from  \'{1}.{2}\'", GetInstanceName(args.Exception), args.Instance, args.Method.Name));
            _hasEntered = false;
            base.OnException(args);
        }

        public override void OnSuccess(MethodExecutionArgs args)
        {
            if (HasEntered()) return;
            if (args.ReturnValue != null)
                Logger.Write(Format("\tOnSuccess. The result of \'{1}.{0}\' is \'{2}\' ", args.Method.Name, GetInstanceName(args.Instance), args.ReturnValue));
            _hasEntered = false;
            base.OnSuccess(args);
        }
    }
}
