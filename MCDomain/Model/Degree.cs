using System;
using System.Text;
using System.ComponentModel;
using CommonBase;

namespace MCDomain.Model
{
    partial class Degree : IObjectId, ICloneable, IDataErrorInfo, IEditableObject
    {
        private EditableObject<Degree> _editable;
        private readonly DataErrorHandlers<Degree> _errorHandlers = new DataErrorHandlers<Degree>
                                                                       {
                                                                           new NameDataErrorHandler(),
                                                                       };

        public bool IsWellKnownId()
        {
            return false;
        }


        partial void  OnCreated()
        {
            _editable = new EditableObject<Degree>(this);
        }
        /// <summary>
        /// Преобразует строковое представление степени в короткую строку вида Х.Х.Х.
        /// </summary>
        /// <param name="degree"></param>
        /// <returns></returns>
        public static string ConvertToShort(string degree)
        {
            if (string.IsNullOrWhiteSpace(degree)) return string.Empty;
            var splited = degree.Split();
            var sb = new StringBuilder();
            for (int i = 0; i < splited.Length; ++i)
            {
                sb.Append(splited[i][0]);
                sb.Append(".");
            }
            return sb.ToString();
        }

        public override string ToString()
        {
            return Name;
        }

        public object Clone()
        {
            return new Degree()
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

        private class NameDataErrorHandler : IDataErrorHandler<Degree>
        {
            #region IDataErrorHandler<Degree> Members

            public string GetError(Degree source, string propertyName, ref bool handled)
            {
                if (propertyName == "Name")
                {
                    return HandleCurrencyError(source, out handled);
                }

                return string.Empty;
            }

            #endregion

            private static string HandleCurrencyError(Degree source, out bool handled)
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
