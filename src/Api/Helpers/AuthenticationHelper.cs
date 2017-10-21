using System;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;

namespace ISTS.Api.Helpers
{
    public static class AuthenticationHelper
    {
        public static Guid GetUserId(this Controller controller)
        {
            var identity = controller.Request.HttpContext.User.Identity as ClaimsIdentity;
            return Guid.Parse(identity.Name);
        }
    }
}