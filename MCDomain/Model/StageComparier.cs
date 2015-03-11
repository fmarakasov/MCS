using System.Collections;
using System.Diagnostics.Contracts;
using CommonBase;

namespace MCDomain.Model
{
    /// <summary>
    /// Используется для сортировки этапов в соответствие с их номерами
    /// </summary>
    public class StageComparier : IComparer
    {
        public int Compare(object x, object y)
        {
            Contract.Assert(x is Stage);
            Contract.Assert(y is Stage);
            return HierarchicalNumberingComparier.Instance.Compare(x.CastTo<Stage>().Num, y.CastTo<Stage>().Num);

        }
    }
}