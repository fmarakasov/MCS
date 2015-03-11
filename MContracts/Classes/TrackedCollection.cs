using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;

namespace MContracts.Classes
{
    internal enum ItemState
    {
        /// <summary>
        /// Новая запись
        /// </summary>
        New,
        /// <summary>
        /// Удалённая запись
        /// </summary>
        Deleted,
        /// <summary>
        /// Неизменённая запись
        /// </summary>
        Unchanged
    } ;

    public class TrackedCollection<T> : ObservableCollection<T>
    {
        private readonly System.Collections.Generic.IList<T> _inserted = new List<T>();
        private readonly System.Collections.Generic.IList<T> _deleted = new List<T>();
                
    }
}
