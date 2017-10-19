using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

using ISTS.Application.Rooms;
using ISTS.Application.Sessions;

namespace ISTS.Api.Controllers
{
    [Route("api/[controller]")]
    public class RoomsController : Controller
    {
        private readonly IRoomService _roomService;

        public RoomsController(
            IRoomService roomService)
        {
            _roomService = roomService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var room = await _roomService.GetAsync(id);
            if (room == null)
            {
                return NotFound();
            }

            return Ok(room);
        }

        // POST api/rooms/1/sessions
        [HttpPost("{id}/sessions")]
        public async Task<IActionResult> CreateSession(Guid id, [FromBody]SessionDto model)
        {
            var session = await _roomService.CreateSessionAsync(id, model);
            var sessionUri = ApiHelper.GetResourceUri("rooms", id, "sessions", session.Id);

            return Created(sessionUri, session);
        }
    }
}