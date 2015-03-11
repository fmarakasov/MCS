namespace McReports.Common
{
    /// <summary>
    /// Определяет провайдер полного пути к шаблону отчёта
    /// </summary>
    public interface ITemplateProvider
    {
        /// <summary>
        /// Определяет полный путь к заданному шаблону
        /// </summary>
        /// <param name="propertyName">Имя свойства с именем файла шаблона</param>
        /// <returns>Полный путь к файлу шаблона</returns>
        string GetTemplate(string propertyName);
    }
}