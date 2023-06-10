using CatalogoAPI.Context;
using CatalogoAPI.Models;
using CatalogoAPI.Repositories.Interfaces;
using System.Linq.Expressions;

namespace CatalogoAPI.Repositories
{
    public class ProdutoRepository : Repository<Produto>, IProdutoRepository
    {
        public ProdutoRepository(AppDbContext context) : base(context) { }

        public IEnumerable<Produto> GetProductsByCategoryId(Expression<Func<Produto, bool>> predicate)
        {
            return GET().Where(predicate);
        }

        public IEnumerable<Produto> GetProductsBySortLowPrice()
        {
           return GET().OrderBy(p => p.Preco).ToList();
        }
    }
}
