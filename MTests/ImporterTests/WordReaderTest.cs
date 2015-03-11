using System.IO;
using MCDomain.Importer;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ExcelReaderAppTestProject
{
    [TestClass]
    public class WordReaderTest
    {
        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext { get; set; }

        [TestMethod]
        public void TestWorkBookOpenIfFileExists()
        {
            var wr = new WordReader();
            wr.FileName = TestContext.DeploymentDirectory + "\\Sample4.doc";
            wr.Open();
            Assert.IsFalse(wr.ActiveDocument == null, "Документ не был открыт");
            wr.Close();
            Assert.IsFalse(wr.ActiveDocument != null, "Документ не был закрыт");
            wr.Clear();
        }

        [TestMethod]
        public void TestWorkBookOpenIfFileNotExists()
        {
            var wr = new WordReader();
            // пытаемся открыть несуществующий файл
            wr.FileName = TestContext.DeploymentDirectory + "\\SampleNE.doc";
            try
            {
                wr.Open();
            }
            catch (FileNotFoundException) // получиться не должно, там исключение
            {
                Assert.IsFalse(wr.ActiveDocument != null, "Документ не был закрыт");
                wr.Close();
            }
            wr.Clear();
        }

        [TestMethod]
        public void TestActiveTableRead()
        {
            var wr = new WordReader();
            wr.FileName = TestContext.DeploymentDirectory + "\\Sample4.doc";
            wr.Open();
            Assert.IsFalse(wr.ActiveDocument == null, "Текущий регион не был прочитан");
            Assert.IsFalse(wr.ActiveTable == null, "В документе нет таблиц");
            wr.ActiveTable.Select();
            wr.Read();
            Assert.IsFalse(wr.Cells.RowCount != (wr.FinishRow - wr.StartRow + 1),
                           "Количество строк в прочитанной таблице не соответствует количеству строк в таблице-источнике");
            Assert.IsFalse(wr.Cells.ColCount != wr.ActiveTable.Columns.Count,
                           "Количество столбцов в прочитанной таблице не соответствует количеству столбцов в таблице-источнике");
            Assert.IsFalse(
                wr.Cells[wr.Cells.RowCount - 2][wr.Cells.ColCount - 2].Value !=
                wr.ActiveTable.Cell(wr.ActiveTable.Rows.Count - 1, wr.ActiveTable.Columns.Count - 1).Range.Text,
                "Данные в прочитанной таблице не соответствуют данным в таблице источнике");

            wr.Clear();
        }
    }
}