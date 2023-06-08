using CatalogoAPI.Models;
using CatalogoAPI.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CatalogoAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProdutosController : ControllerBase
    {
        private readonly IUnityOfWork _unityOfWork;

        public ProdutosController(IUnityOfWork unityOfWork)
        {
            _unityOfWork = unityOfWork;
        }

        [HttpGet("Categoria/{id:int}")]
        public ActionResult<IEnumerable<Produto>> GetProdutoPreco(int id)
        {
            var produtoDB = _unityOfWork.ProdutoRepository.GetProdutosPorCategoria(p => p.CategoriaId == id);

            if (produtoDB.Any())
                return Ok(produtoDB);

            else
                return NotFound("Nenhum produto encontrado");
        }

        [HttpGet("Preco")]
        public ActionResult<IEnumerable<Produto>> GetProdutoPreco()
        {
            var produtoDB = _unityOfWork.ProdutoRepository.GetProdutosPorPreco();

            if (produtoDB.Any())
                return Ok(produtoDB);

            else
                return NotFound("Nenhum produto encontrado");
        }

        [HttpGet]
        public ActionResult<IEnumerable<Produto>> Get()
        {
            var produtosDB = _unityOfWork.ProdutoRepository.GET();

            if (produtosDB.Any())
                return Ok(produtosDB);

            return StatusCode(StatusCodes.Status204NoContent,
                "Nenhum produdo cadastrado.");
        }

        [HttpGet("{id:int}", Name = "OberProdutoPorId")]
        public ActionResult<Produto> Get(int id)
        {
            var produtoDB = _unityOfWork.ProdutoRepository.GetById(p => p.Id == id);

            if (produtoDB is null)
                return NotFound($"Produdo de id {id}, não encontrado");

            return Ok(produtoDB);

        }

        [HttpPost]
        public ActionResult<Produto> Post(Produto produto)
        {
            if (produto is null)
                return BadRequest("Nenhum produto recebido");

            _unityOfWork.ProdutoRepository.Add(produto);
            _unityOfWork.Commit();

            return new CreatedAtRouteResult("OberProdutoPorId", new { id = produto.Id }, produto);
        }

        [HttpPut("{id:int}")]
        public ActionResult<Produto> Put(int id, Produto produto)
        {
            if (id != produto.Id)
                return BadRequest();

            _unityOfWork.ProdutoRepository.Update(produto);
            _unityOfWork.Commit();

            return Ok(produto);
        }

        [HttpDelete("{id:int}")]
        public ActionResult<Produto> Delete(int id)
        {
            var produto = _unityOfWork.ProdutoRepository.GetById(p => p.Id == id);

            if (produto is null)
                return NotFound();

            _unityOfWork.ProdutoRepository.Delete(produto);
            _unityOfWork.Commit();

            return Ok(produto);
        }
    }
}
