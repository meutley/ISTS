using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

using ISTS.Application.Users;

namespace ISTS.Api.Controllers
{
    [Route("api/[controller]")]
    public class UsersController : Controller
    {
        private readonly IUserService _userService;
        
        public UsersController(
            IUserService userService)
        {
            _userService = userService;
        }

        // POST api/users
        public async Task<IActionResult> Create([FromBody]UserPasswordDto model)
        {
            var user = await _userService.CreateAsync(model);

            return Ok(user);
        }
    }
}