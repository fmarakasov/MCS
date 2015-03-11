using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Reflection;
using System.Text;
using System.ComponentModel;
using System.Windows.Media;
using MCDomain.Common;
using CommonBase;

namespace MCDomain.Model
{
    public enum WellKnownApprovalstates
    {
        [Description("Не известно")]
        Unknown = -1,
        [Description("Подписано")]
        Signed = 1, 
        [Description("На подписи")]
        WaitingForSignature = 2,
        [Description("На стадии завершения")]
        AtTheFinishingState = 3,
        [Description("У функционального заказчика")]
        OnTheFunctionalCustomerSide = 4,
        [Description("Работы не могут быть завершены")]
        NoCompletion = 5,
        [Description("В УИР")]
        UIR = 6,
        [Description("Не согласовано")]
        NonApproved
    }

    partial class Approvalstate : IObjectId, INamed, IDataErrorInfo, IColor
    {

        public bool IsWellKnownId()
        {
            var en = Enum.GetValues(typeof(WellKnownApprovalstates));
            return en.Cast<object>().Any(ch => (WellKnownApprovalstates) Id == (WellKnownApprovalstates) ch);
        }

        /// <summary>
        /// Получает известный тип состояния согласования
        /// </summary>
        public WellKnownApprovalstates WellKnownType
        {
            get
            {
                if (IsWellKnownId())
                    return (WellKnownApprovalstates)Id;
                return WellKnownApprovalstates.Unknown;
            }
        }

        public override string ToString()
        {
            return Name;            
        }

        private readonly DataErrorHandlers<Approvalstate> _errorHandlers = new DataErrorHandlers<Approvalstate>
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


        public Color WinColor
        {
            get
            {
                int nval;
                if (int.TryParse(Color.ToString(), out nval))
                {
                    var bytes = BitConverter.GetBytes(nval);
                    var color = System.Windows.Media.Color.FromRgb(bytes[2], bytes[1], bytes[0]);
                    return color;
                }
                return new Color() { R = 255, G = 255, B = 255 };
            }
        }

       
    }
}
