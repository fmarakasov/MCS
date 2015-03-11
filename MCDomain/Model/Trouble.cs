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
    partial class Trouble : IObjectId, INamed, ICloneable, IDataErrorInfo, IEditableObject, IHierarchical
    {

        public bool IsWellKnownId()
        {
            return false;
        }


        public override string ToString()
        {
            return string.Format("{0}. {1}", Num, Name);
        }

        private EditableObject<Trouble> _editable;
        private readonly DataErrorHandlers<Trouble> _errorHandlers = new DataErrorHandlers<Trouble>
                                                                       {
                                                                           new NameDataErrorHandler()
                                                                       };

        partial void OnCreated()
        {
            _editable = new EditableObject<Trouble>(this);
        }

        public object Clone()
        {
            return new Trouble()
            {
                Name = this.Name,
                Num = this.Num,
                //Troubleregistryid = this.Troubleregistryid,
                //Toptroubleid = this.Toptroubleid
                Troublesregistry = this.Troublesregistry,
                ParentTrouble = this.ParentTrouble
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

        #region Nested type: FamilyDataErrorHandler

        private class NameDataErrorHandler : IDataErrorHandler<Trouble>
        {
            #region IDataErrorHandler<Employee> Members

            public string GetError(Trouble source, string propertyName, ref bool handled)
            {
                if (propertyName == "Family")
                {
                    return HandleCurrencyError(source, out handled);
                }

                return string.Empty;
            }

            #endregion

            private static string HandleCurrencyError(Trouble source, out bool handled)
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
                Trouble Parent = Trouble_Toptroubleid;
                while (Parent != null)
                {
                    level++;
                    Parent = Parent.Trouble_Toptroubleid;
                }

                return level;
            }
        }

        public object Parent
        {
            get { return Trouble_Toptroubleid; }
            set
            {
                Trouble_Toptroubleid = value as Trouble;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("Parent"));
                    PropertyChanged(this, new PropertyChangedEventArgs("Level"));
                }
                SendPropertyChanged("Parent");
            }
        }

        public EntitySet<Trouble> Troubles
        {
            get { return Troubles_Toptroubleid; }
        }

        public Trouble ParentTrouble
        {
            get { return Trouble_Toptroubleid; }
            set 
            { 
                Trouble_Toptroubleid = value;
                SendPropertyChanged("ParentTrouble");
            }
        }
    }
}
