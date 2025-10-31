using Bazar.Application.DTOS;
using Bazar.Application.Interfaces;
using Bazar.Application.Services;
using Bazar.Domain.Entites;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bazar.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productservice;
        public ProductController(IProductService productservice)
            => _productservice = productservice;
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllProduct()
            => Ok(await _productservice.GetAllProductsAsync());
        [HttpGet("Id{id:int}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetByIdProduct(int id)
        {
            var product = await _productservice.GetProductByIdAsync(id);
            if (product == null)
                return NotFound();
            return Ok(product);
        }
        [HttpGet("Search/{searchterm}")]
        [AllowAnonymous]
        public async Task<IActionResult> SearchProductAsync(string searchterm)
        {
            var search = await _productservice.SearchProductsAsync(searchterm);
            return Ok(search);
        }
        [HttpGet("category/{categoryName}")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetProductsByCategoryAsync(string categoryName)
        {

            var products = await _productservice.GetProductsByCategoryAsync(categoryName);

            if (products == null || !products.Any())
            {
                return NotFound($"No products found for category: {categoryName}");
            }

            return Ok(products);

        }
        [HttpPost("Product")]
        [Authorize]
        public async Task<IActionResult> CreateProductAsync(ProductDto productDto)
        {
            var createdProduct = await _productservice.CreateProductAsync(productDto);
            return CreatedAtAction(nameof(GetByIdProduct), new { id = createdProduct.Id }, createdProduct);
        }
        [HttpPut("Update/{id:int}")]
        [Authorize]
        public async Task<IActionResult> UpdateProductAsync(int id, ProductDto productDto)
        {
            var updatedProduct = await _productservice.UpdateProductAsync(id, productDto);
            if (!updatedProduct )
                return NotFound();
            return NoContent();
        }
        [HttpDelete("Delete/{id:int}")]
        [Authorize]
        public async Task<IActionResult> DeleteProductAsync(int id)
        {
            var deletedProduct = await _productservice.DeleteProductAsync(id);
            if (!deletedProduct)
                return NotFound();
            return NoContent();
        }
        [HttpGet("Image/Product/{productid:int}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetImageProductAsync(int productid)
        {
            var image = await _productservice.GetProductImagesAsync(productid);
            return Ok(image);
        }
        [HttpPost("{productId}/images")]
        [Authorize]
        public async Task<ActionResult<ImagesDto>> AddProductImageAsync(int productId, ImagesDto imageDto)
        {
            var result = await _productservice.AddProductImageAsync(productId, imageDto);
            return Ok(result);
        }
        [HttpDelete("{productId}/images/{imageId}")]
        [Authorize]
        public async Task<ActionResult> RemoveProductImageAsync(int productId, int imageId)
        {
            try
            {
                var result = await _productservice.RemoveProductImageAsync(productId, imageId);

                if (result)
                    return Ok("تم حذف الصورة بنجاح");
                else
                    return NotFound("لم يتم العثور على الصورة");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{productId}/images/{imageId}/set-main")]
        [Authorize]
        public async Task<ActionResult> SetMainProductImageAsync(int productId, int imageId)
        {
            try
            {
                var result = await _productservice.SetMainProductImageAsync(productId, imageId);

                if (result)
                    return Ok("تم تعيين الصورة كرئيسية بنجاح");
                else
                    return NotFound("لم يتم العثور على الصورة");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("count/product")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetProductsCountAsync()
            => Ok(await _productservice.GetProductsCountAsync());
    }
}
