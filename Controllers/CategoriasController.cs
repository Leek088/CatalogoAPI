using AutoMapper;
using CatalogoAPI.DTOs;
using CatalogoAPI.Models;
using CatalogoAPI.Pagination;
using CatalogoAPI.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace CatalogoAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriasController : ControllerBase
    {
        private readonly IUnityOfWork _unityOfWork;
        private readonly IMapper _mapper;

        public CategoriasController(IUnityOfWork unityOfWork, IMapper mapper)
        {
            _unityOfWork = unityOfWork;
            _mapper = mapper;
        }

        [HttpGet("Produtos")]
        public ActionResult<IQueryable<CategoriaDTO>> GetCategoriasComProdutos()
        {
            var categoriasDB = _unityOfWork.CategoriaRepository.GetCategoriasComProdutos();

            if (categoriasDB is null || !categoriasDB.Any())
                return StatusCode(StatusCodes.Status404NotFound,
                 $"Nenhum resultado encontrado");

            var categoriasDTO = _mapper.Map<List<CategoriaDTO>>(categoriasDB);

            return StatusCode(StatusCodes.Status200OK, categoriasDTO);
        }

        [HttpGet]
        public ActionResult<IQueryable<CategoriaDTO>> Get()
        {
            var categoriasDB = _unityOfWork.CategoriaRepository.GET();

            if (categoriasDB is null || !categoriasDB.Any())
            return StatusCode(StatusCodes.Status404NotFound,
                "Nenhuma categoria cadastrada");

            var categoriasDTO = _mapper.Map<List<CategoriaDTO>>(categoriasDB);

            return Ok(categoriasDTO);
        }

        [HttpGet("Paginada")]
        public ActionResult<IQueryable<CategoriaDTO>> GetCategoryPaginated([FromQuery] CategoriaParameters categoriaParameters)
        {
            var categorias = _unityOfWork.CategoriaRepository.GetAllPaginated(categoriaParameters);

            var metadata = new
            {
                categorias.TotalCount,
                categorias.PageSize,
                categorias.CurrentPage,
                categorias.TotalPages,
                categorias.HasNext,
                categorias.HasPrevious
            };

            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));

            var categoriasDTO = _mapper.Map<List<CategoriaDTO>>(categorias);

            return Ok(categoriasDTO);
        }

        [HttpGet("{id:int}", Name = "RecuperarCategoriaPorId")]
        public ActionResult<CategoriaDTO> Get(int id)
        {
            var categoriaDB = _unityOfWork.CategoriaRepository.GetById(c => c.Id == id);

            if (categoriaDB is null)
                return StatusCode(StatusCodes.Status404NotFound,
                    $"Nenhuma categoria de id {id} encontrada.");

            var categoriaDTO = _mapper.Map<CategoriaDTO>(categoriaDB);

            return StatusCode(StatusCodes.Status200OK, categoriaDTO);
        }

        [HttpPost]
        public ActionResult<CategoriaDTO> Post(CategoriaDTO categoriaDTO)
        {
            if (categoriaDTO is null)
                return BadRequest();

            var categoriaDB = _mapper.Map<Categoria>(categoriaDTO);

            _unityOfWork.CategoriaRepository.Add(categoriaDB);
            _unityOfWork.Commit();

            categoriaDTO = _mapper.Map<CategoriaDTO>(categoriaDB);

            return StatusCode(StatusCodes.Status200OK, categoriaDTO);
        }

        [HttpPut("{id:int}")]
        public ActionResult<CategoriaDTO> Put(int id, CategoriaDTO categoriaDTO)
        {
            if (categoriaDTO is null || id != categoriaDTO.Id)
                return StatusCode(StatusCodes.Status400BadRequest);

            var categoriaDB = _mapper.Map<Categoria>(categoriaDTO);

            _unityOfWork.CategoriaRepository.Update(categoriaDB);
            _unityOfWork.Commit();

            categoriaDTO = _mapper.Map<CategoriaDTO>(categoriaDB);

            return new CreatedAtRouteResult("RecuperarCategoriaPorId",
                new { id = categoriaDTO.Id }, categoriaDTO);
        }

        [HttpDelete("{id:int}")]
        public ActionResult<CategoriaDTO> Delete(int id)
        {
            var categoriaDB = _unityOfWork.CategoriaRepository.GetById(c => c.Id == id);

            if (categoriaDB is null)
                return StatusCode(StatusCodes.Status404NotFound);

            _unityOfWork.CategoriaRepository.Delete(categoriaDB);
            _unityOfWork.Commit();

            var categoriaDTO = _mapper.Map<CategoriaDTO>(categoriaDB);

            return StatusCode(StatusCodes.Status200OK, categoriaDTO);
        }
    }
}
