using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;

namespace MContracts.Classes.Filtering
{
    public class FilterProperty
    {
        public string Property { get; set; }

        public Type ControlType { get; set; }

        public List<ConditionTypes> Conditions { get; set; }

        public object Values { get; set; }
    }
}
