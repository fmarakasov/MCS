using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.Contracts;
using MCDomain.Model;
using System.Data.Linq;
using UOW;

namespace MCDomain.DataAccess
{
    [ContractClassFor(typeof(IContractRepository))]
    abstract class ContractrepositoryContract : IContractRepository
    {
        public IUnitOfWork UnitOfWork { get; private set; }

        public IList<Contractdoc> Contracts
        {
            get
            {
                Contract.Ensures(Contract.Result<IEnumerable<Contractdoc>>() != null);
                return default(IList<Contractdoc>);
            }
        }

        public Contractdoc NewContractdoc()
        {
            Contract.Ensures(Contract.Result<Contractdoc>() != null);
            return default(Contractdoc);
        }

        public void DeleteObject(object obj)
        {
            return;
        }

        public Stage NewStage()
        {
            Contract.Ensures(Contract.Result<Stage>() != null);
            return default(Stage);
        }

        public Act NewAct(Contractdoc contractdoc)
        {
            Contract.Requires(contractdoc != null);
            Contract.Requires(contractdoc.Currency != null);

            Contract.Ensures(Contract.Result<Act>() != null);
            return default(Act);
        }

        public decimal GetPrepaymentRestsForAct(long actId, long contractdoc)
        {
            Contract.Ensures(Contract.Result<decimal>() >= 0);
            return default(decimal);
        }

        public Contractdoc GetContractdoc(long contractId)
        {
            return default(Contractdoc);
        }

        public IList<Contractstate> States
        {
            get
            {
                Contract.Ensures(Contract.Result<IEnumerable<Contractstate>>() != null);
                return default(IList<Contractstate>);
            }
        }

        public IList<Ndsalgorithm> Ndsalgorithms
        {
            get
            {
                Contract.Ensures(Contract.Result<IEnumerable<Ndsalgorithm>>() != null);
                return default(IList<Ndsalgorithm>);
            }
        }

        public IList<Nds> Nds
        {
            get
            {
                Contract.Ensures(Contract.Result<IEnumerable<Nds>>() != null);
                return default(IList<Nds>);
            }
        }

        public IList<Currency> Currencies
        {
            get
            {
                Contract.Ensures(Contract.Result<IEnumerable<Currency>>() != null);
                return default(IList<Currency>);
            }
        }

        public IList<Contracttype> Contracttypes
        {
            get
            {
                Contract.Ensures(Contract.Result<IEnumerable<Contracttype>>() != null);
                return default(IList<Contracttype>);
            }
        }

        public IList<Currencymeasure> Currencymeasures
        {
            get
            {
                Contract.Ensures(Contract.Result<IEnumerable<Currencymeasure>>() != null);
                return default(IList<Currencymeasure>);
            }
        }

        public IList<Contractor> Contractors
        {
            get
            {
                Contract.Ensures(Contract.Result<IEnumerable<Contractor>>() != null);
                return default(IList<Contractor>);
            }
        }

        public IList<Functionalcustomer> Functionalcustomers
        {
            get
            {
                Contract.Ensures(Contract.Result<IEnumerable<Functionalcustomer>>() != null);
                return default(IList<Functionalcustomer>);
            }
        }

        public IList<Disposal> Disposals
        {
            get
            {
                Contract.Ensures(Contract.Result<IEnumerable<Disposal>>() != null);
                return default(IList<Disposal>);
            }
        }

        public IList<Enterpriseauthority> Enterpriseauthorities
        {
            get
            {
                Contract.Ensures(Contract.Result<IEnumerable<Enterpriseauthority>>() != null);
                return default(IList<Enterpriseauthority>);
            }
        }

        public IList<Troublesregistry> TroublesRegistry
        {
            get
            {
                Contract.Ensures(Contract.Result<IEnumerable<Troublesregistry>>() != null);
                return default(IList<Troublesregistry>);
            }
        }

        public IList<Trouble> Troubles
        {
            get
            {
                Contract.Ensures(Contract.Result<IEnumerable<Trouble>>() != null);
                return default(IList<Trouble>);
            }
        }

        public IList<Worktype> Worktypes
        {
            get { throw new NotImplementedException(); }
        }

        public IList<Importingscheme> Importingschemes
        {
            get
            {
                throw new NotImplementedException();

            }
        }

        public IList<Degree> Degrees
        {
            get { throw new NotImplementedException(); }
        }

        public  IList<Filterstate> Filterstates
        {
            get { throw new NotImplementedException(); }
        }

        public IList<Ntpview> Ntpviews
        {
            get { throw new NotImplementedException(); }
        }

        public IList<Ntpsubview> Ntpsubviews
        {
            get { throw new NotImplementedException(); }
        }

        public IList<Economefficiencytype> Economefficiencytypes
        {
            get { throw new NotImplementedException(); }
        }

        public IList<Economefficiencyparameter> Economefficiencyparameters
        {
            get { throw new NotImplementedException(); }
        }

        public IList<Efficienceparametertype> Efficienceparametertypes
        {
            get { throw new NotImplementedException(); }
        }

        public IList<Contractortype> Contractortypes
        {
            get { throw new NotImplementedException(); }
        }

        public IList<Position> Positions
        {
            get { throw new NotImplementedException(); }
        }

        public IList<Property> Properties
        {
            get { throw new NotImplementedException(); }
        }

        public IList<Region> Regions
        {
            get { throw new NotImplementedException(); }
        }

        public IList<Role> Roles
        {
            get { throw new NotImplementedException(); }
        }

        public IList<Authority> Authorities
        {
            get { throw new NotImplementedException(); }
        }

        public IList<Acttype> Acttypes
        {
            get { throw new NotImplementedException(); }
        }

        public IList<Contractorposition> Contractorpositions
        {
            get { throw new NotImplementedException(); }
        }

        public IList<Prepaymentdocumenttype> Prepaymentdocumenttypes
        {
            get { throw new NotImplementedException(); }
        }

        public IList<Person> Persons
        {
            get { throw new NotImplementedException(); }
        }

        public IList<Employee> Employees
        {
            get { throw new NotImplementedException(); }
        }

        public IList<Functionalcustomertype> Functionalcustomertypes
        {
            get { throw new NotImplementedException(); }
        }

        public IList<Document> Documents
        {
            get { throw new NotImplementedException(); }
        }

        public IList<Transferacttype> Transferacttypes
        {
            get { throw new NotImplementedException(); }
        }

        public IList<Transferacttypedocument> Transferacttypedocuments
        {
            get { throw new NotImplementedException(); }
        }

        public IList<Funccustomerperson> Funccustomerpersons
        {
            get { throw new NotImplementedException(); }
        }

        public IList<Contractorpropertiy> Contractorpropertiies
        {
            get { throw new NotImplementedException(); }
        }

        public IList<Sightfuncpersonscheme> Sightfuncpersonschemes
        {
            get { throw new NotImplementedException(); }
        }

        public IList<Stage> AllStages
        {
            get { throw new NotImplementedException(); }
        }

      

        public IList<Responsibleassignmentorder> Responsibleassignmentorders
        {
            get { throw new NotImplementedException(); }
        }

        public IList<Yearreportcolor> Yearreportcolors
        {
            get { throw new NotImplementedException(); }
        }

        public IList<Transferact> Transferacts
        {
            get
            {
                Contract.Ensures(Contract.Result<IEnumerable<Transferact>>() != null);
                return default(IList<Transferact>);
            }
        }

        public void SubmitChanges()
        {
            Contract.Ensures(!IsModified);
        }

        public void RejectChanges()
        {
            
        }

        public void Refresh(RefreshMode mode, object entity)
        {
        }

        public void Refresh(RefreshMode mode, IEnumerable<object> entities)
        {
        }

        public bool IsModified
        {
            get { return default(bool); }
        }

        public void AddContract(Contractdoc contractdoc)
        {
            Contract.Requires(contractdoc != null);
            Contract.Requires(!Contracts.Contains(contractdoc));
            //Contract.Ensures(Contracts.Contains(contractdoc));
            //Contract.Ensures(IsModified);
        }

        

        public void DeleteTroublecontract(Contracttrouble entity)
        {
            Contract.Requires(entity != null);
        }

        public void InsertTroublecontract(Contracttrouble entity)
        {
            Contract.Requires(entity != null);
        }

        public IEnumerable<Stage> GetStages(double scheduleId)
        {
            return null;
        }

        public void InsertShedule(Schedulecontract schedule)
        {
            Contract.Requires(schedule != null);
        }

        public void DeleteShedule(Schedulecontract schedule)
        {
            Contract.Requires(schedule != null);
        }

        public void DeleteShedule(Schedule schedule)
        {
            Contract.Requires(schedule != null);
        }

        public void DeleteStage(Stage stage)
        {
            throw new NotImplementedException();
        }

        public void InsertStage(Stage stage)
        {
            throw new NotImplementedException();
        }

        public void DeleteAllStages(Schedule schedule)
        {
            throw new NotImplementedException();
        }


        public void InsertStages(IEnumerable<Stage> stages)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Удаляет все авансы заданного договора
        /// </summary>
        /// <param name="contractId">Идентификатор договора</param>
        public void DeletePrepayments(double contractId)
        {
            
        }

        /// <summary>
        /// Добавляет авансы по договору
        /// </summary>
        /// <param name="prepayments">Коллекция авансов</param>
        public void AddContractPrepayments(IEnumerable<Prepayment> prepayments)
        {
            Contract.Requires(prepayments != null);
        }

        public void DeleteContractType(Contracttype type)
        {
            throw new NotImplementedException();
        }

        public void InsertContractType(Contracttype type)
        {
            throw new NotImplementedException();
        }

        public void DeleteContractState(Contractstate contractstate)
        {
            throw new NotImplementedException();
        }

        public void InsertContractState(Contractstate contractstate)
        {
            throw new NotImplementedException();
        }

        public void InsertNds(Nds nds)
        {
            throw new NotImplementedException();
        }

        public void DeleteNds(Nds nds)
        {
            throw new NotImplementedException();
        }

        public void InsertCurrency(Currency cur)
        {
            throw new NotImplementedException();
        }

        public void DeleteCurrency(Currency cur)
        {
            throw new NotImplementedException();
        }

        public void InsertDegree(Degree deg)
        {
            throw new NotImplementedException();
        }

        public void DeleteDegree(Degree deg)
        {
            throw new NotImplementedException();
        }

        public void InsertWorktype(Worktype worktype)
        {
            throw new NotImplementedException();
        }

        public void DeleteWorktype(Worktype worktype)
        {
            throw new NotImplementedException();
        }

        public void InsertEconomefficiencytype(Economefficiencytype worktype)
        {
            throw new NotImplementedException();
        }

        public void DeleteEconomefficiencytype(Economefficiencytype worktype)
        {
            throw new NotImplementedException();
        }

        public void InsertStateResult(Stageresult stageresult)
        {
            throw new NotImplementedException();
        }

        public void DeleteStateResult(Stageresult stageresult)
        {
            throw new NotImplementedException();
        }

        public void InsertEconomefficiencyparameter(Economefficiencyparameter param)
        {
            throw new NotImplementedException();
        }

        public void DeleteEconomefficiencyparameter(Economefficiencyparameter param)
        {
            throw new NotImplementedException();
        }

        public void InsertEfficienceparametertype(Efficienceparametertype param_type)
        {
            throw new NotImplementedException();
        }

        public void DeleteEfficienceparametertype(Efficienceparametertype param_type)
        {
            throw new NotImplementedException();
        }

        public void InsertNtpview(Ntpview ntpview)
        {
            throw new NotImplementedException();
        }

        public void DeleteNtpview(Ntpview ntpview)
        {
            throw new NotImplementedException();
        }

        public void InsertNtpsubview(Ntpsubview ntpsubview)
        {
            Contract.Requires(ntpsubview != null);
        }

        public void DeleteNtpsubview(Ntpsubview ntpsubview)
        {
            throw new NotImplementedException();
        }

        public void InsertContractortype(Contractortype contracttype)
        {
            throw new NotImplementedException();
        }

        public void DeleteContractortype(Contractortype contracttype)
        {
            throw new NotImplementedException();
        }

        public void InsertContractor(Contractor contractor)
        {
            throw new NotImplementedException();
        }

        public void DeleteContractor(Contractor contractor)
        {
            throw new NotImplementedException();
        }

        public void InsertPosition(Position position)
        {
            throw new NotImplementedException();
        }

        public void DeletePosition(Position position)
        {
            throw new NotImplementedException();
        }

        public void InsertProperty(Property property)
        {
            throw new NotImplementedException();
        }

        public void DeleteProperty(Property property)
        {
            throw new NotImplementedException();
        }

        public void InsertTrouble(Trouble trouble)
        {
            throw new NotImplementedException();
        }

        public void DeleteTrouble(Trouble trouble)
        {
            throw new NotImplementedException();
        }

        public void InsertNdsalgorithm(Ndsalgorithm ndsalgorithm)
        {
            throw new NotImplementedException();
        }

        public void DeleteNdsalgorithm(Ndsalgorithm ndsalgorithm)
        {
            throw new NotImplementedException();
        }

        public void InsertTroublesregistry(Troublesregistry troublesregistry)
        {
            throw new NotImplementedException();
        }

        public void DeleteTroublesregistry(Troublesregistry troublesregistry)
        {
            throw new NotImplementedException();
        }

        public void InsertCurrencymeasure(Currencymeasure currencymeasure)
        {
            throw new NotImplementedException();
        }

        public void DeleteCurrencymeasure(Currencymeasure currencymeasure)
        {
            throw new NotImplementedException();
        }

        public void InsertRegion(Region region)
        {
            throw new NotImplementedException();
        }

        public void DeleteRegion(Region region)
        {
            throw new NotImplementedException();
        }

        public void InsertRole(Role role)
        {
            throw new NotImplementedException();
        }

        public void DeleteRole(Role role)
        {
            throw new NotImplementedException();
        }

        public void InsertAuthority(Authority authority)
        {
            throw new NotImplementedException();
        }

        public void DeleteAuthority(Authority authority)
        {
            throw new NotImplementedException();
        }

        public void InsertActtype(Acttype acttype)
        {
            throw new NotImplementedException();
        }

        public void DeleteActtype(Acttype acttype)
        {
            throw new NotImplementedException();
        }

        public void DeleteApproval(Approvalprocess approval)
        {
            throw new NotImplementedException();
        }

        public void InsertDepartment(Department department)
        {
            throw new NotImplementedException();
        }

        public void DeleteDepartment(Department department)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IContractStateData> DeleteContractdoc(Contractdoc contractdoc)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IContractStateData> DeleteContract(double id)
        {
            throw new NotImplementedException();
        }



        public IEnumerable<Contractrepositoryview> ActiveContractrepositoryviews
        {
            get { throw new NotImplementedException(); }
        }

        public IEnumerable<Actrepositoryview> Actsrepositoryview { get; set; }
        public List<Contracttranactdoc> Contracttrandocs { get; private set; }

        public void DeleteAct(long p)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Act> Acts { get; private set; }
        public void UpdateStatistics(Contractdoc contractdoc)
        {
            throw new NotImplementedException();
        }


        public void DeleteDisposal(Disposal disposal)
        {
            throw new NotImplementedException();
        }

        public void InsertDisposal(Disposal disposal)
        {
            throw new NotImplementedException();
        }

        public void InsertContractorposition(Contractorposition contractorposition)
        {
            throw new NotImplementedException();
        }

        public void DeleteContractorposition(Contractorposition contractorposition)
        {
            throw new NotImplementedException();
        }

        public void InsertPrepaymentdocumenttype(Prepaymentdocumenttype region)
        {
            throw new NotImplementedException();
        }

        public void DeletePrepaymentdocumenttype(Prepaymentdocumenttype region)
        {
            throw new NotImplementedException();
        }

        public void InsertPerson(Person person)
        {
            throw new NotImplementedException();
        }

        public void DeletePerson(Person person)
        {
            throw new NotImplementedException();
        }

        public void InsertEmployee(Employee employee)
        {
            throw new NotImplementedException();
        }

        public void DeleteEmployee(Employee employee)
        {
            throw new NotImplementedException();
        }

        public void DeleteEfparamStageresult(Efparameterstageresult efparameterstageresult)
        {
            throw new NotImplementedException();
        }

        public void InsertFunctionalcustomer(Functionalcustomer functionalcustomer)
        {
            throw new NotImplementedException();
        }

        public void DeleteFunctionalcustomer(Functionalcustomer functionalcustomer)
        {
            throw new NotImplementedException();
        }

        public void InsertFunctionalcustomertype(Functionalcustomertype functionalcustomertype)
        {
            throw new NotImplementedException();
        }

        public void DeleteFunctionalcustomertype(Functionalcustomertype functionalcustomertype)
        {
            throw new NotImplementedException();
        }

        public void InsertDocument(Document document)
        {
            throw new NotImplementedException();
        }

        public void DeleteDocument(Document document)
        {
            throw new NotImplementedException();
        }

        public void InsertTransferacttype(Transferacttype transferacttype)
        {
            throw new NotImplementedException();
        }

        public void DeleteTransferacttype(Transferacttype transferacttype)
        {
            throw new NotImplementedException();
        }

        public void InsertTransferacttypedocument(Transferacttypedocument transferacttypedocument)
        {
            throw new NotImplementedException();
        }

        public void DeleteTransferacttypedocument(Transferacttypedocument transferacttypedocument)
        {
            throw new NotImplementedException();
        }

        public void InsertFunccustomerperson(Funccustomerperson funccustomerperson)
        {
            throw new NotImplementedException();
        }

        public void DeleteFunccustomerperson(Funccustomerperson funccustomerperson)
        {
            throw new NotImplementedException();
        }

        public void DeleteResponsibleassignmentorder(Responsibleassignmentorder responsibleassignmentorder)
        {
            throw new NotImplementedException();
        }

        public void InsertContractorpropertiy(Contractorpropertiy contractorpropertiy)
        {
            throw new NotImplementedException();
        }

        public void DeleteContractorpropertiy(Contractorpropertiy contractorpropertiy)
        {
            throw new NotImplementedException();
        }

        public void InsertSightfuncpersonscheme(Sightfuncpersonscheme sightfuncpersonscheme)
        {
            throw new NotImplementedException();
        }

        public void DeleteSightfuncpersonscheme(Sightfuncpersonscheme sightfuncpersonscheme)
        {
            throw new NotImplementedException();
        }

        public void DeletePaymentdocument(Actpaymentdocument actpaymentdocument)
        {
            throw new NotImplementedException();
        }

       

        public void DeleteLocation(Location location)
        {
            throw new NotImplementedException();
        }

        public void DeleteMissiveType(Missivetype missivetype)
        {
            throw new NotImplementedException();
        }

        public void DeleteApprovalGoal(Approvalgoal approvalgoal)
        {
            throw new NotImplementedException();
        }

        public void DeleteApprovalState(Approvalstate approvalstate)
        {
            throw new NotImplementedException();
        }

        public void InsertLocation(Location location)
        {
            throw new NotImplementedException();
        }

        public void InsertMissiveType(Missivetype missivetype)
        {
            throw new NotImplementedException();
        }

        public void InsertApprovalGoal(Approvalgoal approvalgoal)
        {
            throw new NotImplementedException();
        }

        public void InsertApprovalState(Approvalstate approvalstate)
        {
            throw new NotImplementedException();
        }

        public void InsertResponsiblefororder(Responsiblefororder responsiblefororder)
        {
            throw new NotImplementedException();
        }

        public void DeleteResponsiblefororder(Responsiblefororder responsiblefororder)
        {
            throw new NotImplementedException();
        }

        public void InsertResponsibleassignmentorder(Responsibleassignmentorder responsibleassignmentorder)
        {
            throw new NotImplementedException();
        }

        public void InsertYearreportcolor(Yearreportcolor yearreportcolor)
        {
            throw new NotImplementedException();
        }

        public void DeleteYearreportcolor(Yearreportcolor yearreportcolor)
        {
            throw new NotImplementedException();
        }


        public IList<Responsiblefororder> Responsiblefororders { get { throw new NotImplementedException(); } }
        public IList<Location> Locations { get { throw new NotImplementedException(); } }
        public IList<Missivetype> MissiveTypes { get { throw new NotImplementedException(); } }
        public IList<Approvalgoal> ApprovalGoals { get { throw new NotImplementedException(); } }
        public IList<Approvalstate> ApprovalStates { get { throw new NotImplementedException(); } }
        public IList<Department> Departments { get {throw new NotImplementedException();} }
        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
