using AutoMapper;
using CatalogoAPI.DTOs;
using CatalogoAPI.Models;
using CatalogoAPI.Pagination;
using CatalogoAPI.Repositories.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace CatalogoAPI.Controllers
{
    [ApiVersion("1")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
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
        public async Task<ActionResult<IQueryable<CategoriaDTO>>> GetCategoriasComProdutosAsync()
        {
            var categoriasDB = await _unityOfWork.CategoriaRepository.GetCategoriasComProdutos().ToListAsync();

            if (categoriasDB is null || !categoriasDB.Any())
                return StatusCode(StatusCodes.Status404NotFound,
                 $"Nenhum resultado encontrado");

            var categoriasDTO = _mapper.Map<List<CategoriaDTO>>(categoriasDB);

            return StatusCode(StatusCodes.Status200OK, categoriasDTO);
        }

        [HttpGet]
        public async Task<ActionResult<IQueryable<CategoriaDTO>>> GetAsync()
        {
            var categoriasDB = await _unityOfWork.CategoriaRepository.Get().ToListAsync();

            if (categoriasDB is null || !categoriasDB.Any())
                return StatusCode(StatusCodes.Status404NotFound,
                    "Nenhuma categoria cadastrada");

            var categoriasDTO = _mapper.Map<List<CategoriaDTO>>(categoriasDB);

            return Ok(categoriasDTO);
        }

        [HttpGet("Paginada")]
        public async Task<ActionResult<IQueryable<CategoriaDTO>>> GetCategoryPaginatedAsync([FromQuery] CategoriaParameters categoriaParameters)
        {
            var categorias = await _unityOfWork.CategoriaRepository.GetAllPaginatedAsync(categoriaParameters);

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
        public async Task<ActionResult<CategoriaDTO>> GetAsync(int id)
        {
            var categoriaDB = await _unityOfWork.CategoriaRepository.GetByIdAsync(c => c.Id == id);

            if (categoriaDB is null)
                return StatusCode(StatusCodes.Status404NotFound,
                    $"Nenhuma categoria de id {id} encontrada.");

            var categoriaDTO = _mapper.Map<CategoriaDTO>(categoriaDB);

            return StatusCode(StatusCodes.Status200OK, categoriaDTO);
        }

        [HttpPost]
        public async Task<ActionResult<CategoriaDTO>> PostAsync(CategoriaDTO categoriaDTO)
        {
            if (categoriaDTO is null)
                return BadRequest();

            var categoriaDB = _mapper.Map<Categoria>(categoriaDTO);

            _unityOfWork.CategoriaRepository.Add(categoriaDB);
            await _unityOfWork.CommitAsync();

            categoriaDTO = _mapper.Map<CategoriaDTO>(categoriaDB);

            return StatusCode(StatusCodes.Status200OK, categoriaDTO);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<CategoriaDTO>> PutAsync(int id, CategoriaDTO categoriaDTO)
        {
            if (categoriaDTO is null || id != categoriaDTO.Id)
                return StatusCode(StatusCodes.Status400BadRequest);

            var categoriaDB = _mapper.Map<Categoria>(categoriaDTO);

            _unityOfWork.CategoriaRepository.Update(categoriaDB);
            await _unityOfWork.CommitAsync();

            categoriaDTO = _mapper.Map<CategoriaDTO>(categoriaDB);

            return new CreatedAtRouteResult("RecuperarCategoriaPorId",
                new { id = categoriaDTO.Id }, categoriaDTO);
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult<CategoriaDTO>> DeleteAsync(int id)
        {
            var categoriaDB = await _unityOfWork.CategoriaRepository.GetByIdAsync(c => c.Id == id);

            if (categoriaDB is null)
                return StatusCode(StatusCodes.Status404NotFound);

            _unityOfWork.CategoriaRepository.Delete(categoriaDB);
            await _unityOfWork.CommitAsync();

            var categoriaDTO = _mapper.Map<CategoriaDTO>(categoriaDB);

            return StatusCode(StatusCodes.Status200OK, categoriaDTO);
        }
    }
}
