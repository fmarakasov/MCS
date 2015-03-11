namespace MContracts.ViewModel.Reports
{
    /// <summary>
    /// ���������� ��������� ������� ���� � ������� ������
    /// </summary>
    public interface ITemplateProvider
    {
        /// <summary>
        /// ���������� ������ ���� � ��������� �������
        /// </summary>
        /// <param name="propertyName">��� �������� � ������ ����� �������</param>
        /// <returns>������ ���� � ����� �������</returns>
        string GetTemplate(string propertyName);
    }
}