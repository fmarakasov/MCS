namespace MCDomain.Model
{
    /// <summary>
    /// Определяет типы, являющиеся элементом описи
    /// </summary>
    public interface IDocumentSetEntry
    {
        /// <summary>
        /// Получает идентификатор элемента
        /// </summary>
        long Id { get; }
        /// <summary>
        /// Получает или устанавливает число страниц документа в описи
        /// </summary>
        long? Pagescount { get; set; }
        /// <summary>
        /// Получает или устанавливает тип документа в описи
        /// </summary>
        Document Document { get; set; }
        /// <summary>
        /// Получает акт, к которому привязан элемент описи. 
        /// Изменение значения свойства производится при вызове IDocumentHolder.BindToAct
        /// </summary>
        Transferact Transferact { get; }
    }
}