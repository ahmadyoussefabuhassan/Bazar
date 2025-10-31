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
    public class ProductService : IProductService
    {
        private readonly IRepositoryProduct _productRepository;
        private readonly IRepositoryCategory _categoryRepository;
        private readonly IRepositoryAdvertisements _advertisementsRepository;
        private readonly IMapper _mapper;
        public ProductService(IRepositoryProduct product, IMapper mapper , IRepositoryCategory category, IRepositoryAdvertisements advertisements)
            => (_productRepository, _categoryRepository, _advertisementsRepository, _mapper) = (product, category, advertisements, mapper);

  

        public async Task<ImagesDto> AddProductImageAsync(int productId, ImagesDto imageDto)
        {
            var product = await _productRepository.GetByIdAsync(productId);
            if (product is null)
                throw new ArgumentException($"المنتج بالرقم {productId} غير موجود");

            var imageEntity = _mapper.Map<Images>(imageDto);
            imageEntity.ProductId = productId;
            imageEntity.CreatedAt = DateTime.UtcNow;

            await _productRepository.AddProductImageAsync(productId, imageEntity);
            return _mapper.Map<ImagesDto>(imageEntity);

        }

        public async Task<ProductDto> CreateProductAsync(ProductDto createDto)
        {
            if (createDto is null)
                throw new ArgumentNullException(nameof(createDto));

            // 1. التحقق من وجود التصنيف
            var category = await _categoryRepository.GetByIdAsync(createDto.CategoryId);
            if (category is null)
                throw new ArgumentException($"التصنيف غير موجود");

            // 2. التحقق من وجود الإعلان
            var advertisement = await _advertisementsRepository.GetByIdAsync(createDto.AdvertisementId);
            if (advertisement is null)
                throw new ArgumentException($"الإعلان غير موجود");

            // 3. عمل Mapping للمنتج
            var productEntity = _mapper.Map<Product>(createDto);

            // 4. إضافة الصور إذا موجودة
            if (createDto.Images != null && createDto.Images.Any())
            {
                foreach (var imageDto in createDto.Images)
                {
                    var imageEntity = _mapper.Map<Images>(imageDto);
                    imageEntity.CreatedAt = DateTime.UtcNow;
                    productEntity.Images.Add(imageEntity);
                }
            }

            // 5. حفظ المنتج
            await _productRepository.AddAsync(productEntity);

            // 6. إرجاع النتيجة
            return _mapper.Map<ProductDto>(productEntity);

        }

        public async Task<bool> DeleteProductAsync(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product is not null)
            {
                await _productRepository.DeleteAsync(product);
                return true;
            }
            return false;
        }

        public async Task<IEnumerable<ProductDto>> GetAllProductsAsync()
        {
            var products = await _productRepository.GetAllWithDetailsAsync();
            return _mapper.Map<IEnumerable<ProductDto>>(products);
        }

        public async Task<ProductDto> GetProductByIdAsync(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product is null)
                throw new ArgumentException($"المنتج بالرقم {id} غير موجود");
            return  _mapper.Map<ProductDto>(product);
        }

        public  async Task<IEnumerable<ImagesDto>> GetProductImagesAsync(int productId)
        {
            var images = await _productRepository.GetProductImagesAsync(productId);
            if (images is null || !images.Any())
                throw new ArgumentException($"لا توجد صور للمنتج بالرقم {productId}");
            return _mapper.Map<IEnumerable<ImagesDto>>(images);
        }



        public async Task<IEnumerable<ProductDto>> GetProductsByCategoryAsync(string categoryName)
        {
            var category = await _categoryRepository.GetCategoryByNameAsync(categoryName);
            if (category is null)
                throw new ArgumentException($"التصنيف {categoryName} غير موجود");
            var products = await _productRepository.GetByCategoryAsync(category.Id);
            return _mapper.Map<IEnumerable<ProductDto>>(products);
        }

        public async Task<int> GetProductsCountAsync()
        {
            var products = await _productRepository.GetAllAsync();
            return products.Count();
        }

        public  async Task<bool> RemoveProductImageAsync(int productId, int imageId)
        {
            var product = await _productRepository.GetByIdAsync(productId);
            if (product is null)
                return false;

            var image = product.Images.FirstOrDefault(img => img.Id == imageId);
            if (image is null)
                return false;

            product.Images.Remove(image);
            await _productRepository.UpdateAsync(product);

            return true;
        }

        public async Task<IEnumerable<ProductDto>> SearchProductsAsync(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                return new List<ProductDto>();

            var products = await _productRepository.SearchAsync(searchTerm);
            return _mapper.Map<IEnumerable<ProductDto>>(products);
        }

        public async Task<bool> SetMainProductImageAsync(int productId, int imageId)
        {
            var product = await _productRepository.GetByIdAsync(productId);
            if (product is null)
                return false;

            var targetImage = product.Images.FirstOrDefault(img => img.Id == imageId);
            if (targetImage is null)
                return false;

            foreach (var image in product.Images)
            {
                image.IsMain = false;
            }

            targetImage.IsMain = true;
            await _productRepository.UpdateAsync(product);
            return true;

        }

        public async Task<bool> UpdateProductAsync(int id, ProductDto updateDto)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product is null)
                return false;

            var category = await _categoryRepository.GetByIdAsync(updateDto.CategoryId);
            if (category is null)
                throw new ArgumentException($"التصنيف غير موجود");

            product.Name = updateDto.Name;
            product.Location = updateDto.Location;
            product.Price = updateDto.Price;
            product.Description = updateDto.Description;
            product.CategoryId = updateDto.CategoryId;
            product.Condition = updateDto.Condition;

            await _productRepository.UpdateAsync(product);
            return true;
        }
    }
}
