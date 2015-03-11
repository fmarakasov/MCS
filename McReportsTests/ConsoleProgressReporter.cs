using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CommonBase.Progress;

namespace McReportsTests
{
    class ConsoleProgressReporter : IProgressReporter
    {
        public int Percents { get; private set; }

        public void ReportProgress(int percents)
        {
            Percents = percents;
            Console.WriteLine(percents);
        }

        public static readonly IProgressReporter Instance = new ConsoleProgressReporter();

    }
}
