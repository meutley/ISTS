using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

using ISTS.Api.Helpers;

namespace ISTS.Api.Controllers
{
    public abstract class AuthControllerBase : Controller
    {
        protected Guid UserId
        {
            get { return this.GetUserId(); }
        }

        protected void ValidateUserIdMatchesAuthenticatedUser(Guid userId)
        {
            if (userId != UserId)
            {
                throw new UnauthorizedAccessException();
            }
        }
    }
}