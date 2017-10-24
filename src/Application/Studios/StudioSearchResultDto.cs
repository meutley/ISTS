using System;

using ISTS.Application.Users;
using ISTS.Domain.Studios;

namespace ISTS.Application.Studios
{
    public class StudioSearchResultDto
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string FriendlyUrl { get; set; }

        public Guid OwnerUserId { get; set; }

        public string PostalCode { get; set; }

        public double? Distance { get; set; }
    }
}