using System;

namespace CommonBase
{
    public class EventParameterArgs<T> : EventArgs
    {
        public T Parameter { get; set; }

        public EventParameterArgs(T param)
        {
            Parameter = param;
        }

        protected EventParameterArgs()
        {
            throw new System.NotImplementedException();
        }
    }
}