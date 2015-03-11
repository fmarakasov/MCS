using System;
using MCDomain.Importer;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ExcelReaderAppTestProject
{
    /// <summary>
    ///This is a test class for ImporterTest and is intended
    ///to contain all ImporterTest Unit Tests
    ///</summary>
    [TestClass]
    public class ImporterTest
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

        [TestMethod]        
        public void TestReadWordFile()
        {
            var imp = new Importer(null, null, null);
            imp.Filename = TestContext.DeploymentDirectory + "\\Sample4.doc";
            Assert.IsFalse(imp.ActiveReader == null, "Не удалось получить объект-конвертор");
            imp.ActiveReader.Open();
            imp.ActiveReader.AcceptParameters(new ReaderCommonParameters(1, 1, 1, null, null));
            imp.ActiveReader.Read();
            Assert.IsFalse(
                imp.Cells[imp.Cells.RowCount - 2][imp.Cells.ColCount - 2].Value !=
                (imp.ActiveReader as WordReader).ActiveTable.Cell(
                    (imp.ActiveReader as WordReader).ActiveTable.Rows.Count - 1,
                    (imp.ActiveReader as WordReader).ActiveTable.Columns.Count - 1).Range.Text,
                "Данные в прочитанной таблице не соответствуют данным в таблице источнике");

            imp.ClearReaders();
        }

        [TestMethod]
        public void TestReadExcelFile()
        {
            var imp = new Importer(null, null, null);
            imp.Filename = TestContext.DeploymentDirectory + "\\Sample1.xls";
            Assert.IsFalse(imp.ActiveReader == null, "Не удалось получить объект-конвертор");
            //imp.Parameters = new ReaderCommonParameters(1, 5, 133, null, null);
            imp.Active = true;

            string s =
                (imp.ActiveReader as ExcelReader).CurrentRange.Cells[
                    (imp.ActiveReader as ExcelReader).CurrentRange.Rows.Count - 1,
                    (imp.ActiveReader as ExcelReader).CurrentRange.Columns.Count - 1].Text;
            Assert.IsFalse(imp.Cells[imp.Cells.RowCount - 2][imp.Cells.ColCount - 2].Value != s,
                           "Данные в прочитанной таблице не соответствуют данным в таблице источнике");
            Assert.IsFalse(imp.StageCount == 0, "Массив этапов остался незаполненным после чтения файла");
            imp.ClearReaders();
        }
    }
}