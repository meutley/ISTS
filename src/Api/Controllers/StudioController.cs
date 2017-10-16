using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

using ISTS.Api.Model;

using ISTS.Application.Studios;

namespace ISTS.Api.Controllers
{
    [Route("api/[controller]")]
    public class StudioController : Controller
    {
        private readonly IStudioService _studioService;

        public StudioController(
            IStudioService studioService)
        {
            _studioService = studioService;
        }
        
        // GET api/studio/1
        [HttpGet("{id}")]
        public ApiModelResult<StudioDto> Get(Guid id)
        {
            var studio = _studioService.Get(id);
            
            return studio != null
                ? ApiModelResult<StudioDto>.Ok(studio)
                : ApiModelResult<StudioDto>.NotFound(null);
        }
        
        // POST api/studio
        [HttpPost]
        public ApiModelResult<StudioDto> Post([FromBody]string name, [FromBody]string friendlyUrl)
        {
            var dto = new StudioDto { Name = name, FriendlyUrl = friendlyUrl };
            var studio = _studioService.Create(dto);
            return ApiModelResult<StudioDto>.Ok(studio);
        }

        // PUT api/studio
        [HttpPut("{id}")]
        public ApiModelResult<StudioDto> Put(Guid id, [FromBody]string name, [FromBody]string friendlyUrl)
        {
            var dto = new StudioDto { Id = id, Name = name, FriendlyUrl = friendlyUrl };
            var studio = _studioService.Update(dto);
            return ApiModelResult<StudioDto>.Ok(studio);
        }
    }
}
