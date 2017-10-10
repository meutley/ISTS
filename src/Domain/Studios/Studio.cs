using System;

namespace ISTS.Domain.Studios
{
    public class Studio : IDomainObject
    {
        public Guid Id { get; private set; }

        public string Name { get; private set; }

        public string FriendlyUrl { get; private set; }

        public static Studio Create()
        {
            var result = new Studio
            {
                Id = Guid.NewGuid()
            };

            return result;
        }
    }
}