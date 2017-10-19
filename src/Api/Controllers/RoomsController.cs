using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

using ISTS.Application.Rooms;
using ISTS.Application.Schedules;

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

        // GET api/rooms/1
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
        public async Task<IActionResult> CreateSession(Guid id, [FromBody]RoomSessionDto model)
        {
            model.RoomId = id;
            var session = await _roomService.CreateSessionAsync(model);
            var sessionUri = ApiHelper.GetResourceUri("rooms", id, "sessions", session.Id);

            return Created(sessionUri, session);
        }

        // PUT api/rooms/1/sessions/1/reschedule
        [HttpPut("{id}/sessions/{sessionId}/reschedule")]
        public async Task<IActionResult> RescheduleSession(Guid id, Guid sessionId, [FromBody]DateRangeDto schedule)
        {
            var session = await _roomService.RescheduleSessionAsync(sessionId, schedule);

            return Ok(session);
        }
    }
}