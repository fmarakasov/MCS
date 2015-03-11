using System.ComponentModel;

namespace MContracts.ViewModel
{
    public class WrappedObjectCommandArgs<T>:CancelEventArgs
    {
        public T WrappedObject { get; private set; }
        public WrappedObjectCommandArgs(T obj)
        {            
            WrappedObject = obj;
        }
    }
}