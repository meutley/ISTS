using System;
using System.Threading.Tasks;

namespace ISTS.Domain.Users
{
    public interface IUserValidator
    {
        Task ValidateAsync(Guid? userId, string email, string displayName, string password, string postalCode);
    }
}