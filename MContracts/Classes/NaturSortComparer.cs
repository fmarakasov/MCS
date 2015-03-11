﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace MContracts.Classes
{
    public class NaturSortComparer<T> : IComparer<string>, IDisposable
    {
        #region IComparer<string> Members

        int IComparer<string>.Compare(string x, string y)
        {
            if (x == null) x = string.Empty;
            if (y == null) y = string.Empty;

            if (x == y)
                return 0;

            string[] x1, y1;

            if (!table.TryGetValue(x, out x1))
            {
                x1 = Regex.Split(x.Replace(" ", "").Replace("\n", ""), "([0-9]+)");
                table.Add(x, x1);
            }

            if (!table.TryGetValue(y, out y1))
            {
                y1 = Regex.Split(y.Replace(" ", "").Replace("\n", ""), "([0-9]+)");
                table.Add(y, y1);
            }

            for (int i = 0; i < x1.Length && i < y1.Length; i++)
            {
                if (x1[i] != y1[i])
                {
                    return PartCompare(x1[i], y1[i]);
                }
            }

            if (y1.Length > x1.Length)
                return -1;
            else if (x1.Length > y1.Length)
                return 1;
            else
                return 0;
        }

        private static int PartCompare(string left, string right)
        {
            int x, y;
            if (!int.TryParse(left, out x))
                return left.CompareTo(right);

            if (!int.TryParse(right, out y))
                return left.CompareTo(right);

            return x.CompareTo(y);
        }

        #endregion

        private Dictionary<string, string[]> table = new Dictionary<string, string[]>();

        public int CompareTo(string x, string y)
        {
            return (this as IComparer<string>).Compare(x, y);
        }

        public void Dispose()
        {
            table.Clear();
            table = null;
        }
    }
}
