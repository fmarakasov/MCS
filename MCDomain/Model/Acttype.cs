using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MCDomain.Common;
using System.ComponentModel;
using CommonBase;

namespace MCDomain.Model
{

    public enum WellKnownActtypes
    {
        [Description("Не определено")]
        Undefined = -1,
        [Description("Газпром НИОКР")]
        GazpromNiokr = 1, 
        [Description("Регионгазхолдинг")]
        RegionGasHolding = 2,
        [Description("Газпром газораспределение")]
        Gazoraspredelenie = 3,
        [Description("Газпром межрегионгаз")]
        MezhRegionGas = 4
    }

    partial class Acttype: ICloneable, IDataErrorInfo, IEditableObject, IObjectId
    {

        public bool IsWellKnownId()
        {
            var en = Enum.GetValues(typeof(WellKnownActtypes));
            return en.Cast<object>().Any(ch => (WellKnownActtypes) Id == (WellKnownActtypes) ch);
        }

        public WellKnownActtypes WellKnownType
        {
            get
            {
                if (IsWellKnownId())
                    return (WellKnownActtypes)Id;
                return WellKnownActtypes.Undefined;
            }
        }

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

        private EditableObject<Acttype> _editable;
        private readonly DataErrorHandlers<Acttype> _errorHandlers = new DataErrorHandlers<Acttype>
                                                                       {
                                                                           new NameDataErrorHandler(),
                                                                           new ContractorDataErrorHandler()
                                                                       };

        partial void OnCreated()
        {
            _editable = new EditableObject<Acttype>(this);
            
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder(Typename);
            if (Contractor != null)
                sb.Append(string.Format(" ({0})", Contractor));
            return sb.ToString();
        }

        public object Clone()
        {
            return new Acttype()
            {
                Typename = this.Typename,
                Contractor = this.Contractor,
                Isactive = this.Isactive
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

        private class ContractorDataErrorHandler : IDataErrorHandler<Acttype>
        {
            #region IDataErrorHandler<Acttype> Members

            public string GetError(Acttype source, string propertyName, ref bool handled)
            {
                if (propertyName == "Contractor")
                {
                    return HandleCurrencyError(source, out handled);
                }

                return string.Empty;
            }

            #endregion

            private static string HandleCurrencyError(Acttype source, out bool handled)
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


        #region Nested type: NameDataErrorHandler

        private class NameDataErrorHandler : IDataErrorHandler<Acttype>
        {
            #region IDataErrorHandler<Acttype> Members

            public string GetError(Acttype source, string propertyName, ref bool handled)
            {
                if (propertyName == "Typename")
                {
                    return HandleCurrencyError(source, out handled);
                }

                return string.Empty;
            }

            #endregion

            private static string HandleCurrencyError(Acttype source, out bool handled)
            {
                handled = false;
                if (String.IsNullOrEmpty(source.Typename))
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
