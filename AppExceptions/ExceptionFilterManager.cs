namespace AppExceptions
{
    public class ExceptionFilterManager
    {
        public static readonly ExceptionFilterManager Instance = new ExceptionFilterManager();

        public ExceptionDescriptionResolvers DescriptionResolvers { get; private set; }
        public ExceptionMappers Mappers { get; private set; }

        public ExceptionFilterManager()
        {
            DescriptionResolvers = new ExceptionDescriptionResolvers();
            Mappers = new ExceptionMappers();
        }


        public string AssemblyVersion { get; set; }
        public string AssemblyTitle { get; set; }
        public string FileVersion{ get; set; }
    }
}