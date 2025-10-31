using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bazar.Application.DTOS
{
    public class UserDto
    {
        public int Id { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public string? ImageUrl { get; set; }
        public required string Location { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public int AdsCount { get; set; }
        public List<AdvertisementsDto> Advertisements { get; set; } = new List<AdvertisementsDto>();

        // للإدارة فقط (تستخدم عند الحاجة)
        public string? Email { get; set; }
        public bool? IsActive { get; set; }

    }
}
