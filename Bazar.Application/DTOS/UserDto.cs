using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;

namespace Bazar.Application.DTOS
{
    public class UserDto
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Location { get; set; }
        public string? ImageUrl { get; set; }
        public DateTime JoinedDate { get; set; }
        public string Role { get; set; }
    }

    public class UserProductSummaryDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string MainImageUrl { get; set; }
    }

    public class UserProfileDto
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Location { get; set; }
        public string? ImageUrl { get; set; }
        public DateTime JoinedDate { get; set; }
        public string Role { get; set; }
        public List<UserProductSummaryDto> Products { get; set; } = new();
    }

    public class UpdateUserProfileDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Location { get; set; }
        public IFormFile? ImageFile { get; set; }
    }
}