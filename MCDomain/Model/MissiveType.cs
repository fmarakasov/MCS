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

    public enum WellKnownMissiveTypes
    {
        [Description("Не задано")]
        Undefined = -1,
        [Description("Исх.")]
        Out = 1,
        [Description("Вх.")]
        In = 2
    }

    public partial class Missivetype: IObjectId, IDataErrorInfo
    {

        public bool IsWellKnownId()
        {
            return false;
        }


        public override string ToString()
        {
            return Name;
        }

        private readonly DataErrorHandlers<Missivetype> _errorHandlers = new DataErrorHandlers<Missivetype>
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
