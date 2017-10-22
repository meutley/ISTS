using System;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

using ISTS.Api.Helpers;

namespace ISTS.Api.Controllers
{
    public abstract class AuthControllerBase : Controller
    {
        protected Guid? UserId
        {
            get
            {
                var identity = HttpContext.User.Identity as ClaimsIdentity;
                return
                    identity == null
                    ? null
                    : (Guid?)Guid.Parse(identity.Name);
            }
        }

        protected void ValidateUserIdMatchesAuthenticatedUser(Guid userId)
        {
            if (UserId != null && userId != UserId)
            {
                throw new UnauthorizedAccessException();
            }
        }
    }
}