namespace CatalogoAPI.Repositories.Interfaces
{
    public interface IUnityOfWork
    {
        IProdutoRepository ProdutoRepository { get; }
        ICategoriaRepository CategoriaRepository { get; }
        Task CommitAsync();
    }
}
