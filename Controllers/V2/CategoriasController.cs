using Microsoft.AspNetCore.Mvc;

namespace CatalogoAPI.Controllers.V2
{
    [ApiVersion("2")]    
    [Route("api/V2/[controller]")]
    [ApiController]
    public class CategoriasController : ControllerBase
    {
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
