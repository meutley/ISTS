using System;
using System.Collections.Generic;

using ISTS.Application.Rooms;

namespace ISTS.Application.Studios
{
    public class StudioDto
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string FriendlyUrl { get; set; }

        public List<RoomDto> Rooms { get; set; }

        public Guid OwnerUserId { get; set; }
    }
}