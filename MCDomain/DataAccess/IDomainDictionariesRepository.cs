using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Reflection;
using CommonBase;
using MCDomain.Model;

namespace MCDomain.DataAccess
{
    interface IDomainDictionariesRepository<out T>  where T : MCDomain.Model.McDataContext, new()
    {
        T DataContext { get; }
        IDictionary<string, DomainDictionaryAttribute> DomainDictionaries { get; }
    }

    class DomainDictionariesRepository<T> : IDomainDictionariesRepository<T> where T :McDataContext, new()
    {
        readonly T _dataContext = new T();
        public T DataContext
        {
            get { return _dataContext; }
        }

        public IDictionary<string, DomainDictionaryAttribute> DomainDictionaries
        {
            get
            {
                if (DataContext == null)
                    throw new NoNullAllowedException("DataContext должен быть задан.");

                IDictionary<string, DomainDictionaryAttribute> result = 
                    new Dictionary<string, DomainDictionaryAttribute>();

                PropertyInfo[] pi = DataContext.GetType().GetProperties();
                foreach (var propertyInfo in pi)
                {
                    var ca = propertyInfo.GetCustomAttributes(true);
                    if (ca.Length > 0)
                    {
                        if (ca[0] is DomainDictionaryAttribute)
                        {
                            result.Add(propertyInfo.Name, (DomainDictionaryAttribute)ca[0]);
                        }
                    }
                }
                return result;
            }
        }
    }
}
