using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;

namespace McReports.DTO
{
    public static class EntityDtoExtensions
    {
        /// <summary>
        /// Преобразует коллекцию DTO в коллекцию объектов предметной области
        /// </summary>
        /// <typeparam name="T">Тип объекта предметной области</typeparam>
        /// <param name="source">Коллекция DTO</param>
        /// <returns>Коллекция объектов предметной области</returns>
        public static IEnumerable<T> AsEntities<T>(this IEnumerable<EntityDto<T>> source) where T:class, new()
        {
            Contract.Requires(source != null);
            Contract.Ensures(Contract.Result<IEnumerable<T>>() != null);
            return source.Select(x => x.AsEntity);
        }
    }
}