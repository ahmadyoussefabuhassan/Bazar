using Bazar.Domain.HelperDomain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Bazar.Application.DTOS.Product
{
    public class CreateProductDto
    {
        [Required, MaxLength(255)]
        public required string Name { get; set; }

        [Required, MaxLength(255)]
        public required string Location { get; set; }

        [Required]
        public int Price { get; set; }

        [Required]
        public required string Description { get; set; }

        [Required, MaxLength(20)]
        public required string ContactPhoneNumber { get; set; }

        [Required]
        public ProductCondition Condition { get; set; }

        public int? CategoryId { get; set; }
        public List<IFormFile>? ImageFiles { get; set; }
    }
}
