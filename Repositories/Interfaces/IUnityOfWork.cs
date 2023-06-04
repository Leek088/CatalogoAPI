namespace CatalogoAPI.Repositories.Interfaces
{
    public interface IUnityOfWork
    {
        IProdutoRepository ProdutoRepository { get; }
        ICategoriaRepository ICategoriaRepository { get; }
        void Commit();
    }
}
