using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bazar.Domain.Entites
{
    public class User : IdentityUser<int>
    {
        [MaxLength(255)]
        public required string FirstName { get; set; }

        [MaxLength(255)]
        public required string LastName { get; set; }

        public string? ImageUrl { get; set; }
        [MaxLength(100)]
        public required string Location { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public ICollection<Advertisements> Advertisements { get; set; } = new List<Advertisements>();
    }
}
