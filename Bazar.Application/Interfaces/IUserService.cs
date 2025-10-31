using Bazar.Application.DTOS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bazar.Application.Interfaces
{
    public interface IUserService
    {
        //  المصادقة
        Task<Result<UserDto>> RegisterAsync(RegisterDto registerDto);
        Task<Result<LoginResponseDto>> LoginAsync(LoginDto loginDto);

        //  الملف الشخصي (للمستخدم العادي)
        Task<UserDto> GetUserProfileAsync(string username);
        Task<bool> UpdateUserProfileAsync(string username, UserDto updateDto);
        Task<IEnumerable<AdvertisementsDto>> GetUserAdvertisementsAsync(string username);
        Task<int> GetUserAdvertisementsCountAsync(string username);

        //  الإدارة (للمدير - تستخدم نفس الـ DTOs)
        Task<IEnumerable<UserDto>> GetAllUsersAsync();
        Task<UserDto> GetUserByIdAsync(int id);
        Task<UserDto> GetUserByEmailAsync(string email);
        Task<bool> UpdateUserAsync(int id, UserDto updateDto);
        Task<bool> DeleteUserAsync(int id);
        Task<bool> DeactivateUserAsync(string username);
        Task<bool> ReactivateUserAsync(string username);
        //  الـ DTOs الثابتة
        public record LoginResponseDto(string Token, UserDto User);
        public record Result<T>(bool Success, T? Data, string? Error = null);
    }


}

