namespace McReports.Common
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