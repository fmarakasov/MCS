using System.Diagnostics;
using System.Linq;
using System.Text;

namespace CommonBase
{
    /// <summary>
    /// Интерфейс для типов, поддерживающих свойство Images для хранения образа документа
    /// </summary>
    public interface IDocumentImageContainer
    {
        byte[] Images { get; set; }
    }
}
