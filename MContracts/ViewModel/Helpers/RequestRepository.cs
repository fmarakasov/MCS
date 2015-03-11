using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MContracts.ViewModel.Helpers
{
    /// <summary>
    /// Определяет строковые константы сообщений медиатора модели представления
    /// </summary>
    static class RequestRepository
    {
        /// <summary>
        /// Запрос на обновление состояния при удалении акта
        /// </summary>
        public const string REQUEST_ACT_DELETED = "RequestActDeleted";

        /// <summary>
        /// Запрос на открытие договора
        /// </summary>
        public const string REQUEST_OPEN_CONTRACTDOC = "RequestOpenContractDoc";
        /// <summary>
        /// Запрос на создание нового договора
        /// </summary>
        public const string REQUEST_NEW_CONTRACTDOC = "RequestNewContractDoc";

        public const string REQUEST_SAVE_WORKSPACE = "SaveWorkspace";

        /// <summary>
        /// команды обновления результатов для синхронизации ScheduleViewModel и StageResultsViewModel
        /// </summary>
        public const string REFRESH_RESULT_SCHEDULE = "RefreshSchedule";
        public const string REFRESH_RESULT_SCHEDULE_STAGE = "RefreshScheduleStage";
        public const string REFRESH_RESULT_SCHEDULE_GENERAL = "RefreshScheduleGeneral";
        public const string REFRESH_RESULT = "RefreshResult";

        /// <summary>
        /// команды обновления актов для синхронизации ScheduleViewModel и ActsViewModel
        /// </summary>
        public const string REFRESH_ACTS_SCHEDULE = "RefreshActsSchedule";
        //public const string REFRESH_ACTS = "RefreshActs";

        /// <summary>
        /// Команда обновления глобальных свойств
        /// </summary>
        public const string REQUEST_GLOBAL_PROPERTIES_CHANGED = "RequestGlobalPropertiesChanged";

        public const string REQUEST_ERROR_CHANGED = "RequestErrorChanged";

        public const string REQUEST_SELECTEDCONTRACT_CHANGED = "RequestSelectedContractChanged";

        public const string REFRESH_SUBGENERALS = "RefreshSubGenerals";

        public const string REFRESH_GENERALS = "RefreshGeneralSubs";

        public const string CATALOG_CHANGED = "CatalogChanged";

        public const string DISPOSALS_CHANGED = "DisposalsChanged";

        public const string ORDERS_CHANGED = "OrdersChanged";

        public const string REQUEST_ACTIVE_WORKSPACE_CHANGED = "REQUEST_ACTIVE_WORKSPACE_CHANGED";

        public const string REFRESH_REFRESH_APPROVAL = "REFRESH_REFRESH_APPROVAL";
        
        /// <summary>
        /// Договор был обновлён в другом представлении 
        /// </summary>
        public const string REQUEST_CONTRACT_UPDATED = "REQUEST_CONTRACT_UPDATED";
   

    }
}
