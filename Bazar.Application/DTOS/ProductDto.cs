using Bazar.Domain.HelperDomain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bazar.Application.DTOS
{
    public record ProductDto(
        int Id,
        string Name,
        string Location,
        int Price,
        string Description,
        string ContactPhoneNumber,
        string Condition, 
        int? CategoryId,
        string? CategoryName,
        int? UserId,
        string? SellerName,   
        List<ImagesDto> Images
    );


}
