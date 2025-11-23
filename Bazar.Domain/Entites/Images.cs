using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bazar.Domain.Entites
{
    public class Images
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [MaxLength(500)]
        public required string FilePath { get; set; }

        [MaxLength(50)]
        public required string ContentType { get; set; }

        public long FileSize { get; set; }

        public int? ProductId { get; set; }
        public Product? Product { get; set; } 
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public bool IsMain { get; set; } = false;

    }
}
