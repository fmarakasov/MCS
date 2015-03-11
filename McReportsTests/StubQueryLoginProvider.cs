using CommonBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace McReportsTests
{
    class StubQueryLoginProvider : IQueryLoginProvider
    {
        public bool QueryCredentails(ILoginProvider loginProvider)
        {
            Exception exception;
            return loginProvider.Connect("UD", "sys", "XE", out exception);
        }

        public Exception ConnectionException
        {
             get; set; 
        }
    }
}
