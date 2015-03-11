using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace MContracts.Classes
{
    public static class IBindingListExtansions
    {
        public static void Move(this IBindingList list, int oldindex, int newindex)
        {
            var item = list[oldindex];
            list.RemoveAt(oldindex);
            list.Insert(newindex, item);
        }

        public static IEnumerable<T> Where<T>(this IBindingList list, Func<T, bool> predicate) where T: class
        {
            return list.Cast<T>().Where(predicate); 
            //List<T> result = new List<T>();

            //foreach (var item in list)
            //{
            //    if (predicate.Invoke(item as T))
            //    {
            //        result.Add(item as T);
            //    }
            //}

            //return result;
        }


    }
}
