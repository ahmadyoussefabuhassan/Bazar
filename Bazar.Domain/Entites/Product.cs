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
        [MaxLength(20)]
        public required string ContactPhoneNumber { get; set; }
        public required ProductCondition Condition { get; set; }
        public int? CategoryId { get; set; }
        public Category? Category { get; set; } 
        public int? UserId { get; set; }
        public User? User { get; set; }
        public  ICollection<Images> Images { get; set; } = new List<Images>();
    }
}
