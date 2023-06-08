using AutoMapper;
using CatalogoAPI.DTOs;
using CatalogoAPI.Models;
using CatalogoAPI.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

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
        public ActionResult<IQueryable<ProdutoDTO>> GetProdutosPorCategoria(int id)
        {
            var produtoDB = _unityOfWork.ProdutoRepository.GetProdutosPorCategoria(p => p.CategoriaId == id);

            if (produtoDB.Any())
            {
                var produtoDTO = _mapper.Map<List<ProdutoDTO>>(produtoDB);
                return Ok(produtoDB);
            }

            return NotFound("Nenhum produto encontrado");
        }

        [HttpGet("Preco")]
        public ActionResult<IQueryable<ProdutoDTO>> GetProdutoOrderByPreco()
        {
            var produtoDB = _unityOfWork.ProdutoRepository.GetProdutosPorPreco();

            if (produtoDB.Any())
            {
                var produtoDTO = _mapper.Map<List<ProdutoDTO>>(produtoDB);
                return Ok(produtoDTO);
            }

            return NotFound("Nenhum produto encontrado");
        }

        [HttpGet]
        public ActionResult<IQueryable<ProdutoDTO>> Get()
        {
            var produtosDB = _unityOfWork.ProdutoRepository.GET();

            if (produtosDB.Any())
            {
                var produtosDTO = _mapper.Map<List<ProdutoDTO>>(produtosDB);
                return Ok(produtosDTO);
            }

            return StatusCode(StatusCodes.Status204NoContent,
                "Nenhum produdo cadastrado.");
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

            return new CreatedAtRouteResult("OberProdutoPorId", new { id = produtoDTO.Id }, produtoDTO);
        }

        [HttpPut("{id:int}")]
        public ActionResult<ProdutoDTO> Put(int id, ProdutoDTO produtoDTO)
        {
            if (id != produtoDTO.Id)
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
