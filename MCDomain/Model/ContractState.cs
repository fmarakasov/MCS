
using System.Diagnostics.Contracts;
using MCDomain.Common;
using System.ComponentModel;
using System;
using CommonBase;

namespace MCDomain.Model
{
	/// <summary>
	/// Возможные состояния договора
	/// </summary>
	public enum WellKnownContractStates
	{
        /// <summary>
        /// Договор не подписан
        /// </summary>
        [Description("Не подписан")]
	    Unsigned = 1,
		/// <summary>
		/// Договор подписан
		/// </summary>
		[Description("Подписан")]
        Signed = 2, 
		/// <summary>
		/// Не определено
		/// </summary>
		[Description("Не определен")]
		Undefined = -1
	}

	partial class Contractstate : IObjectId, INamed, IDataErrorInfo, ICloneable, IEditableObject
	{
		private readonly DataErrorHandlers<Contractstate> _errorHandlers = new DataErrorHandlers<Contractstate>
																	   {
																		   new NameDataErrorHandler()
																	   };


        public bool IsWellKnownId()
        {
            var en = Enum.GetValues(typeof(WellKnownContractStates));
            foreach (var ch in en)
            {
                if ((WellKnownContractStates)Id == (WellKnownContractStates)ch) return true;
            }
            return false;
        }

        /// <summary>
        /// Получает известный тип контрагента
        /// </summary>
        public WellKnownContractStates WellKnownType
        {
            get
            {
                if (IsWellKnownId())
                    return (WellKnownContractStates)Id;
                return WellKnownContractStates.Undefined;
            }
        }


        /// <summary>
		/// Получает состояние по идентификатору
		/// </summary>
		/// <param name="stateId">Идентификатор состояния</param>
		/// <returns>Предопределённое состояние</returns>
        public static WellKnownContractStates IdToState(double stateId)
		{
            var en = Enum.GetValues(typeof(WellKnownContractStates));
            foreach (var ch in en)
            {
                if ((WellKnownContractStates)stateId == (WellKnownContractStates)ch) return (WellKnownContractStates)stateId;
            }
            
            return WellKnownContractStates.Undefined;
	
		}
		/// <summary>
		/// Получает или устанавливает состояние договора из числа предопределённых
		/// </summary>
        public WellKnownContractStates State
		{
			[Pure]
			get { return IdToState(Id); }
			set
			{
				Contract.Ensures((int)Id == (int)value);
				if ((int)value == (int)Id) return;
				SendPropertyChanging("State");
				Id = (int)value;
				SendPropertyChanged("State");
			}
		}

		/// <summary>
		/// Получает признак того, что договор подписан
		/// </summary>
		public bool IsSigned
		{
            get { return State == WellKnownContractStates.Signed; }
		}

		
		/// <summary>
		/// Получает признак того, что договор не подписан
		/// </summary>
		public bool IsUnsigned
		{
			get { return !IsSigned; }
		}

		public override string ToString()
		{
			return Name;
		}

		///// <summary>
		///// Создаёт экземпляр типа Подписан
		///// </summary>
		//public static Contractstate SignedInstance
		//{
		//    get { return new Contractstate() {State = ContractStates.Signed, Name = "Подписан"}; }
		//}

		#region IDataErrorInfo Members

		public string Error
		{
			get
			{
				return this.Validate();
			}
		}

		public string this[string columnName]
		{
			get { return _errorHandlers.HandleError(this, columnName); }
		}

		#endregion
		
		#region Nested type: NameDataErrorHandler

		private class NameDataErrorHandler : IDataErrorHandler<Contractstate>
		{
			#region IDataErrorHandler<Contractstate> Members

			public string GetError(Contractstate source, string propertyName, ref bool handled)
			{
				if (propertyName == "Name")
				{
					return HandleCurrencyError(source, out handled);
				}

				return string.Empty;
			}

			#endregion

			private static string HandleCurrencyError(Contractstate source, out bool handled)
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

		public object Clone()
		{
			return new Contractstate
					   {
						   Name = this.Name
					   };
		}

		#region IEditableObject members

		private Contractstate backup = null;
		private bool inTxn = false;

		public void BeginEdit()
		{
			if (!inTxn)
			{
				backup = Clone() as Contractstate;
				inTxn = true;
			}

		}

		public void CancelEdit()
		{
			if (inTxn)
			{
				this.Name = backup.Name;
				inTxn = false;
			}
		}

		public void EndEdit()
		{
			if (inTxn)
			{
				backup = new Contractstate();
				inTxn = false;
			}

		}

		#endregion
	}
}
