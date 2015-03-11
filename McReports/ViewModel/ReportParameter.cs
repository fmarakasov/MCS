using McReports.Common;

namespace McReports.ViewModel
{
    public class ReportParameter
    {
        public InputParameterAttribute Attribute { get; private set; }
        public string PropertyName { get; private set; }
        public ReportParameter(InputParameterAttribute attribute, string propertyName)
        {
            Attribute = attribute;
            PropertyName = propertyName;
        }
    }
}