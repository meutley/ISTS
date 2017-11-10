using System;
using System.Collections.Generic;

using ISTS.Application.Sessions;

namespace ISTS.Application.Rooms
{
    public class RoomDto
    {
        public Guid Id { get; set; }

        public Guid StudioId { get; set; }

        public string Name { get; set; }

        public List<RoomFunctionDto> RoomFunctions { get; set; }

        public List<SessionDto> Sessions { get; set; }
    }
}