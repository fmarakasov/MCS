using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MCDomain.Common;
using System.ComponentModel;
using CommonBase;

namespace MCDomain.Model
{
    /// <summary>
    /// Определяет известные типы договоров
    /// </summary>
    public enum WellKnownContractTypes
    {
        [Description("Не определён")]
        Undefined,
        [Description("НИОКР")]
        Niokr=1,
        [Description("Газофикация")]
        Gazofication=2,
        [Description("ПИР")]
        Pir=3,
        [Description("Экспертиза")]
        Expertise=4,
        [Description("Производство")]
        Manufacturing=5,        
        [Description("Оказание услуг")]
        ServicesAccomplishment=7,
        [Description("Услуги")]
        Service = 10000
    }

    partial class Contracttype : IComparable<Contracttype>, IComparable, INamed, IObjectId, IDataErrorInfo, ICloneable, IEditableObject, IReportOrderProvider
    {
        private readonly DataErrorHandlers<Contracttype> _errorHandlers = new DataErrorHandlers<Contracttype>
                                                                       {
                                                                           new NameDataErrorHandler(),
                                                                           new ReportOrderErrorHandler()
                                                                       };



        public bool IsWellKnownId()
        {
            var en = Enum.GetValues(typeof(WellKnownContractTypes));
            foreach (var ch in en)
            {
                if ((WellKnownContractTypes)Id == (WellKnownContractTypes)ch) return true;
            }
            return false;
        }

        /// <summary>
        /// Получает известный тип договора
        /// </summary>
        public WellKnownContractTypes WellKnownType
        {
            get
            {
                if (IsWellKnownId())
                    return (WellKnownContractTypes)Id;
                return WellKnownContractTypes.Undefined;
            }
        }

        public override string ToString()
        {
            return Name;
        }

        public int CompareTo(Contracttype other)
        {
            return ComparableNamed.CompareNames(this, other);
        }

        public int CompareTo(object obj)
        {
            return ComparableNamed.CompareNames(this, obj as INamed);
        }

        #region IDataErrorInfo Members

        public string Error
        {
            get { return this.Validate(); }
        }

        public string this[string columnName]
        {
            get { return _errorHandlers.HandleError(this, columnName); }
        }

        #endregion

        #region Nested type: NameDataErrorHandler

        private class NameDataErrorHandler : IDataErrorHandler<Contracttype>
        {
            #region IDataErrorHandler<Contractstate> Members

            public string GetError(Contracttype source, string propertyName, ref bool handled)
            {
                if (propertyName == "Name")
                {
                    return HandleCurrencyError(source, out handled);
                }

                return string.Empty;
            }

            #endregion

            private static string HandleCurrencyError(Contracttype source, out bool handled)
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

        partial void OnReportorderChanging(int? value)
        {
//            ValueRangeChecker.CheckReportOrderValue(value);
        }
       
        public object Clone()
        {
            return new Contracttype
                       {
                           Name = this.Name,
                           Reportorder = Reportorder
                       };
        }

        /// <summary>
        /// краткое наименование, например страницы в экселе называть
        /// </summary>
        public string Shortname
        {
            get
            {

                string sname;
                string snonallowed = ":?*[]/\\"; 
                if (Name.Count() > 32)
                {
                    sname = Name.TakeWhile(s => s != ' ').Aggregate(new StringBuilder(), (x, y) =>
                                                                                                    {
                                                                                                        if (!y.In(snonallowed.ToCharArray()))
                                                                                                            x.Append(y);
                                                                                                        return x;
                                                                                                    },
                                                                           (x) => x.ToString());
                    if (sname.Count() > 32) sname = sname.Substring(0, 32);
                }
                else sname = Name.Aggregate(new StringBuilder(), (x, y) =>
                                                                            {
                                                                               if (!y.In(snonallowed.ToCharArray()))
                                                                                 x.Append(y);
                                                                               return x;
                                                                             },
                                                                 (x) => x.ToString());

                return sname;
            }

        }

        #region IEditableObject members
        private EditableObject<Contracttype> _editable;

    
        partial void OnCreated()
        {
            _editable = new EditableObject<Contracttype>(this);
        }


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

        #endregion
    }
}
