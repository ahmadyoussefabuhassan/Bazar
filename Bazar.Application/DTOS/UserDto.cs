using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bazar.Application.DTOS
{
    public record UserDto(
    int Id,
    string FullName,
    string Email,
    string Location,
    string? ImageUrl,
    DateTime JoinedDate,
    string Role
    );
    public record UserProductSummaryDto(
    int Id,
    string Name,
    decimal Price,
    string MainImageUrl
    );

    public record UserProfileDto(
        int Id,
        string FullName,
        string Email,
        string Location,
        string? ImageUrl,
        DateTime JoinedDate,
        string Role,
        List<UserProductSummaryDto> Products
    );
    public record UpdateUserProfileDto(
        string FirstName,
        string LastName,
        string Location,
        IFormFile? ImageFile
    );

}
