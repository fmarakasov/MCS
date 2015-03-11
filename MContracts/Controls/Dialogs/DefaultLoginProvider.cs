using System;
using CommonBase;

namespace MContracts.Controls.Dialogs
{
    public class DefaultLoginProvider : IQueryLoginProvider
    {
        public bool QueryCredentails(ILoginProvider loginProvider)
        {
            var dlg = new ServerConnectionDialog();
            var result = dlg.QueryCredentails(loginProvider);
            ConnectionException = dlg.ConnectionException;
            return result;
        }

        public Exception ConnectionException { get; private set; }
    }
}