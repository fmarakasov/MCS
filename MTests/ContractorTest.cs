using MCDomain.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MTests
{
    /// <summary>
    ///This is a test class for ContractorTest and is intended
    ///to contain all ContractorTest Unit Tests
    ///</summary>
    [TestClass]
    public class ContractorTest
    {
        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext { get; set; }

        #region Additional test attributes

        // 
        //You can use the following additional attributes as you write your tests:
        //
        //Use ClassInitialize to run code before running the first test in the class
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //Use ClassCleanup to run code after all tests in a class have run
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //Use TestInitialize to run code before running each test
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //

        #endregion

        /*
         * Banking property impl:
           StringBuilder sb = new StringBuilder();
                sb.Append(SelectString(Bank, "Банк", true));
                sb.Append(SelectString(Inn, ", ИНН"));
                sb.Append(SelectString(Account, ", Расчётный счёт"));
                sb.Append(SelectString(Bik, ", БИК"));
                sb.Append(SelectString(Kpp, ", КПП"));
                return sb.ToString();
         */

        /// <summary>
        ///Тест свойства Banking. Должен возвращать строку по умолчанию, если данных по контрагенту нет
        ///</summary>
        [TestMethod]
        public void PersonsBankingShoulReturnDefaultTest()
        {
            var target = new Contractor(); 
            Assert.AreEqual("Банк N/A, ИНН N/A, Расчётный счёт N/A, БИК N/A, КПП N/A", target.Banking);
        }

        /// <summary>
        ///Тест свойства Banking. Должен строку возвращать, если есть данные по контрагенту
        ///</summary>
        [TestMethod]
        public void PersonsBankingShoulReturnNonDefaultTest()
        {
            var target = new Contractor(); 
            target.Bank = "A";
            target.Inn = "B";
            target.Account = "C";
            target.Bik = "D";
            target.Kpp = "E";
            Assert.AreEqual("Банк \"A\", ИНН B, Расчётный счёт C, БИК D, КПП E", target.Banking);
        }

        /*
         *  StringBuilder sb= new StringBuilder();
                sb.Append(SelectString(Zip, "Индекс"));
                sb.Append(SelectString(City, ", Горорд"));
                sb.Append(SelectString(Street, ", Улица"));
                sb.Append(SelectString(Build, ", Дом"));
                sb.Append(SelectString(Block, ", Корпус"));
                sb.Append(SelectString(Appartment, ", Квартира"));
                return sb.ToString();
         */

        /// <summary>
        ///Тест свойства Address. Должна быть строка по умолчанию
        ///</summary>
        [TestMethod]
        public void PersonsAddressShoulReturnDefaultTest()
        {
            var target = new Contractor(); 
            Assert.AreEqual("Индекс N/A, Город N/A, Улица N/A, Дом N/A, Корпус N/A, Квартира N/A", target.Address);
        }
        /// <summary>
        ///Тест свойства Address. Должна быть строка с данными
        ///</summary>
        [TestMethod]
        public void PersonsAddressShoulReturnNonDefaultTest()
        {
            var target = new Contractor()
                             {Zip = "A", Address = "123"};

            Assert.AreEqual("Индекс A, Город B, Улица C, Дом D, Корпус E, Квартира F", target.Address);
            
        }
    }
}