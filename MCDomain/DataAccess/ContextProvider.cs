using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Devart.Data.Linq;
using MCDomain.Model;

namespace MCDomain.DataAccess
{
    /// <summary>
    /// Определяет типы, обеспечивающие доступ к контексту данных
    /// </summary>
    public interface IContextProvider<out T> where T : DataContext
    {
        /// <summary>
        /// Получает контекст данных
        /// </summary>
        T Context { get; }

        event EventHandler<ContextEventArgs<McDataContext>> ContextCreated;
    }

    /// <summary>
    /// Определяет методы расширения для извлечения контектса данных из репозитария
    /// </summary>
    public static class RepositoryExtensions
    {
        /// <summary>
        /// Проверяет, поддерживает ли репозитарий контекст данных 
        /// </summary>
        /// <typeparam name="T">Тип</typeparam>
        /// <param name="source">Репозитария</param>
        /// <returns>Истина, если контекст данных доступен</returns>
        public static bool IsLinq<T>(this T source) where T : IContractRepository
        {
            return (source is IContextProvider<McDataContext>);
        }
        /// <summary>
        /// Получает контекст данных из репозитария. Если тип не поддерживает IContextProvider, возвращается null
        /// </summary>
        /// <typeparam name="T">Тип</typeparam>
        /// <param name="source">Объект репозитария</param>
        /// <returns>Конеткст</returns>
        public static McDataContext TryGetContext<T>(this T source) where T:IContractRepository
        {
            var provider = TryGetContextProvider(source);
            if (provider == null) return null;
            return provider.Context;
        }

        public static IContextProvider<McDataContext> TryGetContextProvider<T>(this T source) where T : IContractRepository
        {
            var provider = (source as IContextProvider<McDataContext>);
            return provider;
        }
    }
}
