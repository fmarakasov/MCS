using System.IO;
using MCDomain.Importer;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ExcelReaderAppTestProject
{
    [TestClass]
    public class TestExcelReader
    {
        public TestContext TestContext { get; set; }

        [TestMethod]
        public void TestWorkBookOpenIfFileExists()
        {
            var er = new ExcelReader();
            er.FileName = this.TestContext.DeploymentDirectory + "\\Sample1.xls";
            er.Open();
            Assert.IsFalse(er.ActiveWorkBook == null, "Книга не была открыта");
            er.Close();
            Assert.IsFalse(er.ActiveWorkBook != null, "Книга не была закрыта");
            er.Clear();
        }

        [TestMethod]
        public void TestWorkBookOpenIfFileNotExists()
        {
            var er = new ExcelReader();
            // пытаемся открыть несуществующий файл
            er.FileName = this.TestContext.DeploymentDirectory + "\\SampleNE.xls";
            try
            {
                er.Open();
            }
            catch (FileNotFoundException) // получиться не должно, там исключение
            {
                Assert.IsFalse(er.ActiveWorkBook != null, "Книга не была открыта");
                er.Close();
            }
            er.Clear();
        }

        [TestMethod]
        public void TestCurrentRegionRead()
        {
            var er = new ExcelReader();
            er.FileName = TestContext.DeploymentDirectory + "\\Sample1.xls";
            er.Open();
            er.StartRow = 5;
            er.StartColumn = 1;
            Assert.IsFalse(er.CurrentRange == null, "Текущий регион не был прочитан");
            er.CurrentRange.Select();

            Assert.IsFalse((er.CurrentRange.Row + er.CurrentRange.Rows.Count - 1) != er.FinishRow,
                           "Неверно определена конечная строка");
            Assert.IsFalse((er.CurrentRange.Column + er.CurrentRange.Columns.Count - 1) != er.FinishColumn,
                           "Неверно определен конечный столбец");

            er.Read();

            Assert.IsFalse(er.Cells.RowCount != (er.FinishRow - er.StartRow + 1),
                           "Количество строк в прочитанной таблице не соответствует количеству строк в таблице-источнике");
            Assert.IsFalse(er.Cells.ColCount != er.FinishColumn - er.StartColumn + 1,
                           "Количество столбцов в прочитанной таблице не соответствует количеству столбцов в таблице-источнике");

            string s = er.CurrentRange.Cells[er.CurrentRange.Rows.Count - 1, er.CurrentRange.Columns.Count - 1].Text;
            Assert.IsFalse(er.Cells[er.Cells.RowCount - 2][er.Cells.ColCount - 2].Value != s,
                           "Данные в прочитанной таблице не соответствуют данным в таблице источнике");

            er.Clear();
        }
    }
}