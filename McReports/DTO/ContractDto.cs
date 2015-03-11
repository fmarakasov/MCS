using System.Collections.Generic;
using MCDomain.Model;
using McReports.ViewModel;
namespace McReports.DTO
{
    /// <summary>
    /// Представление для отчета "Справка по договорам на 20ХХ год"
    /// </summary>
    public class ContractDto //: EntityDto<MCDomain.Model.Contractdoc>
    {

        public List<Employee> Emploees;
        /// <summary>
        /// Год, за который строится отчет
        /// </summary>
        public int Year;

        public int Nextyear
        {
            get { return Year + 1; }
        }

        public Contractortype ContractorType;

        public Contracttype ContractType;

        /// <summary>
        /// Заместитель директора
        /// </summary>
        public string DeputyDirector;

       // public IEnumerable<string> test;

        /// <summary>
        /// Руководитель
        /// </summary>
        public string Manager;

        /// <summary>
        /// Распоряжение "№ Номер от Дата"
        /// </summary>
        public string DisposalFullName;
        /// <summary>
        /// Номер договора
        /// </summary>
        public string ContractNum;

        public string FullSortNumber;
        /// <summary>
        /// Тема договора
        /// </summary>
        public string Subject;

        /// <summary>
        /// Заказчик
        /// </summary>
        public string FunctionalCustomer;

        /// <summary>
        /// Цена за текущий год
        /// </summary>
        public decimal Price;

        /// <summary>
        /// Цена на 20ХХ-1 год
        /// </summary>
        public decimal PrevPrice;

        /// <summary>
        /// Цена за запрашиваемый год по подписанным договорам
        /// </summary>
        public decimal SignedContractsPrice;

        /// <summary>
        /// Цена за предыдущий по отношению к запрашиваемому год по подписанным договорам
        /// </summary>
        public decimal PrevSignedContractsPrice;

        /// <summary>
        /// Цена по соисполнителям
        /// </summary>
        public decimal CoworkersPrice;

        /// <summary>
        /// Цена по соисполнителям за предыдущий по отношению к запрашиваемому период
        /// </summary>
        public decimal PrevCoworkersPrice;

        /// <summary>
        /// Лист согласования
        /// </summary>
        public string SighList;

        /// <summary>
        /// Состояние договора
        /// </summary>
        public string ContractCondition;
        

        /// <summary>
        /// Тип конрагента
        /// </summary>
        public int ContractorTypeID;

        /// <summary>
        /// Тип контракта
        /// </summary>
        public int ContractTypeID;

        /// <summary>
        /// ответственный от договорного отдела
        /// </summary>
        public string CuratorEmployee;

        
    }
}
