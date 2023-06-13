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
        public ActionResult<IQueryable<ProdutoDTO>> GetProductsByCategoryId(int id)
        {
            var produtoDB = _unityOfWork.ProdutoRepository.GetProductsByCategoryId(p => p.CategoriaId == id);

            if (produtoDB is null || !produtoDB.Any())
            {
                return NotFound("Nenhum produto encontrado");

            }

            var produtoDTO = _mapper.Map<List<ProdutoDTO>>(produtoDB);
            return Ok(produtoDB);
        }

        [HttpGet("Preco/Menor")]
        public ActionResult<IQueryable<ProdutoDTO>> GetProductsBySortLowPrice()
        {
            var produtoDB = _unityOfWork.ProdutoRepository.GetProductsBySortLowPrice();

            if (produtoDB is null || !produtoDB.Any())
            {
                return NotFound("Nenhum produto encontrado");

            }

            var produtoDTO = _mapper.Map<List<ProdutoDTO>>(produtoDB);
            return Ok(produtoDTO);
        }

        [HttpGet]
        public ActionResult<IQueryable<ProdutoDTO>> Get()
        {
            var produtosDB = _unityOfWork.ProdutoRepository.GET();

            if (produtosDB is null || !produtosDB.Any())
            {
                return StatusCode(StatusCodes.Status204NoContent,
                "Nenhum produdo cadastrado.");

            }

            var produtosDTO = _mapper.Map<List<ProdutoDTO>>(produtosDB);
            return Ok(produtosDTO);
        }

        [HttpGet("Paginado")]
        public ActionResult<IQueryable<ProdutoDTO>> GetProductsPaginated([FromQuery] ProdutosParameters produtosParameters)
        {
            var produtos = _unityOfWork.ProdutoRepository.GetAllPaginated(produtosParameters);

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
        public ActionResult<ProdutoDTO> Get(int id)
        {
            var produtoDB = _unityOfWork.ProdutoRepository.GetById(p => p.Id == id);

            if (produtoDB is null)
                return NotFound($"Produdo de id {id}, não encontrado");

            var produtoDTO = _mapper.Map<ProdutoDTO>(produtoDB);
            return Ok(produtoDTO);

        }

        [HttpPost]
        public ActionResult<ProdutoDTO> Post(ProdutoDTO produtoDTO)
        {
            if (produtoDTO is null)
                return BadRequest("Nenhum produto recebido");

            var produtoDB = _mapper.Map<Produto>(produtoDTO);

            _unityOfWork.ProdutoRepository.Add(produtoDB);
            _unityOfWork.Commit();

            produtoDTO = _mapper.Map<ProdutoDTO>(produtoDB);

            return new CreatedAtRouteResult("OberProdutoPorId", new { id = produtoDTO.Id }, produtoDTO);
        }

        [HttpPut("{id:int}")]
        public ActionResult<ProdutoDTO> Put(int id, ProdutoDTO produtoDTO)
        {
            if (produtoDTO is null || id != produtoDTO.Id)
                return BadRequest();

            var produtoDB = _mapper.Map<Produto>(produtoDTO);

            _unityOfWork.ProdutoRepository.Update(produtoDB);
            _unityOfWork.Commit();

            return Ok(produtoDTO);
        }

        [HttpDelete("{id:int}")]
        public ActionResult<ProdutoDTO> Delete(int id)
        {
            var produtoDB = _unityOfWork.ProdutoRepository.GetById(p => p.Id == id);

            if (produtoDB is null)
                return NotFound();

            _unityOfWork.ProdutoRepository.Delete(produtoDB);
            _unityOfWork.Commit();

            var produtoDTO = _mapper.Map<ProdutoDTO>(produtoDB);

            return Ok(produtoDTO);
        }
    }
}
