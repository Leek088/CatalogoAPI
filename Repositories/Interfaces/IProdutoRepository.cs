using CatalogoAPI.Models;
using CatalogoAPI.Pagination;
using System.Linq.Expressions;

namespace CatalogoAPI.Repositories.Interfaces
{
    public interface IProdutoRepository : IRepository<Produto>
    {
        Task<PagedList<Produto>> GetAllPaginatedAsync(ProdutosParameters produtosParameters);
        IQueryable<Produto> GetProductsBySortLowPrice();
        IQueryable<Produto> GetProductsByCategoryId(Expression<Func<Produto, bool>> predicate);
    }
}
