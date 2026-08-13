using InventoryApi.Modules.Auth.Dtos;
using InventoryApi.Modules.Auth.Services;
using Microsoft.AspNetCore.Mvc;

namespace InventoryApi.Modules.Auth.Controllers
{
    [ApiController]
    [Route("auth")]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;

        public AuthController(AuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public IActionResult Register([FromBody] RegisterDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var result = _authService.Register(dto);
            if (result == null) return BadRequest(new { Message = "El correo ya está registrado." });

            return Ok(result);
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var result = _authService.Login(dto);
            if (result == null) return Unauthorized(new { Message = "Credenciales incorrectas." });

            return Ok(result);
        }
    }
}