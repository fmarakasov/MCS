using System;
using System.Collections.Generic;
using System.Linq;
using MCDomain.Model;
using System.Diagnostics.Contracts;
using System.Data.Linq;
using UOW;

namespace MCDomain.DataAccess
{
    
    /// <summary>
    /// Определяет типы, которые реализуют доступ к системе хранения
    /// </summary>
    [ContractClass(typeof(ContractrepositoryContract))]
    public interface IContractRepository : IDisposable
    {
        IUnitOfWork UnitOfWork { get; }
        IList<Contractdoc> Contracts { get; }
        /// <summary>
        /// Создаёт экземпляр нового генерального договора с параметрами по умолчанию
        /// </summary>
        /// <returns></returns>
        Contractdoc NewContractdoc();

        /// <summary>
        /// Создаёт экземпляр этапа
        /// </summary>
        /// <returns>Объект этапа</returns>
        Stage NewStage();

        Act NewAct(Contractdoc contractdoc);

        /// <summary>
        /// Получает остаток по авансу для заданного акта. 
        /// Метод вычисляет остатки по всем платёжкам. При расчёте суммы по авансам указанного акта не принимается
        /// </summary>
        /// <param name="actId">Идентификатор акта</param>
        /// <param name="contractdoc">Договор по которому проходят платёжные документы</param>
        /// <returns>Остаток по авансам</returns>
        decimal GetPrepaymentRestsForAct(long actId, long contractdoc);

        /// <summary>
        /// Получает экземпляр договора по идентификатору
        /// </summary>
        /// <param name="contractId"></param>
        /// <returns></returns>
        Contractdoc GetContractdoc(long contractId);


        IList<Contractstate> States { get; }
        IList<Ndsalgorithm> Ndsalgorithms { get; }
        IList<Nds> Nds { get; }
        IList<Currency> Currencies { get; }
        IList<Contracttype> Contracttypes { get; }
        IList<Currencymeasure> Currencymeasures { get; }
        IList<Contractor> Contractors { get; }
        IList<Functionalcustomer> Functionalcustomers { get; }
        IList<Disposal> Disposals { get; }
        IList<Enterpriseauthority> Enterpriseauthorities { get; }
        IList<Troublesregistry> TroublesRegistry { get; }
        IList<Trouble> Troubles { get; }
        IList<Worktype> Worktypes { get; }
        IList<Degree> Degrees { get; }
        IList<Ntpview> Ntpviews { get; }
        IList<Ntpsubview> Ntpsubviews { get; }
        IList<Economefficiencytype> Economefficiencytypes { get; }
        IList<Economefficiencyparameter> Economefficiencyparameters { get; }
        IList<Efficienceparametertype> Efficienceparametertypes { get; }
        IList<Contractortype> Contractortypes { get; }
        IList<Position> Positions { get; }
        IList<Property> Properties { get; }
        IList<Region> Regions { get; }
        IList<Role> Roles { get; }
        IList<Authority> Authorities { get; }
        IList<Acttype> Acttypes { get; }
        IList<Contractorposition> Contractorpositions { get; }
        IList<Prepaymentdocumenttype> Prepaymentdocumenttypes { get; }
        IList<Person> Persons { get; }
        IList<Employee> Employees { get; }
        IList<Functionalcustomertype> Functionalcustomertypes { get; }
        IList<Document> Documents { get; }
        IList<Transferacttype> Transferacttypes { get; }
        IList<Transferacttypedocument> Transferacttypedocuments { get; }
        IList<Funccustomerperson> Funccustomerpersons { get; }
        IList<Contractorpropertiy> Contractorpropertiies { get; }
        IList<Sightfuncpersonscheme> Sightfuncpersonschemes { get; }
        IList<Location> Locations { get; }
        IList<Missivetype> MissiveTypes { get; }
        IList<Approvalgoal> ApprovalGoals { get; }
        IList<Approvalstate> ApprovalStates { get; }
        IList<Department> Departments { get; }
        
        IList<Stage> AllStages { get; }
        IList<Importingscheme> Importingschemes { get; }
        IList<Responsiblefororder> Responsiblefororders { get; }
        IList<Responsibleassignmentorder> Responsibleassignmentorders { get; }
        IList<Filterstate> Filterstates { get;  }
        IList<Yearreportcolor> Yearreportcolors { get; }
        IList<Transferact> Transferacts { get; } 

        /// <summary>
        /// Сохраняет изменения в модели
        /// </summary>
        void SubmitChanges();
        /// <summary>
        /// Отменяет изменения в модели
        /// </summary>
        void RejectChanges();
        /// <summary>
        /// обновляет из модели сведения об объекте
        /// </summary>
        /// <param name="mode">как обновлять</param>
        /// <param name="entity">что обновлять</param>
        void Refresh(RefreshMode mode, object entity);

        void Refresh(RefreshMode mode, IEnumerable<object> entities);

        /// <summary>
        /// Получает признак того, что модель была модифицирована с момента последнего сохранения изменений
        /// </summary>
        bool IsModified { get; }

        /// <summary>
        /// Добавляет в репозитарий новый договор
        /// </summary>
        /// <param name="contractdoc">Объект договора</param>
        void AddContract(Contractdoc contractdoc);

        void DeleteTroublecontract(Contracttrouble entity);

        void InsertTroublecontract(Contracttrouble entity);

        /// <summary>
        /// Получает этапы КП
        /// </summary>
        /// <param name="scheduleId">ID КП</param>
        /// <returns></returns>
        IEnumerable<Stage> GetStages(double scheduleId);

        void DeleteShedule(Schedule schedule);

        void DeleteStage(Stage stage);

        void InsertStage(Stage stage);

        void DeleteAllStages(Schedule schedule);
        

        void InsertStages(IEnumerable<Stage> stages);

        /// <summary>
        /// Удаляет все авансы заданного договора
        /// </summary>
        /// <param name="contractId">Идентификатор договора</param>
        void DeletePrepayments(double contractId);

        /// <summary>
        /// Добавляет авансы по договору
        /// </summary>
        /// <param name="prepayments">Коллекция авансов</param>
        void AddContractPrepayments(IEnumerable<Prepayment> prepayments);

        void DeleteContractType(Contracttype type);

        void InsertContractType(Contracttype type);

        void DeleteContractState(Contractstate contractstate);

        void InsertContractState(Contractstate contractstate);

        void InsertNds(Nds nds);

        void DeleteNds(Nds nds);

        void InsertCurrency(Currency cur);

        void DeleteCurrency(Currency cur);

        void InsertDegree(Degree deg);

        void DeleteDegree(Degree deg);

        void InsertWorktype(Worktype worktype);

        void DeleteWorktype(Worktype worktype);

        void DeleteObject(object obj);

        void InsertEconomefficiencytype(Economefficiencytype worktype);

        void DeleteEconomefficiencytype(Economefficiencytype worktype);

        void InsertStateResult(Stageresult stageresult);

        void DeleteStateResult(Stageresult stageresult);

        void InsertEconomefficiencyparameter(Economefficiencyparameter param);

        void DeleteEconomefficiencyparameter(Economefficiencyparameter param);

        void InsertEfficienceparametertype(Efficienceparametertype param_type);

        void DeleteEfficienceparametertype(Efficienceparametertype param_type);

        void InsertNtpview(Ntpview ntpview);

        void DeleteNtpview(Ntpview ntpview);

        void InsertNtpsubview(Ntpsubview ntpsubview);

        void DeleteNtpsubview(Ntpsubview ntpsubview);

        void InsertContractortype(Contractortype contracttype);

        void DeleteContractortype(Contractortype contracttype);

        void InsertContractor(Contractor contractor);

        void DeleteContractor(Contractor contractor);

        void InsertPosition(Position position);

        void DeletePosition(Position position);

        void InsertProperty(Property property);

        void DeleteProperty(Property property);

        void InsertTrouble(Trouble trouble);

        void DeleteTrouble(Trouble trouble);

        void InsertNdsalgorithm(Ndsalgorithm ndsalgorithm);

        void DeleteNdsalgorithm(Ndsalgorithm ndsalgorithm);

        void InsertTroublesregistry(Troublesregistry troublesregistry);

        void DeleteTroublesregistry(Troublesregistry troublesregistry);

        void InsertCurrencymeasure(Currencymeasure currencymeasure);

        void DeleteCurrencymeasure(Currencymeasure currencymeasure);

        void InsertRegion(Region region);

        void DeleteRegion(Region region);

        void InsertRole(Role role);

        void DeleteRole(Role role);

        void InsertAuthority(Authority authority);

        void DeleteAuthority(Authority authority);

        void InsertActtype(Acttype acttype);

        void DeleteActtype(Acttype acttype);

        void DeleteApproval(Approvalprocess approval);

        void InsertContractorposition(Contractorposition contractorposition);

        void DeleteContractorposition(Contractorposition contractorposition);

        void InsertPrepaymentdocumenttype(Prepaymentdocumenttype region);

        void DeletePrepaymentdocumenttype(Prepaymentdocumenttype region);

        void InsertPerson(Person person);

        void DeletePerson(Person person);

        void InsertEmployee(Employee employee);

        void DeleteEmployee(Employee employee);

        void DeleteEfparamStageresult(Efparameterstageresult efparameterstageresult);

        void InsertFunctionalcustomer(Functionalcustomer functionalcustomer);

        void DeleteFunctionalcustomer(Functionalcustomer functionalcustomer);
        
        void InsertFunctionalcustomertype(Functionalcustomertype functionalcustomertype);

        void DeleteFunctionalcustomertype(Functionalcustomertype functionalcustomertype);

        void InsertDocument(Document document);

        void DeleteDocument(Document document);

        void InsertTransferacttype(Transferacttype transferacttype);

        void DeleteTransferacttype(Transferacttype transferacttype);

        void InsertTransferacttypedocument(Transferacttypedocument transferacttypedocument);

        void DeleteTransferacttypedocument(Transferacttypedocument transferacttypedocument);

        void InsertFunccustomerperson(Funccustomerperson funccustomerperson);

        void DeleteFunccustomerperson(Funccustomerperson funccustomerperson);

        void InsertResponsiblefororder(Responsiblefororder responsiblefororder);
        void DeleteResponsiblefororder(Responsiblefororder responsiblefororder);

        void InsertResponsibleassignmentorder(Responsibleassignmentorder responsibleassignmentorder);
        void DeleteResponsibleassignmentorder(Responsibleassignmentorder responsibleassignmentorder);


        void InsertContractorpropertiy(Contractorpropertiy contractorpropertiy);

        void DeleteContractorpropertiy(Contractorpropertiy contractorpropertiy);

        void InsertSightfuncpersonscheme(Sightfuncpersonscheme sightfuncpersonscheme);

        void DeleteSightfuncpersonscheme(Sightfuncpersonscheme sightfuncpersonscheme);

        void DeletePaymentdocument(Actpaymentdocument actpaymentdocument);

        void DeleteLocation(Location location);

        void InsertLocation(Location location);

        void DeleteMissiveType(Missivetype missivetype);

        void InsertMissiveType(Missivetype missivetype);

        void DeleteApprovalGoal(Approvalgoal approvalgoal);

        void InsertApprovalGoal(Approvalgoal approvalgoal);

        void DeleteApprovalState(Approvalstate approvalstate);
        
        void InsertApprovalState(Approvalstate approvalstate);

        void DeleteDisposal(Disposal disposal);

        void InsertDisposal(Disposal disposal);

        void InsertDepartment(Department department);

        void DeleteDepartment(Department department);

        void InsertYearreportcolor(Yearreportcolor yearreportcolor);
        
        void DeleteYearreportcolor(Yearreportcolor yearreportcolor);

        IEnumerable<IContractStateData> DeleteContractdoc(Contractdoc contractdoc);
        IEnumerable<IContractStateData> DeleteContract(double id);


        IEnumerable<Contractrepositoryview> ActiveContractrepositoryviews { get; }

        IEnumerable<Actrepositoryview> Actsrepositoryview { get; }

        List<Contracttranactdoc> Contracttrandocs { get; }

        void DeleteAct(long p);

        IEnumerable<Act> Acts { get; }

        /// <summary>
        /// Обновляет статистику по заданному договору
        /// </summary>
        /// <param name="contractdoc">Объект договора</param>
        void UpdateStatistics(Contractdoc contractdoc);

    }
}
