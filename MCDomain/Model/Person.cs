using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using MCDomain.Common;
using CommonBase;

namespace MCDomain.Model
{
    partial class Person : IObjectId, IComparable, ICloneable, IDataErrorInfo, IEditableObject, IPerson
    {

        public bool IsWellKnownId()
        {
            return false;
        }

        /// <summary>
        /// Получает признак того, что персона имеет учёную степень
        /// </summary>
        public bool HasDegree
        {
            get
            {
                return Degreeid.HasValue;
            }
        }


        public string PostName(IContractor contractor)
        {
            
                string sPersonPost;
                if (Contractorposition != null) sPersonPost = Contractorposition.ToString();
                else
                {
                    sPersonPost = "<должность не указана>";
                    if (contractor != null) sPersonPost = String.Concat(sPersonPost, " ", contractor.ToString());
                }

                sPersonPost = String.Concat(sPersonPost, " ", this.GetShortFullNameRev());
                return sPersonPost;
            
        }

        public bool Contractheadauthority
        {
            get { return Iscontractheadauthority; }
            set
            {
                SendPropertyChanging("Contractheadauthority");
                Iscontractheadauthority = value;
                SendPropertyChanged("Contractheadauthority");
            }
        }

        public bool Actsignauthority
        {
            get { return Isactsignauthority; }
            set
            {
                SendPropertyChanging("Actsignauthority");
                Isactsignauthority = value;
                SendPropertyChanged("Actsignauthority");
            }
        }

        public bool Valid
        {
            get { return Isvalid; }
            set
            {
                SendPropertyChanging("Valid");
                Isvalid = value;
                SendPropertyChanged("Valid");
            }
        }
        
        public override string ToString()
        {
            if (this.IsValidPersonName())
                return this.GetShortFullName();
            return base.ToString();
        }

        public int CompareTo(object obj)
        {
            return this.ToString().CompareTo((obj as Person).ToString());
        }

        private EditableObject<Person> _editable;
        private readonly DataErrorHandlers<Person> _errorHandlers = new DataErrorHandlers<Person>
                                                                       {
                                                                           new IsactsignauthorityDataErrorHandler(),
                                                                           new IscontractheadauthorityDataErrorHandler(),
                                                                           new IsvalidDataErrorHandler(),
                                                                           new FamilyDataErrorHandler()
                                                                       };

        partial void OnCreated()
        {
            _editable = new EditableObject<Person>(this);
        }

        public object Clone()
        {
            return new Person()
                       {
                           Degreeid = this.Degreeid,
                           Iscontractheadauthority = this.Iscontractheadauthority,
                           Isactsignauthority = this.Isactsignauthority,
                           Contractorpositionid = this.Contractorpositionid,
                           Isvalid = this.Isvalid,
                           Familyname = this.Familyname,
                           Middlename = this.Middlename,
                           Firstname = this.Firstname,
                           Sex = this.Sex

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

        #region Nested type: IsvalidDataErrorHandler

        private class IsvalidDataErrorHandler : IDataErrorHandler<Person>
        {
            #region IDataErrorHandler<Acttype> Members

            public string GetError(Person source, string propertyName, ref bool handled)
            {
                if (propertyName == "Contractor")
                {
                    return HandleCurrencyError(source, out handled);
                }

                return string.Empty;
            }

            #endregion

            private static string HandleCurrencyError(Person source, out bool handled)
            {
                handled = false;
                if (!source.Isvalid)
                {
                    handled = true;
                    return "Атрибут не может быть пустым!";
                }
                return string.Empty;
            }
        }

        #endregion

        #region Nested type: IsactsignauthorityDataErrorHandler

        private class IsactsignauthorityDataErrorHandler : IDataErrorHandler<Person>
        {
            #region IDataErrorHandler<Acttype> Members

            public string GetError(Person source, string propertyName, ref bool handled)
            {
                if (propertyName == "Isactsignauthority")
                {
                    return HandleCurrencyError(source, out handled);
                }

                return string.Empty;
            }

            #endregion

            private static string HandleCurrencyError(Person source, out bool handled)
            {
                handled = false;
                //if (!source.Isactsignauthority)
                //{
                //    handled = true;
                //    return "Атрибут не может быть пустым!";
                //}
                return string.Empty;
            }
        }

        #endregion

        #region Nested type: IscontractheadauthorityDataErrorHandler

        private class IscontractheadauthorityDataErrorHandler : IDataErrorHandler<Person>
        {
            #region IDataErrorHandler<Acttype> Members

            public string GetError(Person source, string propertyName, ref bool handled)
            {
                if (propertyName == "Iscontractheadauthority")
                {
                    return HandleCurrencyError(source, out handled);
                }

                return string.Empty;
            }

            #endregion

            private static string HandleCurrencyError(Person source, out bool handled)
            {
                handled = false;
                //if (source.Iscontractheadauthority)
                //{
                //    handled = true;
                //    return "Атрибут не может быть пустым!";
                //}
                return string.Empty;
            }
        }

        #endregion

        #region Nested type: FamilyDataErrorHandler

        /// <summary>
        /// Фамилия
        /// </summary>
        public String Family
        {
            get { return Familyname; }
            set { Familyname = value; }
        }
        
        private class FamilyDataErrorHandler : IDataErrorHandler<Person>
        {
            #region IDataErrorHandler<Acttype> Members

            public string GetError(Person source, string propertyName, ref bool handled)
            {
                if (propertyName == "Family")
                {
                    return HandleCurrencyError(source, out handled);
                }

                return string.Empty;
            }

            #endregion

            private static string HandleCurrencyError(Person source, out bool handled)
            {
                handled = false;
                if (String.IsNullOrEmpty(source.Family))
                {
                    handled = true;
                    return "Фамилия не может быть пустой!";
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
        /// <summary>
        /// Преобразует фамилию имя и отчество в сокращённый формат И. О. Фамилия
        /// </summary>
        /// <param name="personFamilyname"></param>
        /// <param name="personFirstname"></param>
        /// <param name="personMiddlename"></param>
        /// <returns></returns>
        public static string ToShortName(string personFamilyname, string personFirstname, string personMiddlename)
        {
            var sb = new StringBuilder();
            
            AppendFirstNameChar(sb, personFirstname);
            AppendFirstNameChar(sb, personMiddlename);
            sb.Append(Correct(personFamilyname));
            return sb.ToString();
        }
        /// <summary>
        /// Корректирует первую букву имени, приводя её к верхнему регистру
        /// </summary>
        /// <param name="name">Имя</param>
        /// <returns>Скорректированное имя</returns>
        public static string Correct(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) return name;

            var sb = new StringBuilder(name);
            sb[0] = Char.ToUpper(sb[0]);
            return sb.ToString();
        }

        private static void AppendFirstNameChar(StringBuilder sb, string str)
        {
            if (!string.IsNullOrWhiteSpace(str))
                sb.AppendFormat("{0}. ", Char.ToUpper(str[0]));
        }
    }
}
