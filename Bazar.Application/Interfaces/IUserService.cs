using Bazar.Application.DTOS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bazar.Application.Interfaces
{
    public interface IUserService
    {
        Task<Result<UserProfileDto>> GetUserProfileAsync(int userId);
        Task<Result<UserDto>> UpdateProfileAsync(int userId, UpdateUserProfileDto model);

    }


}

