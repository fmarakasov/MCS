using System;
using System.Text;
using MContracts.Classes;

namespace MContracts.ViewModel.Reports
{
    public class DefaultTemplateProvider : ITemplateProvider
    {
        public static readonly DefaultTemplateProvider Instance = new DefaultTemplateProvider();

        #region ITemplateProvider Members

        public string GetTemplate(string propertyName)
        {
            var sb = new StringBuilder();
            sb.Append(AppDomain.CurrentDomain.BaseDirectory);
            sb.Append(@"\");
            sb.Append(Properties.Settings.Default.ReportTemplateFolder);
            sb.Append(Properties.Settings.Default.GetPropertyValue<string>(propertyName));
            var fullPath = sb.ToString();

            if (!System.IO.File.Exists(fullPath))
                throw new ApplicationException(string.Format("Шаблон документа не найден по заданному пути: \"{0}\".", fullPath));
           
            return fullPath;
        }

        #endregion
    }
    
}