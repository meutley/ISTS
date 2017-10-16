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
    public class StudiosController : Controller
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
        public ApiModelResult<StudioDto> Post([FromBody]StudioDto model)
        {
            var studio = _studioService.Create(model);
            return ApiModelResult<StudioDto>.Ok(studio);
        }

        // PUT api/studio/1
        [HttpPut("{id}")]
        public ApiModelResult<StudioDto> Put([FromBody]StudioDto model)
        {
            var studio = _studioService.Update(model);
            return ApiModelResult<StudioDto>.Ok(studio);
        }
    }
}
