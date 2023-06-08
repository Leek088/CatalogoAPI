using CatalogoAPI.Models;
using CatalogoAPI.Repositories;
using CatalogoAPI.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CatalogoAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriasController : ControllerBase
    {
        private readonly IUnityOfWork _unityOfWork;

        public CategoriasController(IUnityOfWork unityOfWork)
        {
            _unityOfWork = unityOfWork;
        }

        [HttpGet("Produtos")]
        public ActionResult<IEnumerable<Produto>> GetCategoriasComProdutos()
        {
            var categoriasComProdutos = _unityOfWork.CategoriaRepository.GetCategoriasComProdutos();

            if (categoriasComProdutos.Any())
                return StatusCode(StatusCodes.Status200OK, categoriasComProdutos);

            return StatusCode(StatusCodes.Status404NotFound,
                    $"Nenhum resultado encontrado");

        }

        [HttpGet]
        public ActionResult<IEnumerable<Categoria>> Get()
        {
            var categorias = _unityOfWork.CategoriaRepository.GET();

            if (categorias.Any())
                return Ok(categorias);

            return StatusCode(StatusCodes.Status404NotFound,
                "Nenhuma categoria cadastrada");
        }

        [HttpGet("{id:int}", Name = "RecuperarCategoriaPorId")]
        public ActionResult<Categoria> Get(int id)
        {
            var categoria = _unityOfWork.CategoriaRepository.GetById(c => c.Id == id);

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

            _unityOfWork.CategoriaRepository.Add(categoria);
            _unityOfWork.Commit();

            return StatusCode(StatusCodes.Status200OK, categoria);
        }

        [HttpPut("{id:int}")]
        public ActionResult<Categoria> Put(int id, Categoria categoria)
        {
            if (id != categoria.Id)
                return StatusCode(StatusCodes.Status400BadRequest);

            _unityOfWork.CategoriaRepository.Update(categoria);
            _unityOfWork.Commit();

            return new CreatedAtRouteResult("RecuperarCategoriaPorId",
                new { id = categoria.Id }, categoria);
        }

        [HttpDelete("{id:int}")]
        public ActionResult<Categoria> Delete(int id)
        {
            var categoria = _unityOfWork.CategoriaRepository.GetById(c => c.Id == id);

            if (categoria is null)
                return StatusCode(StatusCodes.Status404NotFound);

            _unityOfWork.CategoriaRepository.Delete(categoria);
            _unityOfWork.Commit();

            return StatusCode(StatusCodes.Status200OK, categoria);
        }
    }
}
