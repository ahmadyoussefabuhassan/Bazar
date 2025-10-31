using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bazar.Application.DTOS
{
    public class ImagesDto
    {
        public int Id { get; set; }
        public required string FilePath { get; set; } 
        public required string ContentType { get; set; } 
        public required long FileSize { get; set; }
        public required int ProductId { get; set; }
        public DateTime CreatedAt { get; set; }
        public required bool IsMain { get; set; }
    }
}
