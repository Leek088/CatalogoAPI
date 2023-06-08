using CatalogoAPI.Models;
using System.Linq.Expressions;

namespace CatalogoAPI.Repositories.Interfaces
{
    public interface ICategoriaRepository : IRepository<Categoria>
    {
        IEnumerable<Categoria> GetCategoriasComProdutos();
    }
}
