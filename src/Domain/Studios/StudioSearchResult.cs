using System;

using ISTS.Domain.Users;

namespace ISTS.Domain.Studios
{
    public class StudioSearchResult
    {
        public Guid Id { get; protected set; }

        public string Name { get; protected set; }

        public string FriendlyUrl { get; protected set; }

        public Guid OwnerUserId { get; protected set; }

        public string PostalCode { get; protected set; }

        public double Distance { get; protected set; }

        public static StudioSearchResult Create(
            Guid id,
            string name,
            string friendlyUrl,
            Guid ownerUserId,
            string postalCode,
            double distance)
        {
            return new StudioSearchResult
            {
                Id = id,
                Name = name,
                FriendlyUrl = friendlyUrl,
                OwnerUserId = ownerUserId,
                PostalCode = postalCode,
                Distance = distance
            };
        }
    }
}