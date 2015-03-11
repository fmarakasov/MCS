using MCDomain.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ExcelReaderAppTestProject
{
    [TestClass]
    public class CellsTest
    {
        [TestMethod]
        public void TestCellsCreate()
        {
            var c = new Cells(5, 5);
            Assert.IsFalse(c == null, "Таблица не была создана");
            Assert.IsFalse(c[3][4] == null, "Не удалось получить доступ к элементу");
            Assert.IsFalse(c[3][4].Column != 4, "Неверно проиндексирован столбец");
            Assert.IsFalse(c[3][4].Row != 3, "Неверно проиндексирована строка");
        }
    }
}