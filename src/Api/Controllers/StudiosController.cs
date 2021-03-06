using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using ISTS.Api.Filters;
using ISTS.Api.Helpers;
using ISTS.Application.Rooms;
using ISTS.Application.Studios;
using ISTS.Application.Studios.Search;
using ISTS.Helpers.Validation;

namespace ISTS.Api.Controllers
{
    [Authorize(AuthenticationSchemes = "Bearer")]
    [HandleUnauthorizedAccessException]
    [HandleApiException]
    [Route("api/[controller]")]
    public class StudiosController : AuthControllerBase
    {
        private readonly IStudioService _studioService;

        public StudiosController(
            IStudioService studioService)
        {
            _studioService = studioService;
        }
        
        // GET api/studios/1
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var studio = await _studioService.GetAsync(id);
            if (studio == null)
            {
                return NotFound();
            }
            
            return Ok(studio);
        }

        // GET api/studios/getbyfriendlyurl
        [HttpGet("getbyfriendlyurl")]
        public async Task<IActionResult> GetByFriendlyUrl(string friendlyUrl)
        {
            var studio = await _studioService.GetByFriendlyUrlAsync(friendlyUrl);
            if (studio == null)
            {
                return NotFound();
            }

            return Ok(studio);
        }

        // POST api/studios/search
        [HttpPost("search")]
        public async Task<IActionResult> Search([FromBody]StudioSearchModel model)
        {
            model = await _studioService.BuildSearchModelAsync(UserId, model);
            
            var results = await _studioService.SearchAsync(model);

            return Ok(results);
        }
        
        // POST api/studios
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]StudioDto model)
        {
            ArgumentNotNullValidator.Validate(model, nameof(model));
            
            model.OwnerUserId = UserId.Value;
            var studio = await _studioService.CreateAsync(model);
            var studioUri = ApiHelper.GetResourceUri("studios", studio.Id);
            
            return Created(studioUri, studio);
        }

        // PUT api/studios
        [HttpPut]
        public async Task<IActionResult> Put([FromBody]StudioDto model)
        {
            ArgumentNotNullValidator.Validate(model, nameof(model));
            ValidateUserIsOwner(model.OwnerUserId);
            
            var studio = await _studioService.UpdateAsync(model);
            return Ok(studio);
        }

        // POST api/studios/1/rooms
        [HttpPost("{id}/rooms")]
        public async Task<IActionResult> CreateRoom(Guid id, [FromBody]RoomDto model)
        {
            var studio = await _studioService.GetAsync(id);
            if (studio == null)
            {
                return NotFound();
            }
            
            ValidateUserIsOwner(studio.OwnerUserId);
            
            var room = await _studioService.CreateRoomAsync(UserId.Value, id, model);
            var roomUri = ApiHelper.GetResourceUri("studios", id, "rooms", room.Id);
            
            return Created(roomUri, room);
        }

        // GET api/studios/1/rooms
        [HttpGet("{id}/rooms")]
        public async Task<IActionResult> GetRooms(Guid id)
        {
            var rooms = await _studioService.GetRoomsAsync(id);

            return Ok(rooms);
        }

        protected override void ValidateUserIsOwner(Guid entityId)
        {
            ValidateUserIdMatchesAuthenticatedUser(entityId);
        }
    }
}
