using Bazar.Domain.HelperDomain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bazar.Application.DTOS
{
    public class ProductDto
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string Location { get; set; }
        public required int Price { get; set; }
        public required string Description { get; set; }
        public int AdvertisementId { get; set; }
        public int CategoryId { get; set; }
        public required string CategoryName { get; set; }
        public required ProductCondition Condition { get; set; }
        public List<ImagesDto> Images { get; set; } = new List<ImagesDto>();
    }
}
