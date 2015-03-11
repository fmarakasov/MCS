using System.Diagnostics.Contracts;
using CommonBase;

namespace MCDomain.Model
{
    public static class MeasureExtensions
    {
        /// <summary>
        /// ���������� ��������� � �������� �������� ���������
        /// </summary>
        /// <param name="source">������ ���������</param>
        /// <param name="toFactor">����� ������� ���������</param>
        /// <returns>��������� � ����� ������� ���������</returns>
        public static decimal? ToFactor(this IMeasureSupport source, int toFactor)
        {
            Contract.Requires(source != null);
            return source.PriceValue*source.Measure.Return(x => x.Factor, default(decimal?)) / toFactor;
        }
    }
}