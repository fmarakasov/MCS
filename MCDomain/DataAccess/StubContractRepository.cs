using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Devart.Data.Linq;
using MCDomain.Model;
using System.Data.Linq;

namespace MCDomain.DataAccess
{
    public class StubContractRepository : IContractRepository, IDisposable
    {
        private List<Functionalcustomertype> _funccustomertypes;
        private List<Functionalcustomer> _funccustomers;

        public void DeleteObject(object obj)
        {
            return;
        }


        private readonly List<Troublesregistry> _troublesregistry = new List<Troublesregistry>()
                                                                        {
                                                                            new Troublesregistry()
                                                                                {
                                                                                    Id = 1,
                                                                                    Name =
                                                                                        "Перечень приоритетных научно-технических проблем ОАО \"Газпром\" на 2006-2010 годы",
                                                                                    Approvedat = DateTime.Today,
                                                                                    Ordernum = "100-01",
                                                                                    Validfrom = DateTime.Today,
                                                                                    Validto = DateTime.Today.AddMonths(5)
                                                                                },
                                                                                  new Troublesregistry()
                                                                                {
                                                                                    Id = 2,
                                                                                    Name =
                                                                                        "Перечень приоритетных научно-технических проблем ОАО \"Газпром\" на 2002-2006 годы",
                                                                                    Approvedat = DateTime.Today,
                                                                                    Validfrom = DateTime.Parse("01/01/2002"),
                                                                                    Validto = DateTime.Parse("31/12/2006"),
                                                                                }
                                                                        };









        private List<Disposal> _disposals;

        private readonly List<Authority> _authority = new List<Authority>()
                                                          {
                                                              new Authority() {Id = 1, Name = "Доверенность"},
                                                              new Authority() {Id = 2, Name = "Устав"}
                                                          };


        private List<Enterpriseauthority> _enterpriseauthorities;
        private List<Employee> _employees;

        private readonly List<Role> _roles = new List<Role>()
                                                 {
                                                     new Role() {Id = 1, Name = "Руководитель направления"},
                                                     new Role() {Id = 2, Name = "Ответственный исполнитель"},
                                                     new Role() {Id = 3, Name = "куратор от договорного отдела"}
                                                 };


        private readonly List<Contracttype> _contracttypes = new List<Contracttype>
                                                                 {
                                                                     new Contracttype {Id = 1, Name = "НИОКР"},
                                                                     new Contracttype {Id = 2, Name = "ПИР"},
                                                                     new Contracttype {Id = 3, Name = "Газификация"},
                                                                     new Contracttype {Id = 4, Name = "Производство"}
                                                                 };

        private readonly List<Currency> _currencies = new List<Currency>
                                                          {
                                                              new Currency
                                                                  {
                                                                      Id = 1,
                                                                      Culture = "ru-ru",
                                                                      Name = "Рубли",
                                                                      Code = "RUB",
                                                                      
                                                                  },
                                                              new Currency
                                                                  {
                                                                      Id = 2,
                                                                      Culture = "en-us",
                                                                      Name = "Доллары",
                                                                      Code = "USD"
                                                                  },
                                                              new Currency
                                                                  {
                                                                      Id = 3,
                                                                      Culture = "de-de",
                                                                      Name = "Евро",
                                                                      Code = "EUR"
                                                                  }
                                                          };

        private readonly List<Currencymeasure> _currencymeasures = new List<Currencymeasure>
                                                                       {
                                                                           new Currencymeasure
                                                                               {Id = 1, Name = "ед.", Factor = 1},
                                                                           new Currencymeasure
                                                                               {Id = 2, Name = "тыс.", Factor = 1000},
                                                                           new Currencymeasure
                                                                               {Id = 1, Name = "млн.", Factor = 1000000}
                                                                       };

        private readonly List<Nds> _nds = new List<Nds> { new Nds { Id = 1, Percents = 18, Year = 2004 } };

        private readonly List<Ndsalgorithm> _ndsalgorithms = new List<Ndsalgorithm>
                                                                 {
                                                                     new Ndsalgorithm
                                                                         {
                                                                             Id = (int) TypeOfNds.IncludeNds,
                                                                             Name = "Включая НДС"
                                                                         },
                                                                     new Ndsalgorithm
                                                                         {
                                                                             Id = (int) TypeOfNds.ExcludeNds,
                                                                             Name = "Кроме того НДС"
                                                                         },
                                                                     new Ndsalgorithm
                                                                         {
                                                                             Id = (int) TypeOfNds.NoNds,
                                                                             Name = "НДС не облагается"
                                                                         }
                                                                 };

        private readonly List<Contractstate> _states = new List<Contractstate>
                                                           {
                                                               new Contractstate {Id = 1, Name = "Не подписан"},
                                                               new Contractstate {Id = 2, Name = "Подписан"}
                                                           };

        private IList<Contractdoc> _contracts;

        //private readonly List<Position> _positions = new List<Position>()
        //                                                 {
        //                                                     new Position() {Name = "Директор"},
        //                                                     new Position() {Name = "Генеральный директор"},
        //                                                     new Position() {Name = "Проректор"}
        //                                                 };

        private readonly List<Contractor> _contractors = new List<Contractor>()
                                                             {
                                                                 new Contractor
                                                                     {
                                                                         Id = 1,
                                                                         Name =
                                                                             "Ухтинский государственный технический университет",
                                                                         Shortname = "УГТУ",
                                                                         Contractortype = new Contractortype(){Name = "Другие организации"},
                                                                         
                                                                         Zip = "169300",
                                                                         
                                                                         Inn= "1022-122-1-2-21-1",
                                                                         Bank = "Сберегательный банк России",
                                                                         Bik = "101020303030303",
                                                                         Kpp = "2132333"                                                                                                                                                 
                                                                     },
                                                                 new Contractor
                                                                     {
                                                                         Id = 1,
                                                                         Name =
                                                                             "Институт нефти и газа",
                                                                         Shortname = "ИНиГ",
                                                                         Contractortype = new Contractortype(){Name="Дочерние организации"}
                                                                     },
                                                                 new Contractor
                                                                     {
                                                                         Id = 2,
                                                                         Name = "ОАО \"Газпром\"",
                                                                         Shortname = "ВСП",
                                                                         Contractortype = new Contractortype(){Name = "Газпром"}
                                                                     }
                                                             };


        #region IContractRepository Members

        public StubContractRepository()
        {
            _contracts = CreateContracts();
        }

        public IList<Contractdoc> Contracts
        {
            get
            {
                if (_contracts == null) _contracts = CreateContracts();
                return _contracts;
            }
        }

        public Contractdoc NewContractdoc()
        {
            Contractdoc result = new Contractdoc()
            {
                Active = true,
                Subject = "Предмет договора",
                Currency = _currencies[0],
                Currencymeasure = _currencymeasures[0],
                Nds = _nds[0]
            };
            return result;
        }

        public Stage NewStage()
        {
            return new Stage();
        }

        public Contractdoc GetContractdoc(double contractId)
        {
            return Contracts.SingleOrDefault(x => x.Id == contractId);
        }

        public IList<Contractstate> States
        {
            get { return _states; }
        }

        public IList<Ndsalgorithm> Ndsalgorithms
        {
            get { return _ndsalgorithms; }
        }

        public IList<Nds> Nds
        {
            get { return _nds; }
        }

        public IList<Currency> Currencies
        {
            get { return _currencies; }
        }

        public IList<Contracttype> Contracttypes
        {
            get { return _contracttypes; }
        }

        public IList<Currencymeasure> Currencymeasures
        {
            get { return _currencymeasures; }
        }

        public IList<Contractor> Contractors
        {
            get { return _contractors; }
        }

        public IList<Functionalcustomer> Functionalcustomers
        {
            get { return _funccustomers; }
        }

        public IList<Disposal> Disposals
        {
            get { return _disposals; }
        }

        public IList<Enterpriseauthority> Enterpriseauthorities
        {
            get { return _enterpriseauthorities; }
        }

        public IList<Troublesregistry> TroublesRegistry
        {
            get { return _troublesregistry; }
        }

        public IList<Trouble> Troubles
        {
            get
            {
                Trouble tr1 = new Trouble()
                                          {
                                              Id = 1,
                                              Name = "Большая проблема реестра 1",
                                              Num = "7567567",
                                              Troubleregistryid = 1,
                                          };

                Trouble tr2 = new Trouble()
                                          {
                                              Id = 2,
                                              Name = "Маленькая проблема реестра 1",
                                              Num = "64656",
                                              Troubleregistryid = 1
                                          };

                Trouble tr3 = new Trouble()
                                           {
                                               Id = 3,
                                               Name = "Большая проблема реестра 2",
                                               Num = "675677",
                                               Troubleregistryid = 2
                                           };

                Trouble tr4 = new Trouble()
                                          {
                                              Id = 4,
                                              Name = "Маленькая проблема реестра 2",
                                              Num = "768678",
                                              Troubleregistryid = 2
                                          };
                Trouble tr5 = new Trouble()
                                            {
                                                Id = 5,
                                                Num = "123123",
                                                Name = "Маленькая подпроблема реестра 2",
                                            };

                Trouble tr6 = new Trouble()
                                            {
                                                Id = 6,
                                                Num = "45675675",
                                                Name = "Маленькая подпроблема реестра 2",
                                            };
                tr1.Troubles.Add(tr5);
                tr3.Troubles.Add(tr6);

                List<Trouble> list = new List<Trouble>();
                list.Add(tr1);
                list.Add(tr2);
                list.Add(tr3);
                list.Add(tr4);
                list.Add(tr5);
                list.Add(tr6);
                return list;
            }
        }

        public IList<Worktype> Worktypes
        {
            get
            {
                return new List<Worktype>()
                           {
                               new Worktype()
                                   {
                                       Id = 1,
                                       Name = "Важный вид работы"
                                   },
                               new Worktype()
                                   {
                                       Id = 1,
                                       Name = "Неважный вид работы"
                                   }
                           };
            }
        }

        public IList<Importingscheme> Importingschemes
        {
            get
            {
                return new List<Importingscheme>
                    {
                        new Importingscheme()
                        {
                            Id = 1,
                            Schemename = "Тестовая схема"
                        }
                    };
            }
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

        public IList<Transferactactdocument> Transferactactdocuments
        {
            get { throw new NotImplementedException(); }
        }

        private bool _isModified = false;
        public void SubmitChanges()
        {
            _isModified = false;
        }

        public void RejectChanges()
        {
            _isModified = false;
        }

        public void Refresh(RefreshMode mode, object entity)
        {
        }


        public void Refresh(RefreshMode mode, IEnumerable<object> entities)
        {
        }
        
        public bool IsModified
        {
            get { return _isModified; }
        }

        public void AddContract(Contractdoc contractdoc)
        {
            _contracts.Add(contractdoc);
            _isModified = true;
        }



        public void DeleteTroublecontract(Contracttrouble entity)
        {

        }

        public void InsertTroublecontract(Contracttrouble entity)
        {

        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
        }

        #endregion

        public IList<Contractdoc> CreateContracts()
        {
            var c1 = _contractors[0];

            var c2 = _contractors[1];

            var c3 = _contractors[2];

            var position1 = new Position { Name = "Заведующий кафедрой ТТТ" };
            var position2 = new Position { Name = "Заместитель директора" };
            var position3 = new Position { Name = "Заведующий сектором ПВО" };
            var position4 = new Position { Name = "Начальник отдела наблюдения" };
            var position5 = new Position { Name = "Вице президент по морально-этическим вопросам" };

            var cp1 = new Contractorposition { Contractor = c1, Position = position1 };
            var cp2 = new Contractorposition { Contractor = c2, Position = position2 };
            var cp3 = new Contractorposition { Contractor = c3, Position = position3 };
            var cp4 = new Contractorposition { Contractor = c3, Position = position4 };
            var cp5 = new Contractorposition { Contractor = c3, Position = position5 };

            var person1 = new Person
            {
                Id = 3,
                
                
                    Familyname = "Гизаулин",
                    Firstname = "Артём",
                    Middlename = "Борисович"
                ,
                Degree = new Degree { Id = 1, Name = "д.т.н." },
                Contractorposition = cp1,
                Valid = true,
                Actsignauthority = false,
                Contractheadauthority = true
            };

            var person2 = new Person
            {
                Id = 3,
                    Familyname = "Бергман",
                    Firstname = "Станислав",
                    Middlename = "Валентинович"
                ,
                Degree = new Degree { Id = 1, Name = "без степени" },
                Contractorposition = cp2,
                Valid = true,
                Actsignauthority = false,
                Contractheadauthority = false
            };
            var p3 = new Person
            {
                Id = 1,
                
                    Familyname = "Иванов",
                    Firstname = "Иван",
                    Middlename = "Иванович"
                ,
                Degree = new Degree { Id = 1, Name = "д.т.н." },
                Contractorposition = cp3,
                Valid = true,
                Actsignauthority = true,
                Contractheadauthority = true
            };
            var p4 = new Person
            {
                Id = 1,
                
                    Familyname = "Гюнтер",
                    Firstname = "Алес",
                    Middlename = "Иванович"
                ,
                Degree = new Degree { Id = 2, Name = "к.ф.м.н." },
                Contractorposition = cp4,
                Valid = true,
                Actsignauthority = true,
                Contractheadauthority = false
            };
            var p5 = new Person
            {
                Id = 2,
                
                    Familyname = "Петрова",
                    Firstname = "Ольга",
                    Middlename = "Сергеевна"
                ,
                Contractorposition = cp5,
                Valid = false,
                Actsignauthority = false,
                Contractheadauthority = false
            };




            #region Subject initialize
            //Subject s1 = new Subject()
            //                 {
            //                     Id = 1,
            //                     Name =
            //                         "Нет такой темы договора, котороя бы не влезла в отведённое под заголовок место. Кроме этого заголовка, возможно. Хотя все может быть."
            //                 };

            //Subject s2 = new Subject()
            //                 {
            //                     Id = 2,
            //                     Name =
            //                         "Разработка основы основ нефте-газо-лесо добывающего обеспечения для всех дочерних обществ ОАО Газпром"
            //                 };


            //Subject s3 = new Subject()
            //                 {
            //                     Id = 3,
            //                     Name =
            //                         "Разработка комплекса научно-технической документации на супер-пупер изделие из капрона"
            //                 };


            //Subject s4 = new Subject()
            //                 {
            //                     Id = 3,
            //                     Name =
            //                         "Проектированме научно-технической документации XXX"
            //                 };


            #endregion
            Contracttype niokr = _contracttypes[0];
            Contracttype pir = _contracttypes[1];
            Contracttype gaz = _contracttypes[2];

            _funccustomertypes = new List<Functionalcustomertype>()
                                                                               {
                                                                                   new Functionalcustomertype()
                                                                                       {
                                                                                           Name =
                                                                                               "Первый тип функционального заказчика"
                                                                                       },
                                                                                   new Functionalcustomertype()
                                                                                       {
                                                                                           Name =
                                                                                               "Второй тип функционального заказчика"
                                                                                       }
                                                                               };
            _funccustomers = new List<Functionalcustomer>()
                                 {
                                     new Functionalcustomer()
                                         {
                                             Contractor = c3,
                                             Functionalcustomertype = _funccustomertypes[0],
                                             Name = "Отдел нагруженного погружения"
                                         },
                                     new Functionalcustomer()
                                         {
                                             Contractor = c3,
                                             Functionalcustomertype = _funccustomertypes[1],
                                             Name = "Отдел безответственного обслуживания"
                                         },
                                     new Functionalcustomer()
                                         {
                                             Contractor = c3,
                                             Functionalcustomertype = _funccustomertypes[1],
                                             Name = "Директорат директора"
                                         }
                                 };
            _funccustomers[0].Functionalcustomer1 = _funccustomers[2];
            _funccustomers[1].Functionalcustomer1 = _funccustomers[2];



            _employees = new List<Employee>()
                             {
                                 new Employee()
                                     {
                                         Id = 1,
                                         //Role = _roles[1],
                                                     Firstname = "Леонид",
                                                     Familyname = "Лосев",
                                                     Middlename = "Львович",
                                                     Sex = true
                                         
                                     },
                                 new Employee()
                                     {
                                         Id = 1,
                                         //Role = _roles[2],                                         
                                       
                                                     Firstname = "Надежда",
                                                     Familyname = "Николаева",
                                                     Middlename = "Александровна",
                                                     Sex = false
                                       
                                     }
                             };

            //_employees[1].Employee1 = _employees[0];

            _enterpriseauthorities = new List<Enterpriseauthority>()
                                         {
                                             new Enterpriseauthority()
                                                 {
                                                     Id = 1,
                                                     Authority = _authority[0],
                                                     Num = "10-2010-1",
                                                     Validfrom = DateTime.Today.AddDays(-100),
                                                     Validto = DateTime.Today.AddDays(30),
                                                     Valid = true,
                                                     Employee = _employees[0]
                                                 },
                                                   new Enterpriseauthority()
                                                 {
                                                     Id = 2,
                                                     Authority = _authority[0],
                                                     Num = "20-2009-11",
                                                     Validfrom = DateTime.Today.AddDays(-200),
                                                     Validto = DateTime.Today.AddDays(100),
                                                     Valid = true,
                                                     Employee = _employees[1]
                                                 },
                                                 new Enterpriseauthority()
                                                 {
                                                     Id = 3,
                                                     Authority = _authority[1],
                                                     Num = "223-2008-11",
                                                     Validfrom = DateTime.Today.AddDays(-200),
                                                     Validto = DateTime.Today.AddDays(-100),
                                                     Valid = false,
                                                     Employee = _employees[1]
                                                 }

                                         };



            var contract1 = new Contractdoc
                                {
                                    Id = 1,
                                    Subject =
                                        "Нет такой темы договора, котороя бы не влезла в отведённое под заголовок место. Кроме этого заголовка, возможно. Хотя все может быть.",
                                    Appliedat = DateTime.Today,
                                    Approvedat = DateTime.Today,
                                    Endsat = DateTime.Today,
                                    Internalnum = "12-090-22",
                                    Startat = DateTime.Today,
                                    Price = 1801000.75,
                                    Currency = _currencies[0],
                                    Ndsalgorithm = _ndsalgorithms[(int)TypeOfNds.ExcludeNds - 1],
                                    Nds = _nds[0],
                                    Protectability = true,
                                    Currencymeasure = _currencymeasures[0],
                                    Person = p3,
                                    Contracttype = niokr,
                                    Contractor = c1,
                                    ConditionResolver =
                                        new StubContractConditionResolver(ContractCondition.NormalActive),
                                    FundsResolver = new StubFundsResolver(1000, 1800000.75),
                                    Contractstate = _states[0],
                                    Enterpriseauthority = _enterpriseauthorities[0]
                                };

            var contract2 = new Contractdoc
            {
                Id = 2,
                Subject =
                    "Разработка основы основ нефте-газо-лесо добывающего обеспечения для всех дочерних обществ ОАО Газпром",
                Appliedat = DateTime.Today.AddDays(1),
                Approvedat = DateTime.Today,
                Endsat = DateTime.Today,
                Internalnum = "12-0-90-22",
                Startat = DateTime.Today,
                Price = 1900.60,
                Protectability = true,
                Currencymeasure = _currencymeasures[1],
                Currency = _currencies[2],
                Currencyrate = 30,
                Ratedate = DateTime.Today,
                Ndsalgorithm = _ndsalgorithms[(int)TypeOfNds.NoNds - 1],
                Person = p4,
                Contracttype = niokr,
                Contractor = c2,
                ConditionResolver =
                    new StubContractConditionResolver(ContractCondition.Ended),
                FundsResolver = new StubFundsResolver(1900.60, 0),
                Contractstate = _states[1],
                Enterpriseauthority = _enterpriseauthorities[1]
            };

            var contract3 = new Contractdoc
            {
                Id = 3,
                Subject =
                    "Разработка комплекса научно-технической документации на супер-пупер изделие из капрона",
                Appliedat = DateTime.Today.AddDays(2),
                Approvedat = DateTime.Today.AddDays(2),
                Endsat = DateTime.Today.AddDays(100),
                Internalnum = "12-090-22-33",
                Startat = DateTime.Today,
                Price = 13100.00,
                Currency = _currencies[2],
                Currencyrate = 30,
                Ratedate = DateTime.Today,
                Currencymeasure = _currencymeasures[1],
                Ndsalgorithm = _ndsalgorithms[(int)TypeOfNds.IncludeNds - 1],
                Nds = _nds[0],
                Person = p5,
                Contracttype = pir,
                Contractor = c3,
                ConditionResolver =
                    new StubContractConditionResolver(ContractCondition.TroubledActive),
                FundsResolver = new StubFundsResolver(3000, 10100),
                Contractstate = _states[0],
                Enterpriseauthority = _enterpriseauthorities[0]
            };

            var contract4 = new Contractdoc
            {
                Id = 4,
                Subject = "Проектированме научно-технической документации XXX",
                Appliedat = DateTime.Today.AddDays(-100),
                Approvedat = DateTime.Today.AddDays(-90),
                Endsat = DateTime.Today.AddDays(180),
                Internalnum = "45-22-33",
                Startat = DateTime.Today,
                Price = 1309900.00,
                Currencymeasure = _currencymeasures[0],
                Currency = _currencies[1],
                Currencyrate = 41,
                Ratedate = DateTime.Today,
                Ndsalgorithm = _ndsalgorithms[(int)TypeOfNds.NoNds - 1],
                Person = p5,
                Contracttype = gaz,
                Contractor = c3,
                ConditionResolver =
                    new StubContractConditionResolver(ContractCondition.TransparentActive),
                FundsResolver = new StubFundsResolver(4000, 1305900),
                Contractstate = _states[0],
                Enterpriseauthority = _enterpriseauthorities[0]
            };

            var contract10 = new Contractdoc
            {
                Id = 10,
                Subject =
                    "Тема ух-х-х-х-хххххххххххххххххххххххххххххххххххххххххххххххххххххххххххххххххххххххххххххххххххххххххххххххххххххххххххххххххххххххххххххххххххххххххххххххххххххххххххххххххххххххххххххххххххххххххххххххххххххххххххххххххххххххххххххххххххххххххххххххххххххххх-х-х",
                Appliedat = DateTime.Today.AddDays(-100),
                Approvedat = DateTime.Today.AddDays(-110),
                Endsat = DateTime.Today.AddDays(50),
                Internalnum = "999-90-90889",
                Startat = DateTime.Today.AddDays(-109),
                Price = 40000,
                Currency = _currencies[1],
                Ndsalgorithm = _ndsalgorithms[(int)TypeOfNds.IncludeNds - 1
                ],
                Nds = _nds[0],
                Protectability = true,
                Currencymeasure = _currencymeasures[1],
                Person = p4,
                Contracttype = _contracttypes[3],
                Contractor = c1,
                Currencyrate = 41,
                Ratedate = DateTime.Today.AddDays(-110),
                ConditionResolver = new StubContractConditionResolver(ContractCondition.NormalActive),
                FundsResolver = new StubFundsResolver(10000, 30000),
                Contractstate = _states[1],
                Prepaymentsum = 1000,
                Prepaymentpercent = 2.5M,
                Enterpriseauthority = _enterpriseauthorities[1]
            };

            var agreement1 = (Contractdoc)contract1.Clone();
            agreement1.Id = 5;

            var agreement2 = (Contractdoc)agreement1.Clone();
            agreement1.Id = 7;

            var contract5 = (Contractdoc)contract1.Clone();
            contract5.Id = 6;
            //contract5.Contracthierarchies.Add(contract4);


            agreement1.ConditionResolver = new StubContractConditionResolver(ContractCondition.NormalActive);

            contract1.Agreements.Add(agreement1);

            agreement1.Agreements.Add(agreement2);

            //contract2.RegisterAgreement(agreement2);






            //contract1.SubContracts.Add(contract2);    
            //contract1.SubContracts.Add(contract3);            
            //contract1.SubContracts.Add(contract4);


            return new List<Contractdoc>
                       {
                           contract1,
                           contract2,
                           contract3,
                           agreement1,
                           contract4,
                           contract5,
                           contract10
                       };


        }

        public IEnumerable<Stage> GetStages(double scheduleId)
        {
            return new List<Stage>
                       {
                           new Stage()
                               {
                                   Num = "1",
                                   Subject = "Этап 1"
                               }
                       };
        }



        public void InsertShedule(Schedulecontract schedule)
        {

        }

        public void DeleteShedule(Schedulecontract schedule)
        {

        }

        public void DeleteShedule(Schedule schedule)
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

        public void DeleteStage(Stage stage)
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

        public void InsertDepartment(Department department)
        {
            throw new NotImplementedException();
        }

        public void DeleteDepartment(Department department)
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

        public void InsertContracttype(Contracttype contracttype)
        {
            throw new NotImplementedException();
        }

        public void DeleteContracttype(Contracttype contracttype)
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

        public void InsertResponsiblefororder(Responsiblefororder responsiblefororder)
        {
            throw new NotImplementedException();
        }

        public void DeleteResponsiblefororder(Responsiblefororder responsiblefororder)
        {
            throw new NotImplementedException();
        }

        public IList<Responsiblefororder> Responsiblefororders { get { throw new NotImplementedException(); } }

        public IList<Location> Locations { get { throw new NotImplementedException(); } }
        public IList<Missivetype> MissiveTypes { get { throw new NotImplementedException(); } }
        public IList<Approvalgoal> ApprovalGoals { get { throw new NotImplementedException(); } }
        public IList<Approvalstate> ApprovalStates { get { throw new NotImplementedException(); } }
        public IList<Department> Departments { get { throw new NotImplementedException(); } }
}
}