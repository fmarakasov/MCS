using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using MCDomain.Common;
using CommonBase;

namespace MCDomain.Model
{
    public partial class Sightfuncpersonscheme : ICloneable, IDataErrorInfo, IEditableObject
    {
        public bool? Active
        {
            get { return Isactive; }
            set
            {
                SendPropertyChanging("Active");
                Isactive = value;
                SendPropertyChanged("Active");
            }
        }

        private EditableObject<Sightfuncpersonscheme> _editable;
        private readonly DataErrorHandlers<Sightfuncpersonscheme> _errorHandlers = new DataErrorHandlers<Sightfuncpersonscheme>
                                                                       {
                                                                           new FunccustomerpersonDataErrorHandler()
                                                                       };

        partial void OnCreated()
        {
            _editable = new EditableObject<Sightfuncpersonscheme>(this);
        }

        public override string ToString()
        {
            if (!Active.HasValue)
                return "№ " + Num + " от " + Funccustomerperson.ToString();

            if (Active.Value)
                return "№ " + Num + " от " + Funccustomerperson.ToString() + " - активна";
            else
                return "№ " + Num + " от " + Funccustomerperson.ToString() + " - не активна";
        }

        public object Clone()
        {
            return new Sightfuncpersonscheme()
            {
                Isactive = this.Isactive,
                Num = this.Num,
                Funccustomerperson = this.Funccustomerperson
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

        #region Nested type: FunccustomerpersonDataErrorHandler

        private class FunccustomerpersonDataErrorHandler : IDataErrorHandler<Sightfuncpersonscheme>
        {
            #region IDataErrorHandler<Sightfuncpersonscheme> Members

            public string GetError(Sightfuncpersonscheme source, string propertyName, ref bool handled)
            {
                if (propertyName == "Funccustomerperson")
                {
                    return HandleCurrencyError(source, out handled);
                }

                return string.Empty;
            }

            #endregion

            private static string HandleCurrencyError(Sightfuncpersonscheme source, out bool handled)
            {
                handled = false;
                if (source.Funccustomerperson == null)
                {
                    handled = true;
                    return "Представитель функционального заказчика не может быть пустым!";
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
