using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bazar.Application.DTOS.Product
{
    public class UpdateProductDto : CreateProductDto
    {
        public List<int>? ImagesToDeleteIds { get; set; }
    }
}
