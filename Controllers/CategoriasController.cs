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
    [Produces("application/json")]
    [ApiVersion("1", Deprecated = true)]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/v{version:apiVersion}/[controller]")]    
    [ApiController]
    [ApiConventionType(typeof(DefaultApiConventions))]
    public class CategoriasController : ControllerBase
    {
        private readonly IUnityOfWork _unityOfWork;
        private readonly IMapper _mapper;

        public CategoriasController(IUnityOfWork unityOfWork, IMapper mapper)
        {
            _unityOfWork = unityOfWork;
            _mapper = mapper;
        }

        /// <summary>
        /// Recupera as Categorias com seus devidos produtos
        /// </summary>
        /// <remarks>
        /// Exemplo de retorno:
        ///
        ///     GET api/v1/Categorias/Produtos
        ///     {
        ///         "id": 0,
        ///         "nome": "string",
        ///         "imagemUrl": "string",
        ///         "produtos": 
        ///         [
        ///             {
        ///                 "id": 0,
        ///                 "nome": "string",
        ///                 "descricao": "string",
        ///                 "preco": 0,
        ///                 "imagemUrl": "string",
        ///                 "categoriaId": 0
        ///             }
        ///         ]
        ///     }
        /// </remarks>
        /// <response code="200">(200) Retorna com sucesso, a lista de objetos CategoriaDTO</response>
        /// <response code="404">(404) Retorna "Não encontrado", se a lista for vazia ou nula</response>        
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

        /// <summary>
        /// Recupera todas as Categorias do banco
        /// </summary>
        /// <remarks>
        /// Exemplo de retorno:
        ///
        ///     GET api/v1/Categorias/
        ///     {
        ///         "id": 0,
        ///         "nome": "string",
        ///         "imagemUrl": "string",
        ///         "produtos": []
        ///     }
        /// </remarks>
        /// <response code="200">(200) Retorna com sucesso, a lista de objetos CategoriaDTO</response>
        /// <response code="404">(404) Retorna "Não encontrado", se a lista for vazia ou nula</response>
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

        /// <summary>
        /// Recupera as Categorias baseado na paginação
        /// </summary>
        /// <remarks>        
        /// Exemplo de retorno
        ///
        ///     GET api/v1/Categorias/Paginada
        ///     {
        ///         "id": 0,
        ///         "nome": "string",
        ///         "imagemUrl": "string",
        ///         "produtos": []
        ///     }
        /// </remarks>
        /// <response code="200">(200) Retorna com sucesso, a lista de objetos CategoriaDTO</response>
        /// <response code="404">(404) Retorna "Não encontrado", se a lista for vazia ou nula</response>
        [HttpGet("Paginada")]
        public async Task<ActionResult<IQueryable<CategoriaDTO>>> GetCategoryPaginatedAsync([FromQuery] CategoriaParameters categoriaParameters)
        {
            var categorias = await _unityOfWork.CategoriaRepository.GetAllPaginatedAsync(categoriaParameters);

            if (categorias is null)
                return NotFound();

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

        /// <summary>
        /// Recupera as Categorias baseado no ID
        /// </summary>
        /// <param name="id">ID da categoria</param>
        /// <remarks>        
        /// Exemplo de retorno:
        ///
        ///     GET api/v1/Categorias/{id}
        ///     {
        ///         "id": 0,
        ///         "nome": "string",
        ///         "imagemUrl": "string",
        ///         "produtos": []
        ///     }
        /// </remarks>
        /// <response code="200">(200) Retorna com sucesso, a lista de objetos CategoriaDTO</response>
        /// <response code="404">(404) Retorna "Não encontrado", se a lista for vazia ou nula</response>
        [HttpGet("{id:int}", Name = "RecuperarCategoriaPorId")]
        public async Task<ActionResult<CategoriaDTO>> GetAsyncById(int id)
        {
            var categoriaDB = await _unityOfWork.CategoriaRepository.GetByIdAsync(c => c.Id == id);

            if (categoriaDB is null)
                return StatusCode(StatusCodes.Status404NotFound,
                    $"Nenhuma categoria de id {id} encontrada.");

            var categoriaDTO = _mapper.Map<CategoriaDTO>(categoriaDB);

            return StatusCode(StatusCodes.Status200OK, categoriaDTO);
        }

        /// <summary>
        /// Cria uma nova categoria
        /// </summary>
        /// <remarks>
        /// Exemplo de requisição, enviando uma categoriaDTO:
        ///
        ///     POST /api/v1/Categorias
        ///     {
        ///         "nome": "categoria nome",
        ///         "imagemUrl": "imagem.jpg"
        ///     }        
        /// </remarks>
        /// <response code="201">(201) Retorna uma nova categoriaDTO criada</response>
        /// <response code="400">(400) categoriaDTO recebida é nula</response>
        [HttpPost]
        public async Task<ActionResult<CategoriaDTO>> PostAsync(CategoriaDTO categoriaDTO)
        {
            if (categoriaDTO is null)
                return StatusCode(StatusCodes.Status400BadRequest);

            var categoriaDB = _mapper.Map<Categoria>(categoriaDTO);

            _unityOfWork.CategoriaRepository.Add(categoriaDB);
            await _unityOfWork.CommitAsync();

            categoriaDTO = _mapper.Map<CategoriaDTO>(categoriaDB);

            return StatusCode(StatusCodes.Status201Created, categoriaDTO);
        }

        /// <summary>
        /// Atualiza uma categoria por ID
        /// </summary>
        /// <param name="id">ID da categoria</param>
        /// <param name="categoriaDTO">Parametros da categoria a ser atualizada</param>
        /// <returns>Retorna a categoria atualizada</returns>
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

        /// <summary>
        /// Deleta uma categoria por ID
        /// </summary>
        /// <param name="id">ID da categoria</param>
        /// <returns>Retorna a categoriaDTO que foi deletada</returns>
        /// <response code="200">(200) Retorna a categoriaDTO deletada</response>
        /// <response code="404">(404) Id da Categoria a ser deletada, não foi encontrada</response>
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
