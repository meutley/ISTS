using System;
using System.Threading.Tasks;

namespace ISTS.Application.Users
{
    public interface IUserService
    {
        Task<UserDto> CreateAsync(UserPasswordDto model);
    }
}