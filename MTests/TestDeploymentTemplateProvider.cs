using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using MContracts.Classes;
using MContracts.ViewModel.Reports;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MTests
{
    /// <summary>
    /// Используется для формирования строки имени файла в тестовом окружении
    /// </summary>
    class TestDeploymentTemplateProvider : ITemplateProvider
    {
        /// <summary>
        /// Получает имя файла шаблона 
        /// </summary>
        public string Template { get; private set; }
        /// <summary>
        /// Получает контекст тестирования
        /// </summary>
        public TestContext Context { get; private set; }

        /// <summary>
        /// Создаёт экземпляр TestDeploymentTemplateProvider
        /// </summary>
        /// <param name="ctx">Контекст тестирования</param>
        /// <param name="template">Имя файла шаблона отчёта</param>
        public TestDeploymentTemplateProvider(TestContext ctx, string template)
        {
            Debug.Assert(ctx != null, "ctx != null");
            Debug.Assert(!string.IsNullOrWhiteSpace(template));
            Template = template;
            Context = ctx;
        }

        protected TestDeploymentTemplateProvider()
        {
        }

        public string GetTemplate(string reportFile)
        {
            var result =  string.Format(@"{0}\{1}", Context.DeploymentDirectory, Template);   
            Debug.WriteLine(result);
            return result;
        }
    }
}
