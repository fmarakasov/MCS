using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using MCDomain.Common;
using System.ComponentModel;
using CommonBase;

namespace MCDomain.Model
{

    public class CompareContractorpositions : IComparer<Contractorposition>
    {
        // Because the class implements IComparer, it must define a 
        // Compare method. This Compare method compares integers.
        public int Compare(Contractorposition p1, Contractorposition p2)
        {
            int iResult = string.Compare(p1.Contractor.Name, p2.Contractor.Name);
            if (iResult == 0) iResult = string.Compare(p1.Position.Name, p2.Position.Name);
            return iResult;
        }
    }

    partial class Contractorposition : IObjectId, ICloneable, IDataErrorInfo, IEditableObject
    {

        public bool IsWellKnownId()
        {
            return false;
        }

        /// <summary>
        /// Получает название должности в другой организации
        /// </summary>
        public string Positionname
        {
            get
            {
                if (Position == null) throw new NoNullAllowedException("Position не может быть null.");
                Contract.EndContractBlock();
                return Position.Name;
            }
        }

        public override string ToString()
        {
            return Position.ToString() + " " + Contractor.ToString();
        }

        private EditableObject<Contractorposition> _editable;
        private readonly DataErrorHandlers<Contractorposition> _errorHandlers = new DataErrorHandlers<Contractorposition>
                                                                       {
                                                                           new ContractorDataErrorHandler(),
                                                                           new PositionDataErrorHandler()
                                                                       };

        partial void OnCreated()
        {
            _editable = new EditableObject<Contractorposition>(this);
        }

        public object Clone()
        {
            return new Contractorposition()
            {
                Contractor = this.Contractor,
                Position = this.Position
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

        #region Nested type: ContractorDataErrorHandler

        private class ContractorDataErrorHandler : IDataErrorHandler<Contractorposition>
        {
            #region IDataErrorHandler<Contractorposition> Members

            public string GetError(Contractorposition source, string propertyName, ref bool handled)
            {
                if (propertyName == "Contractor")
                {
                    return HandleCurrencyError(source, out handled);
                }

                return string.Empty;
            }

            #endregion

            private static string HandleCurrencyError(Contractorposition source, out bool handled)
            {
                handled = false;
                if (source.Contractor == null)
                {
                    handled = true;
                    return "Контрагент не может быть пустым!";
                }
                return string.Empty;
            }
        }

        #endregion

        #region Nested type: PositionDataErrorHandler

        private class PositionDataErrorHandler : IDataErrorHandler<Contractorposition>
        {
            #region IDataErrorHandler<Contractorposition> Members

            public string GetError(Contractorposition source, string propertyName, ref bool handled)
            {
                if (propertyName == "Position")
                {
                    return HandleCurrencyError(source, out handled);
                }

                return string.Empty;
            }

            #endregion

            private static string HandleCurrencyError(Contractorposition source, out bool handled)
            {
                handled = false;
                if (source.Position == null)
                {
                    handled = true;
                    return "Должность не может быть пустой!";
                }
                return string.Empty;
            }
        }

        #endregion

        public void BeginEdit()
        {
            _editable.BeginEdit();
        }

        public void CancelEdit()
        {
            _editable.CancelEdit();
        }

        public void EndEdit()
        {
            _editable.EndEdit();
        }
    }
}
