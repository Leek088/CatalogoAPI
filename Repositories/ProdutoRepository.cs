using CatalogoAPI.Context;
using CatalogoAPI.Models;
using CatalogoAPI.Pagination;
using CatalogoAPI.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace CatalogoAPI.Repositories
{
    public class ProdutoRepository : Repository<Produto>, IProdutoRepository
    {
        public ProdutoRepository(AppDbContext context) : base(context) { }

        public async Task<PagedList<Produto>> GetAllPaginatedAsync(ProdutosParameters produtosParameters)
        {
            return await PagedList<Produto>.ToPagedListAsync(Get().OrderBy(p => p.Nome),
                produtosParameters.PageNumber, produtosParameters.PageSize);
        }

        public IQueryable<Produto> GetProductsByCategoryId(Expression<Func<Produto, bool>> predicate)
        {
            return Get().Where(predicate).AsNoTracking();
        }

        public IQueryable<Produto> GetProductsBySortLowPrice()
        {
            return Get().OrderBy(p => p.Preco).AsNoTracking();
        }
    }
}
