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
        public async Task<Result<CategoryDto>> CreateAsync(string name)
        {
            if(string.IsNullOrWhiteSpace(name))
                return Result<CategoryDto>.FailureResult("اسم الفئة لا يمكن أن يكون فارغاً");
            var category = new Category { Name = name.Trim() };
            await _repositoryCategory.AddAsync(category);
            await _repositoryCategory.SaveChangesAsync();
            var categoryDto = _mapper.Map<CategoryDto>(category);
            return Result<CategoryDto>.SuccessResult(categoryDto, "تمت إضافة الفئة بنجاح");
        }

        public async Task<Result<IEnumerable<CategoryDto>>> GetAllAsync()
        {
            var categories = await _repositoryCategory.GetAllAsync();
            var categoryDtos = _mapper.Map<IEnumerable<CategoryDto>>(categories);
            return Result<IEnumerable<CategoryDto>>.SuccessResult(categoryDtos);
        }
    }
}
