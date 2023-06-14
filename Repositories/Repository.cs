using CatalogoAPI.Context;
using CatalogoAPI.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace CatalogoAPI.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly AppDbContext _context;

        public Repository(AppDbContext context)
        {
            _context = context;
        }

        public IQueryable<T> Get()
        {
            return  _context.Set<T>().AsNoTracking();
        }

        public async Task<T> GetByIdAsync(Expression<Func<T, bool>> predicate)
        {
            var result = await _context.Set<T>().AsNoTracking().SingleOrDefaultAsync(predicate);
            return result!;
        }

        public void Add(T entity)
        {
            _context.Set<T>().Add(entity);
        }

        public void Delete(T entity)
        {
            _context.Set<T>().Remove(entity);
        }

        public void Update(T entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            _context.Set<T>().Update(entity);
        }
    }
}
