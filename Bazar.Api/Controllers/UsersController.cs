using Bazar.Application.DTOS;
using Bazar.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Bazar.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [Authorize]
        [HttpGet("profile")]
        public async Task<IActionResult> GetMyProfile()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null) return Unauthorized();

            var userId = int.Parse(userIdClaim.Value);

            var result = await _userService.GetUserProfileAsync(userId);

            if (!result.Success) return BadRequest(result.Error);

            return Ok(result.Data);
        }

        [Authorize]
        [HttpPut("profile")]
        public async Task<IActionResult> UpdateProfile([FromForm] UpdateUserProfileDto model)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null) return Unauthorized();

            var userId = int.Parse(userIdClaim.Value);

            var result = await _userService.UpdateProfileAsync(userId, model);

            if (!result.Success) return BadRequest(result.Error);

            return Ok(result.Data);
        }
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetSellerProfile(int id)
        {
            var result = await _userService.GetUserProfileAsync(id);

            if (!result.Success) return NotFound(result.Error);

            return Ok(result.Data);
        }
    }
}