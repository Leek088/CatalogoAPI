using CatalogoAPI.Context;
using CatalogoAPI.Models;
using CatalogoAPI.Pagination;
using CatalogoAPI.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CatalogoAPI.Repositories
{
    public class CategoriaRepository : Repository<Categoria>, ICategoriaRepository
    {
        public CategoriaRepository(AppDbContext context) : base(context) { }       

        public async Task<PagedList<Categoria>> GetAllPaginatedAsync(CategoriaParameters categoriaParameters)
        {
            return await PagedList<Categoria>.ToPagedListAsync(Get().OrderBy(c => c.Nome),
                           categoriaParameters.PageNumber, categoriaParameters.PageSize);
        }

        public IQueryable<Categoria> GetCategoriasComProdutos()
        {
            return Get().Include(c => c.Produtos);
        }
    }
}
