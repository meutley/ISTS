using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using ISTS.Api.Filters;
using ISTS.Api.Helpers;
using ISTS.Application.Common;
using ISTS.Application.Rooms;
using ISTS.Application.Sessions;
using ISTS.Application.SessionRequests;
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

        [HttpPost("{id}/functions")]
        public async Task<IActionResult> AddRoomFunction(Guid id, [FromBody]RoomFunctionDto model)
        {
            ValidateUserIsOwner(id);
            var function = await _roomService.AddRoomFunctionAsync(id, model);
            if (function == null)
            {
                return NotFound();
            }
            
            var roomUri = ApiHelper.GetResourceUri("rooms", id);

            return Created(roomUri, function);
        }

        // GET api/rooms/1/sessions
        [HttpGet("{id}/sessions")]
        public async Task<IActionResult> GetSessions(Guid id)
        {
            var sessions = await _roomService.GetSessionsAsync(id);
            if (sessions == null)
            {
                return NotFound();
            }

            sessions.ForEach((s) => s.ConvertScheduleFromUtc(UserTimeZone.Name));
            return Ok(sessions);
        }

        // POST api/rooms/1/sessions
        [HttpPost("{id}/sessions")]
        public async Task<IActionResult> CreateSession(Guid id, [FromBody]SessionDto model)
        {
            ValidateUserIsOwner(id);
            model.RoomId = id;
            model.ConvertScheduleToUtc(UserTimeZone.Name);

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

        [HttpPost("{id}/sessions/request")]
        public async Task<IActionResult> RequestSession(Guid id, [FromBody]DateRangeDto requestedTime)
        {
            var room = await _roomService.GetAsync(id);
            if (room == null)
            {
                return NotFound();
            }
            
            var model = new SessionRequestDto
            {
                RoomId = room.Id,
                RequestingUserId = UserId.Value,
                RequestedTime = requestedTime
            };

            model.RequestedTime?.ConvertToUtc(UserTimeZone.Name);
            var request = await _roomService.RequestSessionAsync(model);

            return Ok(request);
        }

        [HttpPut("{id}/sessions/requests/{requestId}/approve")]
        public async Task<IActionResult> ApproveSessionRequest(Guid id, Guid requestId)
        {
            var request = await _roomService.ApproveSessionRequestAsync(id, requestId);
            return Ok(request);
        }

        [HttpPut("{id}/sessions/requests/{requestId}/reject")]
        public async Task<IActionResult> RejectSessionRequest(Guid id, Guid requestId, [FromBody]string reason)
        {
            var request = await _roomService.RejectSessionRequestAsync(id, requestId, reason);
            return Ok(request);
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