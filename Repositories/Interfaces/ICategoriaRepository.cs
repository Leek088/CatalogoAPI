using CatalogoAPI.Models;
using CatalogoAPI.Pagination;

namespace CatalogoAPI.Repositories.Interfaces
{
    public interface ICategoriaRepository : IRepository<Categoria>
    {
        Task<PagedList<Categoria>> GetAllPaginatedAsync(CategoriaParameters categoriaParameters);
        IQueryable<Categoria> GetCategoriasComProdutos();
    }
}
