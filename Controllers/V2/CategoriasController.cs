using Microsoft.AspNetCore.Mvc;

namespace CatalogoAPI.Controllers.V2
{
    [Produces("application/json")]
    [ApiVersion("2.0")]    
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class CategoriasController : ControllerBase
    {

        /// <summary>
        /// Acessa a api V2 como teste
        /// </summary>
        /// <remarks>Um teste para verificar se a API v2 está funionando.</remarks>
        /// <returns>Retorna uma string de teste para verificar o funcionamento da API</returns>
        [HttpGet]
        public ActionResult<string> Get()
        {
            return "Acessado a api versão 2";
        }

        [HttpGet("{id:int}")]
        public ActionResult<string> GetById(int id)
        {
            return $"Acessado a api versão 2 com o id: {id}";
        }
    }
}
