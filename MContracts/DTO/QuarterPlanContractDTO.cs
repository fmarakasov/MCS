using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MCDomain.Model;
using MContracts.Classes;

namespace MContracts.DTO
{
    /// <summary>
    /// Представление для отчета "Текущий тематический план на Х квартал"
    /// </summary>
    public class QuarterPlanContractDto
    {
        /// <summary>
        /// Год, за который строится отчет
        /// </summary>
        public int Year;

        /// <summary>
        /// Квартал, за который строится отчет
        /// </summary>
        public int Quarter;

        public Contractortype ContractorType;

        public Contracttype ContractType;

        public Contractdoc Contract;

        public Stage Stage;
        /// <summary>
        /// Заместитель директора и руководитель напрвления
        /// </summary>
        public string Directors;

        /// <summary>
        /// Ответственный исполнители
        /// </summary>
        public string ResponsiblePeople;

        /// <summary>
        /// Состояние договора (подписа/ не подписан)
        /// </summary>
        public string ContractState;

        /// <summary>
        /// Номер этапа/подэтапа
        /// </summary>
        public string StageNum;

        /// <summary>
        /// Название этапа
        /// </summary>
        public string StageName;

        /// <summary>
        /// Стоимость этапа на 20хх год
        /// </summary>
        public decimal StagePrice;

    
        /// <summary>
        /// Переходящая с прошлых кварталов сумма (всего)
        /// </summary>
        public decimal TransitionPrice;

        /// <summary>
        /// Переходящая с прошлых кварталов сумма по соисполнителям
        /// </summary>
        public decimal TransitionAccomplicePrice;

        /// <summary>
        /// Цена по первому месяцу квартала
        /// </summary>
        public decimal Month1Price;
        
        /// <summary>
        /// Цена по второму месяцу квартала
        /// </summary>
        public decimal Month2Price;
        
        /// <summary>
        /// Цена по третьему месяцу квартала
        /// </summary>
        public decimal Month3Price;

        /// <summary>
        /// Соисполнители
        /// </summary>
        public decimal AccompilePrice;

        /// <summary>
        /// Статус акта (для окрашивания цен в отчете 4)
        /// </summary>
        public bool ActStatus;

        /// <summary>
        /// Номер и тема договора
        /// </summary>
        public string ContractNumSubject;

        /// <summary>
        /// Тип конрагента
        /// </summary>
        public int ContractorTypeID;

        /// <summary>
        /// Тип контракта
        /// </summary>
        public int ContractTypeID;
    }
}

