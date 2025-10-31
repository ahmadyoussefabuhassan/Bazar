using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bazar.Domain.Entites
{
    public class Advertisements
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public int CategoryId { get; set; }
        public Category Category { get; set; } = null!;
        public int UserId { get; set; }
        public User User { get; set; } = null!;
        public ICollection<Product> Products { get; set; } = new List<Product>();
    }
}
