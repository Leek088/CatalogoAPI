using CatalogoAPI.DTOs;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CatalogoAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AutorizaController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IConfiguration _config;

        public AutorizaController(UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            IConfiguration config)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _config = config;
        }

        [HttpGet]
        public ActionResult<string> Get()
        {
            return $"Teste de controlador -> Acessado em :: {DateTime.Now.ToLongDateString()}";
        }

        [HttpPost("register")]
        public async Task<ActionResult> RegisterUser([FromBody] UsuarioDTO usuarioDTO)
        {
            var user = new IdentityUser
            {
                UserName = usuarioDTO.UserName,
                Email = usuarioDTO.Email,
                EmailConfirmed = true
            };

            var result = await _userManager.CreateAsync(user, usuarioDTO.Password);

            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(user, isPersistent: false);
                return Ok(GerarToken(usuarioDTO));
            }

            return BadRequest(result.Errors);
        }

        [HttpPost("login")]
        public async Task<ActionResult> Login([FromBody] UsuarioDTO usuarioDTO)
        {
            var result = await _signInManager.PasswordSignInAsync(usuarioDTO.UserName,
                usuarioDTO.Password, isPersistent: false, lockoutOnFailure: false);

            if (result.Succeeded)
            {
                return Ok(GerarToken(usuarioDTO));
            }

            return BadRequest("Login inválido!");
        }

        private UsuarioToken GerarToken(UsuarioDTO usuarioDTO)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.UniqueName, usuarioDTO.Email!),
                new Claim("Usuario", usuarioDTO.UserName!),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var privateKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
            var digitalSignature = new SigningCredentials(privateKey, SecurityAlgorithms.HmacSha256);

            var expireHours = _config["TokenConfiguration:ExpireHours"];
            var expiration = DateTime.UtcNow.AddHours(double.Parse(expireHours!));

            JwtSecurityToken jwtSecurityToken = new JwtSecurityToken(
                issuer: _config["TokenConfiguration:Issuer"],
                audience: _config["TokenConfiguration:Audience"],
                claims: claims,
                expires: expiration,
                signingCredentials: digitalSignature
                );

            return new UsuarioToken()
            {
                Authenticated = true,
                Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken),
                Expiration = expiration,
                Message = "Token JWT gerado com sucesso."
            };
        }
    }
}
