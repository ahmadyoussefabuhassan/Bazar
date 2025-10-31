using Bazar.Application.DTOS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bazar.Application.Interfaces
{
    public interface ICategoryService
    {
        //  العمليات الأساسية
        Task<IEnumerable<CategoryDto>> GetAllCategoriesAsync();
        Task<CategoryDto> GetCategoryByIdAsync(int id);
        Task<CategoryDto> GetCategoryByNameAsync(string name);
        Task<bool> CategoryExistsAsync(string categoryName);

        //  العمليات الخاصة Admin

        Task<CategoryDto> CreateCategoryAsync(CategoryDto createDto);
        Task<bool> UpdateCategoryAsync(int id, CategoryDto updateDto);
        Task<bool> DeleteCategoryAsync(int id);
        Task<IEnumerable<CategoryDto>> GetPopularCategoriesAsync(int count);
        Task<int> GetCategoryProductsCountAsync(int categoryId);

        Task<int> GetCategoryAdvertisementsCountAsync(int categoryId);
    }
}
