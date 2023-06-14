using CatalogoAPI.Context;
using CatalogoAPI.Repositories.Interfaces;

namespace CatalogoAPI.Repositories
{
    public class UnityOfWork : IUnityOfWork
    {
        private ProdutoRepository? _produtoRepository;
        private CategoriaRepository? _categoriaRepository;
        public AppDbContext _context;

        public UnityOfWork(AppDbContext appDbContext)
        {
            _context = appDbContext;
        }

        public IProdutoRepository ProdutoRepository
        {
            get
            {
                if (_produtoRepository == null)
                    return new ProdutoRepository(_context);

                return _produtoRepository;
            }
        }

        public ICategoriaRepository CategoriaRepository
        {
            get
            {

                if (_categoriaRepository == null)
                    return new CategoriaRepository(_context);

                return _categoriaRepository;
            }
        }

        public async Task CommitAsync()
        {
            await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
