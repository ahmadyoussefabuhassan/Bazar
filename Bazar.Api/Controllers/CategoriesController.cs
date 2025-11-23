using Bazar.Application.DTOS;
using Bazar.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bazar.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        // GET: api/Category (عام للجميع)
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _categoryService.GetAllAsync();
            return Ok(result.Data); // Assuming Result wrapper or direct IEnumerable
        }

        // POST: api/Category (للأدمن فقط)
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([FromBody] CategoryDto dto)
        {
            var result = await _categoryService.CreateAsync(dto.Name);

            if (!result.Success) return BadRequest(result.Error);

            return Ok(result.Data);
        }
    }
}