using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

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
            return Ok(studio);
        }

        // PUT api/studios/1
        [HttpPut("{id}")]
        public async Task<IActionResult> Put([FromBody]StudioDto model)
        {
            var studio = await _studioService.UpdateAsync(model);
            return Ok(studio);
        }
    }
}
