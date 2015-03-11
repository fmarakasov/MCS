namespace CommonBase.Exceptions
{
    /// <summary>
    /// Описание типа исключения
    /// </summary>
    public enum IconDescription
    {
        Information,
        Warning,
        Critical,
        Question
    }

    /// <summary>
    /// Описание исключения
    /// </summary>
    public struct ExceptionDescription
    {
        /// <summary>
        /// Получает или устанавливает строку с содержимиым исключения
        /// </summary>
        public string Content { get; set; } 
        /// <summary>
        /// Получает или устанавливает степень критичности исключения
        /// </summary>
        public IconDescription Icon { get; set; }
        /// <summary>
        /// Получает или устанавливает строку с инструкциями действия для пользователя
        /// </summary>
        public string Instruction { get; set; }
        /// <summary>
        /// Получает или устанавливает строку для заголовка исключения
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// Получает или устанавливает признак необходимости доступа к журналам
        /// </summary>
        public bool ShowFooter { get; set; }
           
    }
}