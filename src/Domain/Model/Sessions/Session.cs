using System;

using ISTS.Helpers.Validation;
using ISTS.Domain.Model.Schedules;
using ISTS.Domain.Model.Studios;

namespace ISTS.Domain.Model.Sessions
{
    public class Session : IDomainObject
    {
        public Guid Id { get; protected set; }

        public Guid StudioId { get; protected set; }

        public DateRange ScheduledTime { get; protected set; }

        public static Session Create(Studio studio, DateRange scheduledTime)
        {
            ArgumentNotNullValidator.Validate(studio, nameof(studio));

            var result = new Session
            {
                Id = Guid.NewGuid(),
                StudioId = studio.Id,
                ScheduledTime = scheduledTime
            };

            return result;
        }
    }
}