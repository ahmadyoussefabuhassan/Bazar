using Bazar.Application.DTOS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bazar.Application.Interfaces
{
    public interface IProductService
    {
        //  العمليات الأساسية (CRUD)
        Task<IEnumerable<ProductDto>> GetAllProductsAsync();
        Task<ProductDto> GetProductByIdAsync(int id);
        Task<ProductDto> CreateProductAsync(ProductDto createDto);
        Task<bool> UpdateProductAsync(int id, ProductDto updateDto);
        Task<bool> DeleteProductAsync(int id);
        Task<ImagesDto> AddProductImageAsync(int productId, ImagesDto imageDto);
        Task<bool> RemoveProductImageAsync(int productId, int imageId);
        Task<bool> SetMainProductImageAsync(int productId, int imageId);
        Task<IEnumerable<ImagesDto>> GetProductImagesAsync(int productId);
        Task<IEnumerable<ProductDto>> GetProductsByCategoryAsync(string categoryName);

        Task<IEnumerable<ProductDto>> SearchProductsAsync(string searchTerm);

        //  العمليات الخاصة بالمنتجات

        Task<int> GetProductsCountAsync();

       
      
    }
}
