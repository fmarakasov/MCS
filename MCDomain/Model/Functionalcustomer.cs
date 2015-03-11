using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Linq;
using System.Linq;
using System.Text;
using MCDomain.Common;
using CommonBase;

namespace MCDomain.Model
{
    partial class Functionalcustomer : INamed, IObjectId, ICloneable, IDataErrorInfo, IEditableObject, IHierarchical
    {

        public bool IsWellKnownId()
        {
            return false;
        }

        public override string ToString()
        {
            return Name;
        }

        private EditableObject<Functionalcustomer> _editable;
        private readonly DataErrorHandlers<Functionalcustomer> _errorHandlers = new DataErrorHandlers<Functionalcustomer>
                                                                                       {
                                                                                            new NameDataErrorHandler(),
                                                                                            new ContractorDataErrorHandler(),
                                                                                            new TypeDataErrorHandler()
                                                                                       };

        partial void OnCreated()
        {
            _editable = new EditableObject<Functionalcustomer>(this);
        }

        public object Clone()
        {
            return new Functionalcustomer()
            {
                Name = this.Name,
                Functionalcustomertypeid = this.Functionalcustomertypeid,
                Contractorid = this.Contractorid,
                Parentfunctionalcustomerid = this.Parentfunctionalcustomerid
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

        private class NameDataErrorHandler : IDataErrorHandler<Functionalcustomer>
        {
            #region IDataErrorHandler<Functionalcustomer> Members

            public string GetError(Functionalcustomer source, string propertyName, ref bool handled)
            {
                if (propertyName == "Name")
                {
                    return HandleCurrencyError(source, out handled);
                }

                return string.Empty;
            }

            #endregion

            private static string HandleCurrencyError(Functionalcustomer source, out bool handled)
            {
                handled = false;
                if (String.IsNullOrEmpty(source.Name))
                {
                    handled = true;
                    return "Наименование не может быть пустым!";
                }
                return string.Empty;
            }
        }

        #endregion

        #region Nested type: ContractorDataErrorHandler

        private class ContractorDataErrorHandler : IDataErrorHandler<Functionalcustomer>
        {
            #region IDataErrorHandler<Functionalcustomer> Members

            public string GetError(Functionalcustomer source, string propertyName, ref bool handled)
            {
                if (propertyName == "Contractor")
                {
                    return HandleCurrencyError(source, out handled);
                }

                return string.Empty;
            }

            #endregion

            private static string HandleCurrencyError(Functionalcustomer source, out bool handled)
            {
                handled = false;
                if (source.Contractor == null)
                {
                    handled = true;
                    return "Организация не может быть пустой!";
                }
                return string.Empty;
            }
        }

        #endregion

        #region Nested type: ContractorDataErrorHandler

        private class TypeDataErrorHandler : IDataErrorHandler<Functionalcustomer>
        {
            #region IDataErrorHandler<Functionalcustomer> Members

            public string GetError(Functionalcustomer source, string propertyName, ref bool handled)
            {
                if (propertyName == "Functionalcustomertype")
                {
                    return HandleCurrencyError(source, out handled);
                }

                return string.Empty;
            }

            #endregion

            private static string HandleCurrencyError(Functionalcustomer source, out bool handled)
            {
                handled = false;
                if (source.Functionalcustomertype == null)
                {
                    handled = true;
                    return "Тип функционального заказчика не может быть пустым!";
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

        public int Level
        {
            get
            {
                int level = 0;
                Functionalcustomer Parent = this.Functionalcustomer_Parentfunctionalcustomerid;
                while (Parent != null)
                {
                    level++;
                    Parent = Parent.Functionalcustomer_Parentfunctionalcustomerid;
                }

                return level;
            }
        }

        public object Parent
        {
            get { return Functionalcustomer_Parentfunctionalcustomerid; }
            set
            {
                Functionalcustomer_Parentfunctionalcustomerid = value as Functionalcustomer;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("Parent"));
                    PropertyChanged(this, new PropertyChangedEventArgs("Level"));
                }
            }
        }

        public EntitySet<Functionalcustomer> Functionalcustomers
        {
            get { return Functionalcustomers_Parentfunctionalcustomerid; }
        }
    }
}
