using System;
using System.Collections.Generic;

namespace ISTS.Application.Studios
{
    public class StudioDto
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string FriendlyUrl { get; set; }

        public List<StudioRoomDto> Rooms { get; set; }
    }
}