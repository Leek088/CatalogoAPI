using CatalogoAPI.Models;
using System.Linq.Expressions;

namespace CatalogoAPI.Repositories.Interfaces
{
    public interface IProdutoRepository : IRepository<Produto>
    {
        IEnumerable<Produto> GetProdutosPorPreco();
        IEnumerable<Produto> GetProdutosPorCategoria(Expression<Func<Produto, bool>> predicate);
    }
}
