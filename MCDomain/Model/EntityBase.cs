using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MCDomain.Common;
using CommonBase;
using UOW;

namespace MCDomain.Model
{
    /// <summary>
    /// Базовый тип сущностей
    /// </summary>
    public class EntityBase
    {
        /// <summary>
        /// Зарезервированный идентификатор для значений по умолчанию. Используется,
        /// если необходимо указать определённое значение по умолчанию.
        /// Например, для указания, что НДС равно 18%
        /// </summary>
        public const long ReservedDefault = 1;
        /// <summary>
        /// Зарезервированный идентификатор объекта для неопределённых значений
        /// </summary>
        public const long ReservedUndifinedOid = -1;
        /// <summary>
        /// идентификатор для ОАО Газпром промгаз
        /// </summary>
        public const long ReservedSelfOid = 0;
        /// <summary>
        /// Зарезервированный нижний индекс идентификаторов для объектов, создаваемых разработчиками
        /// </summary>
        public const long LowBoundReserved = 0;

        /// <summary>
        /// Зарезервированный верхний индекс идентификаторов для объектов, создаваемых разработчиками
        /// </summary>
        
        public const long HighBoundReserved = 9999;  
        
        bool CheckId(long lowBound, long highBound)
        {
            var objectId = (this as IObjectId);
            if (objectId != null)
                return objectId.Id.Between(lowBound, highBound);
            return false;
        }
        /// <summary>
        /// Определяет, имеет ли сущность признак зарезервированной для неопределённых значений
        /// </summary>
        public bool ReservedAsUndefined
        {
            get { return CheckId(ReservedUndifinedOid, ReservedUndifinedOid); }
        }

        /// <summary>
        /// Определяет, имеет ли сущность признак зарезервированной для разработчиков
        /// </summary>
        public bool Reserved
        {
            get { return CheckId(LowBoundReserved, HighBoundReserved); }
        }

        /// <summary>
        /// Получает признак того, что идентификатор не задан или имеет зарезервированное неопределённое значение
        /// </summary>
        /// <param name="entityOid">Идентификатор</param>
        /// <returns>Признак зарезервированного или пустого значения</returns>
        public static bool IsReservedOrEmpty(double? entityOid)
        {
            return (!entityOid.HasValue || entityOid == ReservedUndifinedOid);
        }
        
        /// <summary>
        /// Получает признак того, что идентификатор может принадлежать реальной сущности
        /// </summary>
        /// <param name="entityOid">Идентификатор</param>
        /// <returns>Признак реального значения</returns>
        public static bool IsReal(long? entityOid)
        {
            return !IsReservedOrEmpty(entityOid);
        }
        
    }
}
