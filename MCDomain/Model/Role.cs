using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MCDomain.Common;
using System.ComponentModel;
using CommonBase;

namespace MCDomain.Model
{


    public enum WellKnownRoles:long
    {
        [Description("Не определён")]
        Undfined = -1,
        [Description("Руководитель договора")]
        ContractChief = 1,
        [Description("Руководитель направления")]
        Manager = 2,
        [Description("Замдиректора")]
        Director = 3,
        [Description("Ответственный по договорам")]
        DepartmentResponsible = 4,
        [Description("Куратор от договорного отдела")]
        Curator = 5
    }

    partial class Role : IObjectId, ICloneable, IDataErrorInfo, IEditableObject
    {
        private EditableObject<Role> _editable;
        private readonly DataErrorHandlers<Role> _errorHandlers = new DataErrorHandlers<Role>
                                                                       {
                                                                           new NameDataErrorHandler(),
                                                                       };



        public bool IsWellKnownId()
        {
            var en = Enum.GetValues(typeof(WellKnownRoles));
            foreach (var ch in en)
            {
                if ((WellKnownRoles)Id == (WellKnownRoles)ch) return true;
            }
            return false;
        }


        /// <summary>
        /// Получает известную роль
        /// </summary>
        public WellKnownRoles WellKnownType
        {
            get
            {
                if (IsWellKnownId())
                    return (WellKnownRoles)Id;
                return WellKnownRoles.Undfined;
            }
        }


        partial void OnCreated()
        {
            _editable = new EditableObject<Role>(this);
        }

        public override string ToString()
        {
            return Name;
        }

        public object Clone()
        {
            return new Role()
            {
                Name = this.Name
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

        private class NameDataErrorHandler : IDataErrorHandler<Role>
        {
            #region IDataErrorHandler<Role> Members

            public string GetError(Role source, string propertyName, ref bool handled)
            {
                if (propertyName == "Name")
                {
                    return HandleCurrencyError(source, out handled);
                }

                return string.Empty;
            }

            #endregion

            private static string HandleCurrencyError(Role source, out bool handled)
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
