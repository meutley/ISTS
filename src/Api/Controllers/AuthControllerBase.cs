using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

using ISTS.Api.Helpers;
using ISTS.Api.Models;
using ISTS.Application.Users;

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
                    identity.IsAuthenticated
                    ? (Guid?)Guid.Parse(identity.Name)
                    : null;
            }
        }

        protected UserTimeZoneDto UserTimeZone
        {
            get
            {
                var identity = HttpContext.User.Identity as ClaimsIdentity;
                if (identity == null)
                {
                    return null;
                }
                
                return GetTimeZoneFromClaims(identity.Claims.ToList());
            }
        }

        protected void ValidateUserIdMatchesAuthenticatedUser(Guid userId)
        {
            if (UserId == null || userId != UserId)
            {
                throw new UnauthorizedAccessException();
            }
        }

        protected abstract void ValidateUserIsOwner(Guid entityId);

        private static UserTimeZoneDto GetTimeZoneFromClaims(List<Claim> claims)
        {
            var id = claims.Single(c => c.Type == ApiClaimTypes.TimeZoneId).Value;
            var name = claims.Single(c => c.Type == ApiClaimTypes.TimeZoneName).Value;
            var utcOffset = claims.Single(c => c.Type == ApiClaimTypes.TimeZoneUtcOffset).Value;
            
            return new UserTimeZoneDto
            {
                Id = int.Parse(id),
                Name = name,
                UtcOffset = ushort.Parse(utcOffset)
            };
        }
    }
}