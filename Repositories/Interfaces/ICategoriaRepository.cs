using CatalogoAPI.Models;
using CatalogoAPI.Pagination;

namespace CatalogoAPI.Repositories.Interfaces
{
    public interface ICategoriaRepository : IRepository<Categoria>
    {
        PagedList<Categoria> GetAllPaginated(CategoriaParameters categoriaParameters);
        IEnumerable<Categoria> GetCategoriasComProdutos();
    }
}
