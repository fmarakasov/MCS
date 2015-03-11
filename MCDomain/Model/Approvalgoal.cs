using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Reflection;
using System.Text;
using System.ComponentModel;
using MCDomain.Common;
using CommonBase;

namespace MCDomain.Model
{
    public partial class Approvalgoal: IObjectId, IDataErrorInfo
    {

        public bool IsWellKnownId()
        {
            return false;
        }

        public override string ToString()
        {
            return Name;
        }

        private readonly DataErrorHandlers<Approvalgoal> _errorHandlers = new DataErrorHandlers<Approvalgoal>
                                                                     {
                                                                         
                                                                     };


        public string Error
        {
            get { return this.Validate(); }
        }

        public string this[string columnName]
        {
            get { return _errorHandlers.HandleError(this, columnName); }
        }



        
    }
}
