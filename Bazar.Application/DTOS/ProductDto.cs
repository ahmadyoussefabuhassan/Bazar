using Bazar.Domain.HelperDomain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bazar.Application.DTOS
{
    public class ProductDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public int Price { get; set; }
        public string Description { get; set; }
        public string ContactPhoneNumber { get; set; }
        public string Condition { get; set; }
        public int? CategoryId { get; set; }
        public string? CategoryName { get; set; }
        public int? UserId { get; set; }
        public string? SellerName { get; set; }
        public List<ImagesDto> Images { get; set; } = new();
    }


}
