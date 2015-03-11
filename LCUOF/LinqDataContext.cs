using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Text;
using UOW;

namespace LCUOF
{
    class LinqDataContext : IDataContext
    {
        private readonly DataContext _context;

        public LinqDataContext(DataContext context)
        {
            if (context == null) throw new ArgumentNullException("context");
            _context = context;
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        public void Add<T>(T item) where T : class
        {
            _context.GetTable<T>().InsertOnSubmit(item);
        }

        public void Update<T>(T item) where T : class
        {
            _context.GetTable<T>().Attach(item);
        }

        public void Delete<T>(T item) where T : class
        {
            _context.GetTable<T>().DeleteOnSubmit(item);
        }

        public IQueryable<T> AsQueryable<T>() where T : class
        {
            return _context.GetTable<T>();
        }

        public void Save()
        {
            _context.SubmitChanges();
        }
    }
}
