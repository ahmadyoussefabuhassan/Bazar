using AutoMapper;
using Bazar.Application.DTOS;
using Bazar.Application.Interfaces;
using Bazar.Domain.Entites;
using Bazar.Domain.Interfaces;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Bazar.Application.Interfaces.IUserService;

namespace Bazar.Application.Services
{
    public class UesrService : IUserService
    {
        private readonly UserManager<User> _userManager;
        private readonly IRepositoryAdvertisements _repositoryadvertisements;
        private readonly IMapper _mapper;

        public UesrService(UserManager<User> userManager, IRepositoryAdvertisements repositoryadvertisements,  IMapper mapper)
            => (_userManager,_repositoryadvertisements, _mapper) = (userManager, repositoryadvertisements, mapper);

        public async Task<Result<UserDto>> RegisterAsync(RegisterDto registerDto)
        {
            try
            {
                var user = _mapper.Map<User>(registerDto);
                user.UserName = registerDto.Email;

                var result = await _userManager.CreateAsync(user, registerDto.Password);

                if (!result.Succeeded)
                    return new Result<UserDto>(false, null, "فشل في إنشاء المستخدم: " + string.Join(", ", result.Errors.Select(e => e.Description)));

                var userDto = _mapper.Map<UserDto>(user);
                return new Result<UserDto>(true, userDto, "تم إنشاء الحساب بنجاح");
            }
            catch (Exception ex)
            {
                return new Result<UserDto>(false, null, "حدث خطأ أثناء التسجيل: " + ex.Message);
            }
        }

        public async Task<Result<LoginResponseDto>> LoginAsync(LoginDto loginDto)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(loginDto.Email);
                if (user == null)
                    return new Result<LoginResponseDto>(false, null, "البريد الإلكتروني أو كلمة المرور غير صحيحة");

                var isValid = await _userManager.CheckPasswordAsync(user, loginDto.Password);
                if (!isValid)
                    return new Result<LoginResponseDto>(false, null, "البريد الإلكتروني أو كلمة المرور غير صحيحة");

                var userDto = _mapper.Map<UserDto>(user);

                return new Result<LoginResponseDto>(true, new LoginResponseDto("", userDto), "تم تسجيل الدخول بنجاح");
            }
            catch (Exception ex)
            {
                return new Result<LoginResponseDto>(false, null, "حدث خطأ أثناء تسجيل الدخول: " + ex.Message);
            }
        }

        public async Task<UserDto> GetUserProfileAsync(string username)
        {
            var user = await _userManager.FindByNameAsync(username);
            if (user == null) throw new Exception("المستخدم غير موجود");

            var userDto = _mapper.Map<UserDto>(user);
            userDto.AdsCount = await GetUserAdvertisementsCountAsync(username);

            return userDto;
        }

        public async Task<bool> UpdateUserProfileAsync(string username, UserDto updateDto)
        {
            var user = await _userManager.FindByNameAsync(username);
            if (user == null) return false;

            _mapper.Map(updateDto, user);
            var result = await _userManager.UpdateAsync(user);

            return result.Succeeded;
        }

        public async Task<IEnumerable<AdvertisementsDto>> GetUserAdvertisementsAsync(string username)
        {
            var user = await _userManager.FindByNameAsync(username);
            if (user == null) return Enumerable.Empty<AdvertisementsDto>();

            var ads = await _repositoryadvertisements.GetByUserAsync(user.Id);
            return _mapper.Map<IEnumerable<AdvertisementsDto>>(ads);
        }

        public async Task<int> GetUserAdvertisementsCountAsync(string username)
        {
            var user = await _userManager.FindByNameAsync(username);
            if (user == null) return 0;

            return await _repositoryadvertisements.GetCountByUserIdAsync(user.Id);
        }

        public async Task<IEnumerable<UserDto>> GetAllUsersAsync()
        {
            var users = _userManager.Users.ToList();
            var userDtos = _mapper.Map<List<UserDto>>(users);

            foreach (var userDto in userDtos)
            {
                userDto.AdsCount = await GetUserAdvertisementsCountAsync(userDto?.Email);
            }

            return userDtos;
        }

        public async Task<UserDto> GetUserByIdAsync(int id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user == null) throw new Exception("المستخدم غير موجود");

            var userDto = _mapper.Map<UserDto>(user);
            userDto.AdsCount = await GetUserAdvertisementsCountAsync(user.UserName);

            return userDto;
        }

        public async Task<UserDto> GetUserByEmailAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null) throw new Exception("المستخدم غير موجود");

            var userDto = _mapper.Map<UserDto>(user);
            userDto.AdsCount = await GetUserAdvertisementsCountAsync(user.UserName);

            return userDto;
        }

        public async Task<bool> UpdateUserAsync(int id, UserDto updateDto)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user == null) return false;

            _mapper.Map(updateDto, user);
            var result = await _userManager.UpdateAsync(user);

            return result.Succeeded;
        }

        public async Task<bool> DeleteUserAsync(int id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user == null) return false;

            var result = await _userManager.DeleteAsync(user);
            return result.Succeeded;
        }

        public async Task<bool> DeactivateUserAsync(string username)
        {
            var user = await _userManager.FindByNameAsync(username);
            if (user == null) return false;

            user.LockoutEnabled = true;
            user.LockoutEnd = DateTimeOffset.MaxValue;

            var result = await _userManager.UpdateAsync(user);
            return result.Succeeded;
        }

        public async Task<bool> ReactivateUserAsync(string username)
        {
            var user = await _userManager.FindByNameAsync(username);
            if (user == null) return false;

            user.LockoutEnd = null;
            var result = await _userManager.UpdateAsync(user);

            return result.Succeeded;
        }
    }
}
