using CatalogoAPI.Context;
using CatalogoAPI.Models;
using CatalogoAPI.Repositories.Interfaces;
using System.Linq.Expressions;

namespace CatalogoAPI.Repositories
{
    public class ProdutoRepository : Repository<Produto>, IProdutoRepository
    {
        public ProdutoRepository(AppDbContext context) : base(context) { }

        public IEnumerable<Produto> GetProdutosPorCategoria(Expression<Func<Produto, bool>> predicate)
        {
            return GET().Where(predicate);
        }

        public IEnumerable<Produto> GetProdutosPorPreco()
        {
           return GET().OrderBy(p => p.Preco).ToList();
        }
    }
}
