using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bazar.Application.DTOS
{
    public record RegisterDto(
        string Email, 
        string Password,
        string FirstName,
        string LastName,
        string PhoneNumber,
        string Location
    );
    public record LoginDto(string Email, string Password);
}
