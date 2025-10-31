using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bazar.Domain.Entites
{
    public class Category
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [MaxLength(255)]
        public required string Name { get; set; }
        public ICollection<Advertisements> Advertisements { get; set; } = new List<Advertisements>();
        public ICollection<Product> Products { get; set; } = new List<Product>();
    }
}
