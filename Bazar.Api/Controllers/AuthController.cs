using Bazar.Application.DTOS;
using Bazar.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Bazar.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var result = await _authService.RegisterAsync(model);

            if (!result.Success) return BadRequest(result.Error);

            return Ok(result.Data);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var result = await _authService.LoginAsync(model);

            if (!result.Success) return Unauthorized(new { message = result.Error });

            return Ok(result.Data);
        }
    }
}