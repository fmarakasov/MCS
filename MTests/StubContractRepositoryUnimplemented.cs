using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MCDomain.DataAccess;
using MCDomain.Model;
using System.Data.Linq;

namespace MTests
{
    class StubContractRepositoryUnimplemented : IContractRepository
    {
        public void DeleteObject(object obj)
        {
            return;
        }


        public IList<Contractdoc> Contracts
        {
            get { throw new NotImplementedException(); }
        }

        public Contractdoc NewContractdoc()
        {
            throw new NotImplementedException();
        }

        public Stage NewStage()
        {
            throw new NotImplementedException();
        }

        public Act NewAct(Contractdoc contractdoc)
        {
            throw new NotImplementedException();
        }

        public decimal GetPrepaymentRestsForAct(long actId)
        {
            throw new NotImplementedException();
        }

        public Contractdoc GetContractdoc(long contractId)
        {
            throw new NotImplementedException();
        }

        public Contractdoc GetContractdoc(double contractId)
        {
            throw new NotImplementedException();
        }

        public IList<Contractstate> States
        {
            get { throw new NotImplementedException(); }
        }

        public IList<Ndsalgorithm> Ndsalgorithms
        {
            get { throw new NotImplementedException(); }
        }

        public IList<Nds> Nds
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

        public IList<Currency> Currencies
        {
            get { throw new NotImplementedException(); }
        }

        public IList<Contracttype> Contracttypes
        {
            get { throw new NotImplementedException(); }
        }

        public IList<Currencymeasure> Currencymeasures
        {
            get { throw new NotImplementedException(); }
        }

        public IList<Contractor> Contractors
        {
            get { throw new NotImplementedException(); }
        }

        public IList<Functionalcustomer> Functionalcustomers
        {
            get { throw new NotImplementedException(); }
        }

        public IList<Disposal> Disposals
        {
            get { throw new NotImplementedException(); }
        }

        public IList<Enterpriseauthority> Enterpriseauthorities
        {
            get { throw new NotImplementedException(); }
        }

        public IList<Troublesregistry> TroublesRegistry
        {
            get { throw new NotImplementedException(); }
        }

        public IList<Trouble> Troubles
        {
            get { throw new NotImplementedException(); }
        }

        public IList<Worktype> Worktypes
        {
            get { throw new NotImplementedException(); }
        }

        public IList<Degree> Degrees
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

        public IList<Filterstate> Filterstates
        {
            get { throw new NotImplementedException(); }
        }

        public IList<Yearreportcolor> Yearreportcolors { get; private set; }
        public IList<Transferact> Transferacts { get; private set; }

        public void SubmitChanges()
        {
            throw new NotImplementedException();
        }

        public void RejectChanges()
        {
            throw new NotImplementedException();
        }

        public void Refresh(RefreshMode mode, object entity)
        {
            throw new NotImplementedException();
        }

        public void Refresh(RefreshMode mode, IEnumerable<object> entities)
        {
            throw new NotImplementedException();
        }

        public bool IsModified
        {
            get { throw new NotImplementedException(); }
        }

        public void AddContract(Contractdoc contractdoc)
        {
            throw new NotImplementedException();
        }

        public void DeleteTroublecontract(Contracttrouble entity)
        {
            throw new NotImplementedException();
        }

        public void InsertTroublecontract(Contracttrouble entity)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Stage> GetStages(double scheduleId)
        {
            throw new NotImplementedException();
        }

        public void DeleteShedule(Schedule schedule)
        {
            throw new NotImplementedException();
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

        public void DeletePrepayments(double contractId)
        {
            throw new NotImplementedException();
        }

        public void AddContractPrepayments(IEnumerable<Prepayment> prepayments)
        {
            throw new NotImplementedException();
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
            throw new NotImplementedException();
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

        public void DeleteDisposal(Disposal disposal)
        {
            throw new NotImplementedException();
        }

        public void InsertDisposal(Disposal disposal)
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

        public void InsertYearreportcolor(Yearreportcolor yearreportcolor)
        {
            throw new NotImplementedException();
        }

        public void DeleteYearreportcolor(Yearreportcolor yearreportcolor)
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
        public void DeleteAct(long p)
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

        public IList<Responsiblefororder> Responsiblefororders { get { throw new NotImplementedException(); } }
        public IList<Location> Locations { get { throw new NotImplementedException(); } }
        public IList<Missivetype> MissiveTypes { get { throw new NotImplementedException(); } }
        public IList<Approvalgoal> ApprovalGoals { get { throw new NotImplementedException(); } }
        public IList<Approvalstate> ApprovalStates { get { throw new NotImplementedException(); } }
        public IList<Department> Departments { get { throw new NotImplementedException(); } }
        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
