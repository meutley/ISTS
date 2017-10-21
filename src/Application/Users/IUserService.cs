using System;
using System.Threading.Tasks;

namespace ISTS.Application.Users
{
    public interface IUserService
    {
        Task<UserDto> CreateAsync(UserPasswordDto model);
        Task<UserDto> AuthenticateAsync(string email, string password);
    }
}