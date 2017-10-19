using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

using ISTS.Application.Rooms;
using ISTS.Application.Studios;

namespace ISTS.Api.Controllers
{
    [Route("api/[controller]")]
    public class StudiosController : Controller
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
        
        // POST api/studios
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]StudioDto model)
        {
            var studio = await _studioService.CreateAsync(model);
            var studioUri = ApiHelper.GetResourceUri("studios", studio.Id);
            
            return Created(studioUri, studio);
        }

        // PUT api/studios
        [HttpPut]
        public async Task<IActionResult> Put([FromBody]StudioDto model)
        {
            var studio = await _studioService.UpdateAsync(model);
            if (studio == null)
            {
                return NotFound();
            }
            
            return Ok(studio);
        }

        // POST api/studios/1/rooms
        [HttpPost("{id}/rooms")]
        public async Task<IActionResult> CreateRoom(Guid id, [FromBody]RoomDto model)
        {
            model.StudioId = id;
            var room = await _studioService.CreateRoomAsync(model);
            var roomUri = ApiHelper.GetResourceUri("studios", id, "rooms", room.Id);
            
            return Created(roomUri, room);
        }

        // GET api/studios/1/rooms/1
        [HttpPost("{id}/rooms/{roomId}")]
        public async Task<IActionResult> GetRoom(Guid id, Guid roomId)
        {
            var room = await _studioService.GetRoomAsync(id, roomId);
            if (room == null)
            {
                return NotFound();
            }

            return Ok(room);
        }
    }
}
