using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MCDomain.Model
{
    partial class Subject: IComparable
    {
        public override string ToString()
        {
            return base.ToString();
        }

        #region IComparable Members

        public int CompareTo(object obj)
        {
            return this.ToString().CompareTo((obj as Subject).ToString());
        }

        #endregion
    }
}
