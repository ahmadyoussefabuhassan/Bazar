using Bazar.Application.DTOS;
using Bazar.Application.DTOS.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bazar.Application.Interfaces
{
    public interface IProductService
    {
        // الفلترة والبحث
        Task<Result<IEnumerable<ProductDto>>> GetAllAsync(string? search, string? category, int? minPrice, int? maxPrice);

        // تفاصيل منتج واحد
        Task<Result<ProductDto>> GetByIdAsync(int id);

        // إضافة منتج مع الصور
        Task<Result<int>> CreateAsync(CreateProductDto model, int userId);

        // حذف منتج (للمالك أو الأدمن)
        Task<Result<bool>> DeleteAsync(int id, int userId, bool isAdmin);

    }
}
