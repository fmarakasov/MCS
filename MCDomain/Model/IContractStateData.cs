using System;
using MCDomain.Common;
using CommonBase;

namespace MCDomain.Model
{
    /// <summary>
    /// Определяет базовые характеристики договора для вычисления его состояния
    /// </summary>
    public interface IContractStateData : IObjectId
    {
        /// <summary>
        /// Получает или устанавливает дату применения догвора
        /// </summary>
        DateTime? Appliedat { get; set; }

        /// <summary>
        /// Получает или устанавливает дату утверждения договора
        /// </summary>
        DateTime? Approvedat { get; set; }

        /// <summary>
        /// Получает или устанавливает дату начала договора
        /// </summary>
        DateTime? Startat { get; set; }

        /// <summary>
        /// Получает или устанавливает дату окончания договора
        /// </summary>
        DateTime? Endsat { get; set; }

        /// <summary>
        /// Получает или устанавливает дату разрыва договора
        /// </summary>
        DateTime? Brokeat { get; set; }

        /// <summary>
        /// Получает или устанавливает дату снятия договора с учёта
        /// </summary>
        DateTime? Outofcontrolat { get; set; }

        /// <summary>
        /// Получает или устанавливает дату реального окончания договора
        /// </summary>
        DateTime? Reallyfinishedat { get; set; }

        /// <summary>
        /// Получает или устанавливает номер договора, присвоенный организацией
        /// </summary>
        string Internalnum { get; set; }

        decimal? Agreementreferencecount { get; }

        decimal? Generalreferencecount { get;  }

        long? Maincontractid { get;  }
    }
}