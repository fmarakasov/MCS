using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MCDomain.Common;
using CommonBase;

namespace MCDomain.Model
{
    partial class Functionalcustomercontract : IObjectId
    {

        public bool IsWellKnownId()
        {
            return false;
        }

        private Functionalcustomer _functionalcustomer;
        /// <summary>
        /// Получает или устанавливает контрагента для договора не затрагивая модели (обход проблем LinqConnect)
        /// </summary>
        public Functionalcustomer Customer
        {
            get { return Functionalcustomer ?? _functionalcustomer; }
            set { _functionalcustomer = value; }
        }
    }
}
