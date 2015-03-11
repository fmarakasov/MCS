using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using MCDomain.Common;
using CommonBase;

namespace MCDomain.Model
{
    partial class Contractor : IComparable<Contractor>, IComparable, INamed, IPerson, IObjectId, IDataErrorInfo,
                               ICloneable, IEditableObject, INull
    {
        private readonly DataErrorHandlers<Contractor> _errorHandlers = new DataErrorHandlers<Contractor>
            {
                new NameDataErrorHandler(),
                new ContractortypeDataErrorHandler()
            };

        private EditableObject<Contractor> _editable;



        public bool IsWellKnownId()
        {
            return false;
        }

        /// <summary>
        /// Получает строку адреса контрагента
        /// </summary>
        public string TotalAddress
        {
            get
            {
                var sb = new StringBuilder();
                sb.Append(SelectString(Zip, "Индекс"));
                //sb.Append(SelectString(City, ", Город"));
                //sb.Append(SelectString(Street, ", Улица"));
                //sb.Append(SelectString(Build, ", Дом"));
                //sb.Append(SelectString(Block, ", Корпус"));
                //sb.Append(SelectString(Appartment, ", Квартира"));
                sb.Append(SelectString(Address, ", Адрес"));
                return sb.ToString();
            }
        }

        /// <summary>
        /// Получает строку банка контрагента
        /// </summary>
        public string Banking
        {
            get
            {
                var sb = new StringBuilder();
                sb.Append(SelectString(Bank, "Банк", true));
                sb.Append(SelectString(Inn, ", ИНН"));
                sb.Append(SelectString(Account, ", Расчётный счёт"));
                sb.Append(SelectString(Bik, ", БИК"));
                sb.Append(SelectString(Kpp, ", КПП"));
                return sb.ToString();
            }
        }

        /*
         р/с № 40702810900000000198                                  Р/с № 40702810000000000001
    в ГПБ (ОАО), г. Москва                                           в ГПБ (ОАО), г. Москва
 к/с № 30101810200000000823                                  к/с № 30101810200000000823
 БИК  044525823                                                         БИК  044525823
 ИНН  7734034550/КПП 774850001                          ИНН  7736050003/КПП 997250001
 ОКОНХ 95120, ОКПО 00158847




          */

        public string ContractorFullDesc
        {
            get
            {
                var sb = new StringBuilder();

                sb.Append(!String.IsNullOrWhiteSpace(Zip)
                              ? SelectString(Zip + ", " + Address, "Адрес: ")
                              : SelectString(Address, "Адрес: "));
                sb.Append((char) 13);
                sb.Append(SelectString(Account, "р/c № "));
                sb.Append((char) 13);
                sb.Append(SelectString(Bank, "в "));
                sb.Append((char) 13);
                sb.Append(SelectString(Correspaccount, "к/c № "));
                sb.Append((char) 13);
                sb.Append(SelectString(Bik, "БИК "));
                sb.Append((char) 13);
                sb.Append(SelectString(Inn, "ИНН "));
                sb.Append(SelectString(Kpp, "/КПП "));
                return sb.ToString();
            }
        }


        /// <summary>
        /// Получает список персон контрагента
        /// </summary>
        public IEnumerable<Person> Persons
        {
            get
            {
                Contract.Requires(Contractorpositions != null);
                Contract.Ensures(Contract.Result<IEnumerable<Person>>() != null);
                return Contractorpositions.SelectMany(x => x.People);
            }
        }

        #region ICloneable Members

        public object Clone()
        {
            return new Contractor
                {
                    Name = Name,
                    Shortname = Shortname,
                    Zip = Zip,
                    Account = Account,
                    Bik = Bik,
                    Kpp = Kpp,
                    Bank = Bank,
                    Contractortype = Contractortype,
                    Inn = Inn,
                    Address = Address
                };
        }

        #endregion

        #region IComparable Members

        public int CompareTo(object obj)
        {
            return this.CompareNames(obj as INamed);
        }

        #endregion

        #region IComparable<Contractor> Members

        public int CompareTo(Contractor other)
        {
            return this.CompareNames(other);
        }

        #endregion

        /*
 р/с № 40702810900000000198                                  Р/с № 40702810000000000001
в ГПБ (ОАО), г. Москва                                           в ГПБ (ОАО), г. Москва
к/с № 30101810200000000823                                  к/с № 30101810200000000823
БИК  044525823                                                         БИК  044525823
ИНН  7734034550/КПП 774850001                          ИНН  7736050003/КПП 997250001
ОКОНХ 95120, ОКПО 00158847




  */

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

        #region IEditableObject Members

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

        #region INull Members

        bool INull.IsNull
        {
            get { return false; }
        }

        #endregion

        public override string ToString()
        {
            WellKnownContractorTypes contractortype = Contractortype.Return(x => x.WellKnownType,
                                                                            WellKnownContractorTypes.Undefined);
            return (contractortype == WellKnownContractorTypes.Individual) ? this.GetShortFullName() : Name.Return(x=>x, string.Empty);
        }

        public string Truename
        {
            get
            {
                return ToString();                                                             
            }
        }

        private string SelectString(string value, string constStr, bool quoted = false)
        {
            return string.IsNullOrEmpty(value)
                       ? constStr + " N/A"
                       : string.Format("{0} {1}", constStr, quoted ? "\"" + value + "\"" : value);
        }

        partial void OnCreated()
        {
            _editable = new EditableObject<Contractor>(this);
        }

        #region Nested type: ContractortypeDataErrorHandler

        private class ContractortypeDataErrorHandler : IDataErrorHandler<Contractor>
        {
            #region IDataErrorHandler<Contractor> Members

            public string GetError(Contractor source, string propertyName, ref bool handled)
            {
                if (propertyName == "Contractortype")
                {
                    return HandleCurrencyError(source, out handled);
                }

                return string.Empty;
            }

            #endregion

            private static string HandleCurrencyError(Contractor source, out bool handled)
            {
                handled = false;
                if (source.Contractortype == null)
                {
                    handled = true;
                    return "Тип контрагента не может быть пустым!";
                }
                return string.Empty;
            }
        }

        #endregion

        #region Nested type: NameDataErrorHandler

        private class NameDataErrorHandler : IDataErrorHandler<Contractor>
        {
            #region IDataErrorHandler<Contractor> Members

            public string GetError(Contractor source, string propertyName, ref bool handled)
            {
                if (propertyName == "Name")
                {
                    return HandleCurrencyError(source, out handled);
                }

                return string.Empty;
            }

            #endregion

            private static string HandleCurrencyError(Contractor source, out bool handled)
            {
                handled = false;
                if (string.IsNullOrEmpty(source.Truename))
                {
                    handled = true;
                    return "Наименование не может быть пустым!";
                }
                return string.Empty;
            }
        }

        #endregion
    }

    internal class NullContractor : Contractor, INull
    {
        public static readonly NullContractor Instance = new NullContractor {Name = "Нет данных"};

        #region INull Members

        bool INull.IsNull
        {
            get { return false; }
        }

        #endregion
    }
}