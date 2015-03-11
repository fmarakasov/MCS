using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MCDomain.Model;
using MContracts.Classes;
using CommonBase;
using CommonBase;

namespace MContracts.DTO
{

    public class CompareHandingWorkDTO : IComparer<HandingWorkDTO>
    {
        // Because the class implements IComparer, it must define a 
        // Compare method. This Compare method compares integers.
        public int Compare(HandingWorkDTO p1, HandingWorkDTO p2)
        {
            int iResult = 0;
            if (p1.ActSignDate.HasValue && p2.ActSignDate.HasValue)
                iResult = DateTime.Compare(p1.ActSignDate.Value, p2.ActSignDate.Value);

            if (iResult == 0) iResult = string.Compare(p1.ActName, p2.ActName);
            return iResult;
        }
    }
    /// <summary>
    /// Представление для отчета "Справка о сдаче работ НИОКР"
    /// </summary>
    public class HandingWorkDTO
    {
        /// <summary>
        /// Год, за который строится отчет
        /// </summary>
        public int Year;

        public Quarters Quarter;

        public Stage Stage;

        public Contractortype ContractorType;

        public Contracttype ContractType;

        /// <summary>
        /// Квартал, за который строится отчет
        /// </summary>
        public int QuarterInt;

        /// <summary>
        /// Номер этапа/подэтапа
        /// </summary>
        public string StageNum;

       /// <summary>
        /// Стоимость этапа на период
        /// </summary>
        public decimal FullPrice;

        /// <summary>
        /// Соисполнители
        /// </summary>
        public decimal AccompilePrice;

        /// <summary>
        /// Собственные силы
        /// </summary>
        public decimal Price;

        /// <summary>
        /// Номер договора
        /// </summary>
        public string ContractNum;

        /// <summary>
        /// Тип конрагента
        /// </summary>
        public string ContractorTypeName;

        /// <summary>
        /// Номер и дата подписания акта
        /// </summary>
        public string ActName;

        public DateTime? ActSignDate;


    }
}

