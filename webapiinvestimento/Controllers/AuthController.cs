using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using webapiinvestimento.DTO;
using webapiinvestimento.Services;
using static webapiinvestimento.DTO.AuthDtos;

namespace webapiinvestimento.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IJwtService _jwtService;

        public AuthController(IJwtService jwtService)
        {
            _jwtService = jwtService;
        }


        // Login simples para obter um token JWT.

        [AllowAnonymous]
        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            if (request.Usuario == "cliente" && request.Senha == "123456")
            {
                var token = _jwtService.GerarToken(request.Usuario, "cliente");
                var expiraEm = _jwtService.ObterDataExpiracao();

                return Ok(new LoginResponse
                {
                    Token = token,
                    ExpiraEm = expiraEm
                });
            }
            return Unauthorized(new { mensagem = "Usuário ou senha inválidos." });
        }
    }
}
