using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace MCDomain.Common
{
    public class CountedConsoleWriter : TextWriter
    {
        private static int _counter = 0;

        public override void Write(string value)
        {
            ++_counter;
            Console.Out.Write("[ {0} ]", _counter);
            Console.Out.Write(value);
        }
        public override Encoding Encoding
        {
            get { return Console.Out.Encoding; }
        }
    }
}
