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
        Task<Product?> GetProductWithDetailsAsync(int id);
        Task<IEnumerable<Product>> GetProductsWithFilterAsync(string? search, string? category, int? minPrice, int? maxPrice);
    }
}
