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
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    [ApiController]
    public class ProdutosController : ControllerBase
    {
        private readonly IUnityOfWork _unityOfWork;
        private readonly IMapper _mapper;

        public ProdutosController(IUnityOfWork unityOfWork, IMapper mapper)
        {
            _unityOfWork = unityOfWork;
            _mapper = mapper;
        }

        [HttpGet("Categoria/{id:int}")]
        public async Task<ActionResult<IQueryable<ProdutoDTO>>> GetProductsByCategoryIdAsync(int id)
        {
            var produtoDB = await _unityOfWork.ProdutoRepository.GetProductsByCategoryId(p => p.CategoriaId == id).ToListAsync();

            if (produtoDB is null || !produtoDB.Any())
            {
                return NotFound("Nenhum produto encontrado");
            }

            var produtoDTO = _mapper.Map<List<ProdutoDTO>>(produtoDB);
            return Ok(produtoDB);
        }

        [HttpGet("Preco/Menor")]
        public async Task<ActionResult<IQueryable<ProdutoDTO>>> GetProductsBySortLowPriceAsync()
        {
            var produtoDB = await _unityOfWork.ProdutoRepository.GetProductsBySortLowPrice().ToListAsync();

            if (produtoDB is null || !produtoDB.Any())
            {
                return NotFound("Nenhum produto encontrado");
            }

            var produtoDTO = _mapper.Map<List<ProdutoDTO>>(produtoDB);
            return Ok(produtoDTO);
        }

        [HttpGet]
        public async Task<ActionResult<IQueryable<ProdutoDTO>>> GetAsync()
        {
            var produtosDB = await _unityOfWork.ProdutoRepository.Get().ToListAsync();

            if (produtosDB is null || !produtosDB.Any())
            {
                return StatusCode(StatusCodes.Status204NoContent,
                "Nenhum produdo cadastrado.");
            }

            var produtosDTO = _mapper.Map<List<ProdutoDTO>>(produtosDB);
            return Ok(produtosDTO);
        }

        [HttpGet("Paginado")]
        public async Task<ActionResult<IQueryable<ProdutoDTO>>> GetProductsPaginatedAsync([FromQuery] ProdutosParameters produtosParameters)
        {
            var produtos = await _unityOfWork.ProdutoRepository.GetAllPaginatedAsync(produtosParameters);

            var metadata = new
            {
                produtos.TotalCount,
                produtos.PageSize,
                produtos.CurrentPage,
                produtos.TotalPages,
                produtos.HasNext,
                produtos.HasPrevious
            };

            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));

            var produtosDTO = _mapper.Map<List<ProdutoDTO>>(produtos);

            return Ok(produtosDTO);
        }

        [HttpGet("{id:int}", Name = "OberProdutoPorId")]
        public async Task<ActionResult<ProdutoDTO>> GetAsync(int id)
        {
            var produtoDB = await _unityOfWork.ProdutoRepository.GetByIdAsync(p => p.Id == id);

            if (produtoDB is null)
                return NotFound($"Produdo de id {id}, não encontrado");

            var produtoDTO = _mapper.Map<ProdutoDTO>(produtoDB);
            return Ok(produtoDTO);
        }

        [HttpPost]
        public async Task<ActionResult<ProdutoDTO>> PostAsync(ProdutoDTO produtoDTO)
        {
            if (produtoDTO is null)
                return BadRequest("Nenhum produto recebido");

            var produtoDB = _mapper.Map<Produto>(produtoDTO);

            _unityOfWork.ProdutoRepository.Add(produtoDB);
            await _unityOfWork.CommitAsync();

            produtoDTO = _mapper.Map<ProdutoDTO>(produtoDB);

            return new CreatedAtRouteResult("OberProdutoPorId", new { id = produtoDTO.Id }, produtoDTO);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<ProdutoDTO>> PutAsync(int id, ProdutoDTO produtoDTO)
        {
            if (produtoDTO is null || id != produtoDTO.Id)
                return BadRequest();

            var produtoDB = _mapper.Map<Produto>(produtoDTO);

            _unityOfWork.ProdutoRepository.Update(produtoDB);
            await _unityOfWork.CommitAsync();

            return Ok(produtoDTO);
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult<ProdutoDTO>> DeleteAsync(int id)
        {
            var produtoDB = await _unityOfWork.ProdutoRepository.GetByIdAsync(p => p.Id == id);

            if (produtoDB is null)
                return NotFound();

            _unityOfWork.ProdutoRepository.Delete(produtoDB);
            await _unityOfWork.CommitAsync();

            var produtoDTO = _mapper.Map<ProdutoDTO>(produtoDB);

            return Ok(produtoDTO);
        }
    }
}
