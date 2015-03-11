using System.Text;
using System.Diagnostics.Contracts;
using CommonBase;

namespace McReports.Common
{
    public class DefaultTemplateProvider : ITemplateProvider
    {

        public string TemplateFolder { get; private set; }
        public string BaseDirectory { get; private set; }
        public object PropertySource { get; private set; }
    
        /// <summary>
        /// Создаёт экземпляр провайдера имени шаблона отчёта
        /// </summary>
        /// <param name="baseDirectory">Базовый каталог приложения</param>
        /// <param name="templateFolder">Каталог шаблонов</param>
        /// <param name="propertySource">Источник имён шаблонов</param>
        public DefaultTemplateProvider(string baseDirectory, string templateFolder, object propertySource)
        {
            Contract.Requires(!string.IsNullOrWhiteSpace(baseDirectory));
            Contract.Requires(!string.IsNullOrWhiteSpace(templateFolder));
            Contract.Requires(propertySource != null);
            BaseDirectory = baseDirectory;
            TemplateFolder = templateFolder;
            PropertySource = propertySource;
        }

        #region ITemplateProvider Members

        
        public string GetTemplate(string propertyName)
        {
            var sb = new StringBuilder();
            sb.Append(BaseDirectory);
            sb.Append(@"\");
            sb.Append(TemplateFolder);
            sb.Append(PropertySource.GetPropertyValue<string>(propertyName));
           
            return sb.ToString();
        }

        #endregion
    }
    
}