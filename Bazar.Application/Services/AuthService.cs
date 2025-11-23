using Bazar.Application.DTOS;
using Bazar.Application.Interfaces;
using Bazar.Domain.Entites;
using Bazar.Domain.Helper;
using Bazar.Domain.HelperDomain; // ضروري من أجل UserRole
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
                // يفضل استخدام الايميل كاسم مستخدم لضمان عدم التكرار
                UserName = model.Email,
                Location = model.Location,
                ImageUrl = "/images/default-user.png",

                // هام: تحديد الدور الافتراضي عند إنشاء الكيان
                Role = UserRole.User
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
                return Result<LoginResponseDto>.FailureResult(string.Join(", ", result.Errors.Select(e => e.Description)));

            // إضافة الدور لنظام Identity (جدول AspNetUserRoles)
            await _userManager.AddToRoleAsync(user, "User");

            // إنشاء التوكن
            var token = await GenerateJwtToken(user);

            // إرجاع النتيجة (تم تعديل هذا السطر ليناسب UserDto الجديد)
            return Result<LoginResponseDto>.SuccessResult(new LoginResponseDto(
                new UserDto(
                    user.Id,
                    $"{user.FirstName} {user.LastName}",
                    user.Email,
                    user.Location,
                    user.ImageUrl,
                    user.CreatedAt,
                    user.Role.ToString() // تمرير الدور هنا كنص
                ),
                token
            ));
        }

        // دالة تسجيل الدخول
        public async Task<Result<LoginResponseDto>> LoginAsync(LoginDto model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null || !await _userManager.CheckPasswordAsync(user, model.Password))
                return Result<LoginResponseDto>.FailureResult("بيانات الدخول غير صحيحة");

            var token = await GenerateJwtToken(user);

            // إرجاع النتيجة (تم تعديل هذا السطر ليناسب UserDto الجديد)
            return Result<LoginResponseDto>.SuccessResult(new LoginResponseDto(
                new UserDto(
                    user.Id,
                    $"{user.FirstName} {user.LastName}",
                    user.Email!,
                    user.Location,
                    user.ImageUrl,
                    user.CreatedAt,
                    user.Role.ToString() // تمرير الدور هنا كنص
                ),
                token
            ));
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

            // إضافة الأدوار (Admin, User) للتوكن
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