using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using MCDomain.Common;
using CommonBase;

namespace MCDomain.Model
{
    public partial class Ntpsubview : IObjectId, INamed, ICloneable, IDataErrorInfo, IEditableObject
    {
        private EditableObject<Ntpsubview> _editable;
        private readonly DataErrorHandlers<Ntpsubview> _errorHandlers = new DataErrorHandlers<Ntpsubview>
                                                                       {
                                                                           new NameDataErrorHandler(),
                                                                           new ParentDataErrorHandler()
                                                                       };

        public bool IsWellKnownId()
        {
            return false;
        }

        partial void OnCreated()
        {
            _editable = new EditableObject<Ntpsubview>(this);
        }

        public override string ToString()
        {
            if (Ntpview != null)
              return Name + "/" + Ntpview.Name;
            else
              return Name;


        }

        public object Clone()
        {
            return new Ntpsubview()
            {
                Name = this.Name,
                Shortname = this.Shortname,
                Ntpview = this.Ntpview
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

        private class NameDataErrorHandler : IDataErrorHandler<Ntpsubview>
        {
            #region IDataErrorHandler<Ntpsubview> Members

            public string GetError(Ntpsubview source, string propertyName, ref bool handled)
            {
                if (propertyName == "Name")
                {
                    return HandleCurrencyError(source, out handled);
                }

                return string.Empty;
            }

            #endregion

            private static string HandleCurrencyError(Ntpsubview source, out bool handled)
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

        #region Nested type: ParentDataErrorHandler

        private class ParentDataErrorHandler : IDataErrorHandler<Ntpsubview>
        {
            #region IDataErrorHandler<Ntpsubview> Members

            public string GetError(Ntpsubview source, string propertyName, ref bool handled)
            {
                if (propertyName == "Ntpview")
                {
                    return HandleCurrencyError(source, out handled);
                }

                return string.Empty;
            }

            #endregion

            private static string HandleCurrencyError(Ntpsubview source, out bool handled)
            {
                handled = false;
                if (source.Ntpview == null)
                {
                    handled = true;
                    return "Вид НТП не может быть пустым!";
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

        public IEnumerable<Ntpsubview> Ntpsubviews
        {
            get { return null; }
        }

    }
}
