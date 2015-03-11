using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MCDomain.Common;
using System.ComponentModel;
using System.Diagnostics.Contracts;
using CommonBase;

namespace MCDomain.Model
{
    partial class Responsiblefororder: IObjectId, ICloneable, IDataErrorInfo
    {

        private readonly DataErrorHandlers<Responsiblefororder> _errorHandlers = new DataErrorHandlers<Responsiblefororder>
                                                                       {
                                                                           
                                                                       };


        public bool IsWellKnownId()
        {
            return false;
        }

        partial void OnCreated()
        {
            
        }

        public override string ToString()
        {
            Contract.Assert(Employee != null);
            return Employee.ToString();
        }

        public object Clone()
        {
            return new Responsiblefororder()
            {
                Employee = this.Employee,
                Department = this.Department
            };
        }

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
