using CatalogoAPI.Models;
using CatalogoAPI.Pagination;
using System.Linq.Expressions;

namespace CatalogoAPI.Repositories.Interfaces
{
    public interface IProdutoRepository : IRepository<Produto>
    {
        PagedList<Produto> GetAllPaginated(ProdutosParameters produtosParameters);
        IEnumerable<Produto> GetProductsBySortLowPrice();
        IEnumerable<Produto> GetProductsByCategoryId(Expression<Func<Produto, bool>> predicate);
    }
}
