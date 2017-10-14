using System;
using System.Collections.Generic;

namespace ISTS.Application.Rooms
{
    public class RoomDto
    {
        public Guid Id { get; set; }

        public Guid StudioId { get; set; }

        public string Name { get; set; }

        public List<RoomSessionDto> Sessions { get; set; }
    }
}