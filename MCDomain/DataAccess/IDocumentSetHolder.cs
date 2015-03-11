using System.Collections.Generic;
using MCDomain.Model;

namespace MCDomain.DataAccess
{
    /// <summary>
    /// Определяет типы, которые поддерживают акты с описью документов
    /// </summary>
    public interface IDocumentSetHolder
    {
        /// <summary>
        /// Получает признак, что для заданного типа уже создана опись документов
        /// </summary>
        bool DocumentSetCreated { get; }

        /// <summary>
        /// Создаёт новую опись документов
        /// </summary>
        void CreateDocumentSet();
        /// <summary>
        /// Получает опись документов для типа
        /// </summary>
        /// <returns>Коллекция документов</returns>
        IEnumerable<IDocumentSetEntry> GetDocumentSet();
        
        /// <summary>
        /// Удаляет опись документов, если она была создана
        /// </summary>
        void DeleteDocumentSet();
        
        /// <summary>
        /// Привязывает опись документов к заданному акту 
        /// </summary>
        /// <param name="act">Акт к которому устанавливается привязка описи</param>
        void BindToAct(Transferact act);

        /// <summary>
        /// Получает тип акта передачи для заданного типа.
        /// </summary>
        Transferacttype Transferacttype { get; }

        /// <summary>
        /// Получает акт, к кторорому привязаны документы
        /// </summary>
        Transferact Transferact { get; }
    }
}
