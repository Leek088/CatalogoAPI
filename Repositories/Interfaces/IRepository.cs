using System.Linq.Expressions;

namespace CatalogoAPI.Repositories.Interfaces
{
    public interface IRepository<T>
    {
        IQueryable<T> GET();
        T? GetById(Expression<Func<T, bool>> predicate);
        void Add(T entity);
        void Update(T entity);
        void Delete(T entity);
    }
}
