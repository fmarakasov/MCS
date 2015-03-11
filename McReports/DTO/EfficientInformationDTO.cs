using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MCDomain.Model;

namespace McReports.DTO
{
    public class EfficientInformationDTO
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

        /// <summary>
        /// № Договора
        /// </summary>
        public string ContractNum;

        /// <summary>
        /// Заказчик
        /// </summary>
        public string Customer;

        /// <summary>
        /// Цена всего
        /// </summary>
        public decimal FullPrice;


        /// <summary>
        /// Сумма НДС
        /// </summary>
        public decimal NDS;

        /// <summary>
        /// Сумма собственные силы
        /// </summary>
        public decimal Price;
        
        /// <summary>
        /// Сумма соисполнители
        /// </summary>
        public decimal AccompilePrice;

    
        /// <summary>
        /// Тип исполнителя
        /// </summary>
        public string ContractorTypeName;

        /// <summary>
        /// Тип контракта
        /// </summary>
        public string ContractTypeName;
    }
}
