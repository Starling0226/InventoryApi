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
            var result = _authService.Register(dto);
            
            return Ok(result);
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginDto dto)
        {
            var result = _authService.Login(dto);
            
            return Ok(result);
        }
    }
}