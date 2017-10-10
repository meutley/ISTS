using System;
using System.Collections;

namespace ISTS.Application.Studios
{
    public class StudioDto
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string FriendlyUrl { get; set; }

        public List<SessionDto> Sessions { get; set; }
    }
}