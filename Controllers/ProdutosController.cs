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
        public async Task<ActionResult<IEnumerable<Produto>>> GetAsync()
        {
            var queryProdutos = from produtos in _appDbContext.Produtos
                                join categoria in _appDbContext.Categorias
                                on produtos.CategoriaId equals categoria.Id
                                select new { produtos, NomeCategoria = categoria.Nome };

            var produtosDB = await queryProdutos.AsNoTracking().Take(10).ToListAsync();

            if (produtosDB.Any())
                return Ok(produtosDB);

            return StatusCode(StatusCodes.Status204NoContent,
                "Nenhum produdo cadastrado.");
        }

        [HttpGet("{id:int}", Name = "OberProdutoPorId")]
        public async Task<ActionResult<Produto>> GetAsync(int id)
        {

            var queryProduto = from produto in _appDbContext.Produtos
                               join categoria in _appDbContext.Categorias
                               on produto.CategoriaId equals categoria.Id
                               where produto.Id == id
                               select new { produto, NomeCategoria = categoria.Nome };

            var produtoDB = await queryProduto.AsNoTracking().ToListAsync();

            if (produtoDB.Any())
                return Ok(produtoDB);

            return NotFound($"Produdo de id {id}, não encontrado");
        }

        [HttpPost]
        public async Task<ActionResult<Produto>> PostAsync(Produto produto)
        {
            if (produto is null)
                return BadRequest("Nenhum produto recebido");

            await _appDbContext.AddAsync(produto);
            await _appDbContext.SaveChangesAsync();

            return new CreatedAtRouteResult("OberProdutoPorId", new { id = produto.Id }, produto);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<Produto>> PutAsync(int id, Produto produto)
        {
            if (id != produto.Id)
                return BadRequest();

            _appDbContext.Entry(produto).State = EntityState.Modified;
            await _appDbContext.SaveChangesAsync();

            return Ok(produto);
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult<Produto>> DeleteAsync(int id)
        {
            var produto = await _appDbContext.Produtos.FirstOrDefaultAsync(p => p.Id == id);

            if (produto is null)
                return NotFound();

            _appDbContext.Produtos.Remove(produto);
            await _appDbContext.SaveChangesAsync();

            return Ok(produto);
        }
    }
}
