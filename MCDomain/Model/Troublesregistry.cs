using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using MCDomain.Common;
using CommonBase;

namespace MCDomain.Model
{
    partial class Troublesregistry : IObjectId, INamed, ICloneable, IDataErrorInfo, IEditableObject
    {

        public bool IsWellKnownId()
        {
            return false;
        }

        /// <summary>
        /// Получает коллекцию проблем верхнего уровня
        /// </summary>
        public IEnumerable<Trouble> TopLevelTroubles
        {
            get
            {
                Contract.Requires(Troubles != null);
                Contract.Ensures(Contract.Result<IEnumerable<Trouble>>()!=null);
                return Troubles.Where(x => x.ParentTrouble == null);
            }
        }

        private EditableObject<Troublesregistry> _editable;
        private readonly DataErrorHandlers<Troublesregistry> _errorHandlers = new DataErrorHandlers<Troublesregistry>
                                                                       {
                                                                           new NameDataErrorHandler(),
                                                                       };

        partial void OnCreated()
        {
            _editable = new EditableObject<Troublesregistry>(this);
        }

        public override string ToString()
        {
            return Name;
        }

        public object Clone()
        {
            return new Troublesregistry()
            {
                Name = this.Name,
                Approvedat = this.Approvedat,
                Ordernum = this.Ordernum,
                Shortname = this.Shortname,
                Validfrom = this.Validfrom,
                Validto = this.Validto
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

        #region Nested type: NameDataErrorHandler

        private class NameDataErrorHandler : IDataErrorHandler<Troublesregistry>
        {
            #region IDataErrorHandler<Troublesregistry> Members

            public string GetError(Troublesregistry source, string propertyName, ref bool handled)
            {
                if (propertyName == "Name")
                {
                    return HandleCurrencyError(source, out handled);
                }

                return string.Empty;
            }

            #endregion

            private static string HandleCurrencyError(Troublesregistry source, out bool handled)
            {
                handled = false;
                if (string.IsNullOrEmpty(source.Name))
                {
                    handled = true;
                    return "Наименование не может быть пустым!";
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
