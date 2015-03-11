using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using MCDomain.Common;
using CommonBase;

namespace MCDomain.Model
{
    /// <summary>
    /// Известные типы контрагентов
    /// </summary>
    public enum WellKnownContractorTypes
    {
        [Description("Физическое лицо")]
        Individual = 4,
        [Description("Не определено")]
        Undefined=-1,
        [Description("ОАО \"Газпром\"")]
        Gazprom=3,
        [Description("Дочерние организации")]
        Subsidiary=2,
        [Description("Другие организации")]
        Other=1
    }

    partial class Contractortype : IComparable<Contractortype>, IComparable, INamed, IObjectId, ICloneable, IDataErrorInfo, IEditableObject, IReportOrderProvider
    {
        public bool IsWellKnownId()
        {
            var en = Enum.GetValues(typeof (WellKnownContractorTypes));
            foreach (var ch in en)
            {
                if ((WellKnownContractorTypes)Id == (WellKnownContractorTypes)ch) return true;
            }
            return false;
        }

        /// <summary>
        /// Получает известный тип контрагента
        /// </summary>
        public WellKnownContractorTypes WellKnownType
        {
            get
            {
                if (IsWellKnownId())
                    return (WellKnownContractorTypes) Id;
                return WellKnownContractorTypes.Other;
            }
        }

        public override string ToString()
        {
            return Name;
        }

        public int CompareTo(Contractortype other)
        {
            return ComparableNamed.CompareNames(this, other);
        }

        public int CompareTo(object obj)
        {
            return ComparableNamed.CompareNames(this, obj as INamed);
        }

        private EditableObject<Contractortype> _editable;
        private readonly DataErrorHandlers<Contractortype> _errorHandlers = new DataErrorHandlers<Contractortype>
                                                                       {
                                                                           new NameDataErrorHandler(),
                                                                           new ReportOrderErrorHandler()
                                                                       };

        partial void OnCreated()
        {
            _editable = new EditableObject<Contractortype>(this);
        }

        public object Clone()
        {
            return new Contractortype()
            {
                Name = this.Name,
                Reportorder = Reportorder
            };
        }

        partial void OnReportorderChanging(int? value)
        {
            // ValueRangeChecker.CheckReportOrderValue(value);
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

        private class NameDataErrorHandler : IDataErrorHandler<Contractortype>
        {
            #region IDataErrorHandler<Contractortype> Members

            public string GetError(Contractortype source, string propertyName, ref bool handled)
            {
                if (propertyName == "Name")
                {
                    return HandleCurrencyError(source, out handled);
                }

                return string.Empty;
            }

            #endregion

            private static string HandleCurrencyError(Contractortype source, out bool handled)
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
