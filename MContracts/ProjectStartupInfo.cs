using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MContracts
{
    public static class ProjectStartupInfo
    {
        /// <summary>
        /// Указывает на необходимость создания нового договора при старте программы
        /// </summary>
        public static bool CreateNewContract;
        
        /// <summary>
        /// Указывает на отказ от загрузки данных реестра договоров при старте программы
        /// </summary>
        public static bool FastLoad;
        /// <summary>
        /// указывает на конкретный номер з
        /// </summary>
        public static string Contracts = String.Empty;
        
        /// <summary>
        /// Указывает на необходимости обновления статистики по всем договорам перед запуском 
        /// </summary>
        public static bool UpdateStatistics;
    }
}
