using System;

namespace ISTS.Application.Users
{
    public class UserTimeZoneDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public ushort UtcOffset { get; set; }
    }
}