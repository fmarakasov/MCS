using System;
using System.Linq;
using Devart.Data.Linq;
using MCDomain.Common;
using MCDomain.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MTests
{
    /// <summary>
    /// Summary description for DbSequenceTests
    /// </summary>
    [TestClass]
    public class DbSequenceTests
    {
        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext { get; set; }

        #region Additional test attributes

        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //

        #endregion

        private static void TestObjectIdInsert(McDataContext context, IObjectId namedObj, ITable table)
        {
            const int minId = 10000;
            table.InsertOnSubmit(namedObj);
            namedObj.Id = -1;
            double oldId = namedObj.Id;
            context.SubmitChanges();
            double newId = namedObj.Id;
            table.DeleteOnSubmit(namedObj);
            context.SubmitChanges();
            Assert.AreNotEqual(newId, oldId);
            Assert.IsTrue(newId >= minId);
        }

        [TestMethod]
        [TestCategory("Database")]
        public void TestInsertEmployee()
        {
            using (var ctx = new McDataContext())
            {
                var role = new Role { Name = "Тестовая" };
                ctx.Roles.InsertOnSubmit(role);
                var emp = new Employee
                              {
                                  
                                              Firstname = "Тестовый",
                                              Familyname = "Тестовая",
                                              Middlename = "Тестович",
                                              Sex = true
                                  
                                  //Role = role
                              };
                try
                {
                    TestObjectIdInsert(ctx, emp, ctx.Employees);
                }
                finally
                {
                    ctx.Roles.DeleteOnSubmit(role);
                    ctx.SubmitChanges();
                }
            }
        }

        private Person InsertTestPerson(McDataContext ctx, out Degree degree, out Contractortype contractortype,
                                        out Contractor contractor, out Contractorposition contractorposition,
                                        out Position position)
        {
            degree = new Degree { Name = "Тестовая" };
            ctx.Degrees.InsertOnSubmit(degree);

            contractortype = new Contractortype { Name = "Тестовый" };
            ctx.Contractortypes.InsertOnSubmit(contractortype);

            contractor = new Contractor { Name = "Тестовый", Contractortype = contractortype };
            ctx.Contractors.InsertOnSubmit(contractor);

            position = new Position { Name = "Тестовая" };
            ctx.Positions.InsertOnSubmit(position);

            contractorposition = new Contractorposition { Position = position, Contractor = contractor };
            ctx.Contractorpositions.InsertOnSubmit(contractorposition);

            var person = new Person
                             {
                                
                                             Firstname = "Тестовый",
                                             Familyname = "Тестовая",
                                             Middlename = "Тестович",
                                             Sex = true
                                         ,
                                 Contractorposition = contractorposition,
                                 Actsignauthority = true,
                                 Contractheadauthority = true,
                                 Valid = true,
                                 Degree = degree
                             };
            return person;
        }

        [TestMethod]
        [TestCategory("Database")]
        public void TestInsertPerson()
        {
            using (var ctx = new McDataContext())
            {
                Degree degree;
                Contractortype contractortype;
                Person person;
                Contractor contractor;
                Contractorposition contractorposition;
                Position position;

                person = InsertTestPerson(ctx, out degree, out contractortype, out contractor, out contractorposition,
                                          out position);
                try
                {
                    TestObjectIdInsert(ctx, person, ctx.People);
                }
                finally
                {
                    DeleteOnSubmitPersonLeavs(ctx, contractorposition, contractor, contractortype, degree, position);
                    ctx.SubmitChanges();
                }
            }
        }

        private void DeleteOnSubmitPersonLeavs(McDataContext ctx, Contractorposition contractorposition,
                                               Contractor contractor, Contractortype contractortype, Degree degree,
                                               Position position)
        {
            ctx.Contractorpositions.DeleteOnSubmit(contractorposition);
            ctx.Contractors.DeleteOnSubmit(contractor);
            ctx.Contractortypes.DeleteOnSubmit(contractortype);
            ctx.Degrees.DeleteOnSubmit(degree);
            ctx.Positions.DeleteOnSubmit(position);
        }

        [TestMethod]
        [TestCategory("Database")]
        public void TestContractInsert()
        {
            using (var ctx = new McDataContext())
            {
                const string name = "Тестовый";

                var currency = new Currency { Name = name, Culture = "ru-ru" };
                ctx.Currencies.InsertOnSubmit(currency);

                var contracttype = new Contracttype { Name = name };
                ctx.Contracttypes.InsertOnSubmit(contracttype);

                var state = new Contractstate { Name = name };
                ctx.Contractstates.InsertOnSubmit(state);

                var nds = new Nds { Year = 2010, Percents = 0.18 };
                ctx.Nds.InsertOnSubmit(nds);

                var alg = new Ndsalgorithm { Name = name };
                ctx.Ndsalgorithms.InsertOnSubmit(alg);

                var measure = new Currencymeasure { Name = name };
                ctx.Currencymeasures.InsertOnSubmit(measure);

                //Disposal disposal = new Disposal();
                //ctx.Disposals.InsertOnSubmit(disposal);

                Degree degree;
                Contractortype contractortype;
                Person person;
                Contractor contractor;
                Contractorposition contractorposition;
                Position position;
                person = InsertTestPerson(ctx, out degree, out contractortype, out contractor, out contractorposition,
                                          out position);
                ctx.People.InsertOnSubmit(person);
                ctx.SubmitChanges();

                DateTime start = DateTime.Today;
                var contract = new Contractdoc
                                   {
                                       Price = 100,
                                       Contracttype = contracttype,
                                       Startat = start,
                                       Endsat = start.AddDays(10),
                                       Internalnum = name,
                                       Currency = currency,
                                       Ndsalgorithm = alg,
                                       Prepaymentndsalgorithm = alg,
                                       Nds = nds,
                                       Contractstate = state,
                                       Currencymeasure = measure,
                                       Contractor = contractor,
                                       Person = person,
                                       Protectability = true
                                   };
                try
                {
                    TestObjectIdInsert(ctx, contract, ctx.Contractdocs);
                }

                finally
                {
                    DeleteOnSubmitPersonLeavs(ctx, contractorposition, contractor, contractortype, degree, position);

                    ctx.Currencies.DeleteOnSubmit(currency);
                    ctx.Contracttypes.DeleteOnSubmit(contracttype);
                    ctx.Contractstates.DeleteOnSubmit(state);
                    ctx.Currencymeasures.DeleteOnSubmit(measure);
                    //ctx.Disposals.DeleteOnSubmit(disposal);
                    ctx.People.DeleteOnSubmit(person);
                    ctx.Nds.DeleteOnSubmit(nds);
                    ctx.Ndsalgorithms.DeleteOnSubmit(alg);


                    ctx.SubmitChanges();
                }
            }
        }

        [TestMethod]
        [TestCategory("Database")]
        public void TestSimpleObjectIdTypesInsert()
        {
            using (var ctx = new McDataContext())
            {
                const string name = "Тестовый";
                TestObjectIdInsert(ctx, new Contractortype { Name = name }, ctx.Contractortypes);
                TestObjectIdInsert(ctx, new Contracttype { Name = name }, ctx.Contracttypes);
                TestObjectIdInsert(ctx, new Nds { Percents = 1, Year = 2000 }, ctx.Nds);
                TestObjectIdInsert(ctx, new Ndsalgorithm { Name = name }, ctx.Ndsalgorithms);
                TestObjectIdInsert(ctx, new Currencymeasure { Name = name }, ctx.Currencymeasures);
                TestObjectIdInsert(ctx, new Disposal(), ctx.Disposals);
                TestObjectIdInsert(ctx, new Role { Name = name }, ctx.Roles);
                TestObjectIdInsert(ctx, new Degree { Name = name }, ctx.Degrees);
                TestObjectIdInsert(ctx, new Contractstate { Name = name }, ctx.Contractstates);
                TestObjectIdInsert(ctx, new Troublesregistry { Name = name, Approvedat = DateTime.Today },
                                   ctx.Troublesregistries);
                TestObjectIdInsert(ctx, new Document { Name = name }, ctx.Documents);
                TestObjectIdInsert(ctx, new Transferacttype { Name = name }, ctx.Transferacttypes);
                TestObjectIdInsert(ctx, new Position { Name = name }, ctx.Positions);
                TestObjectIdInsert(ctx, new Authority { Name = name }, ctx.Authorities);
                TestObjectIdInsert(ctx, new Region { Name = name }, ctx.Regions);
                TestObjectIdInsert(ctx, new Prepaymentdocumenttype { Name = name }, ctx.Prepaymentdocumenttypes);
                TestObjectIdInsert(ctx, new Property { Name = name }, ctx.Properties);
                TestObjectIdInsert(ctx, new Ntpview { Name = name }, ctx.Ntpviews);
                TestObjectIdInsert(ctx, new Economefficiencytype { Name = name }, ctx.Economefficiencytypes);
                TestObjectIdInsert(ctx, new Economefficiencyparameter { Name = name }, ctx.Economefficiencyparameters);
                ctx.Dispose();
            }
        }

    }
}