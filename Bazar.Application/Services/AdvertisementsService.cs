using AutoMapper;
using Bazar.Application.DTOS;
using Bazar.Application.Interfaces;
using Bazar.Domain.Entites;
using Bazar.Domain.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bazar.Application.Services
{
    public class AdvertisementsService : IAdvertisementsService
    {
        private readonly IRepositoryAdvertisements _advertisements;
        private readonly IRepositoryCategory _categoryRepository; 
        private readonly UserManager<User> _userManager;         
        private readonly IMapper _mapper;

        public AdvertisementsService(
            IMapper mapper,
            IRepositoryAdvertisements advertisements,
            IRepositoryCategory categoryRepository,              
            UserManager<User> userManager)                       
                => (_mapper, _advertisements, _categoryRepository, _userManager)
                = (mapper, advertisements, categoryRepository, userManager);
        public async Task<AdvertisementsDto> CreateAdvertisementAsync(AdvertisementsDto createDto)
        {
            // 1. التحقق من البيانات
            if (createDto == null)
                throw new ArgumentNullException(nameof(createDto));

            // 2. التحقق من وجود المستخدم
            var user = await _userManager.FindByIdAsync(createDto.UserId.ToString());
            if (user == null)
                throw new Exception("المستخدم غير موجود");

            // 3. التحقق من وجود التصنيف
            var category = await _categoryRepository.GetByIdAsync(createDto.CategoryId);
            if (category == null)
                throw new Exception("التصنيف غير موجود");

            // 4. عمل Mapping وإضافة التاريخ
            var advertisementEntity = _mapper.Map<Advertisements>(createDto);
            advertisementEntity.CreatedAt = DateTime.UtcNow;

            // 5. الحفظ في الداتابيز
            await _advertisements.AddAsync(advertisementEntity);
            

            // 6. إرجاع النتيجة
            return _mapper.Map<AdvertisementsDto>(advertisementEntity);
        }

        public async Task<bool> DeleteAdvertisementAsync(int id)
        {
            
            var advertisement = await _advertisements.GetByIdAsync(id);
            if (advertisement is null)
                return false;
            await _advertisements.DeleteAsync(advertisement);
            return true;
        }

        public async Task<AdvertisementsDto> GetAdvertisementByIdAsync(int id)
        {
            var advertisement =  await _advertisements.GetByIdAsync(id);
            if (advertisement is null)
                throw new ArgumentException($"الإعلان بالرقم {id} غير موجود");
            if (advertisement.Products is null)
                advertisement.Products = new List<Product>();
            return _mapper.Map<AdvertisementsDto>(advertisement);
        }

        public async Task<IEnumerable<AdvertisementsDto>> GetAdvertisementsByCategoryAsync(string categoryname)
        {
            var category = await _categoryRepository.GetCategoryByNameAsync(categoryname);
            if (category is null)
                throw new ArgumentException($"التصنيف '{categoryname}' غير موجود");

            var advertisements = await _advertisements.GetByCategoryAsync(category.Id);
            return _mapper.Map<IEnumerable<AdvertisementsDto>>(advertisements);
        }

        public async Task<IEnumerable<AdvertisementsDto>> GetAdvertisementsByUserAsync(string username)
        {
            var user = await _userManager.Users
         .FirstOrDefaultAsync(u => u.UserName == username);

            if (user is null)
                throw new ArgumentException($"المستخدم '{username}' غير موجود");

            var advertisements = await _advertisements.GetByUserAsync(user.Id);
            return _mapper.Map<IEnumerable<AdvertisementsDto>>(advertisements);
        }

        public async Task<int> GetAdvertisementsCountAsync()
            => await _advertisements.GetAdvertisementsCountAsync();


        public async Task<IEnumerable<AdvertisementsDto>> GetAllAdvertisementsAsync()
        {
            var advertisements = await _advertisements.GetAllWithDetailsAsync();
            return _mapper.Map<IEnumerable<AdvertisementsDto>>(advertisements);
        }



        public async Task<IEnumerable<AdvertisementsDto>> SearchAdvertisementsAsync(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                return new List<AdvertisementsDto>();

            // 1. جلب كل الإعلانات مع التفاصيل
            var allAdvertisements = await _advertisements.GetAllWithDetailsAsync();

            // 2. البحث في multiple fields
            var searchResults = allAdvertisements.Where(ad =>
                // البحث في اسم المنتج
                ad.Products.Any(p =>
                    p.Name.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                    p.Description.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                    p.Location.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)
                ) ||
                // البحث في اسم المستخدم
                ad.User.UserName.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                // البحث في اسم التصنيف
                ad.Category.Name.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)
            );

            // 3. الـMapping
            return _mapper.Map<IEnumerable<AdvertisementsDto>>(searchResults);
        }


        public async Task<bool> UpdateAdvertisementAsync(int id, AdvertisementsDto updateDto)
        {
            var advertisement = await _advertisements.GetByIdAsync(id);
            if (advertisement is null)
                return false;

            // تحقق إذا المستخدم موجود
            var user = await _userManager.FindByIdAsync(updateDto.UserId.ToString());
            if (user is null)
                throw new ArgumentException($"المستخدم غير موجود");

            // تحقق إذا التصنيف موجود
            var category = await _categoryRepository.GetByIdAsync(updateDto.CategoryId);
            if (category is null)
                throw new ArgumentException($"التصنيف غير موجود");

            // تحديث الحقول
            advertisement.CategoryId = updateDto.CategoryId;
            advertisement.UserId = updateDto.UserId;

            await _advertisements.UpdateAsync(advertisement);
            return true;
        }
    }
}
