using Bazar.Domain.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bazar.Domain.Interfaces
{
    public interface IRepositoryProduct : IRepository<Product>
    {
        Task<IEnumerable<Product>> SearchAsync(string searchTerm);
        Task<IEnumerable<Product>> GetByCategoryAsync(int categoryId);
        Task<IEnumerable<Product>> GetByAdvertisementAsync(int advertisementId);
        Task<IEnumerable<Images>> GetProductImagesAsync(int productId);
        Task<Images?> GetMainProductImageAsync(int productId);
        Task AddProductImageAsync(int productId, Images image);

        Task<IEnumerable<Product>> GetAllWithDetailsAsync();
        Task<bool> DeleteProductAsync(int productId);
        Task<int> GetProductsCountAsync();
        Task<IEnumerable<Product>> GetRecentProductsAsync(int days);
    }
}
