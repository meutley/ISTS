using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

using ISTS.Application.Rooms;

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

        // POST api/rooms/1/sessions
        [HttpGet("{id}/sessions")]
        public async Task<IActionResult> CreateSession(Guid id, [FromBody]RoomSessionDto model)
        {
            model.RoomId = id;
            var session = await _roomService.CreateSessionAsync(model);
            var sessionUri = ApiHelper.GetResourceUri("rooms", id, "sessions", session.Id);

            return Created(sessionUri, session);
        }
    }
}