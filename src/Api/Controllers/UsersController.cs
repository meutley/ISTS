using System;
using System.Security.Claims;
using System.Threading.Tasks;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

using ISTS.Api.Helpers;
using ISTS.Api.Models;
using ISTS.Application.Users;

namespace ISTS.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class UsersController : Controller
    {
        private readonly ApplicationSettings _applicationSettings;
        private readonly IUserService _userService;
        
        public UsersController(
            IOptions<ApplicationSettings> applicationSettings,
            IUserService userService)
        {
            _applicationSettings = applicationSettings.Value;
            _userService = userService;
        }

        [AllowAnonymous]
        [HttpPost]
        // POST api/users
        public async Task<IActionResult> Register([FromBody]UserPasswordDto model)
        {
            var user = await _userService.CreateAsync(model);

            return Ok(user);
        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public async Task<IActionResult> Authenticate([FromBody]UserPasswordDto model)
        {
            var user = await _userService.AuthenticateAsync(model.Email, model.Password);
            if (user == null)
            {
                return Unauthorized();
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = System.Text.Encoding.Default.GetBytes(_applicationSettings.AuthenticationSecret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Id.ToString()),
                    new Claim(ApiClaimTypes.TimeZoneId, user.TimeZone.Id.ToString()),
                    new Claim(ApiClaimTypes.TimeZoneName, user.TimeZone.Name),
                    new Claim(ApiClaimTypes.TimeZoneUtcOffset, user.TimeZone.UtcOffset.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            return Ok(new
            {
                Id = user.Id,
                Email = user.Email,
                DisplayName = user.DisplayName,
                Token = tokenString
            });
        }
    }
}