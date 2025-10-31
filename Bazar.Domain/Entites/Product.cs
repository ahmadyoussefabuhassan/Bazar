using Bazar.Domain.HelperDomain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bazar.Domain.Entites
{
    public class Product
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [MaxLength(255)]
        public required string Name { get; set; }
        [MaxLength(255)]
        public required string Location { get; set; }
        public required int Price { get; set; }
        public required string Description { get; set; }
        public required ProductCondition Condition { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; } = null!;
        public int AdvertisementsId { get; set; }
        public Advertisements Advertisements { get; set; } = null!;
        public  ICollection<Images> Images { get; set; } = new List<Images>();
    }
}
