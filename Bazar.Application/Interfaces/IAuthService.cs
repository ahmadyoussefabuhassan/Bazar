using Bazar.Application.DTOS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bazar.Application.Interfaces
{
    public interface IAuthService
    {
        Task<Result<LoginResponseDto>> RegisterAsync(RegisterDto model);
        Task<Result<LoginResponseDto>> LoginAsync(LoginDto model);
    }
}
