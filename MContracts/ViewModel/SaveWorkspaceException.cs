using System;

namespace MContracts.ViewModel
{
    class SaveWorkspaceException : ApplicationException
    {
        public SaveWorkspaceException(string message, Exception exception):base(message, exception)
        {
    
        }

    }
}
