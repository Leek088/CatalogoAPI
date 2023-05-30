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
        public ActionResult<IEnumerable<Produto>> GetProdutos(int id)
        {
            var queryProdutos = from categoria in _appDbContext.Categorias
                                join produtos in _appDbContext.Produtos
                                on categoria.Id equals produtos.CategoriaId
                                where categoria.Id == id
                                select produtos;

            var produtosDB = queryProdutos.AsNoTracking().Take(10).ToList();

            if (produtosDB.Any())
                return StatusCode(StatusCodes.Status200OK, produtosDB);

            return StatusCode(StatusCodes.Status404NotFound,
                    $"Nenhum produto encontrado, para a categoria de id {id}");

        }

        [HttpGet]
        public ActionResult<IEnumerable<Categoria>> Get()
        {
            var categorias = _appDbContext.Categorias.AsNoTracking().Take(10).ToList();
            
            if (categorias.Any())
                return Ok(categorias);

            return StatusCode(StatusCodes.Status404NotFound,
                "Nenhuma categoria cadastrada");
        }

        [HttpGet("{id:int}", Name = "RecuperarCategoriaPorId")]
        public ActionResult<Categoria> Get(int id)
        {
            var categoria = _appDbContext.Categorias
                            .AsNoTracking()
                            .FirstOrDefault(c => c.Id == id);

            if (categoria is null)
                return StatusCode(StatusCodes.Status404NotFound,
                    $"Nenhuma categoria de id {id} encontrada.");

            return StatusCode(StatusCodes.Status200OK, categoria);
        }

        [HttpPost]
        public ActionResult<Categoria> Post(Categoria categoria)
        {
            if (categoria is null)
                return BadRequest();

            _appDbContext.Categorias.Add(categoria);
            _appDbContext.SaveChanges();

            return StatusCode(StatusCodes.Status200OK, categoria);
        }

        [HttpPut("{id:int}")]
        public ActionResult<Categoria> Put(int id, Categoria categoria)
        {
            if (id != categoria.Id)
                return StatusCode(StatusCodes.Status400BadRequest);

            _appDbContext.Entry(categoria).State = EntityState.Modified;
            _appDbContext.SaveChanges();

            return new CreatedAtRouteResult("RecuperarCategoriaPorId",
                new { id = categoria.Id }, categoria);
        }

        [HttpDelete("{id:int}")]
        public ActionResult<Categoria> Delete(int id)
        {
            var categoria = _appDbContext.Categorias.FirstOrDefault(c => c.Id == id);

            if (categoria is null)
                return StatusCode(StatusCodes.Status404NotFound);

            _appDbContext.Categorias.Remove(categoria);
            _appDbContext.SaveChanges();

            return StatusCode(StatusCodes.Status200OK, categoria);
        }
    }
}
