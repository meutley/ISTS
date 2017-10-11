using System;

using ISTS.Helpers.Validation;
using ISTS.Domain.Schedules;

namespace ISTS.Domain.Studios
{
    public class Studio : IDomainObject
    {
        public Guid Id { get; private set; }

        public string Name { get; private set; }

        public string FriendlyUrl { get; private set; }

        public static Studio Create(string name, string friendlyUrl)
        {
            ArgumentNotNullValidator.Validate(name, nameof(name));
            ArgumentNotNullValidator.Validate(friendlyUrl, nameof(friendlyUrl));

            var result = new Studio
            {
                Id = Guid.NewGuid(),
                Name = name,
                FriendlyUrl = friendlyUrl
            };

            return result;
        }

        public StudioSession CreateSession(DateRange scheduledTime)
        {
            var session = StudioSession.Create(this.Id, scheduledTime);

            return session;
        }
    }
}