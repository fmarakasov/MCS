using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Text;
using MCDomain.Model;

namespace MContracts.Classes
{
    public static class IEnumerableExtensions
    {
        public static bool ContaintsEnumerable<T>(this IEnumerable<T> list, IEnumerable<T> other) where T : class
        {
            foreach (var item in list)
            {
                if (!(item is T)) return false;
                
                if (other.Contains(item))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
