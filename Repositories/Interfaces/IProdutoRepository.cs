using CatalogoAPI.Models;
using System.Linq.Expressions;

namespace CatalogoAPI.Repositories.Interfaces
{
    public interface IProdutoRepository : IRepository<Produto>
    {
        IEnumerable<Produto> GetProductsBySortLowPrice();
        IEnumerable<Produto> GetProductsByCategoryId(Expression<Func<Produto, bool>> predicate);
    }
}
