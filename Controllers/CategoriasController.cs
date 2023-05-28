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

        [HttpGet]
        public ActionResult<IEnumerable<Categoria>> Get()
        {
            var categorias = _appDbContext.Categorias.ToList();
            return Ok(categorias);
        }

        [HttpGet("{id:int}", Name = "RecuperarCategoriaPorId")]
        public ActionResult<Categoria> Get(int id)
        {
            var categoria = _appDbContext.Categorias.FirstOrDefault(c => c.Id == id);

            if (categoria is null)
                return NotFound();

            return Ok(categoria);
        }

        [HttpPost]
        public ActionResult<Categoria> Post(Categoria categoria)
        {
            if (categoria is null)
                return BadRequest();

            _appDbContext.Categorias.Add(categoria);
            _appDbContext.SaveChanges();

            return Ok(categoria);
        }

        [HttpPut("{id:int}")]
        public ActionResult<Categoria> Put(int id, Categoria categoria)
        {
            if (id != categoria.Id)
                return BadRequest();

            _appDbContext.Entry(categoria).State = EntityState.Modified;
            _appDbContext.SaveChanges();

            return new CreatedAtRouteResult("RecuperarCategoriaPorId",
                new { id = categoria.Id }, categoria);
        }
    }
}
