using System;
using System.Linq;
using System.Linq.Expressions;

namespace APICatalog_NetCore.Repository
{
    public interface IRepository<T>
    {
        IQueryable<T> Get();

        T GetById(Expression<Func<T, bool>> precidate);
        void Add(T entity);
        void Update(T entity);
        void Delete(T entity);
    }
}
