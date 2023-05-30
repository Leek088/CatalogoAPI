using CatalogoAPI.Context;
using CatalogoAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CatalogoAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriasController : ControllerBase
    {
        private readonly AppDbContext _appDbContext;

        public CategoriasController(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        [HttpGet("{id:int}/Produtos")]
        public async Task<ActionResult<IEnumerable<Produto>>> GetProdutosAsync(int id)
        {
            var queryProdutos = from categoria in _appDbContext.Categorias
                                join produtos in _appDbContext.Produtos
                                on categoria.Id equals produtos.CategoriaId
                                where categoria.Id == id
                                select produtos;

            var produtosDB = await queryProdutos.AsNoTracking().Take(10).ToListAsync();

            if (produtosDB.Any())
                return StatusCode(StatusCodes.Status200OK, produtosDB);

            return StatusCode(StatusCodes.Status404NotFound,
                    $"Nenhum produto encontrado, para a categoria de id {id}");

        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Categoria>>> GetAsync()
        {
            var categorias = await _appDbContext.Categorias.AsNoTracking().Take(10).ToListAsync();
            
            if (categorias.Any())
                return Ok(categorias);

            return StatusCode(StatusCodes.Status404NotFound,
                "Nenhuma categoria cadastrada");
        }

        [HttpGet("{id:int}", Name = "RecuperarCategoriaPorId")]
        public async Task<ActionResult<Categoria>> GetAsync(int id)
        {
            var categoria = await _appDbContext.Categorias
                            .AsNoTracking()
                            .FirstOrDefaultAsync(c => c.Id == id);

            if (categoria is null)
                return StatusCode(StatusCodes.Status404NotFound,
                    $"Nenhuma categoria de id {id} encontrada.");

            return StatusCode(StatusCodes.Status200OK, categoria);
        }

        [HttpPost]
        public async Task<ActionResult<Categoria>> PostAsync(Categoria categoria)
        {
            if (categoria is null)
                return BadRequest();

            await _appDbContext.Categorias.AddAsync(categoria);
            await _appDbContext.SaveChangesAsync();

            return StatusCode(StatusCodes.Status200OK, categoria);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<Categoria>> PutAsync(int id, Categoria categoria)
        {
            if (id != categoria.Id)
                return StatusCode(StatusCodes.Status400BadRequest);

            _appDbContext.Entry(categoria).State = EntityState.Modified;
            await _appDbContext.SaveChangesAsync();

            return new CreatedAtRouteResult("RecuperarCategoriaPorId",
                new { id = categoria.Id }, categoria);
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult<Categoria>> DeleteAsync(int id)
        {
            var categoria = await _appDbContext.Categorias.FirstOrDefaultAsync(c => c.Id == id);

            if (categoria is null)
                return StatusCode(StatusCodes.Status404NotFound);

            _appDbContext.Categorias.Remove(categoria);
            await _appDbContext.SaveChangesAsync();

            return StatusCode(StatusCodes.Status200OK, categoria);
        }
    }
}
