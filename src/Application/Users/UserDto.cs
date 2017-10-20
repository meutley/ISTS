using System;
using System.Collections.Generic;

using ISTS.Application.Studios;

namespace ISTS.Application.Users
{
    public class UserDto
    {
        public Guid Id { get; set; }

        public string Email { get; set; }

        public string DisplayName { get; set; }

        public string PostalCode { get; set; }

        public List<StudioDto> Studios { get; set; }
    }
}