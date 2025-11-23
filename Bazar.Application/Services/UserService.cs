using AutoMapper;
using Bazar.Application.DTOS;
using Bazar.Application.Interfaces;
using Bazar.Domain.Entites;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Bazar.Application.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _env; // للصور

        public UserService(UserManager<User> userManager, IMapper mapper, IWebHostEnvironment env)
        {
            _userManager = userManager;
            _mapper = mapper;
            _env = env;
        }

        public async Task<Result<UserProfileDto>> GetUserProfileAsync(int userId)
        {
            // نحتاج لجلب المستخدم مع منتجاته والصور الخاصة بالمنتجات لعرضها في البروفايل
            var user = await _userManager.Users
                .Include(u => u.Products)
                .ThenInclude(p => p.Images)
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
                return Result<UserProfileDto>.FailureResult("المستخدم غير موجود");

            var userProfileDto = _mapper.Map<UserProfileDto>(user);
            return Result<UserProfileDto>.SuccessResult(userProfileDto);
        }

        public async Task<Result<UserDto>> UpdateProfileAsync(int userId, UpdateUserProfileDto model)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null) return Result<UserDto>.FailureResult("المستخدم غير موجود");

            // تحديث البيانات النصية
            if (!string.IsNullOrEmpty(model.FirstName)) user.FirstName = model.FirstName;
            if (!string.IsNullOrEmpty(model.LastName)) user.LastName = model.LastName;
            if (!string.IsNullOrEmpty(model.Location)) user.Location = model.Location;

            // تحديث الصورة الشخصية
            if (model.ImageFile != null)
            {
                string uploadsFolder = Path.Combine(_env.WebRootPath, "images", "profiles");
                if (!Directory.Exists(uploadsFolder)) Directory.CreateDirectory(uploadsFolder);

                string uniqueFileName = Guid.NewGuid().ToString() + "_" + model.ImageFile.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await model.ImageFile.CopyToAsync(stream);
                }

                // تحديث الرابط في قاعدة البيانات
                user.ImageUrl = $"/images/profiles/{uniqueFileName}";
            }

            var updateResult = await _userManager.UpdateAsync(user);
            if (!updateResult.Succeeded)
                return Result<UserDto>.FailureResult("فشل تحديث البيانات");

            var userDto = _mapper.Map<UserDto>(user);
            return Result<UserDto>.SuccessResult(userDto, "تم تحديث الملف الشخصي");
        }
    }
}