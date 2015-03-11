using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Linq;
using CommonBase;
using Devart.Data.Linq;
using LCUOF;
using MCDomain.Model;
using MCDomain.Common;
using System.Data.Linq;
using Ninject;
using Ninject.Parameters;
using UOW;
using DataLoadOptions = Devart.Data.Linq.DataLoadOptions;
using MCDomain.DataAccess;


namespace MCDomain.DataAccess
{
    public class LinqContractRepository : IContractRepository, IContextProvider<McDataContext>
    {
        private McDataContext _context;
        private readonly IDataContextProvider _contextProvider;

        public LinqContractRepository(IDataContextProvider contextProvider)
        {
            Contract.Assert(contextProvider != null);
            _contextProvider = contextProvider;


        }

        /// <summary>
        /// Возвращает контекст данных
        /// </summary>
        public McDataContext Context
        {
            get
            {
                if (_context == null)
                {
                    _context = _contextProvider.CreateContext();

                    if (_context != null)
                    {
                        SendInitEvent();
                    }
                }
                return _context;
            }
        }
        
        private void SendInitEvent()
        {
            if (ContextCreated != null)
                ContextCreated(this, new ContextEventArgs<McDataContext>(_context));
        }

        private IUnitOfWork _unitOfWork;

        public IUnitOfWork UnitOfWork
        {
            get
            {
                //var ioc = IoC.IoCContainer.Instance.Kernel;
                return _unitOfWork ??(_unitOfWork = new UnitOfWork(new LcDataContext(Context), RepositoryFactory.Default));

            }
        }

        public IList<Contractdoc> Contracts
        {
            get
            {
                return Context.Contractdocs.Where(x => (x.Deleted.HasValue && !x.Deleted.Value) || (!x.Deleted.HasValue)).ToList();
            }
        }

        public Contractdoc NewContractdoc()
        {

            return new Contractdoc()
                       {
                           Subject = "Новый договор",
                           Contractstate = Context.Contractstates.AsEnumerable().FirstOrDefault(x => x.IsUnsigned),
                           Currencymeasure = Context.Currencymeasures.FirstOrDefault(),
                           Currency = Context.Currencies.AsEnumerable().FirstOrDefault(x => x.IsNational),
                           Nds = Context.Nds.FirstOrDefault()
                       };

        }

        public Stage NewStage()
        {
            return new Stage() { Nds = Context.Nds.FirstOrDefault(), Subject = "Новый этап" };
        }

        /// <summary>
        /// Создаёт новый акт по заданному договору
        /// </summary>
        /// <param name="contractdoc"></param>
        /// <returns></returns>
        public Act NewAct(Contractdoc contractdoc)
        {
            var act = new Act()
                          {
                              Currency = Currencies.FirstOrDefault(x => x.IsNational),
                              Acttype = Acttypes.SingleOrDefault(x => x.Id == EntityBase.ReservedDefault),
                              Currencymeasure = Currencymeasures.SingleOrDefault(x => x.Id == EntityBase.ReservedDefault),
                              Region = Regions.SingleOrDefault(x => x.Id == EntityBase.ReservedUndifinedOid),
                              Enterpriseauthority = Enterpriseauthorities.SingleOrDefault(x => x.Id == EntityBase.ReservedUndifinedOid),
                              ContractObject = contractdoc,
                              Nds = contractdoc.Nds ?? Nds.SingleOrDefault(x => x.Id == EntityBase.ReservedDefault),
                              Ndsalgorithm = contractdoc.Ndsalgorithm,
                              Sumfortransfer = 0,
                              Totalsum = 0,
                              Credited = 0,
                              Ratedate = contractdoc.Currency.IsForeign ? DateTime.Today : default(DateTime?),
                              Currencyrate = contractdoc.Currency.IsForeign ? 1 : default(decimal?)

                          };
            return act;
        }


        public decimal GetPrepaymentRestsForAct(long actId, long contractdocid)
        {
            var act = Context.Acts.SingleOrDefault(x => x.Id == actId);
            var contract = Context.Contractdocs.Single(x => x.Id == contractdocid);

            // Платёжные документы ВСЕГДА привязывются к основной версии договора 
            contract = contract.MainContract;
            var totalPrepaymented = contract.TransferedFunds;

            // Получить остатки по авансам, не принимая в расчёт сумму использованного аванса по заданному акту
            // Это требуется для правильного вывода суммы остатка для конкретного акта
            // Например, аванс - 1000 руб. Распределение авансов по двум актам 1 - 300 р., 2 - 500 р. Остаток по авансам - 200 р.
            // Но, если я редактирую, например, акт 2, то в редакторе акта я располагаю суммой 500 + 200 р.
            var totalCredited = contract.Acts.Where(x => x != act).Sum(x => x.Credited);
            return totalPrepaymented - totalCredited;

        }

        public Contractdoc GetContractdoc(long contractId)
        {
            return Context.Contractdocs.SingleOrDefault(x => x.Id == contractId);
        }

        public IList<Contractstate> States
        {
            get { return Context.Contractstates.OrderBy(p => p.Name).ToList(); }
        }

        public IList<Ndsalgorithm> Ndsalgorithms
        {
            get { return Context.Ndsalgorithms.OrderBy(p => p.Name).ToList(); }
        }

        public IList<Nds> Nds
        {
            get { return Context.Nds.OrderBy(p => p.Year).ToList(); }
        }

        public IList<Currency> Currencies
        {
            get { return Context.Currencies.OrderBy(p => p.Name).ToList(); }
        }

        public IList<Contracttype> Contracttypes
        {
            get { return Context.Contracttypes.OrderBy(p => p.Reportorder).ToList(); }
        }

        public IList<Currencymeasure> Currencymeasures
        {
            get { return Context.Currencymeasures.OrderBy(p => p.Id).ToList(); }
        }

        public IList<Importingscheme> Importingschemes
        {
            get { return Context.Importingschemes.OrderBy(p => p.Schemename).ToList(); }
        }

        public IList<Contractor> Contractors
        {
            get
            {
                StringComparer comp = StringComparer.CurrentCultureIgnoreCase;
                return Context.Contractors.ToList().OrderBy(p => p.Name, comp).ToList();
            }
        }

        public IList<Functionalcustomer> Functionalcustomers
        {
            get { return Context.Functionalcustomers.OrderBy(p => p.Name).ToList(); }
        }

        public IList<Filterstate> Filterstates
        {
            get { return Context.Filterstates.OrderBy(p => p.Owner).ToList(); }
        }

        public IList<Disposal> Disposals
        {
            get { return Context.Disposals.OrderBy(p => p.Num).ToList(); }
        }

        public IList<Enterpriseauthority> Enterpriseauthorities
        {
            get { return Context.Enterpriseauthorities.ToList(); }
        }

        public IList<Troublesregistry> TroublesRegistry
        {
            get { return Context.Troublesregistries.OrderBy(p => p.Name).ToList(); }
        }

        public IList<Trouble> Troubles
        {
            get
            {
                return Context.Troubles.OrderBy(p => p.Num).ToList();
            }

        }

        public IList<Worktype> Worktypes
        {
            get { return Context.Worktypes.OrderBy(p => p.Name).ToList(); }
        }

        public IList<Degree> Degrees
        {
            get { return Context.Degrees.OrderBy(p => p.Name).ToList(); }
        }

        public IList<Ntpview> Ntpviews
        {
            get { return Context.Ntpviews.OrderBy(p => p.Name).ToList(); }
        }

        public IList<Ntpsubview> Ntpsubviews
        {
            get { return Context.Ntpsubviews.OrderBy(p => p.Name).ToList(); }
        }

        public IList<Economefficiencytype> Economefficiencytypes
        {
            get { return Context.Economefficiencytypes.OrderBy(p => p.Name).ToList(); }
        }

        public IList<Economefficiencyparameter> Economefficiencyparameters
        {
            get { return Context.Economefficiencyparameters.OrderBy(p => p.Name).ToList(); }
        }

        public IList<Efficienceparametertype> Efficienceparametertypes
        {
            get { return Context.Efficienceparametertypes.OrderBy(p => p.Economefficiencyparameter.Name).ToList(); }
        }

        public IList<Contractortype> Contractortypes
        {
            get { return Context.Contractortypes.OrderBy(p => p.Name).ToList(); }
        }

        public IList<Position> Positions
        {
            get { return Context.Positions.OrderBy(p => p.Name).ToList(); }
        }

        public IList<Property> Properties
        {
            get { return Context.Properties.OrderBy(p => p.Name).ToList(); }
        }

        public IList<Region> Regions
        {
            get { return Context.Regions.OrderBy(p => p.Name).ToList(); }
        }

        public IList<Role> Roles
        {
            get { return Context.Roles.OrderBy(p => p.Id).ToList(); }
        }

        public IList<Authority> Authorities
        {
            get { return Context.Authorities.OrderBy(p => p.Name).ToList(); }
        }

        public IList<Acttype> Acttypes
        {
            get { return Context.Acttypes.OrderBy(p => p.Typename).ToList(); }
        }


        public IList<Contractorposition> Contractorpositions
        {
            get { return Context.Contractorpositions.OrderBy(p => p.Contractor.Name).ToList(); }
        }

        public IList<Prepaymentdocumenttype> Prepaymentdocumenttypes
        {
            get { return Context.Prepaymentdocumenttypes.OrderBy(p => p.Name).ToList(); }
        }

        public IList<Person> Persons
        {
            get { return Context.People.ToList(); }
        }

        public IList<Employee> Employees
        {
            get { return Context.Employees.OrderBy(e => e.Familyname).ToList(); }
        }

        public IList<Functionalcustomertype> Functionalcustomertypes
        {
            get { return Context.Functionalcustomertypes.OrderBy(p => p.Name).ToList(); }
        }

        public IList<Document> Documents
        {
            get { return Context.Documents.OrderBy(p => p.Name).ToList(); }
        }

        public IList<Transferacttype> Transferacttypes
        {
            get { return Context.Transferacttypes.OrderBy(p => p.Name).ToList(); }
        }

        public IList<Transferacttypedocument> Transferacttypedocuments
        {
            get { return Context.Transferacttypedocuments.OrderBy(p => p.Transferacttype.Name).ToList(); }
        }

        public IList<Funccustomerperson> Funccustomerpersons
        {
            get { return Context.Funccustomerpersons.ToList(); }
        }

        public IList<Responsiblefororder> Responsiblefororders
        {
            get { return Context.Responsiblefororders.ToList(); }
        }

        public IList<Responsibleassignmentorder> Responsibleassignmentorders
        {
            get { return Context.Responsibleassignmentorders.ToList(); }
        }

        public IList<Contractorpropertiy> Contractorpropertiies
        {
            get { return Context.Contractorpropertiys.ToList(); }
        }

        public IList<Sightfuncpersonscheme> Sightfuncpersonschemes
        {
            get { return Context.Sightfuncpersonschemes.OrderBy(p => p.Num).ToList(); }
        }

        public IList<Location> Locations
        {
            get { return Context.Locations.OrderBy(p => p.Name).ToList(); }
        }

        public IList<Missivetype> MissiveTypes
        {
            get { return Context.Missivetypes.OrderBy(p => p.Name).ToList(); }
        }

        public IList<Approvalgoal> ApprovalGoals
        {
            get { return Context.Approvalgoals.OrderBy(p => p.Name).ToList(); }
        }

        public IList<Approvalstate> ApprovalStates
        {
            get { return Context.Approvalstates.OrderBy(p => p.Name).ToList(); }
        }

        public IList<Department> Departments
        {
            get { return Context.Departments.OrderBy(p => p.Name.Trim()).ToList(); }
        }

        public IList<Yearreportcolor> Yearreportcolors
        {
            get { return Context.Yearreportcolors.OrderBy(p => p.Quarter * p.Year).ToList(); }
        }

        public IList<Transferact> Transferacts
        {
            get { return Context.Transferacts.ToList(); }
        }

        public IList<Stage> AllStages
        {
            get { return Context.Stages.ToList(); }
        }

        public void SubmitChanges()
        {
            Context.GetChangeSet().DebugPrintRepository("Вызов LinqContractRepository.SubmitChanges");

            Context.SubmitChanges();
        }

        public void RejectChanges()
        {
            Context.RejectChanges();
        }

        public void Refresh(RefreshMode mode, object entity)
        {
            Context.Refresh(mode, entity);
        }

        public void Refresh(RefreshMode mode, IEnumerable<object> entities)
        {
            Context.Refresh(mode, entities);
        }

        /// <summary>
        /// Получает признак, что контекст был модифицировн с момента последнего сохранения изменений
        /// </summary>     
        public bool IsModified
        {
            get { return Context.IsModified; }
        }

        public void AddContract(Contractdoc contractdoc)
        {
            Context.Contractdocs.InsertOnSubmit(contractdoc);
        }

        public void DeleteFunctionalcustomercontract(Functionalcustomercontract entity)
        {
            Context.Functionalcustomercontracts.DeleteOnSubmit(entity);
        }

        /// <summary>
        /// Регистрирует функционального заказчика договора на всатвку
        /// </summary>
        /// <param name="entity">Функциональный заказчик договора</param>
        public void InsertFunctionalcustomercontract(Functionalcustomercontract entity)
        {
            Context.Functionalcustomercontracts.InsertOnSubmit(entity);
        }

        public void DeleteTroublecontract(Contracttrouble entity)
        {
            Context.Contracttroubles.DeleteOnSubmit(entity);
        }

        public void InsertTroublecontract(Contracttrouble entity)
        {
            Context.Contracttroubles.InsertOnSubmit(entity);
        }

        //public event PropertyChangedEventHandler PropertyChanged;

        public IEnumerable<Stage> GetStages(double scheduleId)
        {
            return Context.Schedules.Single(x => x.Id == scheduleId).Stages.ToList();
        }

        public void DeleteApproval(Approvalprocess approval)
        {
            Context.Approvalprocesses.DeleteOnSubmit(approval);
        }

        public void InsertDepartment(Department department)
        {
            Context.Departments.InsertOnSubmit(department);
        }

        public void DeleteDepartment(Department department)
        {
            Context.Departments.DeleteOnSubmit(department);
        }


        private void DeleteContractInternal(Contractdoc contractdoc, IList<IContractStateData> deletedcontracts)
        {

            deletedcontracts.Add(contractdoc);

            for (var i = contractdoc.Agreements.Count() - 1; i >= 0; i--)
                DeleteContractInternal(contractdoc.Agreements.ToList()[i], deletedcontracts);

            for (var i = contractdoc.SubContracts.Count() - 1; i >= 0; i--)
                DeleteContractInternal(contractdoc.SubContracts.ToList()[i], deletedcontracts);

            // зачищаем авансы
            contractdoc.Prepayments.Clear();

            // зачищаем соподчиненность генеральный-субподрядный
            contractdoc.Contracthierarchies.Clear();
            contractdoc.Generalcontracthierarchies.Clear();

            // смотрим по каким документам проходили платежи
            //var paymentdocs =
            //  Context.Paymentdocuments.Where(p => contractdoc.Contractpayments.Count(c => c.Paymentdocument.Id == p.Id) > 0).ToList();
            var paymentdocs = contractdoc.Contractpayments.Select(x => x.Paymentdocument).ToList();
            // зачищаем платежные документы по договору
            contractdoc.Contractpayments.Clear();
            // после того, как содержимое удалено можно удалить сами документы 
            for (var i = paymentdocs.Count() - 1; i >= 0; i--)
                Context.Paymentdocuments.DeleteOnSubmit(paymentdocs.ToList()[i]);

            // зачищаем проблемы
            contractdoc.Contracttroubles.Clear();
            // зачищаем связи с функциональными заказчиками
            contractdoc.Functionalcustomercontracts.Clear();

            var schs = contractdoc.Schedulecontracts.Select(s => s.Schedule).ToList();
            // зачищаем календарные планы
            contractdoc.Schedulecontracts.Clear();

            // после того, как содержимое удалено можно удалить сами документы 
            for (var i = schs.Count() - 1; i >= 0; i--)
                if (!contractdoc.HasTheSameSchedule(schs[i]))
                    Context.Schedules.DeleteOnSubmit(schs.ToList()[i]);

            var ds = contractdoc.Responsibles.Select(d => d.Disposal).ToList();

            ds = ds.Where(d => (d.Contractdocs != null && d.Contractdocs.Any())).ToList();

            // зачищаем ответственных
            contractdoc.Responsibles.Clear();
            // зачищаем распоряжения по этому договору
            for (var i = ds.Count() - 1; i >= 0; i--)
                Context.Disposals.DeleteOnSubmit(ds[i]);

            // зачищаем процесс согласования
            contractdoc.Approvalprocesses.Clear();

            // зачищаем контракторов 
            contractdoc.Contractorcontractdocs.Clear();
            // зачищаем документы 
            contractdoc.Contractdocdocumentimages.Clear();
            contractdoc.ContractdocFundsFacts.Clear();

            // удаляем сам договор
            Context.Contractdocs.DeleteOnSubmit(contractdoc);
            this.DebugPrintRepository();

        }

        public IEnumerable<IContractStateData> DeleteContractdoc(Contractdoc contractdoc)
        {
            Contract.Assert(contractdoc != null, "Удаляемый договор должен быть задан");
            var deletedcontracts = new List<IContractStateData>();
            DeleteContractInternal(contractdoc, deletedcontracts);
            Context.SubmitChanges();
            return deletedcontracts;

        }

        public IEnumerable<IContractStateData> DeleteContract(double id)
        {
            var cdoc = Context.Contractdocs.FirstOrDefault(c => c.Id == id);
            if (cdoc != null) return DeleteContractdoc(cdoc);
            else return null;
        }

        public void DeleteContractdoc(Contractdoc contractdoc, IEnumerable<IContractStateData> deletedcontracts)
        {




        }

        public void DeleteContract(double id, IEnumerable<IContractStateData> deletedcontracts)
        {
            throw new NotImplementedException();
        }



        public IEnumerable<Contractrepositoryview> ActiveContractrepositoryviews
        {
            get
            {
                //contract.Brokeat > onDate || contract.Reallyfinishedat > onDate
                //var query =  Context.Contractrepositoryviews.Where(x => x.Deleted.GetValueOrDefault() == 0 
                //    && x.Agreementreferencecount == 0);
                var query = Context.Contractrepositoryviews;
                return query;
            }
        }

        public IEnumerable<Actrepositoryview> Actsrepositoryview
        {
            get
            {
                return Context.Actrepositoryviews;
            }
        }

        public List<Contracttranactdoc> Contracttrandocs
        {
            get { return Context.Contracttranactdocs.ToList(); }
        }

        public void DeleteAct(long id)
        {
            Context.Acts.DeleteOnSubmit(Context.Acts.Single(x => x.Id == id));
        }

        public IEnumerable<Act> Acts
        {
            get { return Context.Acts; }
        }

        public void UpdateStatistics(Contractdoc contractdoc)
        {
            contractdoc.UpdateFundsStatistics();
        }


        public void DeleteShedule(Schedule schedule)
        {
            Context.Schedules.DeleteOnSubmit(schedule);
        }

        public void DeleteStage(Stage stage)
        {
            Context.Stages.DeleteOnSubmit(stage);
        }

        public void InsertStage(Stage stage)
        {
            if (!String.IsNullOrEmpty(stage.Subject) && stage.Ndsalgorithm != null)
            {
                Context.Stages.InsertOnSubmit(stage);
            }
        }

        public void DeleteAllStages(Schedule schedule)
        {
            foreach (var stage in schedule.Stages)
            {
                foreach (var res in stage.Stageresults)
                {
                    Context.Stageresults.DeleteOnSubmit(res);
                }

                Context.Stages.DeleteOnSubmit(stage);
            }
        }




        public void InsertStages(IEnumerable<Stage> stages)
        {
            foreach (var stage in stages)
            {
                Context.Stages.InsertOnSubmit(stage);
                foreach (var res in stage.Stageresults)
                    Context.Stageresults.InsertOnSubmit(res);

            }
        }

        /// <summary>
        /// Удаляет все авансы заданного договора
        /// </summary>
        /// <param name="contractId">Идентификатор договора</param>
        public void DeletePrepayments(double contractId)
        {
            var contract = Context.Contractdocs.Single(x => x.Id == contractId);
            var prepayments = contract.Prepayments.ToList();
            Context.Prepayments.DeleteAllOnSubmit(prepayments);
        }

        /// <summary>
        /// Добавляет авансы по договору
        /// </summary>
        /// <param name="prepayments">Коллекция авансов</param>
        public void AddContractPrepayments(IEnumerable<Prepayment> prepayments)
        {
            Context.Prepayments.InsertAllOnSubmit(prepayments);
        }

        #region CatalogMembers

        public void DeleteContractType(Contracttype type)
        {
            Context.Contracttypes.DeleteOnSubmit(type);
        }

        public void InsertContractType(Contracttype type)
        {
            Context.Contracttypes.InsertOnSubmit(type);
        }

        public void DeleteContractState(Contractstate contractstate)
        {
            Context.Contractstates.DeleteOnSubmit(contractstate);
        }

        public void InsertContractState(Contractstate contractstate)
        {
            Context.Contractstates.InsertOnSubmit(contractstate);
        }

        public void InsertNds(Nds nds)
        {
            Context.Nds.InsertOnSubmit(nds);
        }

        public void DeleteNds(Nds nds)
        {
            Context.Nds.DeleteOnSubmit(nds);
        }

        public void InsertCurrency(Currency cur)
        {
            Context.Currencies.InsertOnSubmit(cur);
        }

        public void DeleteCurrency(Currency cur)
        {
            Context.Currencies.DeleteOnSubmit(cur);
        }

        public void InsertDegree(Degree deg)
        {
            Context.Degrees.InsertOnSubmit(deg);
        }

        public void DeleteDegree(Degree deg)
        {
            Context.Degrees.DeleteOnSubmit(deg);
        }

        public void InsertWorktype(Worktype worktype)
        {
            Context.Worktypes.InsertOnSubmit(worktype);
        }

        public void DeleteWorktype(Worktype worktype)
        {
            Context.Worktypes.DeleteOnSubmit(worktype);
        }


        public void InsertLocation(Location location)
        {
            Context.Locations.InsertOnSubmit(location);
        }

        public void DeleteLocation(Location location)
        {
            Context.Locations.DeleteOnSubmit(location);
        }

        public void InsertMissiveType(Missivetype missivetype)
        {
            Context.Missivetypes.InsertOnSubmit(missivetype);
        }

        public void DeleteMissiveType(Missivetype missivetype)
        {
            Context.Missivetypes.DeleteOnSubmit(missivetype);
        }

        public void InsertApprovalGoal(Approvalgoal approvalgoal)
        {
            Context.Approvalgoals.InsertOnSubmit(approvalgoal);
        }

        public void DeleteApprovalGoal(Approvalgoal approvalgoal)
        {
            Context.Approvalgoals.DeleteOnSubmit(approvalgoal);
        }

        public void InsertApprovalState(Approvalstate approvalstate)
        {
            Context.Approvalstates.InsertOnSubmit(approvalstate);
        }
        public void DeleteApprovalState(Approvalstate approvalstate)
        {
            Context.Approvalstates.DeleteOnSubmit(approvalstate);
        }


        public void DeleteObject(object obj)
        {
            if (obj is Trouble)
                DeleteTrouble(obj as Trouble);
        }

        public void InsertEconomefficiencytype(Economefficiencytype worktype)
        {
            Context.Economefficiencytypes.InsertOnSubmit(worktype);
        }

        public void DeleteEconomefficiencytype(Economefficiencytype worktype)
        {
            Context.Economefficiencytypes.DeleteOnSubmit(worktype);
        }

        public void InsertStateResult(Stageresult stageresult)
        {
            Context.Stageresults.InsertOnSubmit(stageresult);
        }

        public void DeleteStateResult(Stageresult stageresult)
        {
            Context.Stageresults.DeleteOnSubmit(stageresult);
        }

        public void InsertEconomefficiencyparameter(Economefficiencyparameter param)
        {
            Context.Economefficiencyparameters.InsertOnSubmit(param);
        }

        public void DeleteEconomefficiencyparameter(Economefficiencyparameter param)
        {
            Context.Economefficiencyparameters.DeleteOnSubmit(param);
        }

        public void InsertEfficienceparametertype(Efficienceparametertype param_type)
        {
            Context.Efficienceparametertypes.InsertOnSubmit(param_type);
        }

        public void DeleteEfficienceparametertype(Efficienceparametertype param_type)
        {
            Context.Efficienceparametertypes.DeleteOnSubmit(param_type);
        }

        public void InsertNtpview(Ntpview ntpview)
        {
            Context.Ntpviews.InsertOnSubmit(ntpview);
        }

        public void DeleteNtpview(Ntpview ntpview)
        {
            Context.Ntpviews.DeleteOnSubmit(ntpview);
        }

        public void InsertNtpsubview(Ntpsubview ntpview)
        {
            Context.Ntpsubviews.InsertOnSubmit(ntpview);
        }

        public void DeleteNtpsubview(Ntpsubview ntpview)
        {
            Context.Ntpsubviews.DeleteOnSubmit(ntpview);
        }

        public void InsertContractortype(Contractortype contracttype)
        {
            Context.Contractortypes.InsertOnSubmit(contracttype);
        }

        public void DeleteContractortype(Contractortype contracttype)
        {
            Context.Contractortypes.DeleteOnSubmit(contracttype);
        }

        public void InsertContractor(Contractor contractor)
        {
            Context.Contractors.InsertOnSubmit(contractor);
        }

        public void DeleteContractor(Contractor contractor)
        {
            Context.Contractors.DeleteOnSubmit(contractor);
        }




        public void InsertPosition(Position position)
        {
            Context.Positions.InsertOnSubmit(position);
        }

        public void DeletePosition(Position position)
        {
            Context.Positions.DeleteOnSubmit(position);
        }

        public void InsertProperty(Property property)
        {
            Context.Properties.InsertOnSubmit(property);
        }

        public void DeleteProperty(Property property)
        {
            Context.Properties.DeleteOnSubmit(property);
        }

        public void InsertTrouble(Trouble trouble)
        {
            Context.Troubles.InsertOnSubmit(trouble);
        }

        public void DeleteTrouble(Trouble trouble)
        {
            Context.Troubles.DeleteOnSubmit(trouble);
        }

        public void InsertNdsalgorithm(Ndsalgorithm ndsalgorithm)
        {
            Context.Ndsalgorithms.InsertOnSubmit(ndsalgorithm);
        }

        public void DeleteNdsalgorithm(Ndsalgorithm ndsalgorithm)
        {
            Context.Ndsalgorithms.DeleteOnSubmit(ndsalgorithm);
        }

        public void InsertTroublesregistry(Troublesregistry troublesregistry)
        {
            Context.Troublesregistries.InsertOnSubmit(troublesregistry);
        }

        public void DeleteTroublesregistry(Troublesregistry troublesregistry)
        {
            Context.Troublesregistries.DeleteOnSubmit(troublesregistry);
        }

        public void InsertCurrencymeasure(Currencymeasure currencymeasure)
        {
            Context.Currencymeasures.InsertOnSubmit(currencymeasure);
        }

        public void DeleteCurrencymeasure(Currencymeasure currencymeasure)
        {
            Context.Currencymeasures.DeleteOnSubmit(currencymeasure);
        }

        public void InsertRegion(Region region)
        {
            Context.Regions.InsertOnSubmit(region);
        }

        public void DeleteRegion(Region region)
        {
            Context.Regions.DeleteOnSubmit(region);
        }

        public void InsertRole(Role role)
        {
            Context.Roles.InsertOnSubmit(role);
        }

        public void DeleteRole(Role role)
        {
            Context.Roles.DeleteOnSubmit(role);
        }

        public void InsertAuthority(Authority authority)
        {
            Context.Authorities.InsertOnSubmit(authority);
        }

        public void DeleteAuthority(Authority authority)
        {
            Context.Authorities.DeleteOnSubmit(authority);
        }

        public void InsertActtype(Acttype acttype)
        {
            Context.Acttypes.InsertOnSubmit(acttype);
        }

        public void DeleteActtype(Acttype acttype)
        {
            Context.Acttypes.DeleteOnSubmit(acttype);
        }

        public void InsertContractorposition(Contractorposition contractorposition)
        {
            Context.Contractorpositions.InsertOnSubmit(contractorposition);
        }

        public void DeleteContractorposition(Contractorposition contractorposition)
        {
            Context.Contractorpositions.DeleteOnSubmit(contractorposition);
        }

        public void InsertPrepaymentdocumenttype(Prepaymentdocumenttype region)
        {
            Context.Prepaymentdocumenttypes.InsertOnSubmit(region);
        }

        public void DeletePrepaymentdocumenttype(Prepaymentdocumenttype region)
        {
            Context.Prepaymentdocumenttypes.DeleteOnSubmit(region);
        }

        public void InsertPerson(Person person)
        {
            Context.People.InsertOnSubmit(person);
        }

        public void DeletePerson(Person person)
        {
            Context.People.DeleteOnSubmit(person);
        }

        public void InsertEmployee(Employee employee)
        {
            Context.Employees.InsertOnSubmit(employee);
        }

        public void DeleteEmployee(Employee employee)
        {
            Context.Employees.DeleteOnSubmit(employee);
        }

        public void DeleteEfparamStageresult(Efparameterstageresult efparameterstageresult)
        {
            Context.Efparameterstageresults.DeleteOnSubmit(efparameterstageresult);
        }

        public void InsertFunctionalcustomer(Functionalcustomer functionalcustomer)
        {
            Context.Functionalcustomers.InsertOnSubmit(functionalcustomer);
        }

        public void DeleteFunctionalcustomer(Functionalcustomer functionalcustomer)
        {
            Context.Functionalcustomers.DeleteOnSubmit(functionalcustomer);
        }

        public void InsertFunctionalcustomertype(Functionalcustomertype functionalcustomertype)
        {
            Context.Functionalcustomertypes.InsertOnSubmit(functionalcustomertype);
        }

        public void DeleteFunctionalcustomertype(Functionalcustomertype functionalcustomertype)
        {
            Context.Functionalcustomertypes.DeleteOnSubmit(functionalcustomertype);
        }

        public void InsertDocument(Document document)
        {
            Context.Documents.InsertOnSubmit(document);
        }

        public void DeleteDocument(Document document)
        {
            Context.Documents.DeleteOnSubmit(document);
        }

        public void InsertTransferacttype(Transferacttype transferacttype)
        {
            Context.Transferacttypes.InsertOnSubmit(transferacttype);
        }

        public void DeleteTransferacttype(Transferacttype transferacttype)
        {
            Context.Transferacttypes.DeleteOnSubmit(transferacttype);
        }

        public void InsertTransferacttypedocument(Transferacttypedocument transferacttypedocument)
        {
            Context.Transferacttypedocuments.InsertOnSubmit(transferacttypedocument);
        }

        public void DeleteTransferacttypedocument(Transferacttypedocument transferacttypedocument)
        {
            Context.Transferacttypedocuments.DeleteOnSubmit(transferacttypedocument);
        }

        public void InsertFunccustomerperson(Funccustomerperson funccustomerperson)
        {
            Context.Funccustomerpersons.InsertOnSubmit(funccustomerperson);
        }

        public void DeleteFunccustomerperson(Funccustomerperson funccustomerperson)
        {
            Context.Funccustomerpersons.DeleteOnSubmit(funccustomerperson);
        }

        public void InsertResponsiblefororder(Responsiblefororder responsiblefororder)
        {
            Context.Responsiblefororders.InsertOnSubmit(responsiblefororder);
        }

        public void DeleteResponsiblefororder(Responsiblefororder responsiblefororder)
        {
            Context.Responsiblefororders.DeleteOnSubmit(responsiblefororder);
        }

        public void InsertResponsibleassignmentorder(Responsibleassignmentorder responsibleassignmentorder)
        {
            Context.Responsibleassignmentorders.InsertOnSubmit(responsibleassignmentorder);
        }

        public void DeleteResponsibleassignmentorder(Responsibleassignmentorder responsibleassignmentorder)
        {
            Context.Responsibleassignmentorders.DeleteOnSubmit(responsibleassignmentorder);
        }


        public void InsertContractorpropertiy(Contractorpropertiy contractorpropertiy)
        {
            Context.Contractorpropertiys.InsertOnSubmit(contractorpropertiy);
        }

        public void DeleteContractorpropertiy(Contractorpropertiy contractorpropertiy)
        {
            Context.Contractorpropertiys.DeleteOnSubmit(contractorpropertiy);
        }

        public void InsertSightfuncpersonscheme(Sightfuncpersonscheme sightfuncpersonscheme)
        {
            Context.Sightfuncpersonschemes.DeleteOnSubmit(sightfuncpersonscheme);
        }

        public void DeleteSightfuncpersonscheme(Sightfuncpersonscheme sightfuncpersonscheme)
        {
            Context.Sightfuncpersonschemes.InsertOnSubmit(sightfuncpersonscheme);
        }

        public void DeletePaymentdocument(Actpaymentdocument actpaymentdocument)
        {
            Context.Actpaymentdocuments.DeleteOnSubmit(actpaymentdocument);
        }

        public void DeleteDisposal(Disposal disposal)
        {
            Context.Disposals.DeleteOnSubmit(disposal);
        }

        public void InsertDisposal(Disposal disposal)
        {
            Context.Disposals.InsertOnSubmit(disposal);
        }

        #endregion

        #region Implementation of IDisposable

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <filterpriority>2</filterpriority>
        public void Dispose()
        {
            Context.Do(x => x.Dispose());
        }

        #endregion


        public event EventHandler<ContextEventArgs<McDataContext>> ContextCreated;

        public void InsertYearreportcolor(Yearreportcolor yearreportcolor)
        {
            Context.Yearreportcolors.InsertOnSubmit(yearreportcolor);
        }

        public void DeleteYearreportcolor(Yearreportcolor yearreportcolor)
        {
            Context.Yearreportcolors.DeleteOnSubmit(yearreportcolor);
        }

    }
}
