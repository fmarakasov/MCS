using System.Collections.Generic;

namespace MCDomain.Common
{
    /// <summary>
    /// ��������� ��� �����, ����������� ��������� ���� ������
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface ITree<T>
    {
        /// <summary>
        /// �������� ��� ������������� �������� �������� ������
        /// </summary>
        T Item { get; set; }
        /// <summary>
        /// �������� ��������� �������� ����� ������
        /// </summary>
        IEnumerable<ITree<T>> Childs { get; }
    }
}