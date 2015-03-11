using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Devart.Data.Linq;
using UOW;

namespace LCUOF
{

    public static class DataContextExtensions
    {
        public static bool IsModified(this ChangeSet set)
        {
            if (set == null) throw new ArgumentNullException("set");
            return set.Deletes.Count == 0 && set.Inserts.Count == 0 && set.Updates.Count == 0;
        }

        public static bool IsModified(this DataContext context)
        {
            if (context == null) throw new ArgumentNullException("context");
            var cs = context.GetChangeSet();
            return cs.IsModified();
        }
    }
    /// <summary>
    /// Контекст данных DevArt.LinqConnect
    /// </summary>
    public class LcDataContext : IDataContext
    {
        private readonly DataContext _ctx;

        /// <summary>
        /// Создаёт экземпляр LcDataContext для заданного DataContext
        /// </summary>
        /// <param name="ctx">Объект типа DataContext</param>
        public LcDataContext(DataContext ctx)
        {
            if (ctx == null) throw new ArgumentNullException("ctx");
            _ctx = ctx;
        }

        public void Dispose()
        {
            _ctx.Dispose();
        }

        public bool IsModified { get { return _ctx.IsModified(); } }

        public void Add<T>(T item) where T : class
        {
            _ctx.GetTable<T>().InsertOnSubmit(item);
        }

        public void Update<T>(T item) where T : class
        {
            _ctx.GetTable<T>().Attach(item);
        }

        public void Delete<T>(T item) where T : class
        {
            _ctx.GetTable<T>().DeleteOnSubmit(item);
        }

        public IQueryable<T> AsQueryable<T>() where T : class
        {
            return _ctx.GetTable<T>();
        }

        public void Save()
        {
            _ctx.SubmitChanges();
        }
    }
}
