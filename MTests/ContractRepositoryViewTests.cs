using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using MCDomain.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MTests
{
    [TestClass]
    public class ContractRepositoryViewTests
    {
        [TestMethod]
        public void PersonProfileTest()
        {
            var obj = CreateTestContractRepositoryView();
            Assert.AreEqual("к.т.н., Н. С. Петров", obj.ContractorPersonProfile);

        }
        [TestMethod]
        public void ManagerProfileTest()
        {
            var obj = CreateTestContractRepositoryView();
            Assert.AreEqual("начальник транспортного отдела, А. Г. Сидоров", obj.ManagerProfile);

        }

        private static Contractrepositoryview CreateTestContractRepositoryView()
        {
            return new Contractrepositoryview()
                       {
                           Personfamilyname = "Петров",
                           Personmiddlename = "сергеевич",
                           Personfirstname = "николай",
                           Persondegree = "кандидат технических наук",
                           Degreeid = 2,
                           Price = 100,
                           Ndspercents = 18,
                           Ndsalgorithmid = 1,
                           Culture = "Ru-ru",
                           Factor = 10,
                           Employeefamilyname = "Сидоров",
                           Employeefirstname = "Андрей",
                           Employeemiddlename = "Геннадьевич",
                           Employeepost = "начальник транспортного отдела",
                           Employeepostid = 2
                           
       
                              
                       };
        }

        [TestMethod]
        public void PersonProfileNoDegreeTest()
        {
            var obj = CreateTestContractRepositoryView();
            obj.Degreeid = EntityBase.ReservedUndifinedOid;

            Assert.AreEqual("Н. С. Петров", obj.ContractorPersonProfile);

        }
        

        [TestMethod]
        public void PriceMoneyModelTest()
        {
            var obj = CreateTestContractRepositoryView();
            var actual = obj.PriceMoneyModel;
            Assert.IsNotNull(actual);
            Assert.AreEqual(100, actual.Price);
            Assert.AreEqual(18, actual.Nds.Percents);
            Assert.AreEqual(10, actual.Measure.Factor);
            Assert.AreEqual(1, actual.Algorithm.Id );
            Assert.AreEqual("Ru-ru", actual.Currency.Culture);
            Assert.AreEqual(1000, actual.National.Factor);
        }

    }
}
