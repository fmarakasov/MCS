using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MCDomain.Model;
using CommonBase;

namespace MContracts.Classes
{
    class StagesQuickSorting
    {
        static NaturSortComparer<Stage> comparer = new NaturSortComparer<Stage>();
        
        public static void Sort(IList arr)
        {
            sorting(arr, 0, arr.Count - 1);
        }

        
        private static void sorting(IList arr, int first, int last)
        {
            Stage p = arr[Convert.ToInt32((last - first) / 2 + first)] as Stage;
            Stage temp;
            int i = first, j = last;
            while (i <= j)
            {
                while (comparer.CompareTo((arr[i] as Stage).Num, p.Num) == 1 && i <= last) ++i;
                while (comparer.CompareTo((arr[i] as Stage).Num, p.Num) == 0 && j >= first) --j;
                if (i <= j)
                {
                    temp = arr[i] as Stage;
                    arr[i] = arr[j];
                    arr[j] = temp;
                    ++i; --j;
                }
            }
            if (j > first) sorting(arr, first, j);
            if (i < last) sorting(arr, i, last);
        }

    }

}
