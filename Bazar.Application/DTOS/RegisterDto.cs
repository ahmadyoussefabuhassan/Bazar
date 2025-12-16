using System.ComponentModel.DataAnnotations;

namespace Bazar.Application.DTOS
{
    public record RegisterDto(
        [Required(ErrorMessage = "الاسم الأول مطلوب")] string FirstName,
        [Required(ErrorMessage = "اسم العائلة مطلوب")] string LastName,
        [Required(ErrorMessage = "المدينة مطلوبة")] string Location,
        [Required][EmailAddress] string Email,
        [Required][MinLength(6)] string Password
    );

    public record LoginDto(
        [Required][EmailAddress] string Email,
        [Required] string Password
    );
    public record LoginResponseDto(
    UserDto User,
    string? Token = null
    );

    public record Result<T>(
        bool Success,
        T? Data = default,
        string? Error = null,
        string? Message = null 
    )
    {
        public static Result<T> SuccessResult(T data, string msg = "") => new(true, data, null, msg);
        public static Result<T> FailureResult(string error) => new(false, default, error);
    }
}