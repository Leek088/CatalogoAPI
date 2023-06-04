using CatalogoAPI.Context;
using CatalogoAPI.Models;
using CatalogoAPI.Repositories.Interfaces;

namespace CatalogoAPI.Repositories
{
    public class ProdutoRepository : Repository<Produto>, IProdutoRepository
    {
        public ProdutoRepository(AppDbContext context) : base(context) { }

        public IEnumerable<Produto> GetProdutosPorPreco()
        {
           return GET().OrderByDescending(p => p.Preco).ToList();
        }
    }
}
