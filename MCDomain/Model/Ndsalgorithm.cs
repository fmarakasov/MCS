using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using MCDomain.Common;
using CommonBase;

namespace MCDomain.Model
{
    /// <summary>
    /// Формулировки НДС
    /// </summary>
    public enum TypeOfNds
    {
        /// <summary>
        /// Неопределено
        /// </summary>
        Undefinded = -1,
        /// <summary>
        /// Включая НДС
        /// </summary>
        IncludeNds = 1,
        /// <summary>
        /// Кроме того НДС
        /// </summary>
        ExcludeNds = 2,
        /// <summary>
        /// НДС не предусмотрен
        /// </summary>
        NoNds = 3
    }
    partial class Ndsalgorithm : INamed, IObjectId, ICloneable, IDataErrorInfo, IEditableObject
    {

        public bool IsWellKnownId()
        {
            var en = Enum.GetValues(typeof(TypeOfNds));
            foreach (var ch in en)
            {
                if ((TypeOfNds)Id == (TypeOfNds)ch) return true;
            }
            return false;
        }

        /// <summary>
        /// Получает известный тип контрагента
        /// </summary>
        public TypeOfNds WellKnownType
        {
            get
            {
                if (IsWellKnownId())
                    return (TypeOfNds)Id;
                return TypeOfNds.Undefinded;
            }
        }

        /// <summary>
        /// Получает объект алгоритма "Включая НДС"
        /// </summary>
        public static readonly Ndsalgorithm IncludeNdsAlgorithm = new Ndsalgorithm() {Id = 1, Name = "Включая НДС"};
        /// <summary>
        /// Получает объект алгоритма "Не включая НДС"
        /// </summary>
        public static readonly Ndsalgorithm ExcludeNdsAlgorithm = new Ndsalgorithm() { Id = 2, Name = "Не включая НДС" };
        /// <summary>
        /// Получает объект алгоритма "НДС не предусмотрен"
        /// </summary>
        public static readonly Ndsalgorithm NoNdsAlgorithm = new Ndsalgorithm() { Id = 3, Name = "НДС не предусмотрен" };
      
        /// <summary>
        /// Получает объект Алгоритм НДС по типу
        /// </summary>
        /// <param name="typeOfNds">Тип алгоритма НДС</param>
        /// <returns>Объект алгоритма НДС</returns>
        public static Ndsalgorithm TypeToObject(TypeOfNds typeOfNds)
        {
            switch (typeOfNds)
            {
                case TypeOfNds.IncludeNds:
                    return IncludeNdsAlgorithm;
                case TypeOfNds.ExcludeNds:
                    return ExcludeNdsAlgorithm;
                case TypeOfNds.NoNds:
                    return NoNdsAlgorithm;
                default:
                    return null;
            }
        }

        public override string ToString()
        {
            return Name;
        }
        /// <summary>
        /// Вычисляет НДС в соответствии с типом НДС
        /// </summary>
        /// <param name="price">Цена</param>
        /// <param name="fraction">Доля НДС</param>
        /// <returns>НДС</returns>
        
        public decimal GetNds(decimal price, double fraction)
        {
            DefaultNdsAlgorithm alg = DefaultNdsAlgorithm.CreateInstance((TypeOfNds) Id);
            if (alg == null)
                throw new NoNullAllowedException("Не определён тип НДС по идентификатору. Идентификаторы НДС должны совпадать со значениями типа TypeOfNds.");
            
            return alg.GetNds(price, fraction);
        }

        
        /// <summary>
        /// Получает цену договора без учёта НДС
        /// </summary>
        /// <param name="Price">Цена договора</param>
        /// <param name="fraction">Доля НДС</param>
        /// <returns>Цена без НДС</returns>        
        public decimal GetPure(decimal price, double fraction)
        {
            DefaultNdsAlgorithm alg = DefaultNdsAlgorithm.CreateInstance((TypeOfNds)Id);
            if (alg == null)
                throw new NoNullAllowedException("Не определён тип НДС по идентификатору. Идентификаторы НДС должны совпадать со значениями типа TypeOfNds.");

            return alg.GetPure(price, fraction);
        }

        /// <summary>
        /// Получает тип НДС
        /// </summary>
        [Pure]
        public TypeOfNds NdsType
        {
            
            get
            {
                //Contract.Requires(Id.Between((int)TypeOfNds.IncludeNds, (int)TypeOfNds.NoNds));
                return (TypeOfNds) Id;
            }
        }

        private EditableObject<Ndsalgorithm> _editable;
        private readonly DataErrorHandlers<Ndsalgorithm> _errorHandlers = new DataErrorHandlers<Ndsalgorithm>
                                                                       {
                                                                           new NameDataErrorHandler(),
                                                                       };

        partial void OnCreated()
        {
            _editable = new EditableObject<Ndsalgorithm>(this);
        }

        public object Clone()
        {
            return new Ndsalgorithm()
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

        private class NameDataErrorHandler : IDataErrorHandler<Ndsalgorithm>
        {
            #region IDataErrorHandler<Ndsalgorithm> Members

            public string GetError(Ndsalgorithm source, string propertyName, ref bool handled)
            {
                if (propertyName == "Name")
                {
                    return HandleCurrencyError(source, out handled);
                }

                return string.Empty;
            }

            #endregion

            private static string HandleCurrencyError(Ndsalgorithm source, out bool handled)
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
        /// <summary>
        /// Преобразует идентификатор типа алгоритма НДС в объект
        /// </summary>
        /// <param name="ndsAlgorithmid">Идентьификатор типа</param>
        /// <returns>Объект алгоритма НДС</returns>
        public static Ndsalgorithm TypeIdToObject(long? ndsAlgorithmid)
        {
            if (ndsAlgorithmid.GetValueOrDefault().Between((int)TypeOfNds.IncludeNds, (int)TypeOfNds.NoNds))
                return TypeToObject((TypeOfNds) ndsAlgorithmid.GetValueOrDefault());
            return TypeToObject(TypeOfNds.Undefinded);
        }
    }
}
