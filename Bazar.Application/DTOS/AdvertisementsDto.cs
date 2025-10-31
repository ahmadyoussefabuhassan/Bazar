using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bazar.Application.DTOS
{
    public class AdvertisementsDto
    {
        public int Id { get; set; }
        public required int CategoryId { get; set; }
        public required string CategoryName { get; set; } 
        public required int UserId { get; set; }
        public required string UserName { get; set; } 
        public required string UserPhone { get; set; } 
        public List<ProductDto> Products { get; set; } = new List<ProductDto>();
    }
}
