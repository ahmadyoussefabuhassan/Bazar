using AutoMapper;
using Bazar.Application.DTOS;
using Bazar.Application.DTOS.Product;
using Bazar.Application.Interfaces;
using Bazar.Domain.Entites;
using Bazar.Domain.Interfaces;
using Microsoft.AspNetCore.Hosting; // للتعامل مع الملفات

namespace Bazar.Application.Services
{
    public class ProductService : IProductService
    {
        private readonly IRepositoryProduct _repo;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _env; // للوصول لمجلد wwwroot
        private readonly IRepositoryCategory _catRepo; // للتحقق من الفئة

        public ProductService(IRepositoryProduct repo, IMapper mapper, IWebHostEnvironment env, IRepositoryCategory catRepo)
        {
            _repo = repo;
            _mapper = mapper;
            _env = env;
            _catRepo = catRepo;
        }

        public async Task<Result<IEnumerable<ProductDto>>> GetAllAsync(string? search, string? category, int? minPrice, int? maxPrice)
        {
            // ملاحظة: هنا نفترض أن RepositoryProduct تم تعديله ليقبل هذه الباراميترات كما اتفقنا سابقاً
            var products = await _repo.GetProductsWithFilterAsync(search, category, minPrice, maxPrice);
            var dtos = _mapper.Map<IEnumerable<ProductDto>>(products);
            return Result<IEnumerable<ProductDto>>.SuccessResult(dtos);
        }

        public async Task<Result<ProductDto>> GetByIdAsync(int id)
        {
            var product = await _repo.GetProductWithDetailsAsync(id);
            if (product == null) return Result<ProductDto>.FailureResult("المنتج غير موجود");

            var dto = _mapper.Map<ProductDto>(product);
            return Result<ProductDto>.SuccessResult(dto);
        }

        public async Task<Result<int>> CreateAsync(CreateProductDto model, int userId)
        {
            // 1. التحقق من الفئة (اختياري لكن مفضل)
            // في الفرونت يرسلون اسم القسم، يمكننا تحويله لـ ID أو الاعتماد على الـ ID مباشرة
            // سنفترض أن الـ DTO يستقبل ID

            var product = _mapper.Map<Product>(model);
            product.UserId = userId;

            // 2. معالجة الصور
            if (model.ImageFiles != null && model.ImageFiles.Count > 0)
            {
                // مسار المجلد: wwwroot/images/products
                string uploadsFolder = Path.Combine(_env.WebRootPath, "images", "products");

                if (!Directory.Exists(uploadsFolder))
                    Directory.CreateDirectory(uploadsFolder);

                bool isFirstImage = true; // الصورة الأولى هي الرئيسية

                foreach (var file in model.ImageFiles)
                {
                    if (file.Length > 0)
                    {
                        // إنشاء اسم فريد للصورة
                        string uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName;
                        string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                        // الحفظ في السيرفر
                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await file.CopyToAsync(fileStream);
                        }

                        // الحفظ في الكيان
                        product.Images.Add(new Images
                        {
                            FilePath = $"/images/products/{uniqueFileName}", // المسار النسبي للفرونت
                            ContentType = file.ContentType,
                            FileSize = file.Length,
                            IsMain = isFirstImage
                        });

                        isFirstImage = false;
                    }
                }
            }

            await _repo.AddAsync(product);
            await _repo.SaveChangesAsync();

            return Result<int>.SuccessResult(product.Id, "تمت إضافة المنتج بنجاح");
        }

        public async Task<Result<bool>> DeleteAsync(int id, int userId, bool isAdmin)
        {
            var product = await _repo.GetByIdAsync(id);
            if (product == null) return Result<bool>.FailureResult("المنتج غير موجود");

            // التحقق من الصلاحية: هل المستخدم هو المالك؟ أو هل هو أدمن؟
            if (product.UserId != userId && !isAdmin)
                return Result<bool>.FailureResult("ليس لديك صلاحية لحذف هذا المنتج");

            // يمكن إضافة منطق لحذف الصور من السيرفر هنا لتوفير المساحة

            _repo.Delete(product);
            await _repo.SaveChangesAsync();
            return Result<bool>.SuccessResult(true, "تم الحذف بنجاح");
        }
    }
}