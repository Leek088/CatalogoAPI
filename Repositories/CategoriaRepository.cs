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

        public PagedList<Categoria> GetAllPaginated(CategoriaParameters categoriaParameters)
        {
            return PagedList<Categoria>.ToPagedList(GET().OrderBy(c => c.Nome),
                            categoriaParameters.PageNumber, categoriaParameters.PageSize);
        }

        public IEnumerable<Categoria> GetCategoriasComProdutos()
        {
            return GET().Include(c => c.Produtos);
        }
    }
}
