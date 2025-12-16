using AutoMapper;
using Bazar.Application.DTOS;
using Bazar.Application.DTOS.Product;
using Bazar.Application.Interfaces;
using Bazar.Domain.Entites;
using Bazar.Domain.Interfaces;
using Microsoft.AspNetCore.Hosting;
using System.IO; // ضروري للتعامل مع المجلدات

namespace Bazar.Application.Services
{
    public class ProductService : IProductService
    {
        private readonly IRepositoryProduct _repo;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _env;
        private readonly IRepositoryCategory _catRepo;

        public ProductService(IRepositoryProduct repo, IMapper mapper, IWebHostEnvironment env, IRepositoryCategory catRepo)
        {
            _repo = repo;
            _mapper = mapper;
            _env = env;
            _catRepo = catRepo;
        }

        public async Task<Result<IEnumerable<ProductDto>>> GetAllAsync(string? search, string? category, int? minPrice, int? maxPrice)
        {
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

        // هنا كان الخطأ وتم إصلاحه
        public async Task<Result<int>> CreateAsync(CreateProductDto model, int userId)
        {
            var product = _mapper.Map<Product>(model);
            product.UserId = userId;

            // معالجة الصور
            if (model.ImageFiles != null && model.ImageFiles.Count > 0)
            {
                // --- بداية التعديل ---
                // نستخدم مسار المشروع الحالي بدلاً من _env.WebRootPath لتجنب الخطأ إذا كان null
                string rootPath = Directory.GetCurrentDirectory();

                // المسار النهائي سيكون: ProjectFolder/wwwroot/images/products
                string uploadsFolder = Path.Combine(rootPath, "wwwroot", "images", "products");

                // إنشاء المجلد إذا لم يكن موجوداً
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }
                // --- نهاية التعديل ---

                bool isFirstImage = true; // الصورة الأولى هي الرئيسية

                foreach (var file in model.ImageFiles)
                {
                    if (file.Length > 0)
                    {
                        // إنشاء اسم فريد للصورة
                        string uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName;

                        // المسار الكامل للحفظ على السيرفر
                        string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                        // الحفظ الفعلي للملف
                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await file.CopyToAsync(fileStream);
                        }

                        // إضافة بيانات الصورة للداتابيز
                        product.Images.Add(new Images
                        {
                            // هذا الرابط الذي سيستخدمه الفرونت إند لعرض الصورة
                            FilePath = $"/images/products/{uniqueFileName}",
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

            // التحقق من الصلاحية
            if (product.UserId != userId && !isAdmin)
                return Result<bool>.FailureResult("ليس لديك صلاحية لحذف هذا المنتج");

            // (اختياري) يمكن إضافة كود هنا لحذف الصور من المجلد لتوفير المساحة

            _repo.Delete(product);
            await _repo.SaveChangesAsync();
            return Result<bool>.SuccessResult(true, "تم الحذف بنجاح");
        }
    }
}