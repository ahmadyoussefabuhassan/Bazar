using AutoMapper;
using Bazar.Application.DTOS;
using Bazar.Application.Interfaces;
using Bazar.Domain.Entites;
using Bazar.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bazar.Application.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IRepositoryCategory _category;
        private readonly IRepositoryProduct _productRepository;
        private readonly IRepositoryAdvertisements _advertisementsRepository;
        private readonly IMapper _mapper;
        public CategoryService(IRepositoryCategory category, IMapper mapper , IRepositoryProduct productRepository , IRepositoryAdvertisements advertisementsRepository)
            => (_category , _productRepository, _advertisementsRepository, _mapper) = (category ,productRepository,advertisementsRepository, mapper);
     

        public async Task<bool> CategoryExistsAsync(string categoryName)
            => await _category.CategoryExistsAsync(categoryName);


        public async Task<CategoryDto> CreateCategoryAsync(CategoryDto createDto)
        {
            if (createDto == null)
                throw new ArgumentNullException(nameof(createDto));

            var exists = await _category.CategoryExistsAsync(createDto.Name);
            if (exists)
                throw new ArgumentException($"التصنيف '{createDto.Name}' موجود مسبقاً");

            var categoryEntity = await _category.CreateCategoryAsync(createDto.Name);
            return _mapper.Map<CategoryDto>(categoryEntity);
        }

        public async Task<bool> DeleteCategoryAsync(int id)
        {
            var category = await _category.GetByIdAsync(id);
            if (category is null)
                return false;

            var products = await _productRepository.GetByCategoryAsync(category.Id);
            if (products.Any())
                throw new InvalidOperationException("لا يمكن حذف التصنيف لأنه يحتوي على منتجات");

            await _category.DeleteAsync(category);
            return true;

        }

        public async Task<IEnumerable<CategoryDto>> GetAllCategoriesAsync()
        {
            var categories = await _category.GetAllAsync();
            return _mapper.Map<IEnumerable<CategoryDto>>(categories);
        }
       


        public async Task<int> GetCategoryAdvertisementsCountAsync(int categoryId)
        {
            var advertisements = await _advertisementsRepository.GetByCategoryAsync(categoryId);
            return advertisements.Count();
        }

        public async Task<CategoryDto> GetCategoryByIdAsync(int id)
        {
            var category = await _category.GetByIdAsync(id);

            if (category is null)
                throw new ArgumentException($"التصنيف بالرقم {id} غير موجود");

            return _mapper.Map<CategoryDto>(category);
        }

        public Task<CategoryDto> GetCategoryByNameAsync(string name)
        {
            var category =  _category.GetCategoryByNameAsync(name);
            return _mapper.Map<Task<CategoryDto>>(category);
        }

        public async Task<int> GetCategoryProductsCountAsync(int categoryId)
        { 
            if (await _category.GetByIdAsync(categoryId) is null)
                throw new ArgumentException($"التصنيف بالرقم {categoryId} غير موجود");
            var products = await _productRepository.GetByCategoryAsync(categoryId);
            return products.Count();

        }

        public async Task<IEnumerable<CategoryDto>> GetPopularCategoriesAsync(int count)
        {
            var allCategories = await _category.GetAllAsync();

            // نحسب عدد المنتجات لكل تصنيف ونتريج حسب الشعبية
            var categoriesWithCount = new List<(Category category, int productCount)>();

            foreach (var category in allCategories)
            {
                var productCount = await GetCategoryProductsCountAsync(category.Id);
                categoriesWithCount.Add((category, productCount));
            }

            // نرجع التصنيفات الأكثر شيوعاً
            var popularCategories = categoriesWithCount
                .OrderByDescending(x => x.productCount)
                .Take(count)
                .Select(x => x.category);

            return _mapper.Map<IEnumerable<CategoryDto>>(popularCategories);
        }

        public async Task<bool> UpdateCategoryAsync(int id, CategoryDto updateDto)
        {
            var category = await _category.GetByIdAsync(id);
            if (category is null)
                return false;

            if (category.Name != updateDto.Name)
            {
                var exists = await _category.CategoryExistsAsync(updateDto.Name);
                if (exists)
                    throw new ArgumentException($"التصنيف '{updateDto.Name}' موجود مسبقاً");
            }

            category.Name = updateDto.Name;
            await _category.UpdateAsync(category);
            return true;
        }
    }
}
