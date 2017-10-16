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
        
        [HttpGet("{id}")]
        public ApiModelResult<StudioDto> Get(Guid id)
        {
            var studio = _studioService.Get(id);
            
            return studio != null
                ? ApiModelResult<StudioDto>.Ok(studio)
                : ApiModelResult<StudioDto>.NotFound(null);
        }

        [HttpGet]
        public ApiModelResult<List<StudioDto>> List()
        {
            var studios = _studioService.GetAll();

            return ApiModelResult<List<StudioDto>>.Ok(studios);
        }
        
        // POST api/studio
        [HttpPost]
        public ApiModelResult<StudioDto> Post([FromBody]string name, [FromBody]string friendlyUrl)
        {
            var studio = _studioService.Create(name, friendlyUrl);
            return ApiModelResult<StudioDto>.Ok(studio);
        }
    }
}
