using Bazar.Application.DTOS;
using Bazar.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bazar.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AdvertisementsController : ControllerBase
    {
        private readonly IAdvertisementsService _advertisementsService;
        public AdvertisementsController(IAdvertisementsService advertisementsService)
            => _advertisementsService = advertisementsService;
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllAdvertisementsAsync()
        {
            var advertisements = await _advertisementsService.GetAllAdvertisementsAsync();
            return Ok(advertisements);
        }
        [HttpGet("Id/{id:int}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAdvertisementByIdAsync(int id)
        {
            var advertisement = await _advertisementsService.GetAdvertisementByIdAsync(id);
            if (advertisement == null)
                return NotFound();
            return Ok(advertisement);
        }
        [HttpGet("User/{username}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAdvertisementsByUserAsync(string username)
        {
            var advertisements = await _advertisementsService.GetAdvertisementsByUserAsync(username);
            return Ok(advertisements);
        }
        [HttpGet("Category/{categoryname}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAdvertisementsByCategoryAsync(string categoryname)
        {
            var advertisements = await _advertisementsService.GetAdvertisementsByCategoryAsync(categoryname);
            return Ok(advertisements);
        }
        [HttpGet("Search/{searchterm}")]
        [AllowAnonymous]
        public async Task<IActionResult> SearchAdvertisementsAsync(string searchterm)
        {
            var advertisements = await _advertisementsService.SearchAdvertisementsAsync(searchterm);
            return Ok(advertisements);
        }
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateAdvertisementAsync([FromBody] AdvertisementsDto advertisements)
        {
            await _advertisementsService.CreateAdvertisementAsync(advertisements);
            return CreatedAtAction(nameof(GetAdvertisementByIdAsync), new { id = advertisements.Id }, advertisements);
        }
        [HttpDelete("Delete/{id:int}")]
        [Authorize]
        public async Task<IActionResult> DeleteAdvertisementAsync(int id)
        {
            var result = await _advertisementsService.DeleteAdvertisementAsync(id);
            if (!result)
                return NotFound();
            return NoContent();
        }
        [HttpPut("Update/{id:int}")]
        [Authorize]
        public async Task<IActionResult> UpdateAdvertisementAsync(int id, [FromBody] AdvertisementsDto advertisements)
        {
            var result = await _advertisementsService.UpdateAdvertisementAsync(id, advertisements);
            if (!result)
                return NotFound();
            return NoContent();
        }
        [HttpGet("Count")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAdvertisementsCountAsync()
        {
            var count = await _advertisementsService.GetAdvertisementsCountAsync();
            return Ok(count);

        }
    }
}
