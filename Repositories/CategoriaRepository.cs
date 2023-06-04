using CatalogoAPI.Context;
using CatalogoAPI.Models;
using CatalogoAPI.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CatalogoAPI.Repositories
{
    public class CategoriaRepository : Repository<Categoria>, ICategoriaRepository
    {
        public CategoriaRepository(AppDbContext context) : base(context) { }

        public IEnumerable<Categoria> GetProdutosDaCategoria()
        {
            return GET().Include(p => p.Produtos).ToList();
        }
    }
}
