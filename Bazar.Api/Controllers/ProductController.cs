using Bazar.Application.DTOS.Product;
using Bazar.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Bazar.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        // GET: api/Product?search=...&category=...
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] string? search, [FromQuery] string? category, [FromQuery] int? minPrice, [FromQuery] int? maxPrice)
        {
            var result = await _productService.GetAllAsync(search, category, minPrice, maxPrice);
            return Ok(result.Data);
        }

        // GET: api/Product/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _productService.GetByIdAsync(id);

            if (!result.Success) return NotFound(result.Error);

            return Ok(result.Data);
        }

        // POST: api/Product
        [Authorize] // يجب أن يكون مسجلاً للدخول
        [HttpPost]
        public async Task<IActionResult> Create([FromForm] CreateProductDto model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            // استخراج معرف المستخدم من التوكن
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null) return Unauthorized("المستخدم غير معرف");

            var userId = int.Parse(userIdClaim.Value);

            var result = await _productService.CreateAsync(model, userId);

            if (!result.Success) return BadRequest(result.Error);

            return CreatedAtAction(nameof(GetById), new { id = result.Data }, new { id = result.Data, message = "تمت الإضافة بنجاح" });
        }

        // DELETE: api/Product/5
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null) return Unauthorized();
            var userId = int.Parse(userIdClaim.Value);

            // هل هو أدمن؟
            var isAdmin = User.IsInRole("Admin");

            var result = await _productService.DeleteAsync(id, userId, isAdmin);

            if (!result.Success) return BadRequest(result.Error);

            return Ok(new { message = "تم حذف المنتج بنجاح" });
        }
    }
}