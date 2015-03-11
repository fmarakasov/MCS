using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Reflection;
using System.Text;

namespace MContracts.Classes
{
    /// <summary>
    /// Класс для получения данных о сборке
    /// </summary>
    public static class VersionHelper
    {
        /// <summary>
        /// Получает заголовок сборки
        /// </summary>
        /// <param name="assembly">Сборка</param>
        /// <returns>Строка заголовка</returns>
        public static string AssemblyTitle(this Assembly assembly)
        {
            if (assembly == null) throw new ArgumentNullException("assembly");

            // Get all Title attributes on this assembly
            object[] attributes = assembly.GetCustomAttributes(typeof(AssemblyTitleAttribute), false);
            // If there is at least one Title attribute
            if (attributes.Length > 0)
            {
                // Select the first one
                var titleAttribute = (AssemblyTitleAttribute)attributes[0];
                // If it is not an empty string, return it
                if (!string.IsNullOrEmpty(titleAttribute.Title))
                    return titleAttribute.Title;
            }
            // If there was no Title attribute, or if the Title attribute was the empty string, return the .exe name
            try
            {
                return System.IO.Path.GetFileNameWithoutExtension(assembly.CodeBase);
            }
            catch (NotSupportedException)
            {
                return "Нет данных";
            }

        }
        /// <summary>
        /// Получает строку с версией сборки
        /// </summary>
        /// <param name="assembly">Сборка</param>
        /// <returns>Строка версии сборки</returns>
        public static string AssemblyVersion(this Assembly assembly)
        {
            return assembly.GetName().Version.ToString();
        }

        /// <summary>
        /// Получает строку версии файла сборки
        /// </summary>
        /// <param name="assembly">Сборка</param>
        /// <returns>Строка с версией файла сборки</returns>
        public static string AssemblyFileVersion(this Assembly assembly)
        {
            // Get all Description attributes on this assembly
            object[] attributes = assembly.GetCustomAttributes(typeof(System.Reflection.AssemblyFileVersionAttribute), false);
            // If there aren't any Description attributes, return an empty string
            if (attributes.Length == 0)
                return "";
            // If there is a Description attribute, return its value
            return ((AssemblyFileVersionAttribute)attributes[0]).Version;

        }

        /// <summary>
        /// Получает строку с описанием сборки
        /// </summary>
        /// <param name="assembly">Сборка</param>
        /// <returns>Строка с описанием сборки</returns>
        public static string AssemblyDescription(this Assembly assembly)
        {
            // Get all Description attributes on this assembly
            object[] attributes = assembly.GetCustomAttributes(typeof(AssemblyDescriptionAttribute), false);
            // If there aren't any Description attributes, return an empty string
            if (attributes.Length == 0)
                return "";
            // If there is a Description attribute, return its value
            return ((AssemblyDescriptionAttribute)attributes[0]).Description;

        }
        /// <summary>
        /// Получает строку с именем продукта
        /// </summary>
        /// <param name="assembly">Сборка</param>
        /// <returns>Строка с именем продукта</returns>
        public static string AssemblyProduct(this Assembly assembly)
        {
            // Get all Product attributes on this assembly
            object[] attributes = assembly.GetCustomAttributes(typeof(AssemblyProductAttribute), false);
            // If there aren't any Product attributes, return an empty string
            if (attributes.Length == 0)
                return "";
            // If there is a Product attribute, return its value
            return ((AssemblyProductAttribute)attributes[0]).Product;

        }

        /// <summary>
        /// Получает строку авторского права на сборку
        /// </summary>
        /// <param name="assembly">Сборка</param>
        /// <returns>Строка авторского права</returns>
        public static string AssemblyCopyright(this Assembly assembly)
        {


            // Get all Copyright attributes on this assembly
            object[] attributes = assembly.GetCustomAttributes(typeof(AssemblyCopyrightAttribute), false);
            // If there aren't any Copyright attributes, return an empty string
            if (attributes.Length == 0)
                return "";
            // If there is a Copyright attribute, return its value
            return ((AssemblyCopyrightAttribute)attributes[0]).Copyright;

        }
        /// <summary>
        /// Получает строку с именем компании
        /// </summary>
        /// <param name="assembly">Сборка</param>
        /// <returns>Строка с именем компании</returns>
        public static string AssemblyCompany(this Assembly assembly)
        {
            // Get all Company attributes on this assembly
            object[] attributes = assembly.GetCustomAttributes(typeof(AssemblyCompanyAttribute), false);
            // If there aren't any Company attributes, return an empty string
            if (attributes.Length == 0)
                return "";
            // If there is a Company attribute, return its value
            return ((AssemblyCompanyAttribute)attributes[0]).Company;

        }

    }
}
