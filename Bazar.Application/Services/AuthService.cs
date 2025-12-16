using Bazar.Application.DTOS;
using Bazar.Application.Interfaces;
using Bazar.Domain.Entites;
using Bazar.Domain.Helper;
using Bazar.Domain.HelperDomain;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Bazar.Infrastracture.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<User> _userManager;
        private readonly JWT _jwt;

        public AuthService(UserManager<User> userManager, IOptions<JWT> jwt)
        {
            _userManager = userManager;
            _jwt = jwt.Value;
        }

        // دالة التسجيل
        public async Task<Result<LoginResponseDto>> RegisterAsync(RegisterDto model)
        {
            if (await _userManager.FindByEmailAsync(model.Email) != null)
                return Result<LoginResponseDto>.FailureResult("البريد الإلكتروني مستخدم بالفعل");

            var user = new User
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                UserName = model.Email,
                Location = model.Location,
                ImageUrl = "/images/default-user.png",
                Role = UserRole.User
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
                return Result<LoginResponseDto>.FailureResult(string.Join(", ", result.Errors.Select(e => e.Description)));

            await _userManager.AddToRoleAsync(user, "User");

            var token = await GenerateJwtToken(user);

            // التصحيح هنا: استخدام الأقواس المعقوفة {} بدلاً من () لتعريف UserDto
            var userDto = new UserDto
            {
                Id = user.Id,
                FullName = $"{user.FirstName} {user.LastName}",
                Email = user.Email,
                Location = user.Location,
                ImageUrl = user.ImageUrl,
                JoinedDate = user.CreatedAt,
                Role = user.Role.ToString()
            };

            // افترضنا أن LoginResponseDto ما زال Record (يأخذ قيم بالترتيب)
            // إذا حولته لكلاس أيضاً، يجب تعديله مثل UserDto
            return Result<LoginResponseDto>.SuccessResult(new LoginResponseDto(userDto, token));
        }

        // دالة تسجيل الدخول
        public async Task<Result<LoginResponseDto>> LoginAsync(LoginDto model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null || !await _userManager.CheckPasswordAsync(user, model.Password))
                return Result<LoginResponseDto>.FailureResult("بيانات الدخول غير صحيحة");

            var token = await GenerateJwtToken(user);

            // التصحيح هنا أيضاً: استخدام الأقواس المعقوفة {}
            var userDto = new UserDto
            {
                Id = user.Id,
                FullName = $"{user.FirstName} {user.LastName}",
                Email = user.Email!,
                Location = user.Location,
                ImageUrl = user.ImageUrl,
                JoinedDate = user.CreatedAt,
                Role = user.Role.ToString()
            };

            return Result<LoginResponseDto>.SuccessResult(new LoginResponseDto(userDto, token));
        }

        // دالة خاصة لإنشاء التوكن
        private async Task<string> GenerateJwtToken(User user)
        {
            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email!),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };

            var userRoles = await _userManager.GetRolesAsync(user);
            foreach (var role in userRoles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, role));
            }

            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Key!));

            var token = new JwtSecurityToken(
                issuer: _jwt.Issuer,
                audience: _jwt.Audience,
                expires: DateTime.Now.AddDays(_jwt.DurationInDays),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}