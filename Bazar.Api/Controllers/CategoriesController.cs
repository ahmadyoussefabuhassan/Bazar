using Bazar.Application.DTOS;
using Bazar.Application.Interfaces;
using Bazar.Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bazar.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        public CategoriesController(ICategoryService categoryService)
            => _categoryService = categoryService;

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllCategories()
        {
            var categories = await _categoryService.GetAllCategoriesAsync();
            return Ok(categories);
        }

        [HttpGet("name/{name}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetCategoryByName(string name)
        {
            var category = await _categoryService.GetCategoryByNameAsync(name);
            if (category == null)
            {
                return NotFound();
            }
            return Ok(category);
        }
        [HttpGet("Id/{id:int}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetCategoryById(int id)
        {
            var category = await _categoryService.GetCategoryByIdAsync(id);
            if (category == null)
            {
                return NotFound();
            }
            return Ok(category);
        }
        [HttpGet("exists/{name}")]
        [AllowAnonymous]
        public async Task<IActionResult> CheckCategoryExists(string name)
        {
            var exists = await _categoryService.CategoryExistsAsync(name);
            return Ok(new { exists = exists });
        }
        [HttpGet("popular/{count:int}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetPopularCategories(int count)
        {
            var categories = await _categoryService.GetPopularCategoriesAsync(count);
            return Ok(categories);
        }
        [HttpGet("admin/statistics/advertisements/{count:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetCategoriesWithMostAdvertisements(int count)
        {
            var categories = await _categoryService.GetCategoryAdvertisementsCountAsync(count);
            return Ok(categories);
        }

        [HttpGet("admin/statistics/product/{count:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetCategoriesWithMostProducts(int count)
        {
            var categories = await _categoryService.GetCategoryProductsCountAsync(count);
            return Ok(categories);
        }
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateCategory([FromBody] CategoryDto createDto)
        {
            var category = await _categoryService.CreateCategoryAsync(createDto);
            return CreatedAtAction(nameof(GetCategoryById), new { id = category.Id }, category);
        }
        [HttpPut("Update/{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateCategory(int id, [FromBody] CategoryDto updateDto)
        {
            var category = await _categoryService.UpdateCategoryAsync(id, updateDto);
            if (!category)
            {
                return NotFound();
            }
            return NoContent();
        }
        [HttpDelete("Delete/{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var result = await _categoryService.DeleteCategoryAsync(id);
            if (!result)
            {
                return NotFound();
            }
            return NoContent();
        }
      
    }
    
}
