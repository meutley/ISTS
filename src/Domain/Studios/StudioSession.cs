using System;

using ISTS.Helpers.Validation;
using ISTS.Domain.Schedules;

namespace ISTS.Domain.Studios
{
    public class StudioSession : IDomainObject
    {
        public Guid Id { get; protected set; }

        public Guid StudioId { get; protected set; }

        public DateRange ScheduledTime { get; protected set; }

        public static StudioSession Create(Guid studioId, DateRange scheduledTime)
        {
            var session = new StudioSession
            {
                Id = Guid.NewGuid(),
                StudioId = studioId,
                ScheduledTime = scheduledTime
            };

            return session;
        }
    }
}