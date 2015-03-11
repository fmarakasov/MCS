using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MContracts.Classes.Filtering
{
    public class FilterItem
    {
        public bool IsSelected { get; set; }

        public FilterProperty Property { get; set; }

        private ConditionTypes condition;
        public ConditionTypes Condition
        {
            get { return condition; }
            set { condition = value; }
        }

        public object Value { get; set; }
    }
}
