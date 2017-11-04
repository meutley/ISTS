using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using ISTS.Api.Filters;
using ISTS.Api.Helpers;
using ISTS.Application.Rooms;
using ISTS.Application.Sessions;
using ISTS.Application.Studios;
using ISTS.Helpers.Async;

namespace ISTS.Api.Controllers
{
    [Authorize(AuthenticationSchemes = "Bearer")]
    [HandleUnauthorizedAccessException]
    [HandleApiException]
    [Route("api/[controller]")]
    public class RoomsController : AuthControllerBase
    {
        private readonly IRoomService _roomService;
        private readonly IStudioService _studioService;

        public RoomsController(
            IRoomService roomService,
            IStudioService studioService)
        {
            _roomService = roomService;
            _studioService = studioService;
        }

        [AllowAnonymous]
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

        [AllowAnonymous]
        // GET api/rooms/1/sessions
        [HttpGet("{id}/sessions")]
        public async Task<IActionResult> GetSessions(Guid id)
        {
            var sessions = await _roomService.GetSessions(id);
            if (sessions == null)
            {
                return NotFound();
            }

            return Ok(sessions);
        }

        // POST api/rooms/1/sessions
        [HttpPost("{id}/sessions")]
        public async Task<IActionResult> CreateSession(Guid id, [FromBody]SessionDto model)
        {
            ValidateUserIsOwner(id);
            var session = await _roomService.CreateSessionAsync(id, model);
            var sessionUri = ApiHelper.GetResourceUri("rooms", id, "sessions", session.Id);

            return Created(sessionUri, session);
        }

        // PUT api/rooms/1/sessions/1/start
        [HttpPut("{id}/sessions/{sessionId}/start")]
        public async Task<IActionResult> StartSession(Guid id, Guid sessionId)
        {
            ValidateUserIsOwner(id);
            var session = await _roomService.StartSessionAsync(id, sessionId);

            return Ok(session);
        }

        // PUT api/rooms/1/sessions/1/end
        [HttpPut("{id}/sessions/{sessionId}/end")]
        public async Task<IActionResult> EndSession(Guid id, Guid sessionId)
        {
            ValidateUserIsOwner(id);
            var session = await _roomService.EndSessionAsync(id, sessionId);

            return Ok(session);
        }

        protected override void ValidateUserIsOwner(Guid roomId)
        {
            AsyncHelper.RunSync(async () =>
            {
                var room = await _roomService.GetAsync(roomId);
                var studio = await _studioService.GetAsync(room.StudioId);
                ValidateUserIdMatchesAuthenticatedUser(studio.OwnerUserId);
            });
        }
    }
}