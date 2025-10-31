using Bazar.Application.DTOS;
using Bazar.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static Bazar.Application.Interfaces.IUserService;

namespace Bazar.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        // المصادقة (بدون مصادقة)
        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<ActionResult<UserDto>> RegisterAsync(RegisterDto registerDto)
        {
            var result = await _userService.RegisterAsync(registerDto);
            return result.Success ? Ok(result.Data) : BadRequest(result.Error);
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<ActionResult<LoginResponseDto>> LoginAsync(LoginDto loginDto)
        {
            var result = await _userService.LoginAsync(loginDto);
            return result.Success ? Ok(result.Data) : Unauthorized(result.Error);
        }

        // الملف الشخصي (مصادقة عادية)
        [HttpGet("profile/{username}")]
        [Authorize]
        public async Task<ActionResult<UserDto>> GetUserProfileAsync(string username)
        {
            var user = await _userService.GetUserProfileAsync(username);
            return Ok(user);
        }

        [HttpPut("profile/{username}")]
        [Authorize]
        public async Task<ActionResult> UpdateUserProfileAsync(string username, UserDto updateDto)
        {
            var result = await _userService.UpdateUserProfileAsync(username, updateDto);
            return result ? Ok("تم التحديث") : BadRequest("فشل التحديث");
        }

        [HttpGet("{username}/advertisements")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<AdvertisementsDto>>> GetUserAdvertisementsAsync(string username)
        {
            var ads = await _userService.GetUserAdvertisementsAsync(username);
            return Ok(ads);
        }

        [HttpGet("{username}/advertisements/count")]
        [Authorize]
        public async Task<ActionResult<int>> GetUserAdvertisementsCountAsync(string username)
        {
            var count = await _userService.GetUserAdvertisementsCountAsync(username);
            return Ok(count);
        }

        // الإدارة (للمدير فقط)
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetAllUsersAsync()
        {
            var users = await _userService.GetAllUsersAsync();
            return Ok(users);
        }

        [HttpGet("{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<UserDto>> GetUserByIdAsync(int id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            return Ok(user);
        }

        [HttpGet("email/{email}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<UserDto>> GetUserByEmailAsync(string email)
        {
            var user = await _userService.GetUserByEmailAsync(email);
            return Ok(user);
        }

        [HttpPut("{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> UpdateUserAsync(int id, UserDto updateDto)
        {
            var result = await _userService.UpdateUserAsync(id, updateDto);
            return result ? Ok("تم التحديث") : BadRequest("فشل التحديث");
        }

        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> DeleteUserAsync(int id)
        {
            var result = await _userService.DeleteUserAsync(id);
            return result ? Ok("تم الحذف") : BadRequest("فشل الحذف");
        }

        [HttpPut("{username}/deactivate")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> DeactivateUserAsync(string username)
        {
            var result = await _userService.DeactivateUserAsync(username);
            return result ? Ok("تم الإيقاف") : BadRequest("فشل الإيقاف");
        }

        [HttpPut("{username}/reactivate")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> ReactivateUserAsync(string username)
        {
            var result = await _userService.ReactivateUserAsync(username);
            return result ? Ok("تم التفعيل") : BadRequest("فشل التفعيل");
        }
    }
}
