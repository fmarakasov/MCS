using System.Linq;
using McUIBase.Factories;
namespace MContracts.Statistics
{
    /// <summary>
    /// Вспомогательный класс для обновления статистики по договорам.
    /// Используется отдельный контекст, который гарантирует пересчёт по актуальным данным
    /// </summary>
    public class StatisticsUpdater
    {
        public static void UpdateStatistics(long? contractId)
        {
            using (var repository = RepositoryFactory.CreateContractRepository())
            {
                var contract = repository.Contracts.Single(x => x.Id == contractId);
                contract.UpdateFundsStatistics();
                repository.SubmitChanges();
            }
        }
    }
}
