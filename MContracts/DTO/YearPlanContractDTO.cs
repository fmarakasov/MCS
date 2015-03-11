using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MCDomain.Model;

namespace MContracts.DTO
{
    /// <summary>
    /// Представление для отчета "Текущий тематический план на 20ХХ год"
    /// </summary>
    public class YearPlanContractDto 
    {

        /// <summary>
        /// Год, за который строится отчет
        /// </summary>
        public int Year;


        public Contracttype ContractType;

        public Contractortype ContractorType;

        /// <summary>
        /// Договор
        /// </summary>
        public Contractdoc Contract;
        
        public Stage Stage;

        /// <summary>
        /// руководители по этапу
        /// </summary>
        public string StageDirectors;
        /// <summary>
        /// Ответственныe исполнители по этапу
        /// </summary>
        public string StageResponsiblePeople;
        /// <summary>
        /// Куратор от договорного отдела для этапа
        /// </summary>
        public string StageCurator;
        /// <summary>
        /// Зам.дир
        /// </summary>
        public string StageDirector;


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
        /// переходящая с предыдущего года
        /// </summary>
        public decimal PrevYearPrice;
        /// <summary>
        /// переходящая с предыдущего года по соисполнителям
        /// </summary>
        public decimal PrevYearCoworkersPrice;
        /// <summary>
        /// Стоимость этапа на 20хх год
        /// </summary>
        public decimal StagePrice;

        /// <summary>
        /// Цена за январь
        /// </summary>
        public decimal JanuaryPrice;

        /// <summary>
        /// Цена за февраль
        /// </summary>
        public decimal FebuaryPrice;

        /// <summary>
        /// Цена за март
        /// </summary>
        public decimal MarchPrice;

        /// <summary>
        /// Соисполнители в первом квартале
        /// </summary>
        public decimal FirstQuarterAccomplice;

        /// <summary>
        /// Цена за апрель
        /// </summary>
        public decimal AprilPrice;

        /// <summary>
        /// Цена за май
        /// </summary>
        public decimal MayPrice;

        /// <summary>
        /// Цена за июнь
        /// </summary>
        public decimal JunePrice;

        /// <summary>
        /// Соисполнители во втором квартале
        /// </summary>
        public decimal SecondQuarterAccomplice;

        /// <summary>
        /// Цена за июль
        /// </summary>
        public decimal JulyPrice;

        /// <summary>
        /// Цена за август
        /// </summary>
        public decimal AugustPrice;

        /// <summary>
        /// Цена за сентябрь
        /// </summary>
        public decimal SeptemberPrice;

        /// <summary>
        /// Соисполнители в третьем квартале
        /// </summary>
        public decimal ThirdQuarterAccomplice;

        /// <summary>
        /// Цена за октябрь
        /// </summary>
        public decimal OctoberPrice;

        /// <summary>
        /// Цена за ноябрь
        /// </summary>
        public decimal NovemberPrice;

        /// <summary>
        /// Цена за декабрь
        /// </summary>
        public decimal DecemberPrice;

        /// <summary>
        /// Соисполнители в четвертом квартале
        /// </summary>
        public decimal FouthQuarterAccomplice;

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
