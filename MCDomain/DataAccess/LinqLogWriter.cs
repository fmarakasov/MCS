using System.IO;

namespace MCDomain.DataAccess
{
    public sealed class LinqLogWriter : StreamWriter
    {
        public LinqLogWriter(string path) : base(path, true)
        {
            FilePath = path;
            AutoFlush = true;
        }

        public static readonly LinqLogWriter Instance = new LinqLogWriter(Path.GetTempFileName());


        public string FilePath { get; private set; }
    }
}