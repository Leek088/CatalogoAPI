using CatalogoAPI.Context;
using CatalogoAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CatalogoAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProdutosController : ControllerBase
    {
        private readonly AppDbContext _appDbContext;

        public ProdutosController(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Produto>> Get()
        {
            var queryProdutos = from produtos in _appDbContext.Produtos
                                join categoria in _appDbContext.Categorias
                                on produtos.CategoriaId equals categoria.Id
                                select new { produtos, NomeCategoria = categoria.Nome };

            var produtosDB = queryProdutos.AsNoTracking().Take(10).ToList();

            if (produtosDB is null)
                return NoContent();

            return Ok(produtosDB);
        }

        [HttpGet("{id:int}", Name = "OberProdutoPorId")]
        public ActionResult<Produto> Get(int id)
        {

            var queryProduto = from produto in _appDbContext.Produtos
                               join categoria in _appDbContext.Categorias
                               on produto.CategoriaId equals categoria.Id
                               select new { produto, NomeCategoria = categoria.Nome };

            var produtoDB = queryProduto.AsNoTracking().Take(10).ToList();

            if (produtoDB is null)
                return NotFound("Produdo não encontrado");

            return Ok(produtoDB);
        }

        [HttpPost]
        public ActionResult<Produto> Post(Produto produto)
        {
            if (produto is null)
                return BadRequest("Nenhum produto recebido");

            _appDbContext.Add(produto);
            _appDbContext.SaveChanges();

            return new CreatedAtRouteResult("OberProdutoPorId", new { id = produto.Id }, produto);
        }

        [HttpPut("{id:int}")]
        public ActionResult<Produto> Put(int id, Produto produto)
        {
            if (id != produto.Id)
                return BadRequest();

            _appDbContext.Entry(produto).State = EntityState.Modified;
            _appDbContext.SaveChanges();

            return Ok(produto);
        }

        [HttpDelete("{id:int}")]
        public ActionResult<Produto> Delete(int id)
        {
            var produto = _appDbContext.Produtos.FirstOrDefault(p => p.Id == id);

            if (produto is null)
                return NotFound();

            _appDbContext.Produtos.Remove(produto);
            _appDbContext.SaveChanges();

            return Ok(produto);
        }
    }
}
