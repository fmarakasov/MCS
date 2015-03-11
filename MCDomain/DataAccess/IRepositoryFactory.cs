namespace MCDomain.DataAccess
{
    /// <summary>
    /// Определяет типы, реализующие фабрику репозиториев
    /// </summary>
    public interface IRepositoryFactory
    {
        /// <summary>
        /// Создаёт новый экземпляр репозитория
        /// </summary>
        /// <returns>Репозиторий</returns>
        IContractRepository CreateRepository();
    }
}
