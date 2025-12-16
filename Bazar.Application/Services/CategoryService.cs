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
        private readonly IRepositoryCategory _repositoryCategory;
        private readonly IMapper _mapper;
        public CategoryService(IRepositoryCategory repositoryCategory, IMapper mapper)
            => (_repositoryCategory, _mapper) = (repositoryCategory, mapper);
        public async Task<Result<CategoryDto>> CreateAsync(CreateCategoryDto dto)
        {
            // 1. التحقق من البيانات
            if (string.IsNullOrWhiteSpace(dto.Name))
                return Result<CategoryDto>.FailureResult("اسم الفئة لا يمكن أن يكون فارغاً");

            // 2. التحويل من DTO إلى Entity باستخدام AutoMapper 
            // (أو يدوياً كما كنت تفعل، لكن المابر أنظف)
            var category = _mapper.Map<Category>(dto);

            // أو يدوياً إذا لم تضف المابينج:
            // var category = new Category { Name = dto.Name.Trim() };

            // 3. الحفظ في قاعدة البيانات
            await _repositoryCategory.AddAsync(category);
            await _repositoryCategory.SaveChangesAsync();

            // 4. التحويل العكسي: من Entity إلى CategoryDto (عشان نرجع الـ ID للمستخدم)
            var categoryResponse = _mapper.Map<CategoryDto>(category);

            return Result<CategoryDto>.SuccessResult(categoryResponse, "تمت إضافة الفئة بنجاح");
        }

        public async Task<Result<IEnumerable<CategoryDto>>> GetAllAsync()
        {
            var categories = await _repositoryCategory.GetAllAsync();
            var categoryDtos = _mapper.Map<IEnumerable<CategoryDto>>(categories);
            return Result<IEnumerable<CategoryDto>>.SuccessResult(categoryDtos);
        }
    }
}
