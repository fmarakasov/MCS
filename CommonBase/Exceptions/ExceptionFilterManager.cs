namespace CommonBase.Exceptions
{
    /// <summary>
    /// Класс управления фильтрами исключений и объектами отображения исключений
    /// </summary>
    public class ExceptionFilterManager
    {
        /// <summary>
        /// Получает экземпляр ExceptionFilterManager
        /// </summary>
        public static readonly ExceptionFilterManager Instance = new ExceptionFilterManager();
        /// <summary>
        /// Получает коллекцию фильтров исключений
        /// </summary>
        public ExceptionDescriptionResolvers DescriptionResolvers { get; private set; }
        /// <summary>
        /// Получает коллекцию объектов-отображений исключений
        /// </summary>
        public ExceptionMappers Mappers { get; private set; }
        /// <summary>
        /// Создаёт экземпляр ExceptionFilterManager
        /// </summary>
        public ExceptionFilterManager()
        {
            DescriptionResolvers = new ExceptionDescriptionResolvers();
            Mappers = new ExceptionMappers();
        }

        /// <summary>
        /// Получает или устанавливает троку версии сборки
        /// </summary>
        public string AssemblyVersion { get; set; }
        /// <summary>
        /// Получает или устанавливает строку имени приложения
        /// </summary>
        public string AssemblyTitle { get; set; }
        /// <summary>
        /// Получает или устанавливает строку версии файла
        /// </summary>
        public string FileVersion{ get; set; }
    }
}